using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum Tipo
{
    Grado,
    Materia,
    Bloque,
    Actividad
}

public enum Grado
{
    Ninguno,
    Primero,
    Segundo,
    Tercero,
    Cuarto,
    Quinto,
    Sexto
}

public enum Materia
{
    Ninguno,
    Español,
    Matemáticas
}

public enum Bloque
{
    Ninguno,
    I,
    II,
    III,
    IV,
    V,
    VI,
    VII,
    VIII,
    IX,
    X
}

public enum Seccion
{
    Bienvenida,
    Grados,
    Materias,
    BloquesActividades,
    Actividad
}

public enum TipoResultado
{
    Puntos,
    Tiempo,
    SecuenciaPuntos
}


public enum EstadoRespuesta
{
    Incorrecta,
    Correcta
}

public enum EstadoJuego
{
    Pause,
    Jugando
}

public enum IdentificadorTema
{
    VaC,
    EQSD,
    OyD,
    EIVA,
    CeeS,
    PPyM
}

public class GameManager : MonoBehaviour {
    public static GameManager sharedInstance;

    [Header("Controlador animaciones de transiciones de escenas")]
    public Animator animator;

    // Estas variables corresponden a los elementos que han sido seleccionados
    [Header("Variables de las actividades")]
    public Grado gradoActual;
    public Materia materiaActual;
    public Bloque bloqueActual;

    [Header("Estado del juego")]
    public Seccion seccionActual;
    public EstadoJuego estadoJuego;

    [Header("GameObjects genéricos")]
    public GameObject gradoGenerico;
    public GameObject materiaGenerica;
    public GameObject bloqueGenerico;

    [Header("Lista de todas las actividades")]
    public List<GameObject> listaActividades = new List<GameObject>();

    [Header("Contenedores")]
    public GameObject gridGrados;
    public GameObject gridMaterias;
    public GameObject gridBloques;
    public GameObject gridActividades;

    [Header("Intefaces")]
    public Canvas canvasPrincipal;
    public Canvas canvasBienvenida;
    public Canvas canvasGrados;
    public Canvas canvasMaterias;
    public Canvas canvasBloquesActividades;
    

    public GameObject mainMenu;

    public GameObject puntoReferenciaEscena;

    public GameObject escenaActividadActual;

    [Header("Mensajes resultado")]
    [TextArea]
    public string mensajeRespuestaCorrecta;
    [TextArea]
    public string mensajeRespuestaIncorrecta;


    [Header("Objeto que se sobrepone a todos los demás")]
    //Para evitar que al arrastrar el objeto, este se vea superpuesto por otros objetos
    public Transform superPadre;

    // Este método es el primero que se llama.
    private void Awake()
    {
        // Se inicializa la variable "sharedInstance", haciendo referencia a esta misma clase, esto es para que solo exista una sola instancia.
        sharedInstance = this;

        // Se llama al "EstadoBienvenida()"
        //EstadoBienvenida();
        this.GradoSeleccionado(Grado.Sexto);
        this.MateriaSeleccionada(Materia.Matemáticas);
        EstadoBloquesActividades();
        Jugar();


    }

    private void Start()
    {
        if (!mainMenu.activeSelf)
        {
            mainMenu.SetActive(true);
        }
        if (!canvasPrincipal.isActiveAndEnabled)
        {
            canvasPrincipal.enabled = true;
        }

    }

