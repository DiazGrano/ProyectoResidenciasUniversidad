using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotarObjetos : MonoBehaviour
{
    public float rotSpeed = 20f;

    void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

        transform.Rotate(new Vector3 (rotY, -rotX, this.transform.position.z));
    }
}
