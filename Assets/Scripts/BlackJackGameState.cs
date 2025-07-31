using Cardgames;
using UnityEngine;

public class BlackJackGameState : MonoBehaviour
{
    //Singleton
    private static BlackJackGameState instance;

    CardDeck cardDeck;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (instance = this)
        {
            instance = null;
        }
    }
}
