using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class DragNShoot : MonoBehaviour
{

    [Header("Slow Motion Effect")]
    //Slow Motion variables
    public bool useSlowMotion = false;
    public float slowMotionTimeScale;

    private float startTimeScale;
    private float startFixedDeltaTime;

    [Header("Shoot Variables")]
    public float power = 10f;
    public Vector2 minPower;
    public Vector2 maxPower;

    private Rigidbody2D rb;
    LineTrajectory tl;

    Camera cam;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        tl = GetComponent<LineTrajectory>();

        startTimeScale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
        
    }

    private void Update()
    {
      


        if (Input.GetMouseButtonDown(0))
        {
            // Left Mouse Down
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15;

            if (useSlowMotion)
            {
                StartSlowMotion();
            }
        }

        if (Input.GetMouseButton(0))
        {

            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15;

            tl.RenderLine(startPoint, currentPoint);
        }


        if (Input.GetMouseButtonUp(0))
        {
            

            // Left Mouse Down
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15;

            force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x),
                Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
            rb.AddForce(force * power, ForceMode2D.Impulse);

            tl.EndLine();


            if (useSlowMotion)
            {
                StopSlowMotion();
            }
        }

       // effect.SetVector3("ColliderPos", this.gameObject.transform.position);
    }


    /*
     * Method to start slow motion effect
     */
    public void StartSlowMotion()
    {
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime * slowMotionTimeScale;
    }

    /*
     * Method to stop slow motion effect
     */
    public void StopSlowMotion() 
    {

        Time.timeScale = startTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime;
    }
}
