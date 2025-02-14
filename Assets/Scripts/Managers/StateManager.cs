using TMPro;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum GameState
    {
        StartMenu,
        Play,
        PauseMenu,
        GameOverMenu,
        WinMenu,
        End
    }

    /*
     Defines the Menu Widgets
    */

    [Header("Menus")]
    public GameObject StartMenu;
    public GameObject PauseMenu;
    public GameObject GameOverMenu;
    public GameObject WinMenu;
    public GameObject GameUI;

    public static GameState currentState;
    public GameState startingState;

    public void Start()
    {
        currentState = startingState;
        UpdateState(currentState);

        if (currentState == GameState.Play)
        {
            Invoke(nameof(ActuallyStartTheGame), 0.1f);
        }
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
                Time.fixedDeltaTime = 0;
                StartMenu.SetActive(false);
                PauseMenu.SetActive(false);
                GameOverMenu.SetActive(false);
                WinMenu.SetActive(false);
                GameUI.SetActive(true);
                break;

            case GameState.GameOverMenu:
                Time.timeScale = 0;
                Time.fixedDeltaTime = 0;
                GameOverMenu.SetActive(true);
                break;

            case GameState.WinMenu:
                Time.timeScale = 0;
                Time.fixedDeltaTime = 0;
                WinMenu.GetComponent<TextMeshProUGUI>().text = "Score: " + GameObject.FindFirstObjectByType<ScoreManager>().CurrentStroke.ToString();
                WinMenu.SetActive(true);
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
        Invoke(nameof(ActuallyStartTheGame), 0.1f);
       
    }

    void ActuallyStartTheGame()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<DragNShoot>().StartedGame();
    }

    public void PauseGame()
    {
        UpdateState(GameState.PauseMenu);
    }

    //Not sure if needed
    public void UnPauseGame()
    {
        UpdateState(GameState.Play);
        Invoke(nameof(ActuallyStartTheGame), 0.1f);
    }


    public void QuitGame()
    {
        UpdateState(GameState.End);
    }

    public void GameOver()
    {
        UpdateState(GameState.GameOverMenu);
    }

    public void GameWin()
    {
        UpdateState(GameState.WinMenu);
    }

    public void Reset()
    {
        GetComponent<GameManager>().Reset();
        UpdateState(GameState.Play);
    }


}
