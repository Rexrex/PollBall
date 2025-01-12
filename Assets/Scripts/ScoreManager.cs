using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    int CurrentStroke = 0;
    Vector3 InitialPlayerPos;

    public TextMeshProUGUI textMeshProUGUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitialPlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        DragNShoot.ShootEvent += ShotsFired;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ScoredBall(string Tag)
    {
        Debug.Log("Scored " + Tag);
        switch (Tag) {

            case "Black":
                
                break;

            case "Player" or "White":
                ResetPlayerPosition();
                break;

            default:
                

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
        //CurrentTimer += Increment;
    }

    void ShotsFired()
    {
        CurrentStroke += 1;
        UpdateGUI();


    }

    void UpdateGUI()
    {
        textMeshProUGUI.text = CurrentStroke.ToString();
    }
}
