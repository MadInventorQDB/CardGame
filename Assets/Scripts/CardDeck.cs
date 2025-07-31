using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Cardgames
{
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
}