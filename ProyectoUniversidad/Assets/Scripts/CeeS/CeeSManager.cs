using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CeeSManager : MonoBehaviour
{
    public static CeeSManager sharedInstance;

    private IdentificadorTema identificadorTema = IdentificadorTema.CeeS;
    private TipoResultado tipoResultado = TipoResultado.Tiempo;

    [Header("Objeto que se sobrepone a todos los demás")]
    //Para evitar que al arrastrar el objeto, este se vea superpuesto por otros objetos
    public Transform superPadre;

    public Text campoTiempoActual;
    public Text campoMejorTiempo;
    public Text campoOrdenNumeros;


    public GameObject contenedorOpciones;
    public GameObject contenedorRespuestas;
    public GameObject respuestaArrastrableGenerica;
    public GameObject slot;

    public int cantidadOpciones = 10;

    public int auxSeconds;

    private string tiempoActual = "0 : 0";

    int auxOrden;
    private List<string> modosOrden =  new List<string>() {"Menor a mayor", "Mayor a menor"};

    [Header("Instrucciones")]
    public GameObject objetoInstrucciones;

    public List<float> listaRespuestasCorrectas = new List<float>();

    private void Awake()
    {
        sharedInstance = this;
    }


    private void Start()
    {
        StartGame();

        PanelInstrucciones.sharedInstance.ModificarInstrucciones(objetoInstrucciones);
        AbrirInstrucciones();

        ManagerCalificar.sharedInstance.ActualizarVariablesCalificar(identificadorTema, campoMejorTiempo, campoTiempoActual, tipoResultado);
        ManagerPuntuaciones.sharedInstance.ActualizarVariablesRegistro(identificadorTema, "", tipoResultado);
        campoMejorTiempo.text = GameManager.sharedInstance.ConversionTiempo(ManagerPuntuaciones.sharedInstance.ConsultarMejorRegistro());

        GameManager.sharedInstance.EscenaActividadCargada();

        GameManager.sharedInstance.superPadre = this.superPadre;

    }


    public void AbrirInstrucciones()
    {
        PanelInstrucciones.sharedInstance.AbrirInstrucciones();
    }





    public void StartGame()
    {
        

        auxOrden = Random.Range(0, 2);
        campoOrdenNumeros.text = modosOrden[auxOrden];


        List<GameObject> opcionesGeneradas = new List<GameObject>();
        List<string> listaNumeros = GenerarNumerosAleatorios(cantidadOpciones, -999f, 999f);
        for (int i = 0; i < cantidadOpciones; i++)
        {
            GameObject auxSlotOpcion = Instantiate(slot, contenedorOpciones.transform);
            Instantiate(slot, contenedorRespuestas.transform);

            opcionesGeneradas.Add(Instantiate(respuestaArrastrableGenerica, auxSlotOpcion.transform));
            opcionesGeneradas[i].GetComponentInChildren<Text>().text = listaNumeros[i];

        }

        listaRespuestasCorrectas = OrdenarListaNumeros(listaNumeros, modosOrden[auxOrden]);

        foreach (var respuesta in listaRespuestasCorrectas)
        {
            Debug.Log(respuesta);
        }

        StartCoroutine(CeeSTimeManager());
    }

    private List<float> ConvertirListaStringAFloat(List<string> lista)
    {
        List<float> listaConvertida = new List<float>();
        for (int i = 0; i < lista.Count; i++)
        {
            string[] auxString = lista[i].Split('/');

            if (auxString.Length > 1)
            {
                float auxNum = (float.Parse(auxString[0]) / float.Parse(auxString[1]));
                listaConvertida.Add(auxNum);
            }
            else
            {
                listaConvertida.Add(float.Parse(auxString[0]));
            }
        }


        return listaConvertida;
    }

    private List<float> OrdenarListaNumeros(List<string> lista, string orden)
    {
        List<float> auxLista = ConvertirListaStringAFloat(lista);
        List<float> auxLista2 = new List<float>();

        for (int i = 0; i < auxLista.Count; i++)
        {
            auxLista2.Add(auxLista[i]);
            for (int j = 0; j < auxLista2.Count; j++)
            {
                switch (orden)
                {
                    case "Menor a mayor":
                        if (auxLista2[i] < auxLista2[j])
                        {
                            float auxNum = auxLista2[j];
                            auxLista2[j] = auxLista2[i];
                            auxLista2[i] = auxNum;
                        }
                        break;

                    case "Mayor a menor":
                        if (auxLista2[i] > auxLista2[j])
                        {
                            float auxNum = auxLista2[j];
                            auxLista2[j] = auxLista2[i];
                            auxLista2[i] = auxNum;
                        }
                        break;
                }

            }
        }
        return auxLista2;
    }

    private List<string> GenerarNumerosAleatorios(int cantidad, float numMenor, float numMayor)
    {
        List<string> listaNumerosAleatorios = new List<string>();

        for (int i = 0; i < cantidad; i++)
        {
            float auxNumAleatorio = GameManager.sharedInstance.RedondearNumero(Random.Range(numMenor, numMayor), 0);

            if (Random.Range(0, 2) == 0)
            {
                string fraccion = auxNumAleatorio.ToString() + "/" + Random.Range(2, 10).ToString();
                listaNumerosAleatorios.Add(fraccion);
            }
            else
            {
                listaNumerosAleatorios.Add(auxNumAleatorio.ToString());
            }
        }

        return listaNumerosAleatorios;
    }




    IEnumerator CeeSTimeManager()
    {
        while (true)
        {
            yield return new WaitWhile(() => GameManager.sharedInstance.Jugando() != true);

            yield return new WaitForSeconds(1);

            if (GameManager.sharedInstance.Jugando())
            {
                auxSeconds++;

                tiempoActual = GameManager.sharedInstance.ConversionTiempo(auxSeconds.ToString());
                campoTiempoActual.text = tiempoActual;
            }

        }
    }

    private EstadoRespuesta Comprobar()
    {

        List<float> listaRespuestasDadas = new List<float>();

        List<string> auxListaRespuestasDadas = new List<string>();
        for (int i = 0; i < contenedorRespuestas.transform.childCount; i++)
        {
            
            if (contenedorRespuestas.transform.GetChild(i).transform.childCount > 0)
            {
                auxListaRespuestasDadas.Add(contenedorRespuestas.transform.GetChild(i).GetChild(0).GetComponentInChildren<Text>().text);
            }
            else
            {
                return EstadoRespuesta.Incorrecta;
            }
        }
        listaRespuestasDadas = ConvertirListaStringAFloat(auxListaRespuestasDadas);

        Debug.Log(listaRespuestasCorrectas.Count);

        for (int i = 0; i < listaRespuestasCorrectas.Count; i++)
        {
            Debug.Log("Respuesta: " + listaRespuestasDadas[i] + " \n Respuesta correcta: " + listaRespuestasCorrectas[i]);
        }

        for (int i = 0; i < listaRespuestasCorrectas.Count; i++)
        {
            if (listaRespuestasCorrectas[i] != listaRespuestasDadas[i])
            {
                return EstadoRespuesta.Incorrecta;
            }
        }
        return EstadoRespuesta.Correcta;
    }

    public void Calificar()
    {
        if (GameManager.sharedInstance.Jugando())
        {
            StopAllCoroutines();

            ManagerCalificar.sharedInstance.Calificar(Comprobar(), auxSeconds);
        }
    }


    public void Reiniciar()
    {
        StopAllCoroutines();
        tiempoActual = "0 : 0";
        campoTiempoActual.text = tiempoActual;
        auxSeconds = 0;
        listaRespuestasCorrectas.Clear();
        campoMejorTiempo.text = GameManager.sharedInstance.ConversionTiempo(ManagerPuntuaciones.sharedInstance.ConsultarMejorRegistro());
        GameManager.sharedInstance.RemoveItemsFromParent(contenedorOpciones);
        GameManager.sharedInstance.RemoveItemsFromParent(contenedorRespuestas);
        StartGame();
        
    }



    public void BotonRegresar()
    {

        if (GameManager.sharedInstance.Jugando())
        {
            GameManager.sharedInstance.CerrarEscenaActividad();
        }
    }

}
