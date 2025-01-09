using UnityEngine;

public class PoolBall : MonoBehaviour
{
   
    public Color baseColor;
    //Defined in the renderer
    Color actualBallColor;

    private SpriteRenderer spriteRenderer;

    // Duration to keep the changed color
    public float colorChangeDuration = 0.3f;

    private void Start()
    {
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
        StopAllCoroutines(); // Stop any existing color change coroutine
        StartCoroutine(ChangeColorTemporarily());
    }

    private System.Collections.IEnumerator ChangeColorTemporarily()
    {
        // Change to the hit color
        spriteRenderer.color = actualBallColor;

        // Wait for the specified duration
        yield return new WaitForSeconds(colorChangeDuration);

        // Revert to the original color
        spriteRenderer.color = baseColor;
    }
}
