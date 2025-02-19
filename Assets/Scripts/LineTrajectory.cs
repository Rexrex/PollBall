using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineTrajectory : MonoBehaviour
{

    public LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void RenderLine(Vector3 startPoint, Vector3 targetPoint, Vector2 power, float BallRadius)
    {

        _lineRenderer.positionCount = 2; // Default to 2 points
        List<Vector3> points = new List<Vector3>();

        power *= 1; // Increase power

        Vector3 dir = (startPoint - targetPoint).normalized; // White Ball direction
        Vector3 newPoint = startPoint + new Vector3(power.x, power.y, 0);

        Vector3 hitPoint, hitNormal;

        if (CheckIfHitBalls(startPoint, dir, power.magnitude, out hitPoint, out hitNormal, BallRadius))
        {
            _lineRenderer.positionCount = 3;
            points.Add(startPoint); // First point
            points.Add(hitPoint);   // Collision point

            // Reflection
            Vector3 reflectedDir = Vector3.Reflect(dir, hitNormal);
            Vector3 afterHitPoint = hitPoint + reflectedDir * 2.0f; // Arbitrary extension

            points.Add(afterHitPoint); // Where the ball moves after bouncing

        }
        else
        {
            points.Add(startPoint);
            points.Add(newPoint);
        }

        _lineRenderer.SetPositions(points.ToArray());
    }

    public void EndLine()
    {
        _lineRenderer.positionCount = 0;
    }

    public bool CheckIfHitBalls(Vector3 startPoint, Vector3 dir, float power, out Vector3 hitPoint, out Vector3 hitNormal, float BallRadius)
    {
        Vector3 auxDir = dir.normalized;

        // Adjust ray's starting position to avoid self-collision
        Vector3 actualStartpoint = startPoint + auxDir * (BallRadius * 2);

        // Raycast
        RaycastHit2D hit = Physics2D.Raycast(actualStartpoint, dir, power);

        hitPoint = Vector3.zero;
        hitNormal = Vector3.zero;

        // If we hit something...
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Ball") || hit.collider.CompareTag("Wall"))
            {
                hitPoint = hit.point;

                // Calculate reflection using Unity's built-in method
                Vector3 deflectDirection = Vector3.Reflect(auxDir, hit.normal);

                // Set the hit normal (used for trajectory calculations)
                hitNormal = deflectDirection;

                return true;
            }
        }
        return false;
    }

}
