using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonRespuesta : MonoBehaviour
{
    public EstadoRespuesta respuesta;


    public void Seleccionar()
    {
        ManagerCalificar.sharedInstance.Calificar(respuesta);
    }



}
