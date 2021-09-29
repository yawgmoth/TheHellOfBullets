using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactCleanup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator Cleanup()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
