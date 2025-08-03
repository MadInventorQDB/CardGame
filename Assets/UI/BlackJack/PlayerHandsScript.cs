using Assets.UI.Scripts;
using Cardgames;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHandsScript : UIToolkitScript
{
    List<VisualElement> playerHandVisualElements;
    List<List<Label>> labelPools;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        labelPools = new List<List<Label>>();
        
        playerHandVisualElements = new List<VisualElement>
        {
            root.Q<VisualElement>("PlayerHand0"),
            root.Q<VisualElement>("PlayerHand1"),
            root.Q<VisualElement>("PlayerHand2"),
            root.Q<VisualElement>("PlayerHand3")
        };

        for (int i = 0; i < playerHandVisualElements.Count; i++)
        {
            VisualElement element = playerHandVisualElements[i];
            labelPools.Add(new List<Label>());
            var labels = element.Children().OfType<Label>().ToList();
            foreach (var label in labels)
            {
                element.Remove(label);
                labelPools[i].Add(label);
            }
        }
    }

    public void AddCard(Card card, int handIndex)
    {
        Label label = labelPools[handIndex][0];
        labelPools[handIndex].RemoveAt(0);

        label.text = card.ToString();

        if (card.Suit == Card.suit.Hearts || card.Suit == Card.suit.Diamonds)
        {
            label.style.color = Color.red;
        }
        else
        {
            label.style.color = Color.black;
        }
        playerHandVisualElements[handIndex].Add(label);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    internal void ClearHands()
    {
        for (int i = 0; i < playerHandVisualElements.Count; i++)
        {
            var labels = playerHandVisualElements[i].Children().OfType<Label>().ToList();
            foreach (var label in labels)
            {
                playerHandVisualElements[i].Remove(label);
                labelPools[i].Add(label);
            }
        }
    }
}
