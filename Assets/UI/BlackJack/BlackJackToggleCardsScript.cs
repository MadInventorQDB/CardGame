using Assets.UI.Scripts;
using Cardgames;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BlackJackToggleCardsScript : UIToolkitScript
{
    private VisualElement cardHistoryVisualElement;
    private Button toggleCardHistoryButton;
    private Label toggleCardHistoryLabel;

    private List<Label> labelPool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        cardHistoryVisualElement = root.Q<VisualElement>("CardHistoryVisualElement");
        toggleCardHistoryButton = root.Q<Button>("ToggleCardHistoryButton");

        toggleCardHistoryButton.clicked += ToggleCardHistoryButton_clicked;

        //Remove the labels which will be stored in a pool
        labelPool = new List<Label>();
        var labels = cardHistoryVisualElement.Children().OfType<Label>().ToList();
        foreach (var label in labels)
        { 
            cardHistoryVisualElement.Remove(label);
            labelPool.Add(label);
        }
    }

    public void AddCard(Card card)
    {
        //Recycle if we run out labels
        if (labelPool.Count == 0)
        {
            var labels = cardHistoryVisualElement.Children().OfType<Label>().ToList();
            labelPool.Add(labels[0]);
            cardHistoryVisualElement.Remove(labels[0]);
        }

        Label label = labelPool[0];
        labelPool.RemoveAt(0);

        label.text = card.ToString();
        if (card.Suit == Card.suit.Hearts || card.Suit == Card.suit.Diamonds)
        {
            label.style.color = Color.red;
        }
        else
        {
            label.style.color = Color.black;
        }
        cardHistoryVisualElement.Add(label);
    }

    private void ToggleCardHistoryButton_clicked()
    {
        //Flip the arrrow text...or an image if we get to polish 
        if (toggleCardHistoryButton.text.Contains("<"))
        {
            toggleCardHistoryButton.text = toggleCardHistoryButton.text.Replace("<", ">");
        }
        else
        {
            toggleCardHistoryButton.text = toggleCardHistoryButton.text.Replace(">", "<");
        }

        List<Label> cardLabelList = cardHistoryVisualElement.Children().OfType<Label>().ToList();
        foreach (Label label in cardLabelList)
        {
            label.visible = !(label.visible);
        }
        foreach (Label label in labelPool)
        {
            label.visible = !(label.visible);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

