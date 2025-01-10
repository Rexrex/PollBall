using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public float MaxTime;
    float CurrentTimer;

    public TextMeshProUGUI textMeshProUGUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentTimer = MaxTime;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTimer -= Time.deltaTime;
       textMeshProUGUI.text = Mathf.Round(CurrentTimer).ToString();

    }
}
