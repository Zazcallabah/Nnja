using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	void Start () {	}
	
	// Update is called once per frame
	void Update () {
		var udp = GetComponent<UdpRecv> ();
		var res = Camera.main.ScreenToWorldPoint(new Vector3(udp.latestX, udp.latestY, 40));
		transform.position = res;
		transform.localEulerAngles = new Vector3((udp.latestAngle)+450,90,270);
	}
}