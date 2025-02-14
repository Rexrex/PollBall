using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public int CurrentStroke = 0;
    Vector3 InitialPlayerPos;

    public TextMeshProUGUI textMeshProUGUI;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DragNShoot.ShootEvent += ShotsFired;
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void EndGame()
    {

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
