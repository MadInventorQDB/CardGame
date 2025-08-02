using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cardgames
{
    public class CardHand : List<Card>
    {
        [Flags]
        public enum Options
        { 
            Deal = 2,
            Hit = 4,
            Stand = 8,
            DoubleDown = 16,
            Split = 32,
            TwentyOne = 64,
            BlackJack = 128,
            Bust = 256,
        }


        public CardHand()
        {
        }

        public void AddCard(Card card)
        { 
            Add(card);    
        }


        public CardHand Split()
        {
            CardHand newHand = new CardHand();
            newHand.Add(this[1]);
            RemoveAt(1);
            return newHand;
        }

        public int Score
        {
            get
            {
                int score = 0;

                if (Count == 0)
                {
                    return score;
                }

                int aceCount = 0;
                // Handle non-ace cards first
                for (int i = 0; i < Count; i++)
                {
                    // Skip aces for now, but count them
                    if (this[i].BlackJackValue == 1)
                    {
                        aceCount++;
                        continue;
                    }

                    score += this[i].BlackJackValue;
                }

                // Now, handle the aces strategically
                for (int i = 0; i < aceCount; i++)
                {
                    // If we can add 11 without busting, do it
                    if (score + 11 <= 21)
                    {
                        score += 11;
                    }
                    else
                    {
                        // Otherwise, the ace must be 1
                        score += 1;
                    }
                }

                return score;
            }
        }

        public Options GetOptions()
        {
            Options options = Options.Deal;

            if (Score == 0 || Count == 1)
            {
                return options;
            }

            if (Score <= 20)
            {
                options = Options.Hit | Options.Stand;

            }

            if (Count == 2)
            {
                options |= Options.DoubleDown;

                if (this[0].Value == this[1].Value)
                { 
                    options |= Options.Split;
                }
            }

            if (Score > 21)
            {
                options = Options.Bust;
            }

            if (Score == 21)
            {
                if (Count == 2)
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