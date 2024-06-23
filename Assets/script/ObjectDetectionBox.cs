using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetectionBox : MonoBehaviour
{
    Vector3 box;
    public Vector3 Box{ get { return box;} set{ box = value;} }

    public void OnCollisionEnter(Collision collision)
    {
        box = transform.position;
    }
}
