using UnityEngine;

public class PocketScript : MonoBehaviour
{

    public int pointsPerBall = 10;
    private GameManager gameManager;
    private bool BallToKill = false;
    private GameObject gBallToKill;
    private float KillTimer = 2.0f;
    private float AuxTimer = 0;

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

            GetComponent<AudioSource>().Play();

            if(tag != "Player" && !BallToKill)
            {
                gBallToKill = collision.gameObject;
                BallToKill = true;
                AuxTimer = KillTimer;
            }
              
        }
        
    }

    private void FixedUpdate()
    {

        if(AuxTimer < 0.0f && BallToKill)
        {
            Destroy(gBallToKill);
            BallToKill = false;
        }

        else if( BallToKill) {
            AuxTimer -= Time.deltaTime;

            if(gBallToKill != null)
                gBallToKill.transform.localScale *= 0.99f;
        }
       
    }


}
