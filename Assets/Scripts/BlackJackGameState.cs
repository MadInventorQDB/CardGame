using UnityEngine;

public class BlackJackGameState : MonoBehaviour
{
    //Singleton
    private static BlackJackGameState instance;

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
        CardDeck cardDeck = new CardDeck(10071972);
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
