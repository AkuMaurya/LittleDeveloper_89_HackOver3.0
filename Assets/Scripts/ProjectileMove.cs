using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public float speed;
    public float firePoint;
    //public GameObject Explode;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(speed !=0) {
            transform.position += transform.forward *(speed * Time.deltaTime);
        }else{
            Debug.Log("No Speed");
        }
        Destroy(gameObject,5);
    }

    //void OnCollisionEnter(Collision co){
    //    speed = 0;
    //    Instantiate(Explode,gameObject.transform.position,Quaternion.identity);
    //    Destroy(this.gameObject);
    //}
}
