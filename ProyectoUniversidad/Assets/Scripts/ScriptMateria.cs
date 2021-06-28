using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptMateria : MonoBehaviour {
    // Enumerador materia
    public Materia materia;

    // Método que se llama cuando se hace click sobre un elemento gráfico de tipo botón
    public void Materia()
    {
        // Se llama al método "MateriaSeleccionada()", del GameManager, se le pasa el parámetro "materia".
        GameManager.sharedInstance.MateriaSeleccionada(materia);
    }
}
