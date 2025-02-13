using UnityEngine;
using UnityEngine.Audio;

public class PoolBall : MonoBehaviour
{
   
    public Color baseColor;
    public bool ChangeColorOnCollision;
    //Defined in the renderer
    public Color actualBallColor;

    private SpriteRenderer spriteRenderer;

    //Audio
    public AudioClip[] CollisionSounds;
    private AudioSource audioSource;

    // Duration to keep the changed color
    public float colorChangeDuration = 0.3f;

    // What happens when a ball goes into a pocket
    bool MarkedToDie = false;
    public float KillTimer = 2.0f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = CollisionSounds[0];

        // Get the Renderer component and store the original color
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            //Grabbing the original color
            actualBallColor = spriteRenderer.color;
            //Setting ball to white
            spriteRenderer.color = baseColor;
        }
        else
        {
            Debug.LogError("No Renderer found on this GameObject!");
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (ChangeColorOnCollision)
        {
            StopAllCoroutines(); // Stop any existing color change coroutine
            StartCoroutine(ChangeColorTemporarily());
        }

        PlayRandomCollisionSound();
    }

    private System.Collections.IEnumerator ChangeColorTemporarily()
    {
        ChangeColor();

        // Wait for the specified duration
        yield return new WaitForSeconds(colorChangeDuration);

        // Revert to the original color
        ResetColor();
    }


    public void ChangeColor()
    {
        // Change to the hit color
        spriteRenderer.color = actualBallColor;
    }

    public void ResetColor()
    {
        // Revert to the original color
        spriteRenderer.color = baseColor;
    }

    public void PlayRandomCollisionSound()
    {
        int rand = Random.Range(0, CollisionSounds.Length);
        audioSource.clip = CollisionSounds[rand];
        audioSource.Play();
    }


    public void Dissolve()
    {
        MarkedToDie = true;
    }

    private void FixedUpdate()
    {
        
        if (KillTimer < 0.0f)
        {
            Destroy(this);
        }
       
        else if (MarkedToDie)
        {
            KillTimer -= Time.deltaTime;
            //  A simple animation for when the ball goes into a pocket
            this.transform.localScale *= 0.99f;
        }
    }
}
