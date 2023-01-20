using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eric_LupaScript : MonoBehaviour
{
    [SerializeField]AnimationCurve curve;
    [SerializeField]float animationTime;
    [SerializeField]float smoothTimeLookAtMouse;
    [SerializeField]Vector2 rotationLimit;
    
    void Awake()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
    }
    
    public IEnumerator OnLupaStart()
    {
        //Empieza un timer, creamos dos variables con la escala actual y con la escala que queremos al final.
        //Empezar loop, si el tiempo actual es menor que el tiempo final escala el objeto usando las variables anteriores al ritmo de la curva de animacion.
        float startTime = Time.time;
        float endTime = startTime + animationTime;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = new Vector3(1f,1f,1f);
        if(Input.GetButtonDown("Fire2"))
        {
            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / animationTime;
                float curveValue = curve.Evaluate(t);
                transform.localScale = Vector3.Lerp(startScale, endScale, curveValue);
                yield return null;
            }
        }
    }

    public IEnumerator OnLupaEnd()
    {
        float startTime = Time.time;
        float endTime = startTime + animationTime;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = new Vector3(0f,0f,0f);

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / animationTime;
            float curveValue = curve.Evaluate(t);
            transform.localScale = Vector3.Lerp(startScale, endScale, curveValue);
            yield return null;
        }
    }


    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Gizmos.DrawRay(transform.position, hit.point * 300f);
        }
    }*/
}
