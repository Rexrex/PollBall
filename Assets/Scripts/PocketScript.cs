using UnityEngine;

public class PocketScript : MonoBehaviour
{

    public int pointsPerBall = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != null)
        {
            string tag = collision.gameObject.tag.ToString();

            if(tag == "Black")
            {
                //Game Over
                Debug.Log("GameOverMan");
            }

            else if(tag == "White")
            {
                // White Ball In
                Debug.Log("White Ball IN");
            }

            else
            {
                Destroy(collision.gameObject);
            }

        }
        
    }
}
