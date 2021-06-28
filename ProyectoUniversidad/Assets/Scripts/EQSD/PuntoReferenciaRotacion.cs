using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntoReferenciaRotacion : MonoBehaviour
{

    public RotarObjeto controlador;


    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(-controlador.GetComponent<RotarObjeto>().rotY, controlador.GetComponent<RotarObjeto>().rotX, this.transform.position.z));
    }

}
