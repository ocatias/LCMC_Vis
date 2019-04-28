using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class CircleDrawer : MonoBehaviour {
    [Range(0,200)]
    public int segments = 50;
    [Range(0,30)]
    public float radius = 10;
    LineRenderer line;

    void Start ()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        CreatePoints ();
    }

    void CreatePoints ()
    {
        float x;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos (Mathf.Deg2Rad * angle) * radius;

            line.SetPosition (i,new Vector3(x,0,z) );

            angle += (360f / segments);
        }
    }
}
