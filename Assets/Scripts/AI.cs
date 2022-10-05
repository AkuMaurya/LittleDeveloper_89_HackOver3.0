using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI : MonoBehaviour
{
    // public GameObject player;
    public GameObject parent;
    float rotationSpeed = 5.5f;
	float visDist = 15.0f;
	float visAngle = 45.0f;
	float speed = 15f;


	public GameObject ShellPrefab;
    public GameObject ShellSpawnpos;
	GameObject ply;
    bool canShoot = true;
	// float shootDist = 5.0f;
    // string state = "IDLE";
    void Start()
    {
        ply = GameObject.FindWithTag("Player");
    }
	void CanShootAgain()
    {
        canShoot = true;
    }
    void Fire()
    {
        if(canShoot)
        {
            GameObject shell = Instantiate(ShellPrefab, ShellSpawnpos.transform.position, ShellSpawnpos.transform.rotation);
            shell.GetComponent<Rigidbody>().velocity = speed* this.transform.forward;
            canShoot = false;
            Invoke("CanShootAgain",0.2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
		// Debug.Log("Hello");
		
        Vector3 direction = ply.transform.position - parent.transform.position;
		float angl = Vector3.Angle(direction, this.transform.forward);
		
		if(direction.magnitude < visDist && angl < visAngle)
		{
			Debug.Log(direction.magnitude);
			direction.y = 0;
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			parent.transform.rotation = Quaternion.Slerp(parent.transform.rotation,lookRotation,
										Time.deltaTime * rotationSpeed);

			float? angle = RotateTurret();
            if(angle!=null&&Vector3.Angle(direction,parent.transform.forward)<90)
            {
                Fire();
				// Debug.Log("Fire");
            }
        

		}
    }


	float? RotateTurret()
    {
        float? angle = CalculateAngle(true);
        if(angle!=null)
        {
            this.transform.localEulerAngles= new Vector3(360f -(float)angle,0f,0f);
        }
        return angle;
    }
    float? CalculateAngle(bool low)
    {
        Vector3 targetDir = ply.transform.position - this.transform.position;
        float y = targetDir.y;
        targetDir.y = 0f;
        float x =   targetDir.magnitude;
        float gravity = 9.81f;
        float sSqr = speed*speed;
        float underTheSqrRoot=(sSqr*sSqr)-gravity*(gravity * x*x+2*y*sSqr);

        if(underTheSqrRoot>=0f)
        {
            float root = Mathf.Sqrt(underTheSqrRoot);
            float highAngle = sSqr + root;
            float lowAngle = sSqr - root;
            // Debug.Log(low);
            
            if(low)
            {
                return(Mathf.Atan2(lowAngle,gravity*x)*Mathf.Rad2Deg);
                
            }
            else{
                return(Mathf.Atan2(highAngle,gravity*x)*Mathf.Rad2Deg);
            }
            
            
        }
        else
        return null;
    }
}
