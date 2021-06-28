using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum PPyMEstadoJuego
{
    Espera,
    Apuntar,
    Potenciar,
    Responder
}




public class PPyMManager : MonoBehaviour
{
    IdentificadorTema identificadorTema = IdentificadorTema.PPyM;
    public static PPyMManager sharedInstance;

    public TipoResultado PPyMTipoResultado;

    public string nombreEscena = "PulgadaPieYMilla";

    public PPyMEstadoJuego PPyMEstadoJuego;

    public Slider sliderPotencia;
    public GameObject cabezaTorreta;
    public GameObject proyectil;
    public GameObject puntoDeDisparo;
    public Text distanciaRecorridaText;
    public Text medidaDistanciaRecorridaText;
    public Text medidaConvertirDistanciaText;
    public GameObject gridRespuestas;
    public GameObject respuestaGenerica;
    

    private List<string> medidaMetrica = new List<string> {"centímetros", "metros", "kilómetros"};
    private List<string> medidaImperial = new List<string> { "pulgadas", "pies", "millas" };

    [Header("Respuestas")]
    public int cantidadDecimales = 4;
    public int cantidadRespuestasAGenerar = 4;

    [Header("Puntuaciones por respuesta")]
    public int puntosCorrecta = 5;
    public int puntosIncorrecta = -3;

    [Header("Puntuaciones")]
    public Text campoMejorPuntuacion;
    public Text campoPuntuacionActual;
    public int mejorPuntacion = 0;
    public int puntuacionActual = 0;

    float respuestaCorrecta = 0f;


    [Header("Etc")]
    public Transform puntoReferenciaCamara;

    public float velocidadPotencia = 1f;
    public float velocidadApuntar = 1f;
    public GameObject auxProyectil;

    public GameObject botonAccion;


    [Header("Instrucciones de la actividad")]
    public GameObject objetoInstrucciones;

    


    private void Awake()
    {
        sharedInstance = this;

        distanciaRecorridaText.text = " ";
        medidaDistanciaRecorridaText.text = " ";
        medidaConvertirDistanciaText.text = " ";

        Camara.sharedInstance.target = puntoReferenciaCamara.transform;

        PPyMEstadoJuego = PPyMEstadoJuego.Espera;
    }

    private void Start()
    {
        botonAccion.GetComponentInChildren<TextMeshProUGUI>().text = "Apuntar";
        PanelInstrucciones.sharedInstance.ModificarInstrucciones(objetoInstrucciones);
        
        ManagerPuntuaciones.sharedInstance.ActualizarVariablesRegistro(identificadorTema);
        ManagerCalificar.sharedInstance.ActualizarVariablesCalificar(identificadorTema, campoMejorPuntuacion, campoPuntuacionActual, TipoResultado.Puntos, puntosCorrecta, puntosIncorrecta);

        AbrirInstrucciones();

        campoMejorPuntuacion.text = ManagerPuntuaciones.sharedInstance.ConsultarMejorRegistro();
        campoPuntuacionActual.text = ManagerPuntuaciones.sharedInstance.ConsultarRegistroActual();
        GameManager.sharedInstance.EscenaActividadCargada();
    }

