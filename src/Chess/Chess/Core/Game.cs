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

		private static Player _mPlayerWhite;
		private static Player _mPlayerBlack;
		private static Player _mPlayerToPlay;
		private static int _mIntTurnNo = 0;
		private static Moves _mMovesHistory = new Moves();
		private static Moves _mMovesRedoList = new Moves();
		private static Moves _mMovesAnalysis = new Moves();
		private static string _mStrFileName = "";

		private static bool _mBlnShowThinking = false;
		private static bool _mBlnDisplayMoveAnalysisTree = false;

		static Game()
		{
			_mPlayerWhite = new PlayerWhite();
			_mPlayerBlack = new PlayerBlack();
			_mPlayerToPlay = _mPlayerWhite;
			Board.EstablishHashKey();

			RegistryKey registryKeySoftware =Registry.CurrentUser.OpenSubKey("Software",true);
			RegistryKey registryKeySharpChess = registryKeySoftware.CreateSubKey(@"PeterHughes.org\SharpChess");

			_mBlnShowThinking = ((string)registryKeySharpChess.GetValue("ShowThinking")=="1");
			_mBlnDisplayMoveAnalysisTree = ((string)registryKeySharpChess.GetValue("DisplayMoveAnalysisTree")=="1");

			PlayerToPlay.Clock.Start();
		}

		~Game()
		{
			RegistryKey registryKeySoftware =Registry.CurrentUser.OpenSubKey("Software",true);
			RegistryKey registryKeySharpChess = registryKeySoftware.CreateSubKey(@"PeterHughes.org\SharpChess");

			registryKeySharpChess.SetValue("ShowThinking", _mBlnShowThinking ? "1" : "0");
			registryKeySharpChess.SetValue("DisplayMoveAnalysisTree", _mBlnDisplayMoveAnalysisTree ? "1" : "0");
		}

		public static void New()
		{
			HashTable.Clear();
			HashTablePawn.Clear();
			HashTableCheck.Clear();
			UndoAllMoves();
			_mMovesRedoList.Clear();
			_mStrFileName = "";
			_mPlayerWhite.Clock.Reset();
			_mPlayerBlack.Clock.Reset();
		}

		public static void Load(string fileName)
		{
			_mMovesRedoList.Clear();
			LoadToTurnNo(fileName, -1);
			_mStrFileName = fileName;
		}

		private static void LoadToTurnNo(string strFileName, int noOfTurns)
		{
			New();
			XmlDocument xmldoc = new XmlDocument();
			xmldoc.Load(strFileName);
			XmlNodeList xmlnodelist;
			xmlnodelist = xmldoc.SelectNodes("/Game/Move");
			int intTurnNo = 0;
			TimeSpan tsnTimeStamp;
			foreach (XmlElement xmlnode in xmlnodelist)
			{
				if (noOfTurns!=-1 && intTurnNo>noOfTurns) break;
				MakeAMove( Move.MoveNameFromString(xmlnode.GetAttribute("Name")), Board.GetPiece(Convert.ToInt32(xmlnode.GetAttribute("FromFile")), Convert.ToInt32(xmlnode.GetAttribute("FromRank"))), Board.GetSquare(Convert.ToInt32(xmlnode.GetAttribute("ToFile")), Convert.ToInt32(xmlnode.GetAttribute("ToRank"))) );
				if (xmlnode.GetAttribute("SecondsElapsed")=="")
				{
					if (_mMovesHistory.Count<=2)
					{
						tsnTimeStamp = (new TimeSpan(0));
					}
					else
					{
						tsnTimeStamp = ( _mMovesHistory.PenultimateForSameSide.TimeStamp + (new TimeSpan(0,0,30)) );
					}
				}
				else
				{
					tsnTimeStamp = new TimeSpan(0,0, int.Parse(xmlnode.GetAttribute("SecondsElapsed")));
				}
				_mMovesHistory.Last.TimeStamp = tsnTimeStamp;
				_mMovesHistory.Last.Piece.Player.Clock.TimeElapsed = tsnTimeStamp;
				intTurnNo++;
			}
		}

		public static void Save(string fileName)
		{
			XmlDocument xmldoc = new XmlDocument();
			XmlElement xmlnodeGame = xmldoc.CreateElement("Game");
			xmldoc.AppendChild(xmlnodeGame);
			XmlElement xmlnodeMove;

			foreach(Move move in _mMovesHistory)
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
			while (_mMovesHistory.Count>0)
			{
				UndoMove();
			}
		}

		public static void RedoAllMoves()
		{
			while (_mMovesRedoList.Count>0)
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
			get { return _mMovesHistory; }
		} 

		public static Moves MoveRedoList
		{
			get { return _mMovesRedoList; }
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
				int intWhiteMaterialValue = PlayerWhite.MaterialCount;
				int intBlackMaterialValue = PlayerBlack.MaterialCount;
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
			get	{ return _mPlayerWhite;	}
		}

		public static Player PlayerBlack
		{
			get	{ return _mPlayerBlack;	}
		}

		public static Player PlayerToPlay
		{
			get	{ return _mPlayerToPlay;	}
			set	{ _mPlayerToPlay = value;	}
		}

		public static Move MakeAMove(Move.EnmName moveName, Piece piece, Square square)
		{
			_mMovesRedoList.Clear();

			Move move = piece.Move(moveName, square);
			move.EnemyStatus = move.Piece.Player.OtherPlayer.Status;
			_mPlayerToPlay.Clock.Stop();
			_mMovesHistory.Last.TimeStamp = _mPlayerToPlay.Clock.TimeElapsed;
			_mPlayerToPlay = _mPlayerToPlay.OtherPlayer;
			_mPlayerToPlay.Clock.Start();
			return move;
		}

		public static void UndoMove()
		{
			if (_mMovesHistory.Count>0)
			{
				Move moveUndo = _mMovesHistory.Item(_mMovesHistory.Count-1);
				_mPlayerToPlay.Clock.Revert();
				_mMovesRedoList.Add(moveUndo);
				Move.Undo( moveUndo );
				_mPlayerToPlay = _mPlayerToPlay.OtherPlayer;
				if (_mMovesHistory.Count>1)
				{
					Move movePenultimate = _mMovesHistory.Item(_mMovesHistory.Count-2);
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
			if (_mMovesRedoList.Count>0)
			{
				Move moveRedo = _mMovesRedoList.Item(_mMovesRedoList.Count-1);
				_mPlayerToPlay.Clock.Revert();
				moveRedo.Piece.Move(moveRedo.Name, moveRedo.To);
				_mPlayerToPlay.Clock.TimeElapsed = moveRedo.TimeStamp;
				_mMovesHistory.Last.TimeStamp = moveRedo.TimeStamp;
				_mPlayerToPlay = _mPlayerToPlay.OtherPlayer;
				_mMovesRedoList.RemoveLast();
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
