using System.Collections.Generic;
using UnityEngine;
using System.Linq; // For Linq methods

public class GameManager : MonoBehaviour
{

    [Header("Game Settings")]
    public int NumberOfBalls = 5;
    public bool FollowOrder = false;
    public Color[] ColorLibrary;
    public Vector3[] BallStartingPositions;

    [Header("Prefabs")]
    public GameObject BallPrefab;
    public GameObject BallParent;
    public GameObject BlackBallPrefab;

    [Header("UI")]
    public GameObject BallUIDisplay;
    public GameObject BallUIPrefab;
    public GameObject BlackBallUIPrefab;
    public float StartingAlpha;


    public Vector3 InitialPlayerPos { get; private set; }

    [Header("Private variables")]
    private GameObject _playerBall;
    private List<GameObject> _ballUIList;
    private GameObject _blackBallUIInstance;
    private int _ballInPocketCount = 0;
    private StateManager _gameStateManager;

    void Start()
    {
        _playerBall = GameObject.FindGameObjectWithTag("Player");
        InitialPlayerPos = _playerBall.transform.position;
        _gameStateManager = this.GetComponent<StateManager>();
        _ballUIList = new List<GameObject>();
        SetupPoolTable();

    }

    public void SetupPoolTable()
    {
        // Setting up colors
        if (NumberOfBalls <= 0 || BallUIDisplay == null) return;

        // Shuffle the ColorLibrary array
        ColorLibrary = ColorLibrary.OrderBy(x => Random.value).ToArray();

        // Spawn Balls of Different Colors
        for (int i = 0; i < NumberOfBalls; i++)
        {

            GameObject NewBallUI = GameObject.Instantiate(BallUIPrefab, Vector3.zero, Quaternion.identity, BallUIDisplay.transform);
            NewBallUI.GetComponent<UnityEngine.UI.Image>().color = new Color(ColorLibrary[i].r, ColorLibrary[i].g, ColorLibrary[i].b, StartingAlpha);
            _ballUIList.Add(NewBallUI);

            GameObject NewBall = GameObject.Instantiate(BallPrefab, BallStartingPositions[i], Quaternion.identity, BallParent.transform);
            NewBall.transform.localPosition = BallStartingPositions[i];
            NewBall.GetComponent<SpriteRenderer>().color = ColorLibrary[i];
            NewBall.GetComponent<PoolBall>().actualBallColor = ColorLibrary[i];
        }

        // Spawn the Black Ball in the correct position
        if (BlackBallPrefab != null && BallStartingPositions.Length > NumberOfBalls)
        {
            GameObject blackBall = Instantiate(BlackBallPrefab, Vector3.zero, Quaternion.identity, BallParent.transform);
            blackBall.transform.localPosition = Vector3.zero;
        }

        // Spawn Black Ball UI (if it doesn't exist)
        if (_blackBallUIInstance == null && BlackBallUIPrefab != null)
        {
            _blackBallUIInstance = Instantiate(BlackBallUIPrefab, Vector3.zero, Quaternion.identity, BallUIDisplay.transform);
        }

    }


    public void ResetPlayerBallPosition()
    {

        if (_playerBall == null) return;

        _playerBall.transform.position = InitialPlayerPos;

        if (_playerBall.TryGetComponent<Rigidbody2D>(out var playerBody))
        {
            playerBody.angularVelocity = 0;
            playerBody.linearVelocity = Vector2.zero; 
        }

    }

    public void ScoredBall(GameObject Ball)
    {
        if (!Ball.TryGetComponent<PoolBall>(out var poolBall)) return;

        Color pocketBallColor = poolBall.actualBallColor;

        if(pocketBallColor == Color.black)
        {
            if ( _ballInPocketCount < NumberOfBalls)
            {
                //Game Over
                _gameStateManager.GameOver();

            }
            else 
            {
                //Game Win
                _gameStateManager.GameWin();
            }

        }
        else 
        { 
            _ballInPocketCount++;

            // Finding the corresponding UI and updating it
            var matchingUIBall = _ballUIList.Find(g => CompareRGBs(g.GetComponent<UnityEngine.UI.Image>().color, pocketBallColor));

            if (matchingUIBall != null) {

                matchingUIBall.GetComponent<UnityEngine.UI.Image>().color = new Color(pocketBallColor.r, pocketBallColor.g, pocketBallColor.b, 1.0f);
            }

            // Hide black ball UI when all Balls are in
            if (_ballInPocketCount == NumberOfBalls)
            {
                _blackBallUIInstance.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    // White Ball Ended in a Pocket so we have to remove one from the pocket
    public void HandleCueBallIn() { 

        ResetPlayerBallPosition();

        if (_ballInPocketCount > 0 && BallPrefab != null && BallStartingPositions.Length > 0)
        {
            // Find the corresponding valid ball UI with alpha 1.0
            var ballUI = _ballUIList.Find(g => g.GetComponent<UnityEngine.UI.Image>().color.a == 1.0f);

            if (ballUI != null)
            {
                // Get Restored Ball Color
                Color restoredColor = ballUI.GetComponent<UnityEngine.UI.Image>().color;
                // Update UI
                ballUI.GetComponent<UnityEngine.UI.Image>().color = new Color(restoredColor.r, restoredColor.b, restoredColor.g, StartingAlpha);

                // Spawn Ball in Initial Position
                GameObject newBall = GameObject.Instantiate(BallPrefab, BallStartingPositions[0], Quaternion.identity, BallParent.transform);
                newBall.transform.localPosition = BallStartingPositions[0];
                newBall.GetComponent<SpriteRenderer>().color = restoredColor;
                newBall.GetComponent<PoolBall>().actualBallColor = restoredColor;

                _ballInPocketCount -= 1;

            }
        }
    }

    public bool CompareRGBs(Color a, Color b)
    {
        return Mathf.Approximately(a.r, b.r) &&
           Mathf.Approximately(a.g, b.g) &&
           Mathf.Approximately(a.b, b.b);
    }


    public void Reset()
    {

        // Destroy all UI Objects
        foreach (var uiBall in _ballUIList)
        {
            Destroy(uiBall.gameObject);
        }

        _ballUIList.Clear();

        // Destroy all balls
        foreach (var balls in GameObject.FindGameObjectsWithTag("Ball"))
        {
            Destroy(balls.gameObject);
        }

        // Resetting Player Position
        ResetPlayerBallPosition();

        // Resetting Score
        if (FindFirstObjectByType<ScoreManager>() is ScoreManager scoreManager)
        {
            scoreManager.CurrentStroke = 0;
        }

        // Making sure Black Ball UI is also dealt with
        if (_blackBallUIInstance != null)
        {
            Destroy(_blackBallUIInstance);
            _blackBallUIInstance = null;
        }

        // Pausing and Starting the Game
        if (GameObject.FindGameObjectWithTag("Player")?.TryGetComponent<DragNShoot>(out var dragNShoot) == true)
        {
            dragNShoot.PausedGame();
            Start();
            dragNShoot.StartedGame();
        }

    }
}
