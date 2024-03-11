using System;

namespace FourInARow.Models
{
    public class ScoreHandlerModel
    {
        public string WinnerMessage { get; private set; } = null;
        public string TieMessage { get; private set; } = null;
        private int m_PlayerOneScore = 0;
        private int m_PlayerTwoScore = 0;
        private string m_PlayerOneName = null;
        private string m_PlayerTwoName = null;
        private string m_RoundWinner = null;
        private string m_ScoreStatusMessage = null;

        public void InitializeScoreHandler(string i_PlayerOneName, string i_PlayerTwoName)
        {
            m_PlayerOneName = i_PlayerOneName;
            m_PlayerTwoName = i_PlayerTwoName;
            updateScoreStatusMessage();
        }

        public void UpdateWinner(string i_PlayerName)
        {
            if (i_PlayerName == m_PlayerOneName)
            {
                m_PlayerOneScore++;
            }
            else
            {
                m_PlayerTwoScore++;
            }

            m_RoundWinner = i_PlayerName;
            updateScoreStatusMessage();
        }

        private void updateWinnerMessage()
        {
            string winnerMessage = string.Format("{0}{1}  Won!{0}{2}", Environment.NewLine, m_RoundWinner, m_ScoreStatusMessage);

            WinnerMessage = winnerMessage;
        }

        private void updateTieMessage()
        {
            string tieMessage = string.Format("{0}It's a tie!{0}{1}", Environment.NewLine, m_ScoreStatusMessage);

            TieMessage = tieMessage;
        }

        private void updateScoreStatusMessage()
        {
            string singularOrPluralPlayerOneScore = m_PlayerOneScore == 1 ? "point" : "points";
            string singularOrPluralPlayerTwoScore = m_PlayerTwoScore == 1 ? "point" : "points";
            string scoreSatusMessage = string.Format("{0}Player 1 has {1} {2}.{0}{3} has {4} {5}.",
                Environment.NewLine, m_PlayerOneScore, singularOrPluralPlayerOneScore, m_PlayerTwoName, m_PlayerTwoScore, singularOrPluralPlayerTwoScore);

            m_ScoreStatusMessage = scoreSatusMessage;
            updateWinnerMessage();
            updateTieMessage();
        }
    }
}