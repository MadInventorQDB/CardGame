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
        BetPlus,
        DoubleDown,
        CashOut,
        Restart,
    }
    
    private Button splitButton;
    private Button dealOrHitButton;
    private Button standButton;
    private Button betMinusButton;
    private Button betPlusButton;
    private Button doubleDownButton;
    private DoubleField cashDoubleField;
    private DoubleField betDoubleField;

    private Button cashOutButton;
    private Button restartButton;

    private Label timerLabel;
    private DateTime dateTimeStart;


    public event Action<PlayerAction> PlayerActionEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splitButton = root.Q<Button>("SplitButton");
        dealOrHitButton = root.Q<Button>("DealOrHitButton");
        standButton = root.Q<Button>("StandButton");
        betMinusButton = root.Q<Button>("BetMinusButton");
        betPlusButton = root.Q<Button>("BetPlusButton");
        doubleDownButton = root.Q<Button>("DoubleDownButton");

        cashDoubleField = root.Q<DoubleField>("CashDoubleField");
        betDoubleField = root.Q<DoubleField>("BetDoubleField");

        cashOutButton = root.Q<Button>("CashOutButton");
        restartButton = root.Q<Button>("RestartButton");

        timerLabel = root.Q<Label>("TimerLabel");

        splitButton.clicked += SplitButton_clicked;
        dealOrHitButton.clicked += DealOrHitButton_clicked;
        standButton.clicked += StandButton_clicked;
        betMinusButton.clicked += BetMinusButton_clicked;
        betPlusButton.clicked += BetPlusButton_clicked;
        doubleDownButton.clicked += DoubleDownButton_clicked;

        cashOutButton.clicked += CashOutButton_clicked;
        restartButton.clicked += RestartButton_clicked;

        dateTimeStart = DateTime.Now;
        //TODO clean up later😆
    }

    private void RestartButton_clicked()
    {
        PlayerActionEvent?.Invoke(PlayerAction.Restart);
    }

    private void CashOutButton_clicked()
    {
        PlayerActionEvent?.Invoke(PlayerAction.CashOut);
    }

    private void DoubleDownButton_clicked()
    {
        PlayerActionEvent?.Invoke(PlayerAction.DoubleDown);
    }

    private void BetPlusButton_clicked()
    {
        PlayerActionEvent?.Invoke(PlayerAction.BetPlus);
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

    TimeSpan twoMinutes = new TimeSpan(0, 2, 0);

    // Update is called once per frame
    void Update()
    {
        TimeSpan timePassed = DateTime.Now - dateTimeStart;
        timerLabel.text = (twoMinutes - timePassed).ToString(@"mm\:ss");
        if (timePassed > twoMinutes)
        {
            PlayerActionEvent?.Invoke(PlayerAction.Restart);
        }
    }

    //Set the options for the player based on the player hand.
    internal void UpdateUI(CardHand.Options options, double cash, double bet)
    {
        cashDoubleField.value = cash;
        betDoubleField.value = bet;

        if ((options & CardHand.Options.Deal) != 0)
        {
            dealOrHitButton.text = "DEAL";
            betMinusButton.SetEnabled(true);
            betPlusButton.SetEnabled(true);
            standButton.SetEnabled(false);
        }
        else
        {
            dealOrHitButton.text = "HIT";
            betMinusButton.SetEnabled(false);
            betPlusButton.SetEnabled(false);
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

        standButton.SetEnabled(dealOrHitButton.text == "HIT" && dealOrHitButton.enabledInHierarchy);

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
