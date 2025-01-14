using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineTrajectory : MonoBehaviour
{

    public LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void RenderLine(Vector3 startPoint, Vector3 targetPoint, Vector2 power, float BallRadius)
    {
        lineRenderer.positionCount = 0;
        lineRenderer.positionCount = 2;
        Vector3[] points = new Vector3[lineRenderer.positionCount];

        power *= 2;

        Vector3 dir = startPoint - targetPoint;
        Vector3 newPoint = startPoint + new Vector3(power.x, power.y,1) ;
        newPoint.z = 0;

        Vector3 hitPoint;
        Vector3 hitNormal;
        if (CheckIfHitBalls(startPoint, dir, power.magnitude, out hitPoint, out hitNormal, BallRadius))
        {
            lineRenderer.positionCount = 3;
            points = new Vector3[lineRenderer.positionCount];
            points[1] = hitPoint;
            points[2] = hitNormal;

        }
        

        points[0] = startPoint;
        
        if(points[1] == Vector3.zero)
            points[1]= newPoint;


        
      

        lineRenderer.SetPositions(points);
    }

    public void EndLine()
    {
        lineRenderer.positionCount = 0;
    }

    public bool CheckIfHitBalls(Vector3 startPoint, Vector3 dir, float power, out Vector3 hitPoint, out Vector3 hitNormal, float BallRadius)
    {
        Vector3 auxDir = dir;
        auxDir.Normalize();
        Vector3 actualStartpoint = startPoint + auxDir * (BallRadius * 2);
        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(actualStartpoint, dir, power);
        hitPoint = Vector3.zero;
        hitNormal = Vector3.zero;

        // If it hits something...
        if (hit)
        {
            if(hit.collider.gameObject.tag != null)
            {
                string colTag = hit.collider.gameObject.tag;
                if (colTag != "Player")
                {
                    if (colTag == "Wall" || colTag == "Ball")
                    {
                        hitPoint = hit.point;                                          
                        

                        // Get a rotation to go from our ray direction (negative, so coming from the wall),
                        // to the normal of whatever surface we hit.
                        var deflectRotation = Quaternion.FromToRotation(-dir, hit.normal);

                        // We then take that rotation and apply it to the same normal vector to basically
                        // mirror that angle difference.
                        var deflectDirection = deflectRotation * hit.normal;

                        hitNormal = hitPoint + deflectDirection;

                        return true;
                    }
                    
                }
            }
           
        }
        return false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
