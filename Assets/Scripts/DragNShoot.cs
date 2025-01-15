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
    public float incrementalSlowMod = 0.001f;

    private float startTimeScale;
    private float startFixedDeltaTime;

    [Header("Shoot Variables")]
    public float power = 10f;
    public Vector2 minPower;
    public Vector2 maxPower;
    public float ballRadius = 0.25f;
    public float ShootMaxDistance = 2.0f;

    [Header("Cue Stick")]
    //Slow Motion variables
    public GameObject CueStick;

    [Header("Debug Variables")]
    public GameObject TestBall1;
    public bool UseDebugRays;

    [Header("Audio")]
    private AudioSource AudioSource;
    public AudioClip BreakClip;
    public AudioClip ShootClip;


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
        AudioSource = GetComponent<AudioSource>();
        AudioSource.clip = ShootClip;

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

                Vector3 direction = endPoint - currentPoint;
                direction.z = 0;

                if(direction.magnitude >= ShootMaxDistance)
                {
                    direction.Normalize();
                    endPoint = currentPoint + direction * ShootMaxDistance;
                }

                force = new Vector2(Mathf.Clamp(currentPoint.x - endPoint.x, minPower.x, maxPower.x),
                    Mathf.Clamp(currentPoint.y - endPoint.y, minPower.y, maxPower.y));

                tl.RenderLine(currentPoint, endPoint, force, ballRadius);

                if (CueStick)
                {
                    CueStick.transform.position = endPoint;

                    float targetPosX = currentPoint.x - this.transform.position.x;
                    float targetPosY = this.transform.position.y - currentPoint.y;

                    float angle = Mathf.Atan2(targetPosX, targetPosY) * Mathf.Rad2Deg;
                    CueStick.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


                }

                if (useSlowMotion)
                {
                    if (Time.timeScale < 1.0f)
                    {
                        Time.timeScale += incrementalSlowMod;
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
                AudioSource.volume = force.magnitude * 0.25f;
                AudioSource.Play();
            
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
