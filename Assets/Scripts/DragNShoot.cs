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

    [Header("Cue Stick")]
    //Slow Motion variables
    public GameObject CueStick;

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
            startPoint = this.transform.position;
            startPoint.z = 15;

            if (useSlowMotion)
            {
                StartSlowMotion();
            }

            if (CueStick)
            {
                CueStick.SetActive(true);
                this.rb.angularVelocity = 0;
                CueStick.transform.position = startPoint;

                Vector3 vectorToTarget = CueStick.transform.position - transform.position;

                CueStick.transform.rotation = Quaternion.LookRotation(vectorToTarget, Vector3.forward);
            }
        }

        if (Input.GetMouseButton(0))
        {
            startPoint = this.transform.position;
            startPoint.z = 15;
            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 15;

            tl.RenderLine(startPoint, currentPoint);

            if (CueStick)
            {
                CueStick.transform.position = currentPoint;

                Vector3 vectorToTarget = CueStick.transform.position - transform.position;

                CueStick.transform.rotation = Quaternion.LookRotation(vectorToTarget, Vector3.forward);


            }
        }


        if (Input.GetMouseButtonUp(0))
        {
            

            // Left Mouse Down
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15;
            startPoint.z = 15;

            force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x),
                Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
            rb.AddForce(force * power, ForceMode2D.Impulse);

            tl.EndLine();


            if (useSlowMotion)
            {
                StopSlowMotion();
            }

            if (CueStick)
            {
                CueStick.SetActive(false);
                CueStick.transform.position = endPoint;
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
