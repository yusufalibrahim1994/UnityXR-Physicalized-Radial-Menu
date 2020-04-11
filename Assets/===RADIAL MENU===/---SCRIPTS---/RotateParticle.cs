using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateParticle : MonoBehaviour
{
    public Vector3 particleSpeed;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(particleSpeed * Time.deltaTime);
    }
}
