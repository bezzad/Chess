using System;
using System.Xml;
using Microsoft.Win32;

namespace Chess.Core
{
	public delegate void delegateGameEvent();

	public class Game
	{
		public enum enmStage
		{
				Opening
			,	Middle
			,	End
		}

		private static Player m_playerWhite;
		private static Player m_playerBlack;
		private static Player m_playerToPlay;
		private static int m_intTurnNo = 0;
		private static Moves m_movesHistory = new Moves();
		private static Moves m_movesRedoList = new Moves();
		private static Moves m_movesAnalysis = new Moves();
		private static string m_strFileName = "";

		private static bool m_blnShowThinking = false;
		private static bool m_blnDisplayMoveAnalysisTree = false;

		static Game()
		{
			m_playerWhite = new PlayerWhite();
			m_playerBlack = new PlayerBlack();
			m_playerToPlay = m_playerWhite;
			Board.EstablishHashKey();

			RegistryKey registryKeySoftware =Registry.CurrentUser.OpenSubKey("Software",true);
			RegistryKey registryKeySharpChess = registryKeySoftware.CreateSubKey(@"PeterHughes.org\SharpChess");

			m_blnShowThinking = ((string)registryKeySharpChess.GetValue("ShowThinking")=="1");
			m_blnDisplayMoveAnalysisTree = ((string)registryKeySharpChess.GetValue("DisplayMoveAnalysisTree")=="1");

			Game.PlayerToPlay.Clock.Start();
		}

		~Game()
		{
			RegistryKey registryKeySoftware =Registry.CurrentUser.OpenSubKey("Software",true);
			RegistryKey registryKeySharpChess = registryKeySoftware.CreateSubKey(@"PeterHughes.org\SharpChess");

			registryKeySharpChess.SetValue("ShowThinking", m_blnShowThinking ? "1" : "0");
			registryKeySharpChess.SetValue("DisplayMoveAnalysisTree", m_blnDisplayMoveAnalysisTree ? "1" : "0");
		}

		public static void New()
		{
			HashTable.Clear();
			HashTablePawn.Clear();
			HashTableCheck.Clear();
			UndoAllMoves();
			m_movesRedoList.Clear();
			m_strFileName = "";
			m_playerWhite.Clock.Reset();
			m_playerBlack.Clock.Reset();
		}

		public static void Load(string FileName)
		{
			m_movesRedoList.Clear();
			LoadToTurnNo(FileName, -1);
			m_strFileName = FileName;
		}

