using UnityEngine;
using System.Collections;
using Vectrosity;

public class GolfBallController : MonoBehaviour {
	
	public Material lineMaterial;
	public float speedball;
	public float maxSpeedBall;
	public float limitVelocityYUp = 5;
	private bool inContactBaan = true;

	Vector3 origPos, curMousePos;
	RaycastHit hit;
	VectorLine line;
	Vector3[] linePoints;
	GameObject startpointBall;
	Vector3 newVelocity;
	
	float counterZ;

	float counterY;
	
	// Use this for initialization
	void Start () {
		linePoints = new Vector3[2];
		line = new VectorLine("Line", linePoints, lineMaterial, 4.0f);		
		line.active = false;
		startpointBall = GameObject.FindGameObjectWithTag ("StartPointBall");
	}

	// Update is called once per frame
	void FixedUpdate () {
		
		// Limit the velocity up to avoid the ball is going to high
		// Using a temp variable reason:
		// Cannot modify a value type return value of `UnityEngine.Rigidbody.velocity'. Consider storing the value in a temporary variable
		newVelocity  =  rigidbody.velocity;
		if (newVelocity.y > limitVelocityYUp) {  newVelocity.y = limitVelocityYUp;}
		// option could limit the Ydown, ball down but let's not do this for now
		//else if (newVelocity.y < -limitVelocityY) {  newVelocity.y = -limitVelocityY;}

		if (newVelocity.z > 0 && newVelocity.z < 0.1) {  
			newVelocity.x = 0;
			newVelocity.y = 0;
			newVelocity.z = 0;
			rigidbody.angularVelocity = newVelocity;
		}
		/*
		if (newVelocity.z < 0.4) {  
			counterZ += 1; }
		else {
			counterZ = 0;
		}
		if (counterZ > 20) {
			newVelocity.x = 0;
			newVelocity.y = 0;
			newVelocity.z = 0;
		}
		*/
		
		rigidbody.velocity = newVelocity;

		//Debug.Log("rigidbody.velocity:" + rigidbody.velocity);

		// adding downforce when the distance from the ball to the course is high
		if (!inContactBaan) {
			// calculate distance 
			//float speed = Vector3.Distance(transform.position,);
			RaycastHit hit;
			if (Physics.Raycast(transform.position, -Vector3.up, out hit)) {
				//float distanceToGround = hit.distance;
				Debug.Log ("distanceToGround: " + hit.distance);
				if (hit.distance>.4) {
					this.rigidbody.AddForce(new Vector3(0,-hit.distance*200,0));
				}
			}

		}

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
		counterZ = 0;
	}
	
	void OnMouseUp() {
		print("OnMouseUp");
		//this.rigidbody.AddForce(new Vector3(0,0,100));//hit.point);
		
		float speed = Vector3.Distance(transform.position,hit.point) * speedball;
		Debug.Log(speed);
		if (speed > maxSpeedBall) {speed=maxSpeedBall;}
		
		Vector3 heading = transform.position - hit.point;
		
		float magnitude = heading.magnitude;
		// This is now the normalized direction.
		var directionNormalized = heading / magnitude;
		directionNormalized.y = 0;
		
		this.rigidbody.AddForce(directionNormalized*speed);
		//this.rigidbody.AddTorque(directionNormalized*speed);
		Debug.Log(directionNormalized + " speed: " + speed);
		line.active = false;
	}

	void OnCollisionExit(Collision collisionInfo) {
		print("No longer in contact with " + collisionInfo.transform.name);
		if (collisionInfo.transform.name == "Baan 01") {
			inContactBaan = false;
		}
	}

	void OnCollisionEnter(Collision collision) {
		print("Contact with " + collision.transform.name);
		if (collision.transform.name == "Baan 01") {
			inContactBaan = true;
		}

		// debug draw
		//foreach (ContactPoint contact in collision.contacts) {
		//	Debug.DrawRay(contact.point, contact.normal, Color.white);
		//
		//}


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
