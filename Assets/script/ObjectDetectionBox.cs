using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetectionBox : MonoBehaviour
{
    Collision box;

    public Collision Box { get { return box;} set{ box = value;} }

    public void OnCollisionEnter(Collision collision)
    {
        box = collision;
    }
}
