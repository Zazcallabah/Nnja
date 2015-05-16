using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	void Start () {	}
	
	// Update is called once per frame
	void Update () {
		var udp = GetComponent<UDPLISTEN> (); // get a reference to the other script
		var laserX = udp.latestX;
		var laserY = udp.latestY;
		
		// y is inverted, and i think 480 is max pointer value received
		// since camera is at z=0, convert to world point at z=40, 40 units away from camera.
		var res = Camera.main.ScreenToWorldPoint(new Vector3(laserX, 480-udp.latestY, 40));
		transform.position = res;
		
		// this is the rotation stuff.
		// there is a 90 degree offset, and remember that the y axis is flipped.
		var convertedXAngle = (udp.latestAngle*-1)+450;
		
		//set the space ship rotation to match the travel direction
		transform.localEulerAngles = new Vector3(convertedXAngle,90,270);
	}
}