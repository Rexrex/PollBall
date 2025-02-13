using UnityEngine;

public class PocketScript : MonoBehaviour
{

    public int pointsPerBall = 10;
    private GameManager _gameManager;
    private AudioSource _pocketAudio;

    void Start()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
        _pocketAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            //Informing the manager
            _gameManager.ScoredBall(collision.gameObject);

            // Playing sound
            _pocketAudio.Play();

            Rigidbody2D ballRigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();

            if (ballRigidbody2D != null)
            {
                ballRigidbody2D.angularVelocity = 0;
                ballRigidbody2D.linearVelocity = Vector2.zero;
            }

            collision.gameObject.GetComponent<PoolBall>()?.Dissolve();

        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            _gameManager.HandleCueBallIn();
        }
    }

}
