using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IR : MonoBehaviour {

	public bool hitWall;
	// Use this for initialization
	void Start () {
		hitWall = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider source){
		
		if(!source.name.Contains("L16A")){
			print(this.name + " hit " + source.name);
			if(source.name.Contains("Wall")){
				hitWall = true;
			}

		}
	}
}
