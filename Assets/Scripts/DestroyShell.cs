using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyShell : MonoBehaviour
{

    // Update is called once per frame
    void Start()
    {
        Destroy(this.gameObject, 3);  
    }
}
