using System;
using System.Linq;
using System.Text;
using FourInARow.Enums;

namespace FourInARow.Models
{
    public class BoardGameModel
    {
        private Nullable<eGamePieceType>[,] m_Board = null;

        public BoardGameModel(int i_RowLength, int i_ColumnLength)
        {
            m_Board = new Nullable<eGamePieceType>[i_RowLength, i_ColumnLength];
        }

        public bool IsColumnFull(int i_UserNextMoveIndx)
        {
            return m_Board[0, i_UserNextMoveIndx].HasValue == true;
        }

        public void InsertUserNextMoveToBoard(eGamePieceType i_GamePiece, int i_UserNextMoveIndx, ref int o_PlayerRowInsertPosition)
        {
            if (i_UserNextMoveIndx >= 0 && i_UserNextMoveIndx <= m_Board.GetLength(1) - 1)
            {
                for (int i = m_Board.GetLength(0) - 1; i >= 0; i--)
                {
                    if (m_Board[i, i_UserNextMoveIndx].HasValue == false)
                    {
                        m_Board[i, i_UserNextMoveIndx] = i_GamePiece;
                        o_PlayerRowInsertPosition = i;
                        break;
                    }
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid value.");
            }
        }

        public int GetRowLength()
        {
            return m_Board.GetLength(0);
        }

        public int GetColumnLength()
        {
            return m_Board.GetLength(1);
        }

        public bool IsTie()
        {
            bool isTopRowFull = true;

            for (int i = 0; i < m_Board.GetLength(1); i++)
            {
                if (m_Board[0, i].HasValue == false)
                {
                    isTopRowFull = false;
                    break;
                }
            }

            return isTopRowFull == true;
        }

        public bool CheckIfWinner(int i_LastRowMoveInput, int i_LastColumnMoveInput, eGamePieceType i_GamePiece)
        {
            bool isRowWin = checkForRowWin(i_LastRowMoveInput, i_LastColumnMoveInput, i_GamePiece);
            bool isColumnWin = checkForColumnWin(i_LastRowMoveInput, i_LastColumnMoveInput, i_GamePiece);
            bool isLeftDiagonalWin = checkForLeftDiagonalWin(i_LastRowMoveInput, i_LastColumnMoveInput, i_GamePiece);
            bool isRightDiagonalWin = checkForRightDiagonalWin(i_LastRowMoveInput, i_LastColumnMoveInput, i_GamePiece);

            return isRowWin || isColumnWin || isLeftDiagonalWin || isRightDiagonalWin;
        }

        public override string ToString()
        {
            StringBuilder boardGameHeader = new StringBuilder(Environment.NewLine);
            StringBuilder boardGameBody = new StringBuilder(Environment.NewLine);
            int boardRowLength = m_Board.GetLength(0);
            int boardColumnLength = m_Board.GetLength(1);
            string cellValue = null;
            string rowNumber = null;
            string boardSeperatorButtomLine = string.Concat(Enumerable.Repeat("=", boardColumnLength * 4 + 1));

            for (int i = 0; i < boardColumnLength; i++)
            {
                rowNumber = string.Format("  {0} ", i + 1);
                boardGameHeader.Append(rowNumber);
            }

            for (int i = 0; i < boardRowLength; i++)
            {
                for (int j = 0; j < boardColumnLength; j++)
                {
                    string checker = m_Board[i, j] == null ? " " : m_Board[i, j].ToString();

                    cellValue = string.Format("| {0} ", checker);
                    boardGameBody.Append(cellValue);
                }

                boardGameBody.AppendLine("|");
                boardGameBody.AppendLine(boardSeperatorButtomLine);
            }

            return boardGameHeader.ToString() + boardGameBody.ToString();
        }

        public eGamePieceType?[,] GetBoardGame()
        {
            return m_Board;
        }

        private bool checkForRowWin(int i_LastRowMoveInput, int i_LastColumnMoveInput, eGamePieceType i_GamePiece)
        {
            bool isFourInARow = false;
            int neighborsCounter = 1;

            for (int i = i_LastColumnMoveInput + 1; i <= i_LastColumnMoveInput + 3; i++)
            {
                if (i < m_Board.GetLength(1))
                {
                    if (m_Board[i_LastRowMoveInput, i].HasValue && m_Board[i_LastRowMoveInput, i].Value == i_GamePiece)
                    {
                        neighborsCounter++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

                if (neighborsCounter == 4)
                {
                    isFourInARow = true;
                    break;
                }
            }

            for (int i = i_LastColumnMoveInput - 1; i >= i_LastColumnMoveInput - 3; i--)
            {
                if (i >= 0)
                {
                    if (m_Board[i_LastRowMoveInput, i].HasValue && m_Board[i_LastRowMoveInput, i].Value == i_GamePiece)
                    {
                        neighborsCounter++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

                if (neighborsCounter == 4)
                {
                    isFourInARow = true;
                    break;
                }
            }

            return isFourInARow;
        }

        private bool checkForColumnWin(int i_LastRowMoveInput, int i_LastColumnMoveInput, eGamePieceType i_GamePiece)
        {
            bool isFourInARow = false;
            int neighborsCounter = 1;

            for (int i = i_LastRowMoveInput + 1; i <= i_LastRowMoveInput + 3; i++)
            {
                if (i < m_Board.GetLength(0))
                {
                    if (m_Board[i, i_LastColumnMoveInput].HasValue && m_Board[i, i_LastColumnMoveInput].Value == i_GamePiece)
                    {
                        neighborsCounter++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

                if (neighborsCounter == 4)
                {
                    isFourInARow = true;
                    break;
                }
            }

            for (int i = i_LastRowMoveInput - 1; i >= i_LastRowMoveInput - 3; i--)
            {
                if (i >= 0)
                {
                    if (m_Board[i, i_LastColumnMoveInput].HasValue && m_Board[i, i_LastColumnMoveInput].Value == i_GamePiece)
                    {
                        neighborsCounter++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

                if (neighborsCounter == 4)
                {
                    isFourInARow = true;
                    break;
                }
            }

            return isFourInARow;
        }

        private bool checkForRightDiagonalWin(int i_LastRowMoveInput, int i_LastColumnMoveInput, eGamePieceType i_GamePiece)
        {
            bool isFourInARow = false;
            int neighborsCounter = 1;
            int i = 0;
            int j = 0;

            for (i = i_LastRowMoveInput - 1, j = i_LastColumnMoveInput + 1; i >= i_LastRowMoveInput - 3 && j <= i_LastColumnMoveInput + 3; i--, j++)
            {
                if (i >= 0 && j < m_Board.GetLength(1))
                {
                    if (m_Board[i, j].HasValue && m_Board[i, j].Value == i_GamePiece)
                    {
                        neighborsCounter++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

                if (neighborsCounter == 4)
                {
                    isFourInARow = true;
                    break;
                }
            }

            for (i = i_LastRowMoveInput + 1, j = i_LastColumnMoveInput - 1; i <= i_LastRowMoveInput + 3 && j >= i_LastColumnMoveInput - 3; i++, j--)
            {
                if (i < m_Board.GetLength(0) && j >= 0)
                {
                    if (m_Board[i, j].HasValue && m_Board[i, j].Value == i_GamePiece)
                    {
                        neighborsCounter++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

                if (neighborsCounter == 4)
                {
                    isFourInARow = true;
                    break;
                }
            }

            return isFourInARow;
        }

        private bool checkForLeftDiagonalWin(int i_LastRowMoveInput, int i_LastColumnMoveInput, eGamePieceType i_GamePiece)
        {
            bool isFourInARow = false;
            int neighborsCounter = 1;
            int i = 0;
            int j = 0;

            for (i = i_LastRowMoveInput - 1, j = i_LastColumnMoveInput - 1; i >= i_LastRowMoveInput - 3 && j >= i_LastColumnMoveInput - 3; i--, j--)
            {
                if (i >= 0 && j >= 0)
                {
                    if (m_Board[i, j].HasValue && m_Board[i, j].Value == i_GamePiece)
                    {
                        neighborsCounter++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

                if (neighborsCounter == 4)
                {
                    isFourInARow = true;
                    break;
                }
            }

            for (i = i_LastRowMoveInput + 1, j = i_LastColumnMoveInput + 1; i <= i_LastRowMoveInput + 3 && j <= i_LastColumnMoveInput + 3; i++, j++)
            {
                if (i < m_Board.GetLength(0) && j < m_Board.GetLength(1))
                {
                    if (m_Board[i, j].HasValue && m_Board[i, j].Value == i_GamePiece)
                    {
                        neighborsCounter++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

                if (neighborsCounter == 4)
                {
                    isFourInARow = true;
                    break;
                }
            }

            return isFourInARow;
        }
    }
}