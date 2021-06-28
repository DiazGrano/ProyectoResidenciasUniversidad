using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotarObjeto : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float velocidadRotacion = 100f;
    public float rotX = 0f;
    public float rotY = 0f;

    public void OnDrag(PointerEventData eventData)
    {
        rotX = Input.GetAxis("Mouse X") * velocidadRotacion * Mathf.Deg2Rad;
        rotY = Input.GetAxis("Mouse Y") * velocidadRotacion * Mathf.Deg2Rad;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rotX = 0f;
        rotY = 0f;
    }
}
