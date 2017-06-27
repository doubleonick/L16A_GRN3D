using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LDR : MonoBehaviour {

	// Create a light object to reference the main light
	public GameObject lightSource;
	public float clacLightScore;
	public int maxSpotRange = 300;//641;


	void Start () {
		clacLightScore = 0.0f;
		// Find the light and assign it to the light game object
		lightSource =  GameObject.Find("EnvLight");
		print("Found " + lightSource.name);
	}

	// Update is called once per frame
	void Update () {
		// Calculate the distance to the light from this sensor
		float xyVectorLandro = Mathf.Sqrt(Mathf.Pow(this.transform.position.x, 2) + Mathf.Pow(this.transform.position.y, 2));
		float xyVectorLight = Mathf.Sqrt(Mathf.Pow(this.transform.position.x, 2) + Mathf.Pow(this.transform.position.y, 2));
		float distance = Vector3.Distance(this.transform.position,lightSource.transform.position);

		// Calculate how much light the sensor has collected
		clacLightScore = maxSpotRange - distance;
		print("Light collected at sensor"+ this + " is: "+ clacLightScore );
		/*
		// Print it
		if (distance > maxSpotRange) {
			print ("No Light Detected");
		} else {
			print("Light collected at sensor"+ this + " is: "+ clacLightScore );
		}*/
			


	}
}
