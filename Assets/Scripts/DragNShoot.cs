using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;
using UnityEngine.VFX;
using static GameStateManager;

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
    public float ballRadius = 0.25f;

    [Header("Cue Stick")]
    //Slow Motion variables
    public GameObject CueStick;

    [Header("Debug Variables")]
    public GameObject TestBall1;
    public bool UseDebugRays;

    private Rigidbody2D rb;
    LineTrajectory tl;

    Camera cam;
    Vector2 force;
    Vector3 startPoint;
    Vector3 currentPoint;
    Vector3 endPoint;
    bool gameStarted = false;

    public static event Action ShootEvent;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        tl = GetComponent<LineTrajectory>();

        startTimeScale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        if (GameStateManager.currentState == GameStateManager.GameState.Play && gameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Left Mouse Down
                startPoint = GetSphereHitPoint();
                startPoint.z = 0;

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

                if (TestBall1)
                {
                    TestBall1.transform.position = startPoint;
                    TestBall1.SetActive(true);
                }
            }

            if (Input.GetMouseButton(0))
            {
                // Left Mouse Down
                currentPoint = GetSphereHitPoint();
                currentPoint.z = 0;

                if (UseDebugRays)
                {

                    if (TestBall1)
                    {
                        TestBall1.transform.position = currentPoint;
                    }
                }

                endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                endPoint.z = 0;

                force = new Vector2(Mathf.Clamp(currentPoint.x - endPoint.x, minPower.x, maxPower.x),
                    Mathf.Clamp(currentPoint.y - endPoint.y, minPower.y, maxPower.y));

                tl.RenderLine(currentPoint, endPoint, force);

                if (CueStick)
                {
                    CueStick.transform.position = currentPoint;

                    Vector3 vectorToTarget = CueStick.transform.position - transform.position;

                    CueStick.transform.rotation = Quaternion.LookRotation(vectorToTarget, Vector3.forward);


                }

                if (useSlowMotion)
                {
                    if (Time.timeScale < 1.0f)
                    {
                        Time.timeScale += 0.001f;
                        Time.fixedDeltaTime = startFixedDeltaTime * Time.timeScale;
                    }

                }

             
            }


            if (Input.GetMouseButtonUp(0))
            {


                // Left Mouse Down
                endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                endPoint.z = 0;

                this.rb.linearVelocity = new Vector2(0,0);
                force = new Vector2(Mathf.Clamp(currentPoint.x - endPoint.x, minPower.x, maxPower.x),
                    Mathf.Clamp(currentPoint.y - endPoint.y, minPower.y, maxPower.y));
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

                if (TestBall1)
                {
                    TestBall1.SetActive(false);
                }

                ShootEvent?.Invoke();

            
            }

            // effect.SetVector3("ColliderPos", this.gameObject.transform.position);
        }

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

    private Vector3 GetSphereHitPoint()
    {
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 centerPoint = this.transform.position;
        centerPoint.z = 0;
        // Calculate the vector from the sphere's center to the mouse position
        Vector3 direction = mousePosition - centerPoint;
        direction.z = 0;
        // If the point is inside the sphere, move it to the perimeter
        direction.Normalize(); // Normalize to get the direction
        return centerPoint + (direction * this.ballRadius);
    }

    public void StartedGame()
    {
        gameStarted = true;
    }
}
