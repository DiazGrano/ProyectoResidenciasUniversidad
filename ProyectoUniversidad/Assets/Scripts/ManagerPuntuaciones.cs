using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPuntuaciones : MonoBehaviour
{
    public static ManagerPuntuaciones sharedInstance;


    private string identificadorMejorRegistro = "_mejorRegistro";
    private string identificadorRegistroActual = "_registroActual";

    private string identificadorTema;

    private TipoResultado tipoResultado;



    private void Awake()
    {
        sharedInstance = this;
    }

    public void ActualizarVariablesRegistro(IdentificadorTema auxIdentificadorTema, string auxSubIdentificadorTema = "", TipoResultado auxTipoResultado = TipoResultado.Puntos)
    {
        this.identificadorTema = auxIdentificadorTema.ToString() + auxSubIdentificadorTema;
        this.tipoResultado = auxTipoResultado;

        PlayerPrefs.SetString(identificadorTema + identificadorRegistroActual, "0");
        PlayerPrefs.Save();

        Debug.Log("Actualización de variables de registro de puntuaciones realizada con éxito");
    }

    public void GuardarRegistroActual(string registro, EstadoRespuesta estado = EstadoRespuesta.Correcta)
    {
        string registroAGuardar = "";
        switch (tipoResultado)
        {
            case TipoResultado.Puntos:
                int registroAnterior = int.Parse(ConsultarRegistroActual());
                int registroRecibido = int.Parse(registro);
                int sumaRegistros = registroAnterior + registroRecibido;

                registroAGuardar = sumaRegistros.ToString();

                if (int.Parse(registroAGuardar) > int.Parse(ConsultarMejorRegistro()))
                {
                    GuardarMejorRegistro(registroAGuardar);
                }

                break;
            case TipoResultado.Tiempo:
                registroAGuardar = registro;

                if (((int.Parse(registroAGuardar) < int.Parse(ConsultarMejorRegistro())) || int.Parse(ConsultarMejorRegistro()) == 0) && estado == EstadoRespuesta.Correcta)
                {
                    GuardarMejorRegistro(registroAGuardar);
                }

                break;
        }

        PlayerPrefs.SetString(identificadorTema + identificadorRegistroActual, registroAGuardar);

        PlayerPrefs.Save();
        Debug.Log("Registro actual guardado correctamente \n Identificador: " + identificadorTema +"\n Registro: " + registroAGuardar + "\n");
    }

    private void GuardarMejorRegistro(string registro)
    {
        PlayerPrefs.SetString(identificadorTema + identificadorMejorRegistro, registro);

        Debug.Log("Mejor registro guardado correctamente \n Identificador: " + identificadorTema + "\n Registro: " + registro + "\n");

    }

    public string ConsultarMejorRegistro()
    {
        return PlayerPrefs.GetString(identificadorTema + identificadorMejorRegistro, "0");
    }

    public string ConsultarRegistroActual()
    {
        return PlayerPrefs.GetString(identificadorTema + identificadorRegistroActual, "0");
    }


}
