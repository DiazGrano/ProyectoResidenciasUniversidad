using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelOpciones : MonoBehaviour, IDropHandler
{

    public Transform contenedorOpciones;

    public void OnDrop(PointerEventData eventData)
    {
        if (ObjetoArrastrable.objetoAArrastrar != null)
        {
            ObjetoArrastrable.objetoAArrastrar.transform.SetParent(contenedorOpciones.transform);
        }
    }
}
