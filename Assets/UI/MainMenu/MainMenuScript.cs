using Assets.UI.Scripts;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine;

public class MainMenuScript : UIToolkitScript
{
    Button playButton;
    Button quitButton;
    Button highScoresButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playButton = root.Q<Button>("PlayButton");
        quitButton = root.Q<Button>("QuitButton");
        highScoresButton = root.Q<Button>("HighScoresButton");

        playButton.clicked += PlayButton_clicked;
        quitButton.clicked += QuitButton_clicked;
        highScoresButton.clicked += HighScoreButton_clicked;
    }

    private void HighScoreButton_clicked()
    {
        SceneManager.LoadScene((int)SceneSelectionIndex.HighScoresScene);
    }

    private void QuitButton_clicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // In a build, always quit.
            Application.Quit();
        #endif
    }

    private void PlayButton_clicked()
    {
        SceneManager.LoadScene(SceneSelectionIndex.BlackJackScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
