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

	    private static string _mStrFileName = "";

	    static Game()
		{
			PlayerWhite = new PlayerWhite();
			PlayerBlack = new PlayerBlack();
			PlayerToPlay = PlayerWhite;
			Board.EstablishHashKey();

			var registryKeySoftware =Registry.CurrentUser.OpenSubKey("Software",true);
			var registryKeyChess = registryKeySoftware?.CreateSubKey(@"PeterHughes.org\Chess");

			ShowThinking = (string)registryKeyChess?.GetValue("ShowThinking")=="1";
			DisplayMoveAnalysisTree = (string)registryKeyChess?.GetValue("DisplayMoveAnalysisTree")=="1";

			PlayerToPlay.Clock.Start();
		}

		~Game()
		{
			var registryKeySoftware =Registry.CurrentUser.OpenSubKey("Software",true);
			var registryKeyChess = registryKeySoftware?.CreateSubKey(@"PeterHughes.org\Chess");

			registryKeyChess?.SetValue("ShowThinking", ShowThinking ? "1" : "0");
			registryKeyChess?.SetValue("DisplayMoveAnalysisTree", DisplayMoveAnalysisTree ? "1" : "0");
		}

		public static void New()
		{
			HashTable.Clear();
			HashTablePawn.Clear();
			HashTableCheck.Clear();
			UndoAllMoves();
			MoveRedoList.Clear();
			_mStrFileName = "";
			PlayerWhite.Clock.Reset();
			PlayerBlack.Clock.Reset();
		}

		public static void Load(string fileName)
		{
			MoveRedoList.Clear();
			LoadToTurnNo(fileName, -1);
			_mStrFileName = fileName;
		}

		private static void LoadToTurnNo(string strFileName, int noOfTurns)
		{
			New();
			var xmldoc = new XmlDocument();
			xmldoc.Load(strFileName);
		    var xmlnodelist = xmldoc.SelectNodes("/Game/Move");
			var intTurnNo = 0;
		    if (xmlnodelist != null)
		        foreach (XmlElement xmlnode in xmlnodelist)
		        {
		            if (noOfTurns!=-1 && intTurnNo>noOfTurns) break;
		            MakeAMove( Move.MoveNameFromString(xmlnode.GetAttribute("Name")), Board.GetPiece(Convert.ToInt32(xmlnode.GetAttribute("FromFile")), Convert.ToInt32(xmlnode.GetAttribute("FromRank"))), Board.GetSquare(Convert.ToInt32(xmlnode.GetAttribute("ToFile")), Convert.ToInt32(xmlnode.GetAttribute("ToRank"))) );
		            TimeSpan tsnTimeStamp;
		            if (xmlnode.GetAttribute("SecondsElapsed")=="")
		            {
		                if (MoveHistory.Count<=2)
		                {
		                    tsnTimeStamp = new TimeSpan(0);
		                }
		                else
		                {
		                    tsnTimeStamp = MoveHistory.PenultimateForSameSide.TimeStamp + new TimeSpan(0,0,30);
		                }
		            }
		            else
		            {
		                tsnTimeStamp = new TimeSpan(0,0, int.Parse(xmlnode.GetAttribute("SecondsElapsed")));
		            }
		            MoveHistory.Last.TimeStamp = tsnTimeStamp;
		            MoveHistory.Last.Piece.Player.Clock.TimeElapsed = tsnTimeStamp;
		            intTurnNo++;
		        }
		}

		public static void Save(string fileName)
		{
			var xmldoc = new XmlDocument();
			var xmlnodeGame = xmldoc.CreateElement("Game");
			xmldoc.AppendChild(xmlnodeGame);
			XmlElement xmlnodeMove;

			foreach(Move move in MoveHistory)
			{
				xmlnodeMove = xmldoc.CreateElement("Move");
				xmlnodeGame.AppendChild( xmlnodeMove );
				xmlnodeMove.SetAttribute("MoveNo", move.MoveNo.ToString());
				xmlnodeMove.SetAttribute("Name", move.Name.ToString());
				xmlnodeMove.SetAttribute("FromRank", move.From.Rank.ToString());
				xmlnodeMove.SetAttribute("FromFile", move.From.File.ToString());
				xmlnodeMove.SetAttribute("ToRank", move.To.Rank.ToString());
				xmlnodeMove.SetAttribute("ToFile", move.To.File.ToString());
				xmlnodeMove.SetAttribute("SecondsElapsed", Convert.ToInt32(move.TimeStamp.TotalSeconds).ToString() );
			}

			xmldoc.Save(fileName);
			_mStrFileName = fileName;
		}

		public static void UndoAllMoves()
		{
			while (MoveHistory.Count>0)
			{
				UndoMove();
			}
		}

		public static void RedoAllMoves()
		{
			while (MoveRedoList.Count>0)
			{
				RedoMove();
			}
		}

		public static string FileName => _mStrFileName=="" ? "New Game" : _mStrFileName;

	    public static Moves MoveHistory { get; } = new Moves();

	    public static Moves MoveRedoList { get; } = new Moves();

	    public static Moves MoveAnalysis { get; set; } = new Moves();

	    public static int TurnNo { get; set; }

	    public static int MaxMaterialValue => 7;

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

		public static Player PlayerWhite { get; }

	    public static Player PlayerBlack { get; }

	    public static Player PlayerToPlay { get; set; }

	    public static Move MakeAMove(Move.EnmName moveName, Piece piece, Square square)
		{
			MoveRedoList.Clear();

			var move = piece.Move(moveName, square);
			move.EnemyStatus = move.Piece.Player.OtherPlayer.Status;
			PlayerToPlay.Clock.Stop();
			MoveHistory.Last.TimeStamp = PlayerToPlay.Clock.TimeElapsed;
			PlayerToPlay = PlayerToPlay.OtherPlayer;
			PlayerToPlay.Clock.Start();
			return move;
		}

		public static void UndoMove()
		{
			if (MoveHistory.Count>0)
			{
				var moveUndo = MoveHistory.Item(MoveHistory.Count-1);
				PlayerToPlay.Clock.Revert();
				MoveRedoList.Add(moveUndo);
				Move.Undo( moveUndo );
				PlayerToPlay = PlayerToPlay.OtherPlayer;
				if (MoveHistory.Count>1)
				{
					var movePenultimate = MoveHistory.Item(MoveHistory.Count-2);
					PlayerToPlay.Clock.TimeElapsed = movePenultimate.TimeStamp;
				}
				else
				{
					PlayerToPlay.Clock.TimeElapsed = new TimeSpan(0);
				}
				PlayerToPlay.Clock.Start();
			}
		}

		public static void RedoMove()
		{
			if (MoveRedoList.Count>0)
			{
				var moveRedo = MoveRedoList.Item(MoveRedoList.Count-1);
				PlayerToPlay.Clock.Revert();
				moveRedo.Piece.Move(moveRedo.Name, moveRedo.To);
				PlayerToPlay.Clock.TimeElapsed = moveRedo.TimeStamp;
				MoveHistory.Last.TimeStamp = moveRedo.TimeStamp;
				PlayerToPlay = PlayerToPlay.OtherPlayer;
				MoveRedoList.RemoveLast();
				PlayerToPlay.Clock.Start();
			}
		}

		public static void ResumePlay()
		{
			PlayerToPlay.Clock.Start();
		}

		public static void PausePlay()
		{
			PlayerToPlay.Clock.Stop();
		}

		public static bool ShowThinking { get; set; }

	    public static bool DisplayMoveAnalysisTree { get; set; }
	}
}
