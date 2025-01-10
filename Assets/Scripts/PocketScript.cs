using UnityEngine;

public class PocketScript : MonoBehaviour
{

    public int pointsPerBall = 10;
    private ScoreManager scoreManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != null)
        {
            string tag = collision.gameObject.tag.ToString();

            scoreManager.ScoredBall(tag);

            if(tag != "Player")
                Destroy(collision.gameObject);
        }
        
    }
}