    /* Método encargado de manejar los estados actuales del programa, siendo estos:
          Bienvenida
          Grados
          Materias
          BloquesActividades
          Actividad
      Estos representan la ventana actual que se está mostrando, ninguna ventana se puede sobreponer a otra, por lo tanto,
      si una ventana está activa, las demás se deben desactivar
    
     Recibe una variable de tipo EstadoActual, dependiendo de la variable, es la acción que se realizará
     */
    private void ManagerSecciones(Seccion seccion)
    {
        this.seccionActual = seccion;

        switch (seccionActual)
        {
            case Seccion.Bienvenida:
                canvasBienvenida.enabled = true;
                canvasGrados.enabled = false;
                canvasMaterias.enabled = false;
                canvasBloquesActividades.enabled = false;
                break;
            case Seccion.Grados:
                canvasBienvenida.enabled = false;
                canvasGrados.enabled = true;
                canvasMaterias.enabled = false;
                canvasBloquesActividades.enabled = false;
                break;
            case Seccion.Materias:
                canvasBienvenida.enabled = false;
                canvasGrados.enabled = false;
                canvasMaterias.enabled = true;
                canvasBloquesActividades.enabled = false;
                break;
            case Seccion.BloquesActividades:
                canvasBienvenida.enabled = false;
                canvasGrados.enabled = false;
                canvasMaterias.enabled = false;
                canvasBloquesActividades.enabled = true;
                break;
        }
    }


    private void ManagerEstadosJuego(EstadoJuego estado)
    {
        this.estadoJuego = estado;
    }

    // Método encargado de cargar los elementos (botones) de una sección en específico, recibe el tipo de sección que deberá rellenar con los elementos disponibles
    private void CargarElementos(Tipo tipo)
    {
        // Se limpia la variable de tipo lista "listaElementosSinRepetir"
        List<GameObject> listaElementosSinRepetir = null;

        // Se ingresan a la lista los elementos de tipo "tipo" que retorne la llamada al método "ObtenerListaDeElementosAInstanciar()".
        listaElementosSinRepetir = ObtenerListaDeElementosAInstanciar(tipo);

        // Se llama al método "InstanciarElemento()", para crear los elementos.
        
        for (int i = 0; i < listaElementosSinRepetir.Count; i++)
        {
            InstanciarElementoAMenu(tipo, listaElementosSinRepetir[i]);
        }
    }


    // Método encargado de obtener la lista de elementos del tipo solicitado, y si es el caso, sin repetir, este método recibe una variable con el tipo elemento deseado.
    private List<GameObject> ObtenerListaDeElementosAInstanciar(Tipo tipo)
    {
        // Se obtiene la lista de elementos del tipo deseado llamando al método "ObtenerListaDeElementosDeTipo()", mandándole como parámetro la variable "tipo",
        // al llamar a este método, este retornará una lista con los elementos del tipo deseado.
        List<GameObject> auxListaElementosDelTipo = ObtenerListaDeElementosDeTipo(tipo);

        // Se obtiene la lista de elementos sin repetir llamando al método "ObtenerListaElementosSinRepetir()", mandándole como parámetros la variable "tipo",
        // y la lista de elementos del tipo deseado, este método retornará una lista con los elementos del tipo deseado y sin repetir.
        List<GameObject> auxListaElementosSinRepetir = ObtenerListaElementosSinRepetir(tipo, auxListaElementosDelTipo);


        List<GameObject> auxListaDeElementosAInstanciar = ObtenerListaDeElementosOrdenados(auxListaElementosSinRepetir, tipo);

        // Una vez terminada de armar la lista de elementos sin repetir, habiendo comprobado todos los elementos de cierto tipo en la lista de actividades y habiendo sido agregados los que no estaban repetidos a la lista de elementos
        // sin repetir, esta lista será retornada.
        return auxListaDeElementosAInstanciar;
    }

