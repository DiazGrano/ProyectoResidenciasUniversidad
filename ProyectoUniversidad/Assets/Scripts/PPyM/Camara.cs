using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public static Camara sharedInstance;
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 LimitePosicion;
    public Vector3 offset;
    Transform puntoReferencia;

    private void Awake()
    {
        sharedInstance = this;
        
    }

    private void Start()
    {
        puntoReferencia = PPyMManager.sharedInstance.puntoReferenciaCamara.transform;
    }


    private void FixedUpdate()
    {

        if (target != null)
        {
            Vector3 posicionDeseada = target.position + offset;
            Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, smoothSpeed * Time.deltaTime);
            
            
            LimitePosicion = new Vector3(Mathf.Clamp(posicionSuavizada.x, puntoReferencia.position.x, puntoReferencia.position.x + 500f), Mathf.Clamp(posicionSuavizada.y, puntoReferencia.position.y, puntoReferencia.position.y + 500f), posicionSuavizada.z);
            transform.position = LimitePosicion;
        }
        
    }

}
