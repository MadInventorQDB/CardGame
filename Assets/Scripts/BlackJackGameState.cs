using Cardgames;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    DealerHandScript dealerHandScript;
    PlayerHandsScript playerHandsScript;

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
        blackJackToggleCardsScript = FindAnyObjectByType<BlackJackToggleCardsScript>();
        blackJackPlayerScript = FindAnyObjectByType<BlackJackPlayerScript>();
        dealerHandScript = FindAnyObjectByType<DealerHandScript>();
        playerHandsScript = FindAnyObjectByType<PlayerHandsScript>();


        blackJackPlayerScript.PlayerActionEvent += BlackJackPlayerScript_PlayerActionEvent;

        InitializeGameState();


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
        playerHands = new List<CardHand>();
        playerBets = new List<double>();
        dealerHand = new CardHand();

        UpdateUIFromGameState(CardHand.Options.Deal);
    }

    private void BlackJackPlayerScript_PlayerActionEvent(BlackJackPlayerScript.PlayerAction playerAction)
    {
        if (playerAction == BlackJackPlayerScript.PlayerAction.CashOut)
        {
            HandleCashOut();
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
            // If no hands exist, it's a "Deal" action for a new round.
            if (playerHands.Count == 0)
            {
                HandleDeal();
            }
            else
            {
                // Otherwise, it's a "Hit" action for the current hand.
                HandleHit();
            }
        }

        if (playerAction == BlackJackPlayerScript.PlayerAction.Stand)
        {
            HandleStand();
        }

        if (playerAction == BlackJackPlayerScript.PlayerAction.DoubleDown)
        {
            HandleDoubleDown();
        }

        if (playerAction == BlackJackPlayerScript.PlayerAction.Split)
        {
            HandleSplit();
        }


        if (playerHands.Count > 0)
        {
            UpdateUIFromGameState(playerHands[playerHandIndex].GetOptions());
        }
        else
        {
            UpdateUIFromGameState(CardHand.Options.Deal);
        }
    }

    private void HandleCashOut()
    {
        var currentHighScore = PlayerPrefs.GetFloat("High Score");
        if (currentHighScore < playerCash)
        {
            PlayerPrefs.SetFloat("High Score", (float)playerCash);
        }
        SceneManager.LoadScene(SceneSelectionIndex.MainMenuScene);
    }

    private void HandleSplit()
    {

        playerCash -= playerBets[playerHandIndex];
        playerBets.Add(playerBets[playerHandIndex]);


        CardHand currentHand = playerHands[playerHandIndex];
        CardHand newHand = currentHand.Split();

        playerHands.Add(newHand);

        //Replace the split card and update the UI
        Card cardForFirstHand = cardDeck.Pop();
        playerHands[playerHandIndex].Add(cardForFirstHand);
        blackJackToggleCardsScript.AddCard(cardForFirstHand);

        // The new split hand (which also has one card) gets its first new card.
        Card cardForSecondHand = cardDeck.Pop();
        playerHands[playerHands.Count - 1].Add(cardForSecondHand);
        blackJackToggleCardsScript.AddCard(cardForSecondHand);

        //The hands need to be updated
        playerHandsScript.ClearHands();
        for (int i = 0; i < playerHands.Count; i++)
        {
            CardHand hand = playerHands[i];
            foreach (var card in hand)
            {
                playerHandsScript.AddCard(card, i);
            }
        }

        // After splitting, check if the first hand is already a Blackjack.
        var optionsForFirstHand = playerHands[playerHandIndex].GetOptions();
        if ((optionsForFirstHand & (CardHand.Options.BlackJack)) != 0)
        {
            HandleStand();
        }
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
        playerCash -= playerBets[playerHandIndex];
        playerBets[playerHandIndex] += playerBets[playerHandIndex];

        var card = cardDeck.Pop();
        blackJackToggleCardsScript.AddCard(card);
        playerHands[playerHandIndex].Add(card);
        playerHandsScript.AddCard(card, playerHandIndex);

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
            dealerHandScript.RevealDealerCard();
        }
    }

    private void HandleDeal()
    {
        // 1. Create the first player hand and place the bet
        playerHands.Add(new CardHand());
        playerCash -= currentBet;
        playerBets.Add(currentBet);

        // 2. Deal cards in the standard sequence
        // Player Card 1
        Card playerCard1 = cardDeck.Pop();
        playerHands[0].Add(playerCard1);
        playerHandsScript.AddCard(playerCard1, 0);
        blackJackToggleCardsScript.AddCard(playerCard1);

        // Dealer Card 1 (hidden)
        Card dealerCard1 = cardDeck.Pop();
        dealerHand.Add(dealerCard1);
        dealerHandScript.AddCard(dealerCard1, true);
        blackJackToggleCardsScript.AddCard(dealerCard1, true);

        // Player Card 2
        Card playerCard2 = cardDeck.Pop();
        playerHands[0].Add(playerCard2);
        playerHandsScript.AddCard(playerCard2, 0);
        blackJackToggleCardsScript.AddCard(playerCard2);

        // Dealer Card 2 (visible)
        Card dealerCard2 = cardDeck.Pop();
        dealerHand.Add(dealerCard2);
        dealerHandScript.AddCard(dealerCard2);
        blackJackToggleCardsScript.AddCard(dealerCard2);

        // 3. Check for immediate Blackjack which would end the player's turn
        if ((playerHands[0].GetOptions() & CardHand.Options.BlackJack) > 0)
        {
            // If player gets Blackjack, their turn is over immediately.
            dealersTurn = true;
            blackJackToggleCardsScript.RevealDealerCard();
            dealerHandScript.RevealDealerCard();
        }
    }

    private void HandleHit()
    {
        Card card = cardDeck.Pop();
        playerHands[playerHandIndex].Add(card);

        // Update UI
        playerHandsScript.AddCard(card, playerHandIndex);
        blackJackToggleCardsScript.AddCard(card);

        // If player busts, automatically move to the next state (next hand or dealer's turn).
        if (playerHands[playerHandIndex].GetOptions() == CardHand.Options.Bust)
        {
            HandleStand();
        }
    }


    private void UpdateUIFromGameState(CardHand.Options options)
    {
        blackJackPlayerScript.UpdateUI(options, playerCash, currentBet);
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

        if (settleBets == true)
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

        ResetRound();
    }

    private void ResetRound()
    {
        // Reset game state variables
        playerHandIndex = 0;
        dealersTurn = false;
        settleBets = false;

        // Clear the logical data structures for hands and bets
        playerHands.Clear();
        playerBets.Clear();
        dealerHand.Clear();

        // Clear the visual UI elements
        playerHandsScript.ClearHands();
        dealerHandScript.ClearHand();

        // Update the UI to show the "Deal" button and bet options
        UpdateUIFromGameState(CardHand.Options.Deal);
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
        else
        {
            if (dealerHand.Score <= 16)
            {
                var card = cardDeck.Pop();
                blackJackToggleCardsScript.AddCard(card);
                dealerHand.Add(card);
                dealerHandScript.AddCard(card);
            }
            else
            {
                settleBets = true;
            }
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
