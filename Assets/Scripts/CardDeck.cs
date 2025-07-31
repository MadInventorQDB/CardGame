using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck
{
    private int deckSeed;

    //An ordered list of cards with the 0 index as the top card.
    private List<Card> cardList; 

    public CardDeck(int randSeed)
    {
        deckSeed = randSeed;
        UnityEngine.Random.InitState(randSeed);

        //Start with an empty deck
        cardList = new List<Card>();

        List<int> tempList = new List<int>();
        //We start with a list of 52
        for (int i = 1; i <= 52; i++)
        { 
            tempList.Add(i);
        }
        //Pick a random card to add to the deck from
        //the tempList until all cards are placed in the deck
        for (int i = 0; i < 52; i++)
        {
            //int cardIndex = 0;
            int cardIndex = UnityEngine.Random.Range(0, tempList.Count);
            int cardNum = tempList[cardIndex];
            tempList.RemoveAt(cardIndex);
            cardList.Add(new Card(cardNum));
        }
    }
}

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
            1 => "Ace",
            11 => "Jack",
            12 => "Queen",
            13 => "King",
            _ => cardValue.ToString()
        };
        return $"{valueString} of {cardSuit}";
    }
}