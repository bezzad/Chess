using System;
using System.Xml;
using Microsoft.Win32;

namespace Chess.Core
{
	public delegate void DelegateGameEvent();

	public class Game
	{
		public enum EnmStage
		{
				Opening
			,	Middle
			,	End
		}

		private static readonly Player MPlayerWhite;
		private static readonly Player MPlayerBlack;
		private static Player _mPlayerToPlay;
		private static int _mIntTurnNo;
		private static readonly Moves MMovesHistory = new Moves();
		private static readonly Moves MMovesRedoList = new Moves();
		private static Moves _mMovesAnalysis = new Moves();
		private static string _mStrFileName = "";

		private static bool _mBlnShowThinking;
		private static bool _mBlnDisplayMoveAnalysisTree;

		static Game()
		{
			MPlayerWhite = new PlayerWhite();
			MPlayerBlack = new PlayerBlack();
			_mPlayerToPlay = MPlayerWhite;
			Board.EstablishHashKey();

			var registryKeySoftware =Registry.CurrentUser.OpenSubKey("Software",true);
			var registryKeyChess = registryKeySoftware.CreateSubKey(@"PeterHughes.org\Chess");

			_mBlnShowThinking = ((string)registryKeyChess.GetValue("ShowThinking")=="1");
			_mBlnDisplayMoveAnalysisTree = ((string)registryKeyChess.GetValue("DisplayMoveAnalysisTree")=="1");

			PlayerToPlay.Clock.Start();
		}

		~Game()
		{
			var registryKeySoftware =Registry.CurrentUser.OpenSubKey("Software",true);
			var registryKeyChess = registryKeySoftware.CreateSubKey(@"PeterHughes.org\Chess");

			registryKeyChess.SetValue("ShowThinking", _mBlnShowThinking ? "1" : "0");
			registryKeyChess.SetValue("DisplayMoveAnalysisTree", _mBlnDisplayMoveAnalysisTree ? "1" : "0");
		}

		public static void New()
		{
			HashTable.Clear();
			HashTablePawn.Clear();
			HashTableCheck.Clear();
			UndoAllMoves();
			MMovesRedoList.Clear();
			_mStrFileName = "";
			MPlayerWhite.Clock.Reset();
			MPlayerBlack.Clock.Reset();
		}

		public static void Load(string fileName)
		{
			MMovesRedoList.Clear();
			LoadToTurnNo(fileName, -1);
			_mStrFileName = fileName;
		}

		private static void LoadToTurnNo(string strFileName, int noOfTurns)
		{
			New();
			var xmldoc = new XmlDocument();
			xmldoc.Load(strFileName);
			XmlNodeList xmlnodelist;
			xmlnodelist = xmldoc.SelectNodes("/Game/Move");
			var intTurnNo = 0;
			TimeSpan tsnTimeStamp;
			foreach (XmlElement xmlnode in xmlnodelist)
			{
				if (noOfTurns!=-1 && intTurnNo>noOfTurns) break;
				MakeAMove( Move.MoveNameFromString(xmlnode.GetAttribute("Name")), Board.GetPiece(Convert.ToInt32(xmlnode.GetAttribute("FromFile")), Convert.ToInt32(xmlnode.GetAttribute("FromRank"))), Board.GetSquare(Convert.ToInt32(xmlnode.GetAttribute("ToFile")), Convert.ToInt32(xmlnode.GetAttribute("ToRank"))) );
				if (xmlnode.GetAttribute("SecondsElapsed")=="")
				{
					if (MMovesHistory.Count<=2)
					{
						tsnTimeStamp = (new TimeSpan(0));
					}
					else
					{
						tsnTimeStamp = ( MMovesHistory.PenultimateForSameSide.TimeStamp + (new TimeSpan(0,0,30)) );
					}
				}
				else
				{
					tsnTimeStamp = new TimeSpan(0,0, int.Parse(xmlnode.GetAttribute("SecondsElapsed")));
				}
				MMovesHistory.Last.TimeStamp = tsnTimeStamp;
				MMovesHistory.Last.Piece.Player.Clock.TimeElapsed = tsnTimeStamp;
				intTurnNo++;
			}
		}

		public static void Save(string fileName)
		{
			var xmldoc = new XmlDocument();
			var xmlnodeGame = xmldoc.CreateElement("Game");
			xmldoc.AppendChild(xmlnodeGame);
			XmlElement xmlnodeMove;

			foreach(Move move in MMovesHistory)
			{
				xmlnodeMove = xmldoc.CreateElement("Move");
				xmlnodeGame.AppendChild( xmlnodeMove );
				xmlnodeMove.SetAttribute("MoveNo", move.MoveNo.ToString());
				xmlnodeMove.SetAttribute("Name", move.Name.ToString());
				xmlnodeMove.SetAttribute("FromRank", move.From.Rank.ToString());
				xmlnodeMove.SetAttribute("FromFile", move.From.File.ToString());
				xmlnodeMove.SetAttribute("ToRank", move.To.Rank.ToString());
				xmlnodeMove.SetAttribute("ToFile", move.To.File.ToString());
				xmlnodeMove.SetAttribute("SecondsElapsed", (Convert.ToInt32(move.TimeStamp.TotalSeconds)).ToString() );
			}

			xmldoc.Save(fileName);
			_mStrFileName = fileName;
		}

