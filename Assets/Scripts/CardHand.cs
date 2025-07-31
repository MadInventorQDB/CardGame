using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cardgames
{
    public class CardHand
    {
        enum Options
        { 
            Deal = 0,
            Hit = 2,
            Stand = 4,
            DoubleDown = 8,
            Split = 16,
        }

        List<Card> cardList;

        public CardHand()
        {
            cardList = new List<Card>();
        }

        public void AddCard(Card card)
        { 
            cardList.Add(card);    
        }

        /*
        public int Score()
        {
            int score = 0;

            if (cardList.Count == 0)
            {

            }
    

            int aceCount = 0;
            //Handle non-ace
            for (int i = 0; i < cardList.Count; i++)
            {
                //Skip aces
                if (cardList[i].value == 1)
                {
                    aceCount++;
                    continue;
                }

                score += cardList[i].value; 
            }

            //handle aces
            for (int i = 0; i < aceCount; i++)
            {
                throw NotImplementedException();
            }

            return score;
        }

        public Options GetOptions()
        { 
            
        }
        */

        
    }
}