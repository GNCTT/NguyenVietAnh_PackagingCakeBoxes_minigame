using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCake : MonoBehaviour
{

    public void Start()
    {
        StartCoroutine(LiveRoutine());
    }

    private IEnumerator LiveRoutine()
    {
        yield return new WaitForSeconds(2f);
        DestroySelf();
    }
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
