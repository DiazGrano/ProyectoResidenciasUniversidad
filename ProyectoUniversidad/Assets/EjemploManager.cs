using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EjemploManager : MonoBehaviour
{
    void Start()
    {
        GameManager.sharedInstance.EscenaActividadCargada();
        GameManager.sharedInstance.Jugar();
    }

    public void BotonRegresar()
    {
        if (GameManager.sharedInstance.Jugando())
        {
            GameManager.sharedInstance.CerrarEscenaActividad();
        }
    }

}
