using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    private bool Libre()
    {
        if (this.transform.childCount > 0)
        {
            return false;
        }
        return true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (Libre())
        {
            if (ObjetoArrastrable.objetoAArrastrar != null)
            {
                ObjetoArrastrable.objetoAArrastrar.transform.SetParent(this.transform);
            } 
        }
    }


}
