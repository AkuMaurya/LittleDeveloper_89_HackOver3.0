using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public GameObject explosion;
    Rigidbody rb;

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            GameObject exp = Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(exp, 0.5f);
            Destroy(this.gameObject);
        }
    }

     void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.forward = rb.velocity;
    }
}
