using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineTrajectory : MonoBehaviour
{

    public LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void RenderLine(Vector3 startPoint, Vector3 targetPoint, Vector2 power)
    {
        lineRenderer.positionCount = 2;
        Vector3[] points = new Vector3[lineRenderer.positionCount];

        Vector3 dir = startPoint - targetPoint;
        Vector3 newPoint = startPoint + new Vector3(power.x, power.y,1) ;
        newPoint.z = 1;

        Vector3 hitPoint;
        if (CheckIfHitBalls(startPoint, dir, power.magnitude, out hitPoint))
        {
            lineRenderer.positionCount = 3;
            points = new Vector3[lineRenderer.positionCount];
            points[2] = hitPoint;

        }


        points[0] = startPoint;
        points[1] = newPoint;


        
      

        lineRenderer.SetPositions(points);
    }

    public void EndLine()
    {
        lineRenderer.positionCount = 0;
    }

    public bool CheckIfHitBalls(Vector3 startPoint, Vector3 dir, float power, out Vector3 hitPoint)
    {
        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(startPoint, dir, power);
        hitPoint = Vector3.zero;

        // If it hits something...
        if (hit)
        {
            if(hit.collider.gameObject.tag != null)
            {
                string colTag = hit.collider.gameObject.tag;

                if (colTag != "Player")
                {
                    if (colTag == "Wall")
                    {
                        hitPoint = hit.point;
                        return true;
                    }

                    else
                    {
                        // Calculate the distance from the surface and the "error" relative
                        // to the floating height.
                        hitPoint = hit.normal;
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
