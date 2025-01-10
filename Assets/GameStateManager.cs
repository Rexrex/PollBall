using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameState
    {
        StartMenu,
        Play,
        PauseMenu,
        End
    }

    public GameObject StartMenu;
    public GameObject PauseMenu;
    public static GameState currentState;
    public GameState startingState;

    public void Start()
    {
        currentState = startingState;
        StartMenu.SetActive(true);
        PauseMenu.SetActive(false);
    }

    public void UpdateState(GameState state)
    {
        switch (state) { 
        
            case GameState.StartMenu:
                Time.timeScale = 0;
                Time.fixedDeltaTime = 0;
                StartMenu.SetActive(true);
                break;

            case GameState.PauseMenu:
                Time.timeScale = 0;
                Time.fixedDeltaTime = 0;
                PauseMenu.SetActive(true);
                break;

            case GameState.Play:
                Time.timeScale = 1;
                Time.fixedDeltaTime = 1;
                StartMenu.SetActive(false);
                PauseMenu.SetActive(false);
                break;

            case GameState.End:
                Application.Quit();
                break;
        }

        currentState = state;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        UpdateState(GameState.Play);
    }

    public void PauseGame()
    {
        UpdateState(GameState.PauseMenu);
    }

    //Not sure if needed
    public void UnPauseGame()
    {
        UpdateState(GameState.Play);
    }


    public void QuitGame()
    {
        UpdateState(GameState.End);
    }

}
