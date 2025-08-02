using Cardgames;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BlackJackGameState : MonoBehaviour
{
    //Singleton
    private static BlackJackGameState instance;

    CardDeck cardDeck;
    List<CardHand> playerHands;
    List<Double> playerBets;
    int playerHandIndex;
    CardHand dealerHand;
    double playerCash;
    double currentBet;
    
    bool dealersTurn;
    bool settleBets;

    //UI Elements
    BlackJackToggleCardsScript blackJackToggleCardsScript;
    BlackJackPlayerScript blackJackPlayerScript;

    public static BlackJackGameState Instance
    {
        get
        {
            if (instance == null)
            { 
                instance = FindAnyObjectByType<BlackJackGameState>();

                if (instance == null)
                { 
                    GameObject singletonGameObject = new GameObject(typeof(BlackJackGameState).Name);
                    instance = singletonGameObject.AddComponent<BlackJackGameState>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeGameState();

        blackJackToggleCardsScript = FindAnyObjectByType<BlackJackToggleCardsScript>();
        blackJackPlayerScript = FindAnyObjectByType<BlackJackPlayerScript>();

        blackJackPlayerScript.PlayerActionEvent += BlackJackPlayerScript_PlayerActionEvent;

    }

    private void InitializeGameState()
    {
        playerHandIndex = 0;
        dealersTurn = false;
        settleBets = false;

        currentBet = 50;
        playerCash = 1000;

        int pjb = 11012006;
        int lob = 11022004;
        int jab = 10071972;

        int randSeed = jab + lob + pjb;

        cardDeck = new CardDeck(randSeed);
        playerHands = new List<CardHand>() { new CardHand() };
        dealerHand = new CardHand();
    }

    private void BlackJackPlayerScript_PlayerActionEvent(BlackJackPlayerScript.PlayerAction playerAction)
    {
        if (playerAction == BlackJackPlayerScript.PlayerAction.Bet)
        {
            HandleBet();
        }

        if (playerAction == BlackJackPlayerScript.PlayerAction.BetMinus)
        {
            HandleBetMinus();
        }

        if (playerAction == BlackJackPlayerScript.PlayerAction.BetPlus)
        {
            HandleBetPlus();
        }


        if (playerAction == BlackJackPlayerScript.PlayerAction.DealOrHit)
        {
            HandleDealOrHit();
        }

        if (playerAction == BlackJackPlayerScript.PlayerAction.Stand)
        {
            HandleStand();
        }

        if (playerAction == BlackJackPlayerScript.PlayerAction.DoubleDown)
        {
            HandleDoubleDown();
        }


        UpdateUIFromGameState(playerHands[playerHandIndex].GetOptions());
    }

    private void HandleBetPlus()
    {
        currentBet += 50;
    }

    private void HandleBetMinus()
    {
        currentBet -= 50;
    }

    private void HandleDoubleDown()
    {
        playerCash -= currentBet;
        playerBets[playerHandIndex] += currentBet;

        var card = cardDeck.Pop();
        blackJackToggleCardsScript.AddCard(card);
        playerHands[playerHandIndex].Add(card);

        HandleStand();

    }

    private void HandleBet()
    {
        playerCash -= currentBet;
        playerBets.Add(currentBet);
    }

    private void HandleStand()
    {
        if (playerHandIndex + 1 < playerHands.Count)
        {
            playerHandIndex++;
            return;
        }

        if (playerHandIndex + 1 == playerHands.Count)
        {
            //The player has finished and it is time for the dealer
            //to player out the hand.
            dealersTurn = true;
            //We can reveal the hidden cards.
            blackJackToggleCardsScript.RevealDealerCard();
        }
    }

    private void HandleDealOrHit()
    {
        Card card;

        if (playerHands[playerHandIndex].Count > 1)
        {
            card = cardDeck.Pop();
            blackJackToggleCardsScript.AddCard(card);
            playerHands[playerHandIndex].Add(card);
        }

        if (playerHands[playerHandIndex].Count == 0)
        {
            card = cardDeck.Pop();
            blackJackToggleCardsScript.AddCard(card);
            playerHands[playerHandIndex].Add(card);

            if (dealerHand.Count == 0)
            {
                card = cardDeck.Pop();
                blackJackToggleCardsScript.AddCard(card, true);
                dealerHand.Add(card);
            }
        }

        if (playerHands[playerHandIndex].Count == 1)
        {
            card = cardDeck.Pop();
            blackJackToggleCardsScript.AddCard(card);
            playerHands[playerHandIndex].Add(card);
        }

        if (dealerHand.Count == 1)
        {
            card = cardDeck.Pop();
            blackJackToggleCardsScript.AddCard(card);
            dealerHand.Add(card);
        }
    }

    private void UpdateUIFromGameState(CardHand.Options options)
    {
        blackJackPlayerScript.UpdateUI(options);
    }

    // Update is called once per frame
    void Update()
    {
        //Figure out which hand is the current hand? playerHands[currentHand]
        //Update the UI for the current hand?
        if (dealersTurn == true)
        {
            FollowDealerRulesForDealerHand();
        }

        if (settleBets = true)
        {
            SettleBetsHandByHand();
        }

    }

    private void SettleBetsHandByHand()
    {
        //I'm sure we want to do something here to tell the player wins and losses on each hand.
        for (int i = 0; i < playerHands.Count; i++)
        {
            CardHand hand = playerHands[i];
            SettlePlayerVsDealerHand(hand, dealerHand, i);
        }
    }

    private void SettlePlayerVsDealerHand(CardHand hand, CardHand dealerHand, int betIndex)
    {
        if (hand.GetOptions() == CardHand.Options.Bust)
        {
            return;
        }
        if ((hand.GetOptions() & CardHand.Options.BlackJack) == CardHand.Options.BlackJack)
        {
            //As long as the dealer doesn't have a Black Jack then player won
            if ((dealerHand.GetOptions() & CardHand.Options.BlackJack) == CardHand.Options.BlackJack)
            {
                //Tie Push
                playerCash += playerBets[betIndex];
            }
            else 
            {
                //BlackJack pays 3 to 2
                playerCash += playerBets[betIndex] + playerBets[betIndex] * 1.5;
            }
        }

        if ((hand.GetOptions() &
            (CardHand.Options.Stand|
             CardHand.Options.TwentyOne)) > 0)
        {
            if (dealerHand.GetOptions() == CardHand.Options.Bust)
            {
                //Winner
                playerCash += playerBets[betIndex] * 2;
            }
            else
            {
                //Judge by score
                //Equal score is a push
                if (hand.Score == dealerHand.Score)
                {
                    playerCash += playerBets[betIndex];
                }
                if (hand.Score > dealerHand.Score)
                {
                    //Winner
                    playerCash += playerBets[betIndex] * 2;
                }
            }
        }
    }

    private void FollowDealerRulesForDealerHand()
    {
        var dealerOptions = dealerHand.GetOptions();

        //Check for end of game
        if ((dealerOptions &
            (CardHand.Options.BlackJack |
             CardHand.Options.TwentyOne |
             CardHand.Options.Bust)) > 0)
        { 
            settleBets = true;
        }

        if (dealerHand.Score <= 16)
        {
            var card = cardDeck.Pop();
            blackJackToggleCardsScript.AddCard(card);
            dealerHand.Add(card);
        }
    }

    private void OnDestroy()
    {
        if (instance = this)
        {
            instance = null;
        }
    }
}