		public static void UndoAllMoves()
		{
			while (MMovesHistory.Count>0)
			{
				UndoMove();
			}
		}

		public static void RedoAllMoves()
		{
			while (MMovesRedoList.Count>0)
			{
				RedoMove();
			}
		}

		public static string FileName
		{
			get { return _mStrFileName=="" ? "New Game" : _mStrFileName; }
		} 

		public static Moves MoveHistory
		{
			get { return MMovesHistory; }
		} 

		public static Moves MoveRedoList
		{
			get { return MMovesRedoList; }
		} 

		public static Moves MoveAnalysis
		{
			get { return _mMovesAnalysis; }
			set { _mMovesAnalysis = value; }
		} 

		public static int TurnNo 
		{
			get	{ return _mIntTurnNo; }
			set { _mIntTurnNo = value; }
		}

		public static int MaxMaterialValue
		{
			get { return 7; }
		}

		public static int LowestMaterialValue
		{
			get
			{
				var intWhiteMaterialValue = PlayerWhite.MaterialCount;
				var intBlackMaterialValue = PlayerBlack.MaterialCount;
				return intWhiteMaterialValue<intBlackMaterialValue ? intWhiteMaterialValue : intBlackMaterialValue;
			}
		}

		public static EnmStage Stage
		{
			get
			{
				if (LowestMaterialValue >= MaxMaterialValue)
				{
					return EnmStage.Opening;
				}
				else if (LowestMaterialValue <= 3)
				{
					return EnmStage.End;
				}
				return EnmStage.Middle;
			}
		}

		public static Player PlayerWhite
		{
			get	{ return MPlayerWhite;	}
		}

		public static Player PlayerBlack
		{
			get	{ return MPlayerBlack;	}
		}

		public static Player PlayerToPlay
		{
			get	{ return _mPlayerToPlay;	}
			set	{ _mPlayerToPlay = value;	}
		}

		public static Move MakeAMove(Move.EnmName moveName, Piece piece, Square square)
		{
			MMovesRedoList.Clear();

			var move = piece.Move(moveName, square);
			move.EnemyStatus = move.Piece.Player.OtherPlayer.Status;
			_mPlayerToPlay.Clock.Stop();
			MMovesHistory.Last.TimeStamp = _mPlayerToPlay.Clock.TimeElapsed;
			_mPlayerToPlay = _mPlayerToPlay.OtherPlayer;
			_mPlayerToPlay.Clock.Start();
			return move;
		}

		public static void UndoMove()
		{
			if (MMovesHistory.Count>0)
			{
				var moveUndo = MMovesHistory.Item(MMovesHistory.Count-1);
				_mPlayerToPlay.Clock.Revert();
				MMovesRedoList.Add(moveUndo);
				Move.Undo( moveUndo );
				_mPlayerToPlay = _mPlayerToPlay.OtherPlayer;
				if (MMovesHistory.Count>1)
				{
					var movePenultimate = MMovesHistory.Item(MMovesHistory.Count-2);
					_mPlayerToPlay.Clock.TimeElapsed = movePenultimate.TimeStamp;
				}
				else
				{
					_mPlayerToPlay.Clock.TimeElapsed = new TimeSpan(0);
				}
				_mPlayerToPlay.Clock.Start();
			}
		}

		public static void RedoMove()
		{
			if (MMovesRedoList.Count>0)
			{
				var moveRedo = MMovesRedoList.Item(MMovesRedoList.Count-1);
				_mPlayerToPlay.Clock.Revert();
				moveRedo.Piece.Move(moveRedo.Name, moveRedo.To);
				_mPlayerToPlay.Clock.TimeElapsed = moveRedo.TimeStamp;
				MMovesHistory.Last.TimeStamp = moveRedo.TimeStamp;
				_mPlayerToPlay = _mPlayerToPlay.OtherPlayer;
				MMovesRedoList.RemoveLast();
				_mPlayerToPlay.Clock.Start();
			}
		}

		public static void ResumePlay()
		{
			_mPlayerToPlay.Clock.Start();
		}

		public static void PausePlay()
		{
			_mPlayerToPlay.Clock.Stop();
		}

		public static bool ShowThinking 
		{
			get	{ return _mBlnShowThinking;}
			set { _mBlnShowThinking = value; }
		}

		public static bool DisplayMoveAnalysisTree 
		{
			get	{ return _mBlnDisplayMoveAnalysisTree;}
			set { _mBlnDisplayMoveAnalysisTree = value; }
		}

	}
}
