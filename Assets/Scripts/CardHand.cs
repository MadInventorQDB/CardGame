using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cardgames
{
    public class CardHand
    {
        [Flags]
        public enum Options
        { 
            Deal = 0,
            Hit = 2,
            Stand = 4,
            DoubleDown = 8,
            Split = 16,
            TwentyOne = 32,
            BlackJack = 64,
            Bust = 128,
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


        public CardHand Split()
        {
            CardHand newHand = new CardHand();
        }

        public void ClearHand() => cardList.Clear();
        
        public int Score()
        {
            int score = 0;

            if (cardList.Count == 0)
            {
                return score;
            }
    

            int aceCount = 0;
            //Handle non-ace
            for (int i = 0; i < cardList.Count; i++)
            {
                //Skip aces
                if (cardList[i].BlackJackValue == 1)
                {
                    aceCount++;
                    continue;
                }

                score += cardList[i].BlackJackValue; 
            }

            //handle Aces
            for (int i = 0; i < aceCount; i++)
            {
                if (score + 11 <= 21)
                {
                    score += 11;
                }
                else
                {
                    score += 1;
                }
            }

            return score;
        }

        public Options GetOptions()
        {
            Options options = Options.Deal;

            if (Score() == 0)
            {
                return options;
            }

            if (Score() <= 20)
            {
                options = Options.Hit | Options.Stand;

            }

            if (cardList.Count == 2)
            {
                options |= Options.DoubleDown;

                if (cardList[0].Value == cardList[1].Value)
                { 
                    options |= Options.Split;
                }
            }

            if (Score() > 21)
            {
                options = Options.Bust;
            }

            if (Score() == 21)
            {
                if (cardList.Count == 2)
                {
                    options = Options.BlackJack;
                }
                else
                {
                    options = Options.TwentyOne;
                }
            }

            return options;
        }
        

        
    }
}