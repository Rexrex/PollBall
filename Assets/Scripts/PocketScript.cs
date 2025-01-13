using UnityEngine;

public class PocketScript : MonoBehaviour
{

    public int pointsPerBall = 10;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != null)
        {
            string tag = collision.gameObject.name.ToString();

            gameManager.ScoredBall(collision.gameObject);

            if(tag != "Player")
                Destroy(collision.gameObject);
        }
        
    }
}
