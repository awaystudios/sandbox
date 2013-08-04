using UnityEngine;
using System.Collections;

public class GolfBallController : MonoBehaviour {
	
	Vector3 origPos, curMousePos;
	RaycastHit hit;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnMouseDrag() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast(ray, out hit)){
	    	Debug.DrawLine(transform.position, hit.point, Color.red);
			//print(hit.point);
		
 		//print ("angle", AngleDir (transform.position,hit.point, new Vector3(0,1,0)));		
		float angle = AngleDir (transform.position,hit.point, new Vector3(0,1,0));
		print ("angle is:" + angle);
					}

    }

	//returns -1 when to the left, 1 to the right, and 0 for forward/backward
    public float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 upDir)

    {
  		float angle = Vector3.Angle(fwd, targetDir);
		return angle;
    
		/*
		//The AngleDir function is the one from the other thread.

    if (AngleDir(fwd, targetDir, upDir) == -1) {

        return 360 - angle;

    } else {

        return angle;

    }
		 */
    }
	
	void OnMouseDown() {
		print("OnMouseDown");
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
	}

}