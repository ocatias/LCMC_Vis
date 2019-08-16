using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class CircleDrawer : MonoBehaviour {
    [Range(0,200)]
    public int segments = 50;
    [Range(0,30)]
    public float radius = 10;
    LineRenderer line;

    public int rotation = 0;

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
        float r;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            r = Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos (Mathf.Deg2Rad * angle) * radius;

            if(rotation == 0)
                line.SetPosition (i,new Vector3(r,0,z) );
            else if (rotation == 1)
                line.SetPosition(i, new Vector3(0, r, z));
            else if (rotation == 2)
                line.SetPosition(i, new Vector3(r, z, 0));

            angle += (360f / segments);
        }
    }
}