		private static void LoadToTurnNo(string strFileName, int NoOfTurns)
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
				if (NoOfTurns!=-1 && intTurnNo>NoOfTurns) break;
				MakeAMove( Move.MoveNameFromString(xmlnode.GetAttribute("Name")), Board.GetPiece(Convert.ToInt32(xmlnode.GetAttribute("FromFile")), Convert.ToInt32(xmlnode.GetAttribute("FromRank"))), Board.GetSquare(Convert.ToInt32(xmlnode.GetAttribute("ToFile")), Convert.ToInt32(xmlnode.GetAttribute("ToRank"))) );
				if (xmlnode.GetAttribute("SecondsElapsed")=="")
				{
					if (m_movesHistory.Count<=2)
					{
						tsnTimeStamp = (new TimeSpan(0));
					}
					else
					{
						tsnTimeStamp = ( m_movesHistory.PenultimateForSameSide.TimeStamp + (new TimeSpan(0,0,30)) );
					}
				}
				else
				{
					tsnTimeStamp = new TimeSpan(0,0, int.Parse(xmlnode.GetAttribute("SecondsElapsed")));
				}
				m_movesHistory.Last.TimeStamp = tsnTimeStamp;
				m_movesHistory.Last.Piece.Player.Clock.TimeElapsed = tsnTimeStamp;
				intTurnNo++;
			}
		}

		public static void Save(string FileName)
		{
			XmlDocument xmldoc = new XmlDocument();
			XmlElement xmlnodeGame = xmldoc.CreateElement("Game");
			xmldoc.AppendChild(xmlnodeGame);
			XmlElement xmlnodeMove;

			foreach(Move move in m_movesHistory)
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

			xmldoc.Save(FileName);
			m_strFileName = FileName;
		}

		public static void UndoAllMoves()
		{
			while (m_movesHistory.Count>0)
			{
				UndoMove();
			}
		}

		public static void RedoAllMoves()
		{
			while (m_movesRedoList.Count>0)
			{
				RedoMove();
			}
		}

		public static string FileName
		{
			get { return m_strFileName=="" ? "New Game" : m_strFileName; }
		} 

		public static Moves MoveHistory
		{
			get { return m_movesHistory; }
		} 

		public static Moves MoveRedoList
		{
			get { return m_movesRedoList; }
		} 

		public static Moves MoveAnalysis
		{
			get { return m_movesAnalysis; }
			set { m_movesAnalysis = value; }
		} 

		public static int TurnNo 
		{
			get	{ return m_intTurnNo; }
			set { m_intTurnNo = value; }
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

		public static Game.enmStage Stage
		{
			get
			{
				if (LowestMaterialValue >= MaxMaterialValue)
				{
					return Game.enmStage.Opening;
				}
				else if (LowestMaterialValue <= 3)
				{
					return Game.enmStage.End;
				}
				return Game.enmStage.Middle;
			}
		}

		public static Player PlayerWhite
		{
			get	{ return m_playerWhite;	}
		}

		public static Player PlayerBlack
		{
			get	{ return m_playerBlack;	}
		}

		public static Player PlayerToPlay
		{
			get	{ return m_playerToPlay;	}
			set	{ m_playerToPlay = value;	}
		}

		public static Move MakeAMove(Move.enmName MoveName, Piece piece, Square square)
		{
			m_movesRedoList.Clear();

			Move move = piece.Move(MoveName, square);
			move.EnemyStatus = move.Piece.Player.OtherPlayer.Status;
			m_playerToPlay.Clock.Stop();
			m_movesHistory.Last.TimeStamp = m_playerToPlay.Clock.TimeElapsed;
			m_playerToPlay = m_playerToPlay.OtherPlayer;
			m_playerToPlay.Clock.Start();
			return move;
		}

		public static void UndoMove()
		{
			if (m_movesHistory.Count>0)
			{
				Move moveUndo = m_movesHistory.Item(m_movesHistory.Count-1);
				m_playerToPlay.Clock.Revert();
				m_movesRedoList.Add(moveUndo);
				Move.Undo( moveUndo );
				m_playerToPlay = m_playerToPlay.OtherPlayer;
				if (m_movesHistory.Count>1)
				{
					Move movePenultimate = m_movesHistory.Item(m_movesHistory.Count-2);
					m_playerToPlay.Clock.TimeElapsed = movePenultimate.TimeStamp;
				}
				else
				{
					m_playerToPlay.Clock.TimeElapsed = new TimeSpan(0);
				}
				m_playerToPlay.Clock.Start();
			}
		}

		public static void RedoMove()
		{
			if (m_movesRedoList.Count>0)
			{
				Move moveRedo = m_movesRedoList.Item(m_movesRedoList.Count-1);
				m_playerToPlay.Clock.Revert();
				moveRedo.Piece.Move(moveRedo.Name, moveRedo.To);
				m_playerToPlay.Clock.TimeElapsed = moveRedo.TimeStamp;
				m_movesHistory.Last.TimeStamp = moveRedo.TimeStamp;
				m_playerToPlay = m_playerToPlay.OtherPlayer;
				m_movesRedoList.RemoveLast();
				m_playerToPlay.Clock.Start();
			}
		}

		public static void ResumePlay()
		{
			m_playerToPlay.Clock.Start();
		}

		public static void PausePlay()
		{
			m_playerToPlay.Clock.Stop();
		}

		public static bool ShowThinking 
		{
			get	{ return m_blnShowThinking;}
			set { m_blnShowThinking = value; }
		}

		public static bool DisplayMoveAnalysisTree 
		{
			get	{ return m_blnDisplayMoveAnalysisTree;}
			set { m_blnDisplayMoveAnalysisTree = value; }
		}

	}
}
