using UnityEngine;

public class LightZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Tags should be compared like this
        if (collision.gameObject.CompareTag("Ball"))
            collision.gameObject.GetComponent<PoolBall>().ChangeColor();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
                collision.gameObject.GetComponent<PoolBall>().ResetColor();

    }
}