    // Método encargano de obtener la lista de ementos del tipo solicitado.
    private List<GameObject> ObtenerListaDeElementosDeTipo(Tipo tipo)
    {

        // Lista auxiliar para obtener la lista de lementos del tipo deseado.
        List<GameObject> auxListaElementos = new List<GameObject>();

        // Variable auxiliar para purgar los elementos repetidos de la lista de actividades, si su valor es verdadero, entonces el elemento será descartado.
        bool descartarElemento = false;

        // Dependiendo el tamaño de la lista de las actividades (esta es una lista con todas las actividades disponibles), son las veces que se realizará el ciclo.
        for (int i = 0; i < listaActividades.Count; i++)
        {

            // Dependiendo del valor de la variable "tipo", es la acción que se realizará.
            switch (tipo)
            {
                case Tipo.Grado:
                    // Si es de tipo "Grado", no se descartarán elementos.
                    break;

                case Tipo.Materia:
                    // Si es de tipo "Materia", eso quiere decir que entonces ya se ha seleccionado un grado, por lo tanto, el grado de las 
                    // materias a mostrar debe coincidir, por lo tanto, no se agregará el elemento en la posición "i" de la lista
                    // "listaActividades[]" si su enumerador "Grado" no es igual al de la variable "gradoActual".
                    if (this.gradoActual != listaActividades[i].GetComponent<ScriptActividad>().grado)
                    {
                        descartarElemento = true;
                    }
                    break;

                case Tipo.Bloque:
                    /*
                     Si es de tipo "Bloque", eso quiere decir que se ha seleccionado un grado y una materia, por lo tanto,
                     el grado y materia de los bloques a mostrar deben coincidir, por lo tanto, no se agregará
                     el elemento en la posición "i" de la lista"listaActividades[]" si su enumeradores "Grado"
                     y "Materia" no son iguales a los de las variable "gradoActual" y "materiaActual", correspondientemente.
                    */
                    if (this.materiaActual != listaActividades[i].GetComponent<ScriptActividad>().materia || this.gradoActual != listaActividades[i].GetComponent<ScriptActividad>().grado)
                    {
                        descartarElemento = true;
                    }
                    break;

                case Tipo.Actividad:
                    /*
                     Si es de tipo "Actividad", eso quiere decir que se ha seleccionado un grado, una materia y un bloque, por lo tanto,
                     el grado, materia y bloque de las actividades a mostrar deben coincidir, por lo tanto, no se agregará 
                     el elemento en la posición "i" de la lista"listaActividades[]" si su enumeradores "Grado", "Materia" y "Bloque"
                     no son iguales a los de las variable "gradoActual", "materiaActual" y "bloqueActual", correspondientemente.
                    */
                    if (this.materiaActual != listaActividades[i].GetComponent<ScriptActividad>().materia || this.gradoActual != listaActividades[i].GetComponent<ScriptActividad>().grado || this.bloqueActual != listaActividades[i].GetComponent<ScriptActividad>().bloque)
                    {
                        descartarElemento = true;
                    }
                    break;
            }

            // si el valor de la variable "descartarElemento" es falso, eso quiere decir que no se va a descartar el
            // elemento en la posición "i" de la lista "listaActividades", por lo tanto, este será agregado a la lista
            // "auxListaElementos", la cual contiene la los elementos del tipo solicitado.
            if (!descartarElemento)
            {
                auxListaElementos.Add(listaActividades[i]);
            }// Si el valor de la variable "descartarElemento" es verdadero, entonces se cambia su valor a falso, para
            // poder realizar el ciclo de nuevo y sin problemas.
            else
            {
                descartarElemento = false;
            }
        }


        return auxListaElementos;
    }
    
