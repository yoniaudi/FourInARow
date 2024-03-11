using System;
using FourInARow.Enums;

namespace FourInARow.Models
{
    public class ComputerPlayerAIModel
    {
        public const string k_Name = "Computer AI";
        public int RowMoveInput { get; set; } = 0;
        public int ColumnMoveInput { get; set; } = 0;
        private BoardGameModel m_BoardGame = null;
        private Nullable<eGamePieceType>[,] m_Board = null;

        public ComputerPlayerAIModel(BoardGameModel i_BoardGame)
        {
            m_BoardGame = i_BoardGame;
            m_Board = m_BoardGame.GetBoardGame();
        }

        public int GetBestMove()
        {
            int bestMove = -1;
            int bestScore = int.MinValue;

            for (int column = 0; column < m_Board.GetLength(1); column++)
            {
                int row = 0;
                int score = 0;

                if (isColumnFull(column))
                {
                    continue;
                }

                row = getEmptyRow(column);
                m_Board[row, column] = eGamePieceType.O;
                score = evaluateBoard(m_BoardGame, eGamePieceType.O);
                m_Board[row, column] = null;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = column;
                    RowMoveInput = row;
                    ColumnMoveInput = bestMove;
                }
            }

            return bestMove;
        }

        private bool isColumnFull(int i_Column)
        {
            return m_Board[0, i_Column].HasValue;
        }

        private int getEmptyRow(int i_Column)
        {
            int row = -1;

            for (int i = m_Board.GetLength(0) - 1; i >= 0; i--)
            {
                if (m_Board[i, i_Column].HasValue == false)
                {
                    row = i;
                    break;
                }
            }

            return row;
        }

        private int evaluateBoard(BoardGameModel i_GameBoard, eGamePieceType i_ComputerPlayerGamePiece)
        {
            eGamePieceType opponentGamePiece = eGamePieceType.X;
            int score = 0;
            bool isOpponentTurn = true;

            score += evaluatePotentialWins(i_GameBoard, i_ComputerPlayerGamePiece, !isOpponentTurn);
            score -= evaluatePotentialWins(i_GameBoard, opponentGamePiece, isOpponentTurn);

            return score;
        }

        private int evaluatePotentialWins(BoardGameModel i_Board, eGamePieceType i_Player, bool i_IsOpponent)
        {
            int score = 0;

            score += evaluateDirection(i_Player, 0, 1, i_IsOpponent);
            score += evaluateDirection(i_Player, 0, -1, i_IsOpponent);
            score += evaluateDirection(i_Player, 1, 0, i_IsOpponent);
            score += evaluateDirection(i_Player, 1, 1, i_IsOpponent);
            score += evaluateDirection(i_Player, -1, -1, i_IsOpponent);
            score += evaluateDirection(i_Player, 1, -1, i_IsOpponent);
            score += evaluateDirection(i_Player, -1, 1, i_IsOpponent);

            return score;
        }

        private int evaluateDirection(eGamePieceType i_Player, int i_RowDirection, int i_ColDirection, bool i_IsOpponent)
        {
            int score = 0;

            for (int row = 0; row < m_Board.GetLength(0); row++)
            {
                for (int col = 0; col < m_Board.GetLength(1); col++)
                {
                    int count = 0;
                    int emptyCount = 0;

                    for (int i = 0; i < 4; i++)
                    {
                        int newRow = row + i * i_RowDirection;
                        int newCol = col + i * i_ColDirection;

                        if (newRow >= 0 && newRow < m_Board.GetLength(0) && newCol >= 0 && newCol < m_Board.GetLength(1))
                        {
                            if (m_Board[newRow, newCol] == i_Player)
                            {
                                count++;
                            }
                            else if (m_Board[newRow, newCol] == null)
                            {
                                emptyCount++;
                            }
                            else
                            {
                                count = 0;
                                emptyCount = 0;
                                break;
                            }
                        }
                        else
                        {
                            count = 0;
                            emptyCount = 0;
                            break;
                        }
                    }

                    if (i_IsOpponent == false)
                    {
                        score += countToScore(count, emptyCount);
                    }
                    else
                    {
                        score += countToScoreForOpponent(count, emptyCount);
                    }
                }
            }

            return score;
        }

        private int countToScore(int i_Count, int i_EmptyCount)
        {
            int score = 0;

            switch (i_Count)
            {
                case 1:
                    score = i_EmptyCount == 3 ? 1 : 0;
                    break;
                case 2:
                    score = i_EmptyCount == 2 ? 10 : 0;
                    break;
                case 3:
                    score = i_EmptyCount == 1 ? 100 : 0;
                    break;
                case 4:
                    score = 500;
                    break;
                default:
                    score = 0;
                    break;
            }

            return score;
        }

        private int countToScoreForOpponent(int i_Count, int i_EmptyCount)
        {
            int score = 0;

            switch (i_Count)
            {
                case 1:
                    score = i_EmptyCount == 3 ? 1 : 0;
                    break;
                case 2:
                    score = i_EmptyCount == 2 ? 10 : 0;
                    break;
                case 3:
                    score = i_EmptyCount == 1 ? 500 : 0;
                    break;
                case 4:
                    score = 1000;
                    break;
                default:
                    score = 0;
                    break;
            }

            return score;
        }
    }
}