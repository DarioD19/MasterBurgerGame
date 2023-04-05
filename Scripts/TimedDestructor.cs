using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestructor : MonoBehaviour
{
    private float _lifetime = 3f;
   
    

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, _lifetime);
    }
}
