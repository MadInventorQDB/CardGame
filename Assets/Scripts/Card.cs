using System;

namespace Cardgames
{

    public class Card
    {
        public enum suit
        {
            Hearts,
            Diamonds,
            Clubs,
            Spades,
        }

        readonly int cardValue;
        readonly suit cardSuit;

        readonly int deckNumber;

        /// <summary>
        /// The value of the card with face cards as 10
        /// </summary>
        public int BlackJackValue 
        {
            get
            { 
                return Math.Min(cardValue, 10);
            }
        }

        public int Value
        {
            get
            { 
                return cardValue;
            }
        }

        public suit Suit
        {
            get => cardSuit;
        }

        /// <summary>
        /// Make a number into a card
        /// </summary>
        /// <param name="cardNumber">Take a number and makes a suit ordered card</param>
        public Card(int cardNumber)
        {
            deckNumber = cardNumber;

            //11 = J 12 = Q 13 = K 
            cardValue = cardNumber % 13 == 0 ? 13 : cardNumber % 13;
            //Convert Suit
            cardSuit = cardNumber switch
            {
                <= 13 => suit.Hearts,
                <= 26 => suit.Diamonds,
                <= 39 => suit.Clubs,
                <= 52 => suit.Spades,
                _ => throw new ArgumentException("Invalid card number")
            };
        }

        public override string ToString()
        {
            string valueString = cardValue switch
            {
                1 => "A",
                11 => "J",
                12 => "Q",
                13 => "K",
                _ => cardValue.ToString()
            };

            string suitString = cardSuit switch
            {
                suit.Hearts => "♥",
                suit.Diamonds => "♦",
                suit.Clubs => "♣",
                suit.Spades => "♠",
                _ => throw new ArgumentException()
            };
            return $"{valueString}{suitString}";
        }
    }
}