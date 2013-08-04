using UnityEngine;
using System.Collections;
using Vectrosity;

public class GolfBallController : MonoBehaviour {
	
	public Material lineMaterial;
	Vector3 origPos, curMousePos;
	RaycastHit hit;
	VectorLine line;
	Vector3[] linePoints;
	
	GameObject startpointBall;
	
	// Use this for initialization
	void Start () {
		linePoints = new Vector3[2];
		line = new VectorLine("Line", linePoints, lineMaterial, 4.0f);		
		line.active = false;
		startpointBall = GameObject.FindGameObjectWithTag ("StartPointBall");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDrag() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast(ray, out hit)){
			linePoints[0] = transform.position;
			linePoints[1] = hit.point;
			// Draw the line
			line.Draw3D();
		}

    }
	
	void OnTriggerEnter (Collider enterer) {
		if (enterer.CompareTag("Goal"))
    	{
       		Debug.Log("Ball is putted!");
			rigidbody.velocity = new Vector3(0.0f,0.0f,0.0f);
			rigidbody.angularVelocity = new Vector3(0.0f,0.0f,0.0f);
			
			// TODO GOAL
			
    	   	transform.position = startpointBall.transform.position;// new Vector3(0.0f,0.25f,-22.0f);
	    }
	}
	
	void OnMouseDown() {
		print("OnMouseDown");
		line.active = true;
	}
	
	void OnMouseUp() {
		print("OnMouseUp");
		//this.rigidbody.AddForce(new Vector3(0,0,100));//hit.point);
		
		float speed = Vector3.Distance(transform.position,hit.point) * 500;
		
		
		Vector3 heading = transform.position - hit.point;
		Debug.Log(heading);
		float distance = heading.magnitude;
		// This is now the normalized direction.
		var directionNormalized = heading / distance;
		directionNormalized.y = 0;
		
		this.rigidbody.AddForce(directionNormalized*speed);
		Debug.Log(directionNormalized + " speed: " + speed);
		line.active = false;
	}

}


	/*	
 			//float angle = AngleDir (transform.position,hit.point, new Vector3(0,1,0));
			//print ("angle is:" + angle);

		//returns -1 when to the left, 1 to the right, and 0 for forward/backward
    public float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 upDir)

    {
  		float angle = Vector3.Angle(fwd, targetDir);
		return angle;
    }
    */
