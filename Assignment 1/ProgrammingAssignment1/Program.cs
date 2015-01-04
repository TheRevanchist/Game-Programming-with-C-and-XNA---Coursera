using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleCards;

namespace ProgrammingAssignment3
{
    /// <summary>
    /// The class that implements ProgrammingAssignment3 as required by the specifications
    /// </summary>
    class Program
    {
        /// <summary>
        /// The entire code lives inside the main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            /* Declare variables for and create a deck of cards and blackjack hands 
             * for the dealer and the player */
            Deck deck = new Deck();
            BlackjackHand blackjackHandDealer = new BlackjackHand("Dealer");
            BlackjackHand blackjackHandPlayer = new BlackjackHand("Player");

            // The welcome message
            Console.WriteLine("Welcome to the game! The program will play a single hand of Blackjack with you.");
            Console.WriteLine();

            // Shuffle the deck of cards
            deck.Shuffle();

            // Deal 2 cards to the player and dealer
            blackjackHandDealer.AddCard(deck.TakeTopCard());
            blackjackHandDealer.AddCard(deck.TakeTopCard());
            blackjackHandPlayer.AddCard(deck.TakeTopCard());
            blackjackHandPlayer.AddCard(deck.TakeTopCard());

            // Make all the player’s cards face up 
            blackjackHandPlayer.ShowAllCards();

            // Make the dealer’s first card face up (the second card is the dealer’s “hole” card)
            blackjackHandDealer.ShowFirstCard();

            //Print both the player’s hand and the dealer’s hand
            blackjackHandPlayer.Print();
            blackjackHandDealer.Print();
            Console.WriteLine();

            // Let the player hit if they want to
            blackjackHandPlayer.HitOrNot(deck);
            Console.WriteLine();

            // Make all the dealer’s cards face up; there's a method for this in the BlackjackHand class
            blackjackHandDealer.ShowAllCards();

            // Print both the player’s hand and the dealer’s hand
            blackjackHandPlayer.Print();
            blackjackHandDealer.Print();
            Console.WriteLine();

            // Print the scores for both hands
            int playerScore = blackjackHandPlayer.Score;
            int dealerScore = blackjackHandDealer.Score;
            Console.WriteLine("The player's score is " + playerScore);
            Console.WriteLine("The dealer's score is " + dealerScore);
            Console.WriteLine();
        }
    }
}
