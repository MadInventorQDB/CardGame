using Cardgames;
using System.Collections.Generic;
using UnityEngine;

public class BlackJackGameState : MonoBehaviour
{
    //Singleton
    private static BlackJackGameState instance;

    CardDeck cardDeck;
    List<CardHand> playerHands;
    CardHand dealerHand;

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
        int pjb = 11012006;
        int lob = 11022004;
        int jab = 10071972;

        int randSeed = jab + lob + pjb;

        cardDeck = new CardDeck(randSeed);
        playerHands = new List<CardHand>() { new CardHand()};
        dealerHand = new CardHand();

        //playerHands[0].AddCard(cardDeck.PopCard());
        playerHands[0].AddCard(new Card(1));
        playerHands[0].AddCard(new Card(14));
        playerHands.Add(playerHands[0].Split());
        playerHands[0].AddCard(new Card(27));
        playerHands.Add(playerHands[0].Split());
        playerHands[0].AddCard(new Card(40));
        playerHands.Add(playerHands[0].Split());

    }

    // Update is called once per frame
    void Update()
    {
        //Figure out which hand is the current hand? playerHands[0]
        //Update the UI for the current hand?

    }

    private void OnDestroy()
    {
        if (instance = this)
        {
            instance = null;
        }
    }
}
