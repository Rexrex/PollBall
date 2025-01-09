using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    
    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
       SceneManager.LoadScene(1);
    }

    public void Options()
    {
        SceneManager.LoadScene(1);
    }
}
