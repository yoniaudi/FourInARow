using FourInARow.Models;
using FourInARow.Enums;
using System;
using System.Collections.Generic;

namespace FourInARow.Logic
{
    // $G$ SFN-014 : The program Includes AI computer player.
    public class GameLogic
    {
        public const int k_BoardGameMinLength = 4;
        public const int k_BoardGameMaxLength = 8;
        public bool IsGameRunning { get; private set; } = false;
        public bool IsMultiplayer { get; private set; } = false;
        private readonly ScoreHandlerModel r_ScoreHandler = null;
        private BoardGameModel m_BoardGame = null;
        private ComputerPlayerAIModel m_ComputerPlayerAI = null;
        private List<HumanPlayerModel> m_Players = null;
        private int m_CurrentPlayerIndx = 0;

        public GameLogic()
        {
            IsGameRunning = true;
            r_ScoreHandler = new ScoreHandlerModel();
        }

        public void InitializeBoardGame(int i_RowLength, int i_ColumnLength)
        {
            if (i_RowLength < k_BoardGameMinLength || i_RowLength > k_BoardGameMaxLength ||
                i_ColumnLength < k_BoardGameMinLength || i_ColumnLength > k_BoardGameMaxLength)
            {
                string exceptionMessage = string.Format("Row length or column length must be within the specified range {0}-{1}.",
                    k_BoardGameMinLength, k_BoardGameMaxLength);

                throw new ArgumentOutOfRangeException("i_RowLength or i_ColumnLength", exceptionMessage);
            }

            m_BoardGame = new BoardGameModel(i_RowLength, i_ColumnLength);
        }

        public string BoardGameToString()
        {
            return m_BoardGame.ToString();
        }

        public void SetGameMode(string i_GameMode)
        {
            Random random = new Random();
            string singlePlayer = "SinglePlayer";
            string multiPlayer = "Multiplayer";

            m_Players = new List<HumanPlayerModel>() { new HumanPlayerModel() };

            if (i_GameMode == singlePlayer)
            {
                m_ComputerPlayerAI = new ComputerPlayerAIModel(m_BoardGame);
                r_ScoreHandler.InitializeScoreHandler(m_Players[0].Name, ComputerPlayerAIModel.k_Name);
            }
            else if (i_GameMode == multiPlayer)
            {
                IsMultiplayer = true;
                m_CurrentPlayerIndx = random.Next(2);
                m_Players.Add(new HumanPlayerModel());
                r_ScoreHandler.InitializeScoreHandler(m_Players[0].Name, m_Players[1].Name);
            }
            else
            {
                string exceptionMessage = "Invalid game mode. Please specify either 'SinglePlayer' or 'Multiplayer'.";

                throw new ArgumentException(exceptionMessage);
            }
        }

        public string GetCurrentPlayerName()
        {
            string currentPlayerName = null;

            if (m_CurrentPlayerIndx == 0)
            {
                currentPlayerName = m_Players[0].Name;
            }
            else if (IsMultiplayer == true)
            {
                currentPlayerName = m_Players[1].Name;
            }
            else
            {
                currentPlayerName = "Computer";
            }

            return currentPlayerName;
        }

        public int GetBoardGameColumnLength()
        {
            return m_BoardGame.GetColumnLength();
        }

        public void InsertUserNextMoveToBoard(int i_UserNextMoveIndx)
        {
            int playerRowInsertPosition = 0;

            eGamePieceType gamePiece = m_CurrentPlayerIndx == 0 ? eGamePieceType.X : eGamePieceType.O;
            m_BoardGame.InsertUserNextMoveToBoard(gamePiece, i_UserNextMoveIndx, ref playerRowInsertPosition);

            if (m_CurrentPlayerIndx == 0)
            {
                m_Players[0].RowMoveInput = playerRowInsertPosition;
                m_Players[0].ColumnMoveInput = i_UserNextMoveIndx;
            }
            else if (IsMultiplayer == true)
            {
                m_Players[1].RowMoveInput = playerRowInsertPosition;
                m_Players[1].ColumnMoveInput = i_UserNextMoveIndx;
            }
        }

        public bool CheckForTie()
        {
            return m_BoardGame.IsTie() == true;
        }

        public bool CheckForWinner()
        {
            eGamePieceType gamePiece = eGamePieceType.O;
            int lastRowMoveInput = 0;
            int lastColumnMoveInput = 0;
            string winnerName = null;
            bool isWinner = false;

            if (m_CurrentPlayerIndx == 0)
            {
                winnerName = m_Players[0].Name;
                lastRowMoveInput = m_Players[0].RowMoveInput;
                lastColumnMoveInput = m_Players[0].ColumnMoveInput;
                gamePiece = eGamePieceType.X;
            }
            else if (IsMultiplayer == true)
            {
                winnerName = m_Players[1].Name;
                lastRowMoveInput = m_Players[1].RowMoveInput;
                lastColumnMoveInput = m_Players[1].ColumnMoveInput;
            }
            else
            {
                winnerName = ComputerPlayerAIModel.k_Name;
                lastRowMoveInput = m_ComputerPlayerAI.RowMoveInput;
                lastColumnMoveInput = m_ComputerPlayerAI.ColumnMoveInput;
            }

            isWinner = m_BoardGame.CheckIfWinner(lastRowMoveInput, lastColumnMoveInput, gamePiece);
            
            if (isWinner == true)
            {
                r_ScoreHandler.UpdateWinner(winnerName);
            }

            return isWinner;
        }

        public void UpdateNextPlayerIndex()
        {
            m_CurrentPlayerIndx = m_CurrentPlayerIndx % 2 == 0 ? 1 : 0;
        }

        public string GetWinnerMessage()
        {
            return r_ScoreHandler.WinnerMessage;
        }

        public string GetTieMessage()
        {
            return r_ScoreHandler.TieMessage;
        }

        public void StartNextRound()
        {
            m_BoardGame = new BoardGameModel(m_BoardGame.GetRowLength(), m_BoardGame.GetColumnLength());
            if (IsMultiplayer == true)
            {
                UpdateNextPlayerIndex();
            }
            else
            {
                m_CurrentPlayerIndx = 0;
                m_ComputerPlayerAI = new ComputerPlayerAIModel(m_BoardGame);
            }
        }

        public void EndGame()
        {
            IsGameRunning = false;
        }

        public void Surrender()
        {
            string winnerName = null;

            if (m_CurrentPlayerIndx != 0)
            {
                winnerName = m_Players[0].Name;
            }
            else if (IsMultiplayer == true)
            {
                winnerName = m_Players[1].Name;
            }
            else
            {
                winnerName = ComputerPlayerAIModel.k_Name;
            }

            r_ScoreHandler.UpdateWinner(winnerName);
        }

        public bool IsColumnFull(int i_UserNextMoveIndx)
        {
            return m_BoardGame.IsColumnFull(i_UserNextMoveIndx);
        }

        public bool PlayComputerAINextMove(bool i_IsHumanTurn)
        {
            if (IsMultiplayer == false && i_IsHumanTurn == false && m_CurrentPlayerIndx == 1)
            {
                int bestAIMove = m_ComputerPlayerAI.GetBestMove();

                InsertUserNextMoveToBoard(bestAIMove);
            }
            else if (IsMultiplayer == true)
            {
                i_IsHumanTurn = false;
            }

            return !i_IsHumanTurn;
        }
    }
}