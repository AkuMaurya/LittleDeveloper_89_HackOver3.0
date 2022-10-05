using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 1;
    public float rotationspeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float translate = Input.GetAxis("Vertical")*speed;
        float rotation = Input.GetAxis("Horizontal")*rotationspeed;

        translate *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // if(Input.GetKeyDown())
        transform.Translate(0,0,translate);
        transform.Rotate(0,rotation,0);
    }
}