    public void BotonAccion()
    {
        if (GameManager.sharedInstance.Jugando())
        {
             switch (PPyMEstadoJuego)
            {
                case PPyMEstadoJuego.Espera:
                    StartCoroutine(Apuntar(false));
                    PPyMEstadoJuego = PPyMEstadoJuego.Apuntar;
                    botonAccion.GetComponentInChildren<TextMeshProUGUI>().text = "Potenciar";
                    break;

                case PPyMEstadoJuego.Apuntar:
                    StopAllCoroutines();
                    Transform auxRotatation = cabezaTorreta.transform;
                    StartCoroutine(Potenciar());
                    PPyMEstadoJuego = PPyMEstadoJuego.Potenciar;
                    botonAccion.GetComponentInChildren<TextMeshProUGUI>().text = "Disparar";
                    break;

                case PPyMEstadoJuego.Potenciar:
                    float multiplicadorPotenciaDisparo = sliderPotencia.GetComponent<Slider>().value;
                    auxProyectil = Instantiate(proyectil, puntoDeDisparo.transform.position, cabezaTorreta.transform.rotation);
                    Vector2 direccion = puntoDeDisparo.transform.position - cabezaTorreta.transform.position;
                    auxProyectil.GetComponent<Rigidbody2D>().AddForce(direccion * multiplicadorPotenciaDisparo, ForceMode2D.Impulse);

                    Camara.sharedInstance.target = auxProyectil.transform;


                    StopAllCoroutines();
                    StartCoroutine(Apuntar(true));
                    StartCoroutine(DistanciaRecorrida(auxProyectil));
                    PPyMEstadoJuego = PPyMEstadoJuego.Responder;

                    botonAccion.GetComponentInChildren<TextMeshProUGUI>().text = "Apuntar";
                    break;
            }   
        }
        

    }


    IEnumerator Potenciar()
    {
        
        bool alternarDireccionMedidorPotencia = false;
        float potenciaMax = sliderPotencia.GetComponent<Slider>().maxValue;
        float potenciaMin = sliderPotencia.GetComponent<Slider>().minValue;
        float auxPotencia = potenciaMin;
        sliderPotencia.GetComponent<Slider>().value = auxPotencia;

        while (true)
        {
            if (!alternarDireccionMedidorPotencia)
            {
                while (auxPotencia <= potenciaMax)
                {
                    auxPotencia += 0.08f;
                    sliderPotencia.GetComponent<Slider>().value = auxPotencia;
                    yield return new WaitForSeconds(velocidadPotencia * Time.deltaTime);
                }

                alternarDireccionMedidorPotencia = true;

            }
            else
            {
                while (auxPotencia >= potenciaMin)
                {
                    auxPotencia -= 0.08f;
                    sliderPotencia.GetComponent<Slider>().value = auxPotencia;
                    yield return new WaitForSeconds(velocidadPotencia * Time.deltaTime);

                }
                alternarDireccionMedidorPotencia = false;
            }
        }
    }

    IEnumerator Apuntar(bool regresar)
    {
        

        bool alternarSentidoMovimiento = false;
        float anguloMaximo = 60f;
        float anguloMinimo = 0f;
        float velocidad = 100f;
        Vector3 rotacionCabeza = cabezaTorreta.transform.eulerAngles;

        if (!regresar)
        {
            while (true)
            {
                if (!alternarSentidoMovimiento)
                {
                    while (rotacionCabeza.z < (anguloMaximo - (velocidad * Time.deltaTime)))
                    {
                        rotacionCabeza += new Vector3(0, 0, velocidad * Time.deltaTime);
                        cabezaTorreta.transform.eulerAngles = rotacionCabeza;
                        yield return new WaitForSeconds(velocidadApuntar * Time.deltaTime);
                    }
                    alternarSentidoMovimiento = true;
                }
                else
                {
                    while (rotacionCabeza.z > (anguloMinimo + (velocidad * Time.deltaTime)))
                    {
                        rotacionCabeza -= new Vector3(0, 0, velocidad * Time.deltaTime);
                        cabezaTorreta.transform.eulerAngles = rotacionCabeza;
                        yield return new WaitForSeconds(velocidadApuntar * Time.deltaTime);
                    }
                    alternarSentidoMovimiento = false;
                }
            }
        }
        else
        {
            while (rotacionCabeza.z > (anguloMinimo + (velocidad * Time.deltaTime)))
            {
                rotacionCabeza -= new Vector3(0, 0, velocidad * Time.deltaTime);
                cabezaTorreta.transform.eulerAngles = rotacionCabeza;
                yield return new WaitForSeconds(1 * Time.deltaTime);
            }
            sliderPotencia.GetComponent<Slider>().value = sliderPotencia.GetComponent<Slider>().minValue;
            yield break;
        }


    }

