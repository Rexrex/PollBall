using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    [Header("Game Settings")]
    public int NumberOfBalls = 5;
    public bool FollowOrder = false;

    public Color[] ColorLibrary;
    public Vector3[] BallStartingPositions;
    public Color[] ForbiddenBalls;

    [Header("GUI")]
    public GameObject BallPrefab;
    public GameObject BallParent;

    [Header("GUI")]
    public GameObject BallUIDisplay;
    public GameObject BallUIPrefab;
    public GameObject BlackBallUIPrefab;
    public float StartingAlpha;

    public Vector3 InitialPlayerPos { get; private set; }

    private List<GameObject> BallUI;
    private GameObject BlackBallUIInstance;
    private int BallInPocketCount = 0;
    private GameStateManager gameStateManager;

    void Start()
    {

        InitialPlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        gameStateManager = this.GetComponent<GameStateManager>();
        BallUI = new List<GameObject>();

        // Setting up colors
        if (NumberOfBalls > 0)
        {
            if (BallUIDisplay != null)
            {

                if (BallUIDisplay != null)
                {

                    for (int i = 0; i < NumberOfBalls; i++)
                    {
                        
                        GameObject NewBallUI = GameObject.Instantiate(BallUIPrefab, Vector3.zero, Quaternion.identity, BallUIDisplay.transform);
                        NewBallUI.GetComponent<UnityEngine.UI.Image>().color = new Color(ColorLibrary[i].r, ColorLibrary[i].g, ColorLibrary[i].b, StartingAlpha);
                        BallUI.Add(NewBallUI);

                        GameObject NewBall = GameObject.Instantiate(BallPrefab, BallStartingPositions[i], Quaternion.identity, BallParent.transform);
                        NewBall.transform.localPosition = BallStartingPositions[i]; 
                        NewBall.GetComponent<SpriteRenderer>().color = ColorLibrary[i];
                        NewBall.GetComponent<PoolBall>().actualBallColor = ColorLibrary[i];
                    }

                    if (BallUIDisplay == null)
                    {
                        BlackBallUIInstance = GameObject.Instantiate(BlackBallUIPrefab, Vector3.zero, Quaternion.identity, BallUIDisplay.transform);
                    }
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetPlayerBallPosition()
    {

        Debug.Log("Resetting Player");

        GameObject.FindGameObjectWithTag("Player").transform.position = InitialPlayerPos;
    }

    public void ScoredBall(GameObject Ball)
    {

        if(Ball.GetComponent<PoolBall>() != null)
        {
            Color PocketBallColor = Ball.GetComponent<PoolBall>().actualBallColor;

            if (PocketBallColor == Color.black && BallInPocketCount < NumberOfBalls)
            {
                //Game Over
                gameStateManager.GameOver();

            }
            else if (PocketBallColor == Color.black && BallInPocketCount == NumberOfBalls)
            {
                //Game Win
                gameStateManager.GameWin();
            }

            else if (PocketBallColor == Color.white)
            {
                // Reset
                ResetPlayerBallPosition();

                if (BallInPocketCount > 0)
                {
                    //Instantiate a Ball

                    foreach (var g in BallUI)
                    {
                        
                        if (CompareRGBs(g.GetComponent<UnityEngine.UI.Image>().color , PocketBallColor))
                        {
                            g.GetComponent<UnityEngine.UI.Image>().color = new Color(PocketBallColor.r, PocketBallColor.g, PocketBallColor.b, StartingAlpha);
                            GameObject NewBall = GameObject.Instantiate(BallPrefab, BallStartingPositions[0], Quaternion.identity, BallParent.transform);
                            NewBall.transform.localPosition = BallStartingPositions[0];
                            NewBall.GetComponent<SpriteRenderer>().color = PocketBallColor;
                            NewBall.GetComponent<PoolBall>().actualBallColor = PocketBallColor;
                            break;
                        }
                    }
                }
            }
            else
            {
                BallInPocketCount += 1;

                foreach (var g in BallUI)
                {
                    if (CompareRGBs(g.GetComponent<UnityEngine.UI.Image>().color, PocketBallColor))
                    {
                        g.GetComponent<UnityEngine.UI.Image>().color = new Color(PocketBallColor.r, PocketBallColor.g, PocketBallColor.b, 1.0f);
                        break;
                    }
                }

                if(BallInPocketCount == NumberOfBalls)
                {
                    BlackBallUIInstance.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }   
        

    }


    public bool CompareRGBs(Color a, Color b)
    {
        if (a.r == b.r && a.b == b.b && a.g == b.g)
            return true;
        return false;
    }


    public void Reset()
    {

        foreach (var g in BallUI)
        {
            Destroy(g.gameObject);
        }

        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        ResetPlayerBallPosition();

        FindFirstObjectByType<ScoreManager>().CurrentStroke = 0;

        foreach (var g in balls)
        {
            Destroy(g.gameObject);
        }

        Start();
    }
}
