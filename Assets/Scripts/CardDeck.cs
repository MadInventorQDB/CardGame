using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Cardgames
{
    public class CardDeck : Stack<Card>
    {
        private int deckSeed;

        public CardDeck(int randSeed)
        {
            deckSeed = randSeed;
            UnityEngine.Random.InitState(randSeed);

            List<int> tempList = new List<int>();
            //Make 6 decks that will be mixed
            for (int i = 0; i < 6; i++)
            {
                //Each deck is a list of 52
                for (int j = 1; j <= 52; j++)
                {
                    tempList.Add(j);
                }
            }
            //Pick a random card to add to the deck from
            //the tempList until all cards are placed in the deck
            for (int i = 0; i < 312; i++)
            {
                //int cardIndex = 0;
                int cardIndex = UnityEngine.Random.Range(0, tempList.Count);
                int cardNum = tempList[cardIndex];
                tempList.RemoveAt(cardIndex);
                Push(new Card(cardNum));
            }
        }
    }
}