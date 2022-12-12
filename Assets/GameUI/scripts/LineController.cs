using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3[] points;
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetUpLine(Vector3[] points)
    {
        lineRenderer.positionCount = points.Length;
        this.points = points;
    }

    void Update()
    {
        try
        {
            for (int i = 0; i < points.Length; i++)
            {
                lineRenderer.SetPosition(i, points[i]);
            }
        }
        catch { }
    }
}
