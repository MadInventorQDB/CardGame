using Assets.UI.Scripts;
using Cardgames;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BlackJackPlayerScript : UIToolkitScript
{
    public enum PlayerAction
    {
        Split,
        DealOrHit,
        Stand,
        BetMinus,
        Bet,
        BetPlus,
        DoubleDown,
    }
    
    private Button splitButton;
    private Button dealOrHitButton;
    private Button standButton;
    private Button betMinusButton;
    private Button betButton;
    private Button betPlusButton;
    private Button doubleDownButton;


    public event Action<PlayerAction> PlayerActionEvent; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splitButton = root.Q<Button>("SplitButton");
        dealOrHitButton = root.Q<Button>("DealOrHitButton");
        standButton = root.Q<Button>("StandButton");
        betMinusButton = root.Q<Button>("BetMinusButton");
        betButton = root.Q<Button>("BetButton");
        betPlusButton = root.Q<Button>("BetPlusButton");
        doubleDownButton = root.Q<Button>("DoubleDownButton");

        splitButton.clicked += SplitButton_clicked;
        dealOrHitButton.clicked += DealOrHitButton_clicked;
        standButton.clicked += StandButton_clicked;
        betMinusButton.clicked += BetMinusButton_clicked;
        betButton.clicked += BetButton_clicked;
        betPlusButton.clicked += BetPlusButton_clicked;
        doubleDownButton.clicked += DoubleDownButton_clicked;

    }

    private void DoubleDownButton_clicked()
    {
        PlayerActionEvent?.Invoke(PlayerAction.DoubleDown);
    }

    private void BetPlusButton_clicked()
    {
        PlayerActionEvent?.Invoke(PlayerAction.BetPlus);
    }

    private void BetButton_clicked()
    {
        PlayerActionEvent?.Invoke(PlayerAction.Bet);
    }

    private void BetMinusButton_clicked()
    {
        PlayerActionEvent?.Invoke(PlayerAction.BetMinus);

    }

    private void StandButton_clicked()
    {
        PlayerActionEvent?.Invoke(PlayerAction.Stand);
    }

    private void SplitButton_clicked()
    {
        PlayerActionEvent?.Invoke(PlayerAction.Split);
    }

    private void DealOrHitButton_clicked()
    {
        PlayerActionEvent?.Invoke(PlayerAction.DealOrHit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Set the options for the player based on the player hand.
    internal void UpdateUI(CardHand.Options options)
    {
        if ((options & CardHand.Options.Deal) != 0)
        {
            dealOrHitButton.text = "DEAL";
            betButton.SetEnabled(true);
        }
        else
        {
            dealOrHitButton.text = "HIT";
            betButton.SetEnabled(false);
        }

        if ((options & CardHand.Options.Deal) != 0 ||
            (options & CardHand.Options.Hit) != 0)
        {
            dealOrHitButton.SetEnabled(true);
        }
        else
        { 
            dealOrHitButton?.SetEnabled(false);
        }

        if ((options & CardHand.Options.Split) != 0)
        {
            splitButton.SetEnabled(true);
        }
        else
        { 
            splitButton.SetEnabled(false); 
        }

        if ((options & CardHand.Options.DoubleDown) != 0)
        {
            doubleDownButton.SetEnabled(true);
        }
        else
        {
            doubleDownButton.SetEnabled(false);
        }

        if ((options &
            (CardHand.Options.TwentyOne|
             CardHand.Options.BlackJack|
             CardHand.Options.Bust)) != 0)
        {
            dealOrHitButton?.SetEnabled(false);
        }

    }
}
