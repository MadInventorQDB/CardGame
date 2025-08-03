using Assets.UI.Scripts;
using Cardgames;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DealerHandScript : UIToolkitScript
{
    VisualElement dealerHandVisualElement;
    List<Label> labelPool;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dealerHandVisualElement = root.Q<VisualElement>("DealerHand");
        //Remove the labels which will be stored in a pool
        labelPool = new List<Label>();
        ClearLabelsAndReturnToLabelPool();
    }

    private void ClearLabelsAndReturnToLabelPool()
    {
        var labels = dealerHandVisualElement.Children().OfType<Label>().ToList();
        foreach (var label in labels)
        {
            dealerHandVisualElement.Remove(label);
            labelPool.Add(label);
        }
    }

    public void AddCard(Card card, bool hide = false)
    {
        Label label = labelPool[0];
        labelPool.RemoveAt(0);

        label.text = hide ? "X" : card.ToString();
        label.userData = card.ToString();


        if (card.Suit == Card.suit.Hearts || card.Suit == Card.suit.Diamonds)
        {
            label.style.color = Color.red;
        }
        else
        {
            label.style.color = Color.black;
        }
        dealerHandVisualElement.Add(label);
    }

    public void ClearHand()
    {
        ClearLabelsAndReturnToLabelPool();
    }

    public void RevealDealerCard()
    {
        var labels = dealerHandVisualElement.Children().OfType<Label>().ToList();
        foreach (var label in labels)
        {
            label.text = label.userData.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
