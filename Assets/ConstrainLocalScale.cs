using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrainLocalScale : MonoBehaviour
{
    public bool ConstrainLocalX = false;
    public float LocalXMin = 0;
    public float LocalXMax = 1f;

    public bool ConstrainLocalY = false;
    public float LocalYMin = 0;
    public float LocalYMax = 1f;

    public bool ConstrainLocalZ = false;
    public float LocalZMin = 0;
    public float LocalZMax = 1f;

    void Update()
    {
        /// Constrain a Transform's LocalPosition to a given value.
        doConstrain();
    }

    void doConstrain()
    {
        // Save a lookup
        if (!ConstrainLocalX && !ConstrainLocalY && !ConstrainLocalZ)
        {
            return;
        }

        Vector3 currentPos = transform.localScale;
        float newX = ConstrainLocalX ? Mathf.Clamp(currentPos.x, LocalXMin, LocalXMax) : currentPos.x;
        float newY = ConstrainLocalY ? Mathf.Clamp(currentPos.y, LocalYMin, LocalYMax) : currentPos.y;
        float newZ = ConstrainLocalZ ? Mathf.Clamp(currentPos.z, LocalZMin, LocalZMax) : currentPos.z;

        transform.localScale = new Vector3(newX, newY, newZ);
    }
}
