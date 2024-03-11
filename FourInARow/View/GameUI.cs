using FourInARow.Logic;
using System;

namespace FourInARow.View
{
    public class GameUI
    {
        private readonly GameLogic r_GameLogic = new GameLogic();

        public GameUI()
        {
            initializeBoardGame();
            initializeGameMode();
            startGame();
        }
        
        private void initializeBoardGame()
        {
            int validUserInputValidRowLength = 0;
            int validUserInputColumnLength = 0;
            string boardGameSetupMessage = string.Format("Please determine the size of the board game ({0}x{0} - {1}x{1}).",
                GameLogic.k_BoardGameMinLength, GameLogic.k_BoardGameMaxLength);

            Console.WriteLine(boardGameSetupMessage);
            Console.Write("Enter a desired row length: ");
            validUserInputValidRowLength = getValidUserBoardGameSizeInput();
            Console.Write("Enter a desired column length: ");
            validUserInputColumnLength = getValidUserBoardGameSizeInput();

            try
            {
                r_GameLogic.InitializeBoardGame(validUserInputValidRowLength, validUserInputColumnLength);
            }
            catch (ArgumentException ex)
            {
                string exceptionMessage = string.Format("Invalid input: ", ex.Message);

                Console.WriteLine(exceptionMessage);
            }
        }

        private void initializeGameMode()
        {
            string gameModeMessage = "For single player press 's', for multiplayer press 'm': ";
            string gameMode = null;

            Console.Write(gameModeMessage);
            gameMode = getValidUserGameModeInput();

            try
            {
                r_GameLogic.SetGameMode(gameMode);
            }
            catch (Exception ex)
            {
                string exceptionMessage = string.Format("Invalid input: ", ex.Message);

                Console.WriteLine(exceptionMessage);
            }
        }

        private void startGame()
        {
            bool isTie = true;
            bool hasWinner = true;
            bool isSurrendered = false;
            bool isHumanTurn = true;

            while (r_GameLogic.IsGameRunning == true)
            {
                printBoardGame();

                if (r_GameLogic.IsMultiplayer == true)
                {
                    isSurrendered = isSurrenderedOrDoNextMove();
                }
                else
                {
                    if (isHumanTurn == true)
                    {
                        isSurrendered = isSurrenderedOrDoNextMove();
                    }

                    isHumanTurn = r_GameLogic.PlayComputerAINextMove(isHumanTurn);
                }

                printBoardGame();
                hasWinner = r_GameLogic.CheckForWinner();
                isTie = r_GameLogic.CheckForTie();

                if (hasWinner == true || isSurrendered == true)
                {
                    string winnerMessage = r_GameLogic.GetWinnerMessage();

                    Console.WriteLine(winnerMessage);
                    setAnotherRoundOrEndGame();
                    isHumanTurn = true;
                }
                else if (isTie == true)
                {
                    string tieMessage = r_GameLogic.GetTieMessage();

                    Console.WriteLine(tieMessage);
                    setAnotherRoundOrEndGame();
                    isHumanTurn = true;
                }
                else
                {
                    r_GameLogic.UpdateNextPlayerIndex();
                }
            }
        }

        private void setAnotherRoundOrEndGame()
        {
            bool doesPlayerWantsAnotherRound = checkIfPlayerWantsAnotherRound();

            if (doesPlayerWantsAnotherRound == true)
            {
                r_GameLogic.StartNextRound();
            }
            else
            {
                r_GameLogic.EndGame();
            }
        }

        private bool checkIfPlayerWantsAnotherRound()
        {
            string userInput = null;
            string startNewRound = "y";
            string dontStartNewRound = "n";
            string rematchMessage = "Would you like to start a new round? y/n: ";

            Console.WriteLine();
            Console.Write(rematchMessage);
            userInput = Console.ReadLine();

            while (userInput != startNewRound && userInput != dontStartNewRound)
            {
                string invalidInputMessage = string.Format("{0}ERROR - Invalid input!{0}{1}", Environment.NewLine, rematchMessage);

                Console.Write(invalidInputMessage);
                userInput = Console.ReadLine();
            }

            return userInput == startNewRound;
        }

