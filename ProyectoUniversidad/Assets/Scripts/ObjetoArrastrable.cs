using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjetoArrastrable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    static public GameObject objetoAArrastrar;
    private Vector3 posicionInicial;
    private Transform padreInicial;

    [Header("Objeto que se sobrepone a todos los demás")]
    //Para evitar que al arrastrar el objeto, este se vea superpuesto por otros objetos
    private Transform superPadre;

    public void ObtenerSuperPadre()
    {
        this.superPadre = GameManager.sharedInstance.superPadre;
        /*
        if (EQSDManager.sharedInstance.superPadre != null)
        {
            this.superPadre = EQSDManager.sharedInstance.superPadre;
            Debug.Log("modi par1");
        }
        else if (CeeSManager.sharedInstance.superPadre != null)
        {
            this.superPadre = CeeSManager.sharedInstance.superPadre;
            Debug.Log("modi par2");
        }*/
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        ObtenerSuperPadre();

        objetoAArrastrar = this.gameObject;
        posicionInicial = this.transform.position;
        padreInicial = this.transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        this.transform.SetParent(superPadre);

    }

    public void OnDrag(PointerEventData eventData)
    {
        objetoAArrastrar.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        objetoAArrastrar = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (this.transform.parent == padreInicial || this.transform.parent == superPadre)
        {
            transform.position = posicionInicial;
            this.transform.SetParent(padreInicial);
        }
    }

}
