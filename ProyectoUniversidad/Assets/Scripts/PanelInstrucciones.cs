using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelInstrucciones : MonoBehaviour
{
    public static PanelInstrucciones sharedInstance;


    [Header("Controlador animación panel instrucciones")]
    public Animator animator;

    public Button botonCerrar;

    private void Awake()
    {
        sharedInstance = this;
    }

    [Header("Instrucciones de la actividad")]

    public GameObject contenedorInstrucciones;

    void Start()
    {
        this.gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void CerrarInstrucciones()
    {
        StartCoroutine(CerrarPanelInstrucciones());
    }

    public void AbrirInstrucciones()
    {
        if (!PanelResultado.sharedInstance.VentanaResultadoAbierta() && !VentanaInstruccionesAbierta())
        {
            GameManager.sharedInstance.Pausar();
            StartCoroutine(AbrirPanelInstrucciones());
        } 
    }


    IEnumerator AbrirPanelInstrucciones()
    {
        animator.SetTrigger("InicioInstrucciones");
        this.GetComponent<Canvas>().enabled = true;
        contenedorInstrucciones.GetComponent<RectTransform>().localPosition = new Vector2(0f,0f);
        yield return new WaitForSeconds(0.5f);

        botonCerrar.interactable = true;

        StopAllCoroutines();
    }

    IEnumerator CerrarPanelInstrucciones()
    {
        botonCerrar.interactable = false;

        animator.SetTrigger("FinInstrucciones");
        yield return new WaitForSeconds(0.5f);

        this.GetComponent<Canvas>().enabled = false;

        if (!PanelResultado.sharedInstance.VentanaResultadoAbierta())
        {
            GameManager.sharedInstance.Jugar();
        }

        StopAllCoroutines();
    }

    public void ModificarInstrucciones(GameObject objetoInstrucciones = null)
    {
        for (int i = 0; i < this.contenedorInstrucciones.transform.childCount; i++)
        {
            Destroy(this.contenedorInstrucciones.transform.GetChild(i).gameObject);
        }

        if (objetoInstrucciones != null)
        {
            for (int i = 0; i < objetoInstrucciones.transform.childCount; i++)
            {
                Instantiate(objetoInstrucciones.transform.GetChild(i), this.contenedorInstrucciones.transform);
            }
        }
    }


    public bool VentanaInstruccionesAbierta()
    {
        return this.GetComponent<Canvas>().isActiveAndEnabled;
    }
}