    // Método encargado de obtener la lista de ementos sin repetir, dada una lista
    private List<GameObject> ObtenerListaElementosSinRepetir(Tipo tipo, List<GameObject> auxListaElementos)
    {
        // Lista axualiar para obtener los elementos sin repetir, dada una lista de elementos del tipo deseado.
        List<GameObject> auxListaElementosSinRepetir = new List<GameObject>();


        // Variable auxiliar para purgar los elementos repetidos de la lista de actividades, si su valor es verdadero, entonces el elemento será descartado.
        bool descartarElemento = false;

        // Obtener la lista de todos los elementos a agregar sin repetir.
        // Por cada elemento en la lista de actividades, el ciclo se realizará una vez.
        for (int i = 0; i < auxListaElementos.Count; i++)
        {
            // Si el tamaño de la lista de elementos sin repetir es 0, entonces se agregará a la lista de elementos sin repetir, el elemento en la posición 0.

            if (auxListaElementosSinRepetir.Count == 0 && tipo == Tipo.Grado)
            {
                auxListaElementosSinRepetir.Add(auxListaElementos[i]);
            }


            // Si el tamaño de la lista de elementos es diferente de 0, entonces ya se hará la revisión para evitar que haya elementos repetidos.
            else
            {
                // Este ciclo se ejecutará una vez por cada elemento en la lista de elementos sin repetir.
                for (int o = 0; o < auxListaElementosSinRepetir.Count; o++)
                {
                    // Dependiendo del tipo es la comparación que se realizará.
                    // Si el elemento "o" de la lista de elementos de repetir es igual al elemento "i" de la lista de actividades, entonces el elemento es un elemento repetido, ya que existe en la lista de elementos sin repetir,
                    // entonces se marca como un elemento repido al hacer verdadera la variable booleana "descartarElemento", para que dicho elemento no sea agregado de nuevo a la lista de elementos sin repetir.
                    switch (tipo)
                    {
                        case Tipo.Grado:
                            if (auxListaElementosSinRepetir[o].GetComponent<ScriptActividad>().grado == auxListaElementos[i].GetComponent<ScriptActividad>().grado)
                            {
                                descartarElemento = true;
                            }
                            break;
                        case Tipo.Materia:
                            if (auxListaElementosSinRepetir[o].GetComponent<ScriptActividad>().materia == auxListaElementos[i].GetComponent<ScriptActividad>().materia)
                            {
                                descartarElemento = true;
                            }
                            break;
                        case Tipo.Bloque:
                            if (auxListaElementosSinRepetir[o].GetComponent<ScriptActividad>().bloque == auxListaElementos[i].GetComponent<ScriptActividad>().bloque)
                            {
                                descartarElemento = true;
                            }
                            break;
                        case Tipo.Actividad:
                            break;
                    }
                }
                // Si el elemento "i" de la lista de actividades no está repetido, entonces se agrega a la lista de elementos sin repetir.
                if (descartarElemento == false)
                {
                    auxListaElementosSinRepetir.Add(auxListaElementos[i]);
                }
                // En caso de que el elemento "i" de la lista de actividades esté repetido, simplemente será ignorado y la variable "elementoRepetido" se hará falsa para repetir el ciclo sin fallos.
                else
                {
                    descartarElemento = false;
                }
            }
        }


        // Una vez terminada de armar la lista de elementos sin repetir, habiendo comprobado todos los elementos de cierto tipo en la lista de actividades y habiendo sido agregados los que no estaban repetidos a la lista de elementos
        // sin repetir, esta lista será retornada.
        return auxListaElementosSinRepetir;
    }


    
    private List<GameObject> ObtenerListaDeElementosOrdenados(List<GameObject> listaElementosSinOrdenar, Tipo tipo)
    {
        List<GameObject> auxListaElementosOrdenados = listaElementosSinOrdenar;

        // Bubble sort

        int n = auxListaElementosOrdenados.Count;
        GameObject temp;
        bool swapped;


        switch (tipo)
        {
            case Tipo.Grado:
                for (int i = 0; i < n - 1; i++)
                {
                    swapped = false;
                    for (int j = 0; j < n - i - 1; j++)
                    {
                        if ((int)auxListaElementosOrdenados[j].GetComponent<ScriptActividad>().grado > (int)auxListaElementosOrdenados[j + 1].GetComponent<ScriptActividad>().grado)
                        {
                            // swap arr[j] and arr[j+1] 
                            temp = auxListaElementosOrdenados[j];
                            auxListaElementosOrdenados[j] = auxListaElementosOrdenados[j + 1];
                            auxListaElementosOrdenados[j + 1] = temp;
                            swapped = true;
                        }
                    }

                    // IF no two elements were  
                    // swapped by inner loop, then break 
                    if (swapped == false)
                        break;
                }
                break;

            case Tipo.Materia:
                for (int i = 0; i < n - 1; i++)
                {
                    swapped = false;
                    for (int j = 0; j < n - i - 1; j++)
                    {
                        if ((int)auxListaElementosOrdenados[j].GetComponent<ScriptActividad>().materia > (int)auxListaElementosOrdenados[j + 1].GetComponent<ScriptActividad>().materia)
                        {
                            // swap arr[j] and arr[j+1] 
                            temp = auxListaElementosOrdenados[j];
                            auxListaElementosOrdenados[j] = auxListaElementosOrdenados[j + 1];
                            auxListaElementosOrdenados[j + 1] = temp;
                            swapped = true;
                        }
                    }

                    // IF no two elements were  
                    // swapped by inner loop, then break 
                    if (swapped == false)
                        break;
                }
                break;

            case Tipo.Bloque:
                for (int i = 0; i < n - 1; i++)
                {
                    swapped = false;
                    for (int j = 0; j < n - i - 1; j++)
                    {
                        if ((int)auxListaElementosOrdenados[j].GetComponent<ScriptActividad>().bloque > (int)auxListaElementosOrdenados[j + 1].GetComponent<ScriptActividad>().bloque)
                        {
                            // swap arr[j] and arr[j+1] 
                            temp = auxListaElementosOrdenados[j];
                            auxListaElementosOrdenados[j] = auxListaElementosOrdenados[j + 1];
                            auxListaElementosOrdenados[j + 1] = temp;
                            swapped = true;
                        }
                    }

                    // IF no two elements were  
                    // swapped by inner loop, then break 
                    if (swapped == false)
                        break;
                }
                break;

            case Tipo.Actividad:
                break;
        }

        return auxListaElementosOrdenados;

    }


