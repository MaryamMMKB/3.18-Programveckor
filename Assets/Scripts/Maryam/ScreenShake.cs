using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
 

   
    public AnimationCurve curve;
    public float duration = 1f;

   public void CalleableShake()
    {
        
     StartCoroutine(Shaking());
        Debug.Log("Screen shake called");
    }

    public IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {     
            elapsedTime += Time.deltaTime;

            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;

            yield return null;
        }

        transform.position = startPosition;
    }
}



