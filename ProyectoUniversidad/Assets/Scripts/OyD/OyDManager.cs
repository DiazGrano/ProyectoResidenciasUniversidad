using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OyDManager : MonoBehaviour
{
    public static OyDManager sharedInstance;

    IdentificadorTema identificadorTema = IdentificadorTema.OyD;


    [Header("Varios")]
    public List<Sprite> listaArticulos = new List<Sprite>();

    public Image campoImagenArticulo;

    public GameObject gridRespuestas;
    public GameObject respuestaGenerica;

    public Text campoPrecio;
    public Text campoDescuento;

    [Header("Respuestas")]
    public int cantidadDecimales = 2;
    public int cantidadRespuestasAGenerar = 6;

    [Header("Puntuaciones por respuesta")]
    public int puntosCorrecta = 5;
    public int puntosIncorrecta = -3;

    float respuestaCorrecta;

    [Header("Puntuaciones")]
    public Text campoMejorPuntuacion;
    public Text campoPuntuacionActual;
    public int mejorPuntacion = 0;
    public int puntuacionActual = 0;

    [Header("Instrucciones de la actividad")]
    public GameObject objetoInstrucciones;


    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        PanelInstrucciones.sharedInstance.ModificarInstrucciones(objetoInstrucciones);

        ManagerPuntuaciones.sharedInstance.ActualizarVariablesRegistro(identificadorTema);
        ManagerCalificar.sharedInstance.ActualizarVariablesCalificar(identificadorTema, campoMejorPuntuacion, campoPuntuacionActual, TipoResultado.Puntos, puntosCorrecta, puntosIncorrecta);

        campoMejorPuntuacion.text = ManagerPuntuaciones.sharedInstance.ConsultarMejorRegistro();
        campoPuntuacionActual.text = ManagerPuntuaciones.sharedInstance.ConsultarRegistroActual();

        AbrirInstrucciones();

        IniciarJuego();

        GameManager.sharedInstance.EscenaActividadCargada();
    }

    public void IniciarJuego()
    {
        respuestaCorrecta = 0;

        int numRandom = Random.Range(0, listaArticulos.Count);

        campoImagenArticulo.sprite = listaArticulos[numRandom];

        float precioRandom = Random.Range(100, 9999);

        precioRandom = GameManager.sharedInstance.RedondearNumero(precioRandom, cantidadDecimales);

        campoPrecio.text = "$"+ precioRandom.ToString();

        float descuentoRandom = Random.Range(0, 100);

        campoDescuento.text = descuentoRandom.ToString() + "%";

        float auxDescuento = precioRandom * (descuentoRandom/100);
        respuestaCorrecta = precioRandom - auxDescuento;
        respuestaCorrecta = GameManager.sharedInstance.RedondearNumero(respuestaCorrecta, cantidadDecimales);

        Debug.Log("Respuesta correcta: " + respuestaCorrecta);

        List<string> listaRespuestas = GameManager.sharedInstance.GenerarListaNumerosAleatorios(cantidadRespuestasAGenerar, cantidadDecimales, respuestaCorrecta);
        List<string> listaRespuestasDesordenadas = GameManager.sharedInstance.DesordenarLista(listaRespuestas);
        GameManager.sharedInstance.AgregarOpcionesAContenedor(gridRespuestas, listaRespuestasDesordenadas, respuestaGenerica, respuestaCorrecta);



    }



    public void Reiniciar()
    {
        GameManager.sharedInstance.RemoveItemsFromParent(gridRespuestas);

        campoDescuento.text = " ";
        campoPrecio.text = " ";

        respuestaCorrecta = 0f;
        IniciarJuego();
    }





    public void BotonRegresar()
    {
        if (GameManager.sharedInstance.Jugando())
        {
            GameManager.sharedInstance.CerrarEscenaActividad();
        }
    }

    public void AbrirInstrucciones()
    {
        PanelInstrucciones.sharedInstance.AbrirInstrucciones();
    }

}
