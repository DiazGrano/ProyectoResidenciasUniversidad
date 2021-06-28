using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBloque : MonoBehaviour {
    // Enumerador bloque
    public Bloque bloque;

    // Método que se llama cuando se hace click sobre un elemento gráfico de tipo botón
    public void Bloque()
    {
        // Se llama al método "BloqueSeleccionado()", del GameManager, se le pasa el parámetro "bloque".
        GameManager.sharedInstance.BloqueSeleccionado(bloque);
    }
}
