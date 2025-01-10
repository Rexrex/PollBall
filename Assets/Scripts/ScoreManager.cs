using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public float MaxTime;
    float CurrentTimer;
    Vector3 InitialPlayerPos;

    public TextMeshProUGUI textMeshProUGUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentTimer = MaxTime;
        InitialPlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTimer -= Time.deltaTime;
       textMeshProUGUI.text = Mathf.Round(CurrentTimer).ToString();

    }

    public void ScoredBall(string Tag)
    {
        Debug.Log("Scored " + Tag);
        switch (Tag) {

            case "Black":
                CurrentTimer = 0;
                break;

            case "Player" or "White":
                IncrementTime(-10);
                ResetPlayerPosition();
                break;

            default:
                IncrementTime(10);
                break;

        }

    }

    public void EndGame()
    {

    }

    public void ResetPlayerPosition() {

        Debug.Log("Resetting Player");

        GameObject.FindGameObjectWithTag("Player").transform.position = InitialPlayerPos;
    }

    public void IncrementTime(float Increment)
    {
        CurrentTimer += Increment;
    }
}
