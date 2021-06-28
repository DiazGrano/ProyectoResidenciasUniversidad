using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ManagerCalificar : MonoBehaviour
{
    public static ManagerCalificar sharedInstance;

    private IdentificadorTema identificadorTema;

    private Text campoMejorPuntuacion;
    private Text campoPuntuacionActual;
    private int puntosRespuestaCorrecta;
    private int puntosRespuestaIncorrecta;
    private TipoResultado tipoResultado;

    private void Awake()
    {
        sharedInstance = this;
    }

    public void ActualizarVariablesCalificar(IdentificadorTema identificador, Text auxCampoMejorPuntuacion, Text auxCampoPuntuacionActual,
        TipoResultado auxTipoResultado = TipoResultado.Puntos, int auxPuntosRespuestaCorrecta = 0, int auxPuntosRespuestaIncorrecta = 0)
    {
        this.campoMejorPuntuacion = auxCampoMejorPuntuacion;
        this.campoPuntuacionActual = auxCampoPuntuacionActual;
        this.puntosRespuestaCorrecta = auxPuntosRespuestaCorrecta;
        this.puntosRespuestaIncorrecta = auxPuntosRespuestaIncorrecta;
        this.identificadorTema = identificador;
        this.tipoResultado = auxTipoResultado;
    }


    public void Calificar(EstadoRespuesta respuesta, int tiempo = 0, int cantidadCorrectas = 0, int cantidadIncorrectas = 0)
    {
        if (GameManager.sharedInstance.Jugando())
        {
            switch (tipoResultado)
            {
                case TipoResultado.Puntos:
                    int puntos = 0;
                    switch (respuesta)
                    {
                        case EstadoRespuesta.Incorrecta:
                            puntos = puntosRespuestaIncorrecta;
                            break;

                        case EstadoRespuesta.Correcta:
                            puntos = puntosRespuestaCorrecta;
                            break;
                    }
                    PanelResultado.sharedInstance.MostrarVentanaResultado(respuesta, puntos.ToString());
                    ManagerPuntuaciones.sharedInstance.GuardarRegistroActual(puntos.ToString());
                    campoPuntuacionActual.text = ManagerPuntuaciones.sharedInstance.ConsultarRegistroActual();
                    break;

                case TipoResultado.Tiempo:
                    PanelResultado.sharedInstance.MostrarVentanaResultado(respuesta, tiempo.ToString(), tipoResultado);
                    ManagerPuntuaciones.sharedInstance.GuardarRegistroActual(tiempo.ToString(), respuesta);
                    campoPuntuacionActual.text = "0 : 0";
                    break;

                case TipoResultado.SecuenciaPuntos:
                    int resultado = ((cantidadCorrectas * puntosRespuestaCorrecta) + (cantidadIncorrectas * puntosRespuestaIncorrecta));
                    ManagerPuntuaciones.sharedInstance.GuardarRegistroActual(resultado.ToString(), respuesta);
                    PanelResultado.sharedInstance.MostrarVentanaResultado(respuesta, resultado.ToString(), tipoResultado, cantidadCorrectas, cantidadIncorrectas);
                    campoPuntuacionActual.text = ManagerPuntuaciones.sharedInstance.ConsultarRegistroActual();
                    break;
            }
            campoMejorPuntuacion.text = ManagerPuntuaciones.sharedInstance.ConsultarMejorRegistro();

            Reiniciar();
        }
    }




    private void Reiniciar()
    {
        switch (identificadorTema)
        {
            case IdentificadorTema.VaC:
                VaCManager.sharedInstance.Reiniciar();
                break;
            case IdentificadorTema.EQSD:
                EQSDManager.sharedInstance.Reiniciar();
                break;
            case IdentificadorTema.OyD:
                OyDManager.sharedInstance.Reiniciar();
                break;
            case IdentificadorTema.EIVA:
                EIVAManager.sharedInstance.Reiniciar();
                break;
            case IdentificadorTema.CeeS:
                CeeSManager.sharedInstance.Reiniciar();
                break;
            case IdentificadorTema.PPyM:
                PPyMManager.sharedInstance.Reiniciar();
                break;
        }
    }

}
