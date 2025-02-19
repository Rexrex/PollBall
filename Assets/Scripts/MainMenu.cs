using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    MainMenu,
    GameScene,
    OptionsScene
}

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        LoadScene(SceneName.GameScene);
    }

    public void Options()
    {
        LoadScene(SceneName.OptionsScene);
    }

    private void LoadScene(SceneName scene)
    {
        SceneManager.LoadScene(scene.ToString());  // Convert enum to string
    }

    public void Quit()
    {
        Application.Quit();
    }
}