    // Método para crear y añadir elementos del tipo deseado a una sección, se recibe el tipo y la actividad del elemento a crear.
    private void InstanciarElementoAMenu(Tipo tipo, GameObject elemento)
    {
        // Se crea la variable gridElementos, la cual almacenará el gameobject del contenedor.
        GameObject gridElemento = null;

        // Variable auxiliar para almacenar el elemento a crear.
        GameObject elementoInstanciado = null;

        // Variable auxiliar que ayudará a modificar el texto del elemento a crear.
        Text auxTextoAModificar;

        // Variable auxiliar para almacenar el texto del elemento que se recibe.
        string auxtextoAAgregar = "";

        /*
         Dependiendo del tipo de elemento, algunas cosas van a variar.
            1.- Se define el gameobject del contenedor del tipo deseado, para cargar los elementos en el contenedor correcto.
            2.- Se instancia un gameobject genérico del tipo deseado.
            3.- Se obtiene el texto del tipo deseado de la actividad recibida, (Si es un grado se obtiene el grado en específico, ej: Primero, Segundo, etc, esto depende del tipo de elemento que sea).
            4.- Se modifica el enumerador (grado/materia/bloque) del elemento genérico creado, dependiendo del tipo deseado.


        Una actividad tiene tres variables diferentes, una de tipo Grado, otra de tipo Materia y otra de tipo Bloque, de esas variables se obtienen los datos de los elementos genéricos a crear,
        en otras palabras, sirven para definir la ruta a seguir para encontrar dicha actividad en los contenedores.
        */
        switch (tipo)
        {
            case Tipo.Grado:
                gridElemento = gridGrados;
                elementoInstanciado = Instantiate(gradoGenerico);
                auxtextoAAgregar = elemento.GetComponent<ScriptActividad>().grado.ToString();
                elementoInstanciado.GetComponent<ScriptGrado>().grado = elemento.GetComponent<ScriptActividad>().grado;
                break;

            case Tipo.Materia:
                    gridElemento = gridMaterias;
                    elementoInstanciado = Instantiate(materiaGenerica);
                    auxtextoAAgregar = elemento.GetComponent<ScriptActividad>().materia.ToString();
                    elementoInstanciado.GetComponent<ScriptMateria>().materia = elemento.GetComponent<ScriptActividad>().materia;
                break;

            case Tipo.Bloque:
                    gridElemento = gridBloques;
                    elementoInstanciado = Instantiate(bloqueGenerico);
                    auxtextoAAgregar = elemento.GetComponent<ScriptActividad>().bloque.ToString();
                    elementoInstanciado.GetComponent<ScriptBloque>().bloque = elemento.GetComponent<ScriptActividad>().bloque;
                break;

            case Tipo.Actividad:
                gridElemento = gridActividades;
                elementoInstanciado = Instantiate(elemento);
                break;

        }

        // Un elemento Actividad no es necesario modificar el texto a mostrar, simplemente es instanciado en el contenedor correcto, ya que ya cuenta con los datos necesarios, por esta razón solo
        // se modifica el texto de los elementos que no son actividades.
        if (tipo != Tipo.Actividad)
        {
            // Se obtiene el componente Text del hijo del elemento genérico.
            auxTextoAModificar = elementoInstanciado.GetComponentInChildren<Text>();
            // Se cambia el elemento .text por el string obtenido del elemento recibido.
            auxTextoAModificar.text = auxtextoAAgregar;
        }

        
        // Se agrega el elemento genérico, ya con el tipo y texto modificado, al contenedor de su tipo.
        elementoInstanciado.transform.SetParent(gridElemento.transform, false);

    }


