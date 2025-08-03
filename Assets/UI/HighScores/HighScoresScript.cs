using Assets.UI.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HighScoresScript : UIToolkitScript
{
    Button exitButton;
    Label highScoreValueLabel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        exitButton = root.Q<Button>("ExitButton");
        highScoreValueLabel = root.Q<Label>("HighScoreValueLabel");


        exitButton.clicked += ExitButton_clicked;

        highScoreValueLabel.text = "$" + PlayerPrefs.GetFloat("High Score");
    }

    private void ExitButton_clicked()
    {
        SceneManager.LoadScene(SceneSelectionIndex.MainMenuScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
