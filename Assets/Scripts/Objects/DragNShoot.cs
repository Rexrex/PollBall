using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;
using UnityEngine.VFX;
using static StateManager;

public class DragNShoot : MonoBehaviour
{

    [Header("Slow Motion Effect")]
    //Slow Motion variables
    public bool useSlowMotion = false;
    public float slowMotionTimeScale;
    public float incrementalSlowMod = 0.001f;

    private float _startTimeScale;
    private float _startFixedDeltaTime;

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
    private AudioSource _audioSource;
    public AudioClip BreakClip;
    public AudioClip ShootClip;


    private Rigidbody2D _rigidbody2d;
    LineTrajectory _lineTrajectory;

    Camera _camera;
    Vector2 _force;
    Vector3 _startPoint, _currentPoint, _endPoint;
    bool gameStarted = false;

    public static event Action ShootEvent;


    private void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _lineTrajectory = GetComponent<LineTrajectory>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = ShootClip;

        _startTimeScale = Time.timeScale;
        _startFixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        bool pointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (StateManager.currentState == StateManager.GameState.Play && gameStarted && !pointerOverUI)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleFirstInput();
            }

            if (Input.GetMouseButton(0))
            {
                HandleInputDown();
            }


            if (Input.GetMouseButtonUp(0))
            {

                HandleReleasedInput();
            }

        }
        else Time.timeScale = 1;

    }

    void HandleReleasedInput()
    {

        // Left Mouse Released
        _endPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
        _endPoint.z = 0;

        this._rigidbody2d.linearVelocity = new Vector2(0, 0);
        _force = new Vector2(Mathf.Clamp(_currentPoint.x - _endPoint.x, minPower.x, maxPower.x),
            Mathf.Clamp(_currentPoint.y - _endPoint.y, minPower.y, maxPower.y));
        _rigidbody2d.AddForce(_force * power, ForceMode2D.Impulse);


        _lineTrajectory.EndLine();


        if (useSlowMotion)
        {
            StopSlowMotion();
        }

        if (CueStick)
        {
            CueStick.SetActive(false);
            CueStick.transform.position = _endPoint;
        }

        // Debugging
        if (TestBall1)
        {
            TestBall1.SetActive(false);
        }

        ShootEvent?.Invoke();
        _audioSource.volume = _force.magnitude * 0.25f;
        _audioSource.Play();

    }

    void HandleInputDown()
    {
        // Left Mouse Down
        _currentPoint = GetSphereHitPoint();
        _currentPoint.z = 0;

        if (UseDebugRays)
        {

            if (TestBall1)
            {
                TestBall1.transform.position = _currentPoint;
            }
        }

        _endPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
        _endPoint.z = 0;

        Vector3 direction = _endPoint - _currentPoint;
        direction.z = 0;

        if (direction.magnitude >= ShootMaxDistance)
        {
            direction.Normalize();
            _endPoint = _currentPoint + direction * ShootMaxDistance;
        }

        _force = new Vector2(Mathf.Clamp(_currentPoint.x - _endPoint.x, minPower.x, maxPower.x),
            Mathf.Clamp(_currentPoint.y - _endPoint.y, minPower.y, maxPower.y));

        _lineTrajectory.RenderLine(_currentPoint, _endPoint, _force, ballRadius);

        if (CueStick)
        {
            CueStick.transform.position = _endPoint;

            float targetPosX = _currentPoint.x - this.transform.position.x;
            float targetPosY = this.transform.position.y - _currentPoint.y;

            float angle = Mathf.Atan2(targetPosX, targetPosY) * Mathf.Rad2Deg;
            CueStick.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


        }

        if (useSlowMotion)
        {
            if (Time.timeScale < 1.0f)
            {
                Time.timeScale += incrementalSlowMod;
                Time.fixedDeltaTime = _startFixedDeltaTime * Time.timeScale;
            }

        }
    }

    void HandleFirstInput()
    {

        // Left Mouse Down
        _startPoint = GetSphereHitPoint();
        _startPoint.z = 0;

        if (useSlowMotion)
        {
            StartSlowMotion();
        }

        if (CueStick)
        {
            CueStick.SetActive(true);
            this._rigidbody2d.angularVelocity = 0;
            CueStick.transform.position = _startPoint;

            Vector3 vectorToTarget = CueStick.transform.position - transform.position;

            CueStick.transform.rotation = Quaternion.LookRotation(vectorToTarget, Vector3.forward);
        }

        if (TestBall1)
        {
            TestBall1.transform.position = _startPoint;
            TestBall1.SetActive(true);
        }
    }

    /*
     * Method to start slow motion effect
     */
    public void StartSlowMotion()
    {
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = _startFixedDeltaTime * slowMotionTimeScale;
    }

    /*
     * Method to stop slow motion effect
     */
    public void StopSlowMotion() 
    {

        Time.timeScale = _startTimeScale;
        Time.fixedDeltaTime = _startFixedDeltaTime;
    }

    private Vector3 GetSphereHitPoint()
    {
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
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

    public void PausedGame()
    {
        gameStarted = false;

    }
}
