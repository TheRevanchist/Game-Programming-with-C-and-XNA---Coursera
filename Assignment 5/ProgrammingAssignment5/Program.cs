using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProgrammingAssignment5
{
    /// <summary>
    /// A small program that simulates a game of War. The program does everything including printing the winner and leaves the player the
    /// possibility of continuing the game
    /// </summary>
    class Program
    {
        /// <summary>
        /// Everything lives inside the main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Declare a variable for a random number generator and create a new Random object for that variable
            Random random = new Random();

            // Declare other variables as necessary to keep track of: Each player’s roll, Each player’s number of wins, 
            // Whether or not we should play another game of War and player's symbols
            int playerOneRoll;
            int playerTwoRoll;
            int playerOneNumberOfWins;
            int playerTwoNumberOfWins;
            bool play = true;
            string playerOne = "P1";
            string playerTwo = "P2";
            string player;

            // These two variables are used only to compute the number of spaces we need to have in order 
            // for the result layout to look better. 
            string numberOfSpaces;
            string numberOfSpaces2;

            // Print a “welcome” message to the user telling them that the program will play games of War
            Console.WriteLine();
            Console.WriteLine("Welcome to the game of War! The game is starting:");
            Console.WriteLine();
            Console.WriteLine();

            // While the player wants to play another game
            while (play)
            {
                // Set both player win counts to 0
                playerOneNumberOfWins = 0;
                playerTwoNumberOfWins = 0;

                // Loop for 21 battles
                for (int i = 0; i < 21; i++)
                {
                    // Roll the die for player 1
                    playerOneRoll = random.Next(1, 14);

                    //  Roll the die for player 2
                    playerTwoRoll = random.Next(1, 14);

                    //  While the rolls have the same value (a tie)
                    while (playerOneRoll == playerTwoRoll)
                    {
                        // Print “WAR” and the die values for the two players
                        if (playerOneRoll >= 10)
                            numberOfSpaces = "   ";
                        else
                            numberOfSpaces = "    ";
                        Console.WriteLine("   WAR! " + playerOne + ":" + playerOneRoll + numberOfSpaces + playerTwo + ":" + playerTwoRoll);
                        Console.WriteLine();

                        // Roll the die for player 1
                        playerOneRoll = random.Next(1, 14);

                        // Roll the die for player 2
                        playerTwoRoll = random.Next(1, 14);
                    }

                    // increment that player’s win count
                    if (playerOneRoll > playerTwoRoll)
                    {
                        playerOneNumberOfWins++;
                        player = playerOne;
                    }
                    else
                    {
                        playerTwoNumberOfWins++;
                        player = playerTwo;
                    }

                    // Print the die values for the two players
                    if (playerOneRoll >= 10)
                        numberOfSpaces = "   ";
                    else
                        numberOfSpaces = "    ";

                    if (playerTwoRoll >= 10)
                        numberOfSpaces2 = "   ";
                    else
                        numberOfSpaces2 = "    ";

                    Console.WriteLine("BATTLE:" + " P1:" + playerOneRoll + numberOfSpaces + "P2:" + playerTwoRoll 
                        + numberOfSpaces2 + player + " Wins!");
                    Console.WriteLine();     
                }
                // Print out which player won more battles
                int whoWonMore = Math.Max(playerOneNumberOfWins, playerTwoNumberOfWins);

                if (whoWonMore == playerOneNumberOfWins)
                    player = playerOne;
                else
                    player = playerTwo;

                Console.WriteLine(player + " is the overall Winner with " + whoWonMore + " battles");
                Console.WriteLine();

                // Prompt for and get whether the player wants to play another game
                Console.Write("Do you want to play again (y/n)?");
                string read = Console.ReadLine();
                Console.WriteLine();

                if (read == "n" || read == "N")
                    play = false;

            }
        }
    }
}
