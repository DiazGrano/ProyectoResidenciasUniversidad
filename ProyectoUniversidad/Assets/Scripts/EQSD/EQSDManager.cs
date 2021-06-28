using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class EQSDManager : MonoBehaviour
{

    public static EQSDManager sharedInstance;

    IdentificadorTema identificadorTema = IdentificadorTema.EQSD;

    [Header("Objeto que se sobrepone a todos los demás")]
    //Para evitar que al arrastrar el objeto, este se vea superpuesto por otros objetos
    public Transform superPadre;

    public List<GameObject> listaObjetos = new List<GameObject>();
    public GameObject objetoRespuestaGenerica;


    public List<Transform> puntosRefenciaSpawn = new List<Transform>();

    public List<GameObject> puntoReferenciaRotacion = new List<GameObject>();

    public List<GameObject> controladorObjeto = new List<GameObject>();



    public GameObject contenedorOpciones;
    public List<GameObject> contenedoresRespuestas = new List<GameObject>();





    [Header("Puntuaciones por respuesta")]
    public int puntosCorrecta = 5;
    public int puntosIncorrecta = -3;


    [Header("Puntuaciones")]
    public Text campoMejorPuntuacion;
    public Text campoPuntuacionActual;
    public int mejorPuntacion = 0;
    public int puntuacionActual = 0;


    [Header("Instrucciones de la actividad")]
    public GameObject objetoInstrucciones;


    public List<string> listaRespuestasCorrectas =  new List<string>();


    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        PanelInstrucciones.sharedInstance.ModificarInstrucciones(objetoInstrucciones);
        ManagerPuntuaciones.sharedInstance.ActualizarVariablesRegistro(identificadorTema);
        ManagerCalificar.sharedInstance.ActualizarVariablesCalificar(identificadorTema, campoMejorPuntuacion, campoPuntuacionActual, TipoResultado.SecuenciaPuntos, puntosCorrecta, puntosIncorrecta);

        campoMejorPuntuacion.text = ManagerPuntuaciones.sharedInstance.ConsultarMejorRegistro();
        campoPuntuacionActual.text = ManagerPuntuaciones.sharedInstance.ConsultarRegistroActual();

        AbrirInstrucciones();


        IniciarJuego();

        GameManager.sharedInstance.EscenaActividadCargada();
        GameManager.sharedInstance.superPadre = this.superPadre;
    }




    public void IniciarJuego()
    {
        List<string> listaCaracteristicas = new List<string>();
        List<string> listaCaracteristicasDesordenadas = new List<string>();

        for (int i = 0; i < 2; i++)
        {
            
            int auxRandom = Random.Range(0, listaObjetos.Count);

            GameObject auxObjeto = Instantiate(listaObjetos[auxRandom], puntosRefenciaSpawn[i]);

            GameObject puntoRotacion = Instantiate(puntoReferenciaRotacion[i], auxObjeto.transform);
            puntoRotacion.GetComponent<PuntoReferenciaRotacion>().controlador = controladorObjeto[i].GetComponent<RotarObjeto>();


            listaCaracteristicas.Add(auxObjeto.GetComponent<Objeto>().poligonodeLaBase);
            listaCaracteristicas.Add(auxObjeto.GetComponent<Objeto>().numCarasLaterales.ToString());
            listaCaracteristicas.Add(auxObjeto.GetComponent<Objeto>().aristas.ToString());
            listaCaracteristicas.Add(auxObjeto.GetComponent<Objeto>().vertices.ToString());

        }

        listaRespuestasCorrectas = listaCaracteristicas;

        listaCaracteristicasDesordenadas = GameManager.sharedInstance.DesordenarLista(listaCaracteristicas);

        for (int i = 0; i < listaCaracteristicasDesordenadas.Count; i++)
        {
            GameObject auxOpcionRespuesta = Instantiate(objetoRespuestaGenerica, contenedorOpciones.transform);
            auxOpcionRespuesta.GetComponentInChildren<Text>().text = listaCaracteristicasDesordenadas[i];
        }

    }


    public void Comprobar()
    {
        int x = 0;
        int respuestasCorrectas = 0;
        int respuestasIncorrectas = 0;

        Debug.Log(this.listaRespuestasCorrectas.Count);

        for (int i = 0; i < 2; i++)
        {

            for (int j = 0; j < contenedoresRespuestas[i].transform.childCount; j++)
            {
                if (contenedoresRespuestas[i].transform.GetChild(j).transform.childCount>0)
                {
                    if (listaRespuestasCorrectas[x].ToString() == contenedoresRespuestas[i].transform.GetChild(j).GetChild(0).GetComponentInChildren<Text>().text)
                    {
                        respuestasCorrectas++;
                        
                    }
                    else
                    {
                        respuestasIncorrectas++;

                    }
                    Debug.Log("Respuesta correcta: " + listaRespuestasCorrectas[x].ToString() + "\n Respuesta dada: " + contenedoresRespuestas[i].transform.GetChild(j).GetChild(0).GetComponentInChildren<Text>().text);
                    
                }
                else
                {
                    respuestasIncorrectas++;
                }
                x++;
                
            }
        }
        int resultado = ((respuestasCorrectas * puntosCorrecta) + (respuestasIncorrectas * puntosIncorrecta));
        Debug.Log("Calificación: " + resultado);

        Debug.Log("Cantidad respuestas correctas: " + respuestasCorrectas);
        Debug.Log("Cantidad respuestas incorrectas: " + respuestasIncorrectas);

        EstadoRespuesta estado = EstadoRespuesta.Correcta;

        if (respuestasIncorrectas > 0)
        {
            estado = EstadoRespuesta.Incorrecta;
        }

        ManagerCalificar.sharedInstance.Calificar(estado, 0, respuestasCorrectas, respuestasIncorrectas);

    }


    public void Reiniciar()
    {
        //Calificar();

        //      listaRespuestasCorrectas.Clear();

        GameManager.sharedInstance.RemoveItemsFromParent(contenedorOpciones);

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < contenedoresRespuestas[i].transform.childCount; j++)
            {
                GameManager.sharedInstance.RemoveItemsFromParent(contenedoresRespuestas[i].transform.GetChild(j).gameObject);
            }
            GameManager.sharedInstance.RemoveItemsFromParent(puntosRefenciaSpawn[i].gameObject);
        }

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