    IEnumerator DistanciaRecorrida(GameObject proyectil)
    {
        float distanciaRecorrida = 0f;
        float auxDistanciaRecorrida = 0f;
        int medidaMetricaRandom = Random.Range(0, medidaMetrica.Count);
        medidaDistanciaRecorridaText.text = medidaMetrica[medidaMetricaRandom];

        while (proyectil.GetComponent<Rigidbody2D>().velocity.x > 0.01f)
        {
            distanciaRecorrida = (proyectil.transform.position - cabezaTorreta.transform.position).x;


            switch (medidaMetricaRandom)
            {
                case 0:
                    auxDistanciaRecorrida = distanciaRecorrida * 100;
                    break;
                case 1:
                    auxDistanciaRecorrida = distanciaRecorrida;
                    break;
                case 2:
                    auxDistanciaRecorrida = distanciaRecorrida * 0.001f;
                    break;
            }

            auxDistanciaRecorrida = (float) System.Math.Round(double.Parse(auxDistanciaRecorrida.ToString()), 4);

            distanciaRecorridaText.text = auxDistanciaRecorrida.ToString();

            yield return new WaitForSeconds(Time.deltaTime);
        }

        CargarProblema(medidaMetrica[medidaMetricaRandom], auxDistanciaRecorrida);

        

        yield break;
    }


    public void CargarProblema(string medidaMetrica, float distanciaRecorrida)
    {
        float auxDistanciaRecorridaEnMetros = 0f;
        switch (medidaMetrica)
        {
            case "centímetros":
                auxDistanciaRecorridaEnMetros = distanciaRecorrida * 0.01f;
                break;
            case "metros":
                auxDistanciaRecorridaEnMetros = distanciaRecorrida;
                break;
            case "kilómetros":
                auxDistanciaRecorridaEnMetros = distanciaRecorrida * 1000f;
                break;
        }

        int medidaImperialRandom = Random.Range(0, medidaImperial.Count);
        
        medidaConvertirDistanciaText.text = medidaImperial[medidaImperialRandom];

        respuestaCorrecta = 0f;

        switch (medidaImperial[medidaImperialRandom])
        {
            case "pulgadas":
                respuestaCorrecta = auxDistanciaRecorridaEnMetros * 39.3701f;
                break;

            case "pies":
                respuestaCorrecta = auxDistanciaRecorridaEnMetros * 3.28084f;
                break;

            case "millas":
                respuestaCorrecta = auxDistanciaRecorridaEnMetros * 0.000621371f;
                break;
        }

        respuestaCorrecta = GameManager.sharedInstance.RedondearNumero(respuestaCorrecta, cantidadDecimales);

        List<string> listaRespuestas = GameManager.sharedInstance.GenerarListaNumerosAleatorios(cantidadRespuestasAGenerar, cantidadDecimales, respuestaCorrecta);
        List<string> listaRespuestasDesordenadas = GameManager.sharedInstance.DesordenarLista(listaRespuestas);
        GameManager.sharedInstance.AgregarOpcionesAContenedor(gridRespuestas, listaRespuestasDesordenadas, respuestaGenerica, respuestaCorrecta);

        Debug.Log(distanciaRecorrida + " " + medidaMetrica + " es igual a " + respuestaCorrecta + " " +  medidaImperial[medidaImperialRandom]);

        Camara.sharedInstance.target = puntoReferenciaCamara.transform;

    }


    public void Reiniciar()
    {
        Destroy(auxProyectil);    

            GameManager.sharedInstance.RemoveItemsFromParent(gridRespuestas);

            distanciaRecorridaText.text = " ";
            medidaDistanciaRecorridaText.text = " ";
            medidaConvertirDistanciaText.text = " ";

            respuestaCorrecta = -1f;

            PPyMEstadoJuego = PPyMEstadoJuego.Espera;
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
