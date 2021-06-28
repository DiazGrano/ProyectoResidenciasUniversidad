using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptGrado : MonoBehaviour {

    // Enumerado grado
    public Grado grado;

    // Método que se llama cuando se hace click sobre un elemento gráfico de tipo botón
    public void Grado()
    {
        // Se llama al método "GradoSeleccionado()", del GameManager, se le pasa el parámetro "grado".
        GameManager.sharedInstance.GradoSeleccionado(grado);
    }
}
