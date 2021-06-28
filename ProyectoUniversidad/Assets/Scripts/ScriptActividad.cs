using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptActividad : MonoBehaviour {
    // Enumeradores
    [Header("Variables de la actividad")]
    public Grado grado;
    public Materia materia;
    public Bloque bloque;

    [Header("Actividad ligada")]
    public GameObject escenaActividad;

    [Header("Boton")]
    public Button boton;

   public void SeleccionarActividad()
    {
        if (GameManager.sharedInstance.Jugando())
        {
            GameManager.sharedInstance.AbrirEscenaActividad(escenaActividad);
        }
    }
}
