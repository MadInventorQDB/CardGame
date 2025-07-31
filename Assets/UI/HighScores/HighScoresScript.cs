using Assets.UI.Scripts;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HighScoresScript : UIToolkitScript
{
    Button exitButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        exitButton = root.Q<Button>("ExitButton");

        exitButton.clicked += ExitButton_clicked;
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
