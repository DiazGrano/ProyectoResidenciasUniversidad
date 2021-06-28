using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelResultado : MonoBehaviour
{
    public static PanelResultado sharedInstance;

    [Header("Controlador animación del panel de resultados")]
    public Animator animator;

    public Text campoMensajeResultado;
    public Text campoResultado;


    public Button botonContinuar;


    private string mensajeAMostrar;

    private void Awake()
    {
        sharedInstance = this;
        this.GetComponent<Canvas>().enabled = false;
    }


    public void MostrarVentanaResultado(EstadoRespuesta respuesta, string resultado, TipoResultado tipoResultado = TipoResultado.Puntos, int cantidadCorrectas = 0, int cantidadIncorrectas = 0)
    {
        if (!PanelInstrucciones.sharedInstance.VentanaInstruccionesAbierta())
        {
            GameManager.sharedInstance.Pausar();

            switch (respuesta)
            {
                case EstadoRespuesta.Correcta:
                    mensajeAMostrar = GameManager.sharedInstance.mensajeRespuestaCorrecta;
                    break;
                case EstadoRespuesta.Incorrecta:
                    mensajeAMostrar = GameManager.sharedInstance.mensajeRespuestaIncorrecta;
                    break;
            }

            switch (tipoResultado)
            {
                case TipoResultado.Puntos:                    
                    if (int.Parse(resultado) > 0)
                    {
                        campoResultado.text = "+" + resultado + " puntos";
                    }
                    else
                    {
                        campoResultado.text = resultado + " puntos";
                    }
                    break;

                case TipoResultado.Tiempo:
                    campoResultado.text ="Tiempo: " + GameManager.sharedInstance.ConversionTiempo(resultado);
                    break;

                case TipoResultado.SecuenciaPuntos:
                    string puntos = "";
                    if (int.Parse(resultado) > 0)
                    {
                        puntos = "+" + resultado + " puntos";
                    }
                    else
                    {
                        puntos = resultado + " puntos";
                    }
                    campoResultado.text = "Respuestas correctas: " + cantidadCorrectas + "\n Respuestas incorrectas: " + cantidadIncorrectas + "\n" + puntos;
                    break;
            }
            campoMensajeResultado.text = mensajeAMostrar;

            StartCoroutine(AbrirPanelResultado());            
        } 
    }

    IEnumerator AbrirPanelResultado()
    {
        animator.SetTrigger("InicioResultado");
        this.GetComponent<Canvas>().enabled = true;
        yield return new WaitForSeconds(0.5f);

        botonContinuar.interactable = true;

        StopAllCoroutines();

    }

    IEnumerator CerrarPanelResultado()
    {
        botonContinuar.interactable = false;

        animator.SetTrigger("FinResultado");
        yield return new WaitForSeconds(0.5f);

        this.GetComponent<Canvas>().enabled = false;

        if (!PanelInstrucciones.sharedInstance.VentanaInstruccionesAbierta())
        {
            GameManager.sharedInstance.Jugar();
        }

        StopAllCoroutines();

    }



    public void CerrarVentanaResultado()
    {
        StartCoroutine(CerrarPanelResultado());
    }


    public bool VentanaResultadoAbierta()
    {
        return this.GetComponent<Canvas>().isActiveAndEnabled;
    }


}