    private void EliminarElementosDeContenedor(GameObject contenedor)
    {
        foreach (Transform child in contenedor.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    

    public void AbrirEscenaActividad(GameObject escena)
    {
        //mainMenu.SetActive(false);
        //escenaActividadActual = Instantiate(escena, puntoReferenciaEscena.transform);
       Pausar();

        StartCoroutine(CargarEscenaActividad(escena));
    }


    IEnumerator CargarEscenaActividad(GameObject escena)
    {
        animator.SetTrigger("Comenzar");
        yield return new WaitForSeconds(1f);

        mainMenu.SetActive(false);
        escenaActividadActual = Instantiate(escena, puntoReferenciaEscena.transform);
        //yield return new WaitForSeconds(0.5f);


        //animator.SetTrigger("Terminar");
        //yield return new WaitForSeconds(1f);

        StopAllCoroutines();

    }

    IEnumerator TerminarCargarEscenaActividad()
    {
        animator.SetTrigger("Terminar");
        yield return new WaitForSeconds(1f);

        StopAllCoroutines();
    }

    public void EscenaActividadCargada()
    {
        StartCoroutine(TerminarCargarEscenaActividad());
    }

    IEnumerator CargarMenuPrincipal()
    {
        animator.SetTrigger("Comenzar");
        yield return new WaitForSeconds(1f);

        mainMenu.SetActive(true);
        Destroy(escenaActividadActual);


        animator.SetTrigger("Terminar");
        yield return new WaitForSeconds(1f);

        Jugar();

        StopAllCoroutines();
    }


    public void CerrarEscenaActividad()
    {
        //mainMenu.SetActive(true);
        //Destroy(escenaActividadActual);

        if (Jugando())
        {
            Pausar();
            StartCoroutine(CargarMenuPrincipal());
        }
        
    }


    public void Pausar()
    {
        this.ManagerEstadosJuego(EstadoJuego.Pause);
    }

    public void Jugar()
    {
        this.ManagerEstadosJuego(EstadoJuego.Jugando);
    }

    public bool Jugando()
    {
        return this.estadoJuego == EstadoJuego.Jugando;
    }




    public string ConversionTiempo(string auxSegundos)
    {
        int minutos = (int)(int.Parse(auxSegundos) / 60);
        int segundos = int.Parse(auxSegundos) - (minutos * 60);

        return (minutos + " : " + segundos);
    }





    public float RedondearNumero(float numero, int decimales)
    {
        float numeroRedondeado = (float)System.Math.Round(double.Parse(numero.ToString()), decimales);

        return numeroRedondeado;
    }

    public List<string> GenerarListaNumerosAleatorios(int cantidad, int decimales, float numeroReferencia)
    {
        List<string> listaNumerosAleatorios = new List<string>();
        for (int i = 0; i < cantidad; i++)
        {
            float numeroRandom = Random.Range(numeroReferencia / 2, numeroReferencia * 2);
            numeroRandom = RedondearNumero(numeroRandom, decimales);
            if (numeroRandom != numeroReferencia)
            {
                listaNumerosAleatorios.Add(numeroRandom.ToString());
            }
            else
            {
                cantidad++;
            }

        }
        listaNumerosAleatorios.Add(numeroReferencia.ToString());

        return listaNumerosAleatorios;
    }

    public List<string> DesordenarLista(List<string> lista)
    {
        List<string> listaNumerosDesordenados = new List<string>();
        List<string> auxLista = new List<string>();
        for (int i = 0; i < lista.Count; i++)
        {
            auxLista.Add(lista[i]);
        }
        while (auxLista.Count > 0)
        {
            int aux = Random.Range(0, auxLista.Count);
            listaNumerosDesordenados.Add(auxLista[aux]);
            auxLista.Remove(auxLista[aux]);
        }

        return listaNumerosDesordenados;
    }

    public void AgregarOpcionesAContenedor(GameObject contenedor, List<string> itemList, GameObject itemGenerico, float respuestaCorrecta)
    {
        RemoveItemsFromParent(contenedor);

        for (int i = 0; i < itemList.Count; i++)
        {
            GameObject item = Instantiate(itemGenerico, contenedor.transform);

            item.GetComponentInChildren<Text>().text = itemList[i];

            if (item.GetComponentInChildren<Text>().text == respuestaCorrecta.ToString())
            {
                item.GetComponent<BotonRespuesta>().respuesta = EstadoRespuesta.Correcta;
            }
        }
    }

    public void RemoveItemsFromParent(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Destroy(parent.transform.GetChild(i).gameObject);
        }
    }








    private void ManagerVariablesActuales(Tipo tipo, Grado grado, Materia materia, Bloque bloque)
    {
        switch (tipo)
        {
            case Tipo.Grado:
                this.gradoActual = grado;
                this.materiaActual = materia;
                this.bloqueActual = bloque;
                break;
            case Tipo.Materia:
                this.materiaActual = materia;
                this.bloqueActual = bloque;
                break;
            case Tipo.Bloque:
                this.bloqueActual = bloque;
                break;
            case Tipo.Actividad:
                break;
        }



    }


    // Los elementos como grados, materias, bloques, actividades, que están dentro de los canvas en las interfaces, son elementos de tipo botón.
    // Al presionar dichos botones, se harán las acciones correspondientes, por ejemplo, cuando un botón de grado sea presionado, se cargarán los elementos
    // de tipo materia que correspondan al grado seleccionado.
    // Grado> Materia> Bloque> Actividad





    public void GradoSeleccionado(Grado gradoSeleccionado)
    {
        ManagerVariablesActuales(Tipo.Grado, gradoSeleccionado, Materia.Ninguno, Bloque.Ninguno);
        EstadoMaterias();
        EliminarElementosDeContenedor(gridMaterias);
        EliminarElementosDeContenedor(gridBloques);
        EliminarElementosDeContenedor(gridActividades);
        CargarElementos(Tipo.Materia);
    }
    public void MateriaSeleccionada(Materia materiaSeleccionada)
    {
        ManagerVariablesActuales(Tipo.Materia, Grado.Ninguno, materiaSeleccionada, Bloque.Ninguno);
        EstadoBloquesActividades();
        EliminarElementosDeContenedor(gridBloques);
        EliminarElementosDeContenedor(gridActividades);
        CargarElementos(Tipo.Bloque);
    }
    public void BloqueSeleccionado(Bloque bloqueSeleccionado)
    {
        ManagerVariablesActuales(Tipo.Bloque, Grado.Ninguno, Materia.Ninguno, bloqueSeleccionado);
        EliminarElementosDeContenedor(gridActividades);
        CargarElementos(Tipo.Actividad);
    }




    public void EstadoBienvenida()
    {
        ManagerSecciones(Seccion.Bienvenida);
    }
    public void EstadoGrados()
    {
        EliminarElementosDeContenedor(gridGrados);
        EliminarElementosDeContenedor(gridMaterias);
        CargarElementos(Tipo.Grado);
        ManagerSecciones(Seccion.Grados);
    }

    public void EstadoMaterias()
    {
        ManagerSecciones(Seccion.Materias);
    }
    public void EstadoBloquesActividades()
    {
        ManagerSecciones(Seccion.BloquesActividades);
    }
    public void EstadoActividad()
    {
        ManagerSecciones(Seccion.Actividad);
    }


    public void SalirDelJuego()
    {
        Application.Quit();
    }
}