        private bool isSurrenderedOrDoNextMove()
        {
            int userNextMoveIndx = 0;
            int surrender = -1;
            bool isUserSurrendered = false;
            string playerName = r_GameLogic.GetCurrentPlayerName();
            string userNextMoveMessage = string.Format("{0}{1}{0}Please choose a column number to place your checker: ", Environment.NewLine, playerName);

            Console.Write(userNextMoveMessage);
            userNextMoveIndx = getValidUserNextMoveInput();
            isUserSurrendered = userNextMoveIndx == surrender;

            if (isUserSurrendered == false)
            {
                try
                {
                    r_GameLogic.InsertUserNextMoveToBoard(userNextMoveIndx);
                }
                catch (Exception ex)
                {
                    string exceptionMessage = string.Format("{0}Invalid input: {1}", Environment.NewLine, ex.Message);

                    Console.WriteLine(exceptionMessage);
                }
            }
            else
            {
                r_GameLogic.Surrender();
            }

            return isUserSurrendered;
        }

        private int getValidUserNextMoveInput()
        {
            int validUserInput = 0;
            int boardGameRowLength = r_GameLogic.GetBoardGameColumnLength();
            bool isUserInputValid = false;
            bool isColumnFull = false;

            while (isUserInputValid == false)
            {
                ConsoleKeyInfo userNextMoveIndxInput = Console.ReadKey();

                isUserInputValid = int.TryParse(userNextMoveIndxInput.KeyChar.ToString(), out validUserInput);
                isUserInputValid = isUserInputValid == true && validUserInput > 0 && validUserInput <= boardGameRowLength;

                if (isUserInputValid == true)
                {
                    isColumnFull = r_GameLogic.IsColumnFull(validUserInput - 1);
                }

                if (userNextMoveIndxInput.Key == ConsoleKey.Q)
                {
                    validUserInput = 0;
                    isUserInputValid = true;
                }
                else if (isColumnFull == true)
                {
                    string columnIsFullErrorMessage = string.Format("{0}{0}ERROR - Invalid input!{0}The column is full!{0}Enter the desired column number to insert your checker: ",
                        Environment.NewLine);

                    Console.Write(columnIsFullErrorMessage);
                    isUserInputValid = false;
                    isColumnFull = false;
                }
                else if (isUserInputValid == false)
                {
                    string boardGameRowRange = $"1 - {boardGameRowLength}";
                    string userInputOutOfRangeErrorMessage = string.Format("{0}{0}ERROR - Invalid input!{0}The column number should be between {1}.{0}Enter the desired column number to insert your checker: ",
                        Environment.NewLine, boardGameRowRange);

                    Console.Write(userInputOutOfRangeErrorMessage);
                }
            }

            return validUserInput - 1;
        }

        private void printBoardGame()
        {
            string boardGame = r_GameLogic.BoardGameToString();

            Console.Clear();
            Console.Write(boardGame);
        }

        private int getValidUserBoardGameSizeInput()
        {
            int validUserInput = 0;
            int boardGameMinLength = GameLogic.k_BoardGameMinLength;
            int boardGameMaxLength = GameLogic.k_BoardGameMaxLength;
            string userInput = Console.ReadLine();
            bool isUserInputValid = int.TryParse(userInput, out validUserInput);

            while (isUserInputValid == false || validUserInput < boardGameMinLength || validUserInput > boardGameMaxLength)
            {
                string invalidInputErrorMessage = string.Format("{0}Error - Invalid input!{0}The length should be between {1} - {2}.{0}Enter the desired length: ",
                    Environment.NewLine, boardGameMinLength, boardGameMaxLength);

                Console.Write(invalidInputErrorMessage);
                userInput = Console.ReadLine();
                isUserInputValid = int.TryParse(userInput, out validUserInput);
            }

            return validUserInput;
        }

        private string getValidUserGameModeInput()
        {
            ConsoleKeyInfo userInput = Console.ReadKey();
            ConsoleKey singlePlayer = ConsoleKey.S;
            ConsoleKey multiplayer = ConsoleKey.M;
            string gameModeMessage = "For single player press 's', for multiplayer press 'm': ";

            Console.Write(gameModeMessage);

            while (userInput.Key != singlePlayer && userInput.Key != multiplayer)
            {
                string invalidInputErrorMessage = string.Format("{0}{0}Error - Invalid input!{0}For single player press 's', for multiplayer press 'm': ", Environment.NewLine);

                Console.Write(invalidInputErrorMessage);
                userInput = Console.ReadKey();
            }

            return userInput.Key == singlePlayer ? "SinglePlayer" : "Multiplayer";
        }
    }
}