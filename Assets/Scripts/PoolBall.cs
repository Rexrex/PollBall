using UnityEngine;
using UnityEngine.Audio;

public class PoolBall : MonoBehaviour
{
   
    public Color baseColor;
    public bool ChangeColorOnCollision;
    //Defined in the renderer
    public Color actualBallColor;

    private SpriteRenderer spriteRenderer;

    public AudioClip CollisioSound;
    private AudioSource audioSource;

    // Duration to keep the changed color
    public float colorChangeDuration = 0.3f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = CollisioSound;

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

        audioSource.Play();
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
}
