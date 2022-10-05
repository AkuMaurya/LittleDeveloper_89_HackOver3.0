using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    // public GameObject firepoint;
    public List<GameObject> vfx = new List<GameObject>();
    public RotateToMouse rotateToMouse;
    private GameObject effectToSpawn;
    public GameObject[] fond;
    private float timeToFire = 0;
    void Start()
    {
        effectToSpawn = vfx[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && Time.time >= timeToFire){
            timeToFire = Time.time + 1 / effectToSpawn.GetComponent<ProjectileMove>().firePoint;
            SpawnVFX();
        }
    }

    void SpawnVFX(){
        GameObject vfx;
        
        fond = GameObject.FindGameObjectsWithTag("Cube");
        foreach (GameObject go in fond)
        {
            if(fond != null){
                vfx = Instantiate(effectToSpawn, go.transform.position, Quaternion.identity);
                if(rotateToMouse != null){
                    vfx.transform.localRotation = rotateToMouse.GetRotation();  
                }
            }
            else{
                Debug.Log("No Fire Point");
            }
        }
    }
}
