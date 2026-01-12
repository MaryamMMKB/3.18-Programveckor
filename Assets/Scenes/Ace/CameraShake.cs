using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    Vector3 startPos;

    void Awake()
    {
        startPos = transform.localPosition;
    }

    public IEnumerator Shake(float intensity)
    {
        white(true){
            transform.localPosition = startPos + Random.insideUnitSphere * intensity;
            yield return null;
        }
    }

    public void StopShake()
    {
        StopAllCoroutines();
        transform.localPosition = startPos;
    }
    
}
