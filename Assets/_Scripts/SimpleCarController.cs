using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}
 
public class SimpleCarController : MonoBehaviour {
    public List<AxleInfo> axleInfos; 
    public float maxMotorTorque;
    public float maxSteeringAngle;

	GameObject wheelColliders;
    List<WheelCollider> wheels = new List<WheelCollider>();
    public float timeNext;
    public float timeCurrent;
    public int steerMode;
    public Light morningstar;

    //public List<GameObject> ir_sensors_go;
    private IR[] ir_sensors;
    private LDR[] ldr_sensors;

    public void Start(){
    	//Rigidbody ir_sensor	   = new Rigidbody();
    	int i = 0;
    	Quaternion ir_rotation = new Quaternion();
    	Vector3 ir_position    = new Vector3();
		Quaternion ldr_rotation = new Quaternion();
    	Vector3 ldr_position    = new Vector3();
    	wheelColliders = GameObject.Find("WheelColliders");
    	//morningstar = GameObject.Find("Directional Light").GetComponent<Light>();
    	wheels.Add(wheelColliders.transform.Find("frontRight").GetComponent<WheelCollider>());
		wheels.Add(wheelColliders.transform.Find("frontLeft").GetComponent<WheelCollider>());
		//wheels.Add(wheelColliders.transform.Find("backRight").GetComponent<WheelCollider>());
		//wheels.Add(wheelColliders.transform.Find("backLeft").GetComponent<WheelCollider>());
		//Wheels = wheelColliders.transform.Find("frontRight").GetComponent<WheelCollider>();
		foreach(WheelCollider wheel in wheels){
			
			if(wheel.name.Contains("front")){
				wheel.motorTorque = 50f;
				//wheel.steerAngle = 45f;
			}
		}
		timeNext  = 0;
		timeCurrent = 0;
		steerMode   = 0;
		/*
		ir_sensors = GameObject.FindObjectsOfType<IR>();
		foreach(IR ir_sensor in ir_sensors){
			ir_rotation = Quaternion.Euler(0, 45 * i, 90);
			ir_position = this.transform.position;
			ir_position.y = 115f;
			ir_position.z += Mathf.Cos(Mathf.PI/4 * i) * 525f/2f;//Divide by 2 because scaled IRs by 0.5
			ir_position.x += Mathf.Sin(Mathf.PI/4 * i) * 525f/2f;
			print("ir_rotation " + ir_rotation.ToString());
			ir_sensor.name = "ir" + i.ToString();
			ir_sensor.transform.rotation = ir_rotation;
			ir_sensor.transform.position = ir_position;

			i++;
		}
		*/
		i = -2;
		ldr_sensors = GameObject.FindObjectsOfType<LDR>();
		foreach(LDR ldr_sensor in ldr_sensors){
			ldr_rotation = Quaternion.Euler(0, 45f * float.Parse(i.ToString()), 90f);
			ldr_position = this.transform.position;
			ldr_position.y = 115f;
			ldr_position.z += Mathf.Cos(Mathf.PI/8 + Mathf.PI/4 * i) * 80f;//i-4 only for use of a subset of sensors
			ldr_position.x += Mathf.Sin(Mathf.PI/8 + Mathf.PI/4 * i) * 80f;
			print("ldr_rotation " + ldr_rotation.ToString());
			ldr_sensor.name = "ldr" + i.ToString();
			ldr_sensor.transform.rotation = ldr_rotation;
			ldr_sensor.transform.position = ldr_position;
			i++;
		}
    	//print(wheelColliders.name + " contains " + frontRightCollider.name + "and that's nice, 'cause....");
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }
 
        Transform visualWheel = collider.transform.GetChild(0);
 
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
 
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
        print(visualWheel.name + ": position " + visualWheel.transform.position.ToString() + " : rotation " + visualWheel.transform.rotation.ToString());
    }
 
    public void FixedUpdate()
    {
    	//print("Bringer of light: " + morningstar.intensity.ToString());
    	timeCurrent = Time.timeSinceLevelLoad;
		//Relative angle of sensor on the sensor ring
		foreach(IR ir_sensor in ir_sensors){
			
			if(ir_sensor.hitWall == true){
				print(ir_sensor.name + " hitWall: " + ir_sensor.hitWall.ToString());
				avoidWall(ir_sensor.transform.localEulerAngles.y);
				ir_sensor.hitWall = false;
			}
		}
		//lightSeek();

    	if(timeCurrent > timeNext){
    		if(steerMode == 0){
				foreach(WheelCollider wheel in wheels){
					
					if(wheel.name.Contains("Left")){
						wheel.motorTorque = 500f;
						wheel.steerAngle = 0f;
					}else if(wheel.name.Contains("Right")){
						wheel.motorTorque = 500f;
						wheel.steerAngle = 0f;
					}
				}
				steerMode = 1;
			}else if(steerMode == 1){
				foreach(WheelCollider wheel in wheels){
					if(wheel.name.Contains("Left")){
						wheel.motorTorque = 500f;
						wheel.steerAngle = 0f;
					}else if(wheel.name.Contains("Right")){
						wheel.motorTorque = 500f;
						wheel.steerAngle = 0f;
					}
				}
				steerMode = 0;
			}
			timeNext = timeCurrent + 30.0f;


		}
//        float motor = maxMotorTorque * Input.GetAxis("Vertical");
//        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
// 
//        foreach (AxleInfo axleInfo in axleInfos) {
//            if (axleInfo.steering) {
//                axleInfo.leftWheel.steerAngle = steering;
//                axleInfo.rightWheel.steerAngle = steering;
//            }
//            if (axleInfo.motor) {
//                axleInfo.leftWheel.motorTorque = motor;
//                axleInfo.rightWheel.motorTorque = motor;
//            }
//            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
//            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
//        }
    }

    void lightSeek(){
		WheelCollider leftMotor = GameObject.Find("frontLeft").GetComponent<WheelCollider>();
		WheelCollider rightMotor = GameObject.Find("frontRight").GetComponent<WheelCollider>();
		float ldrAngle;
		foreach(LDR ldr_sensor in ldr_sensors){
			ldrAngle = ldr_sensor.transform.localEulerAngles.y;
			if(ldr_sensor.clacLightScore > 10){
				if(ldrAngle < 90){
					print("Veer left...");
					rightMotor.motorTorque = 500;
					leftMotor.motorTorque = 300;
				}else{
					leftMotor.motorTorque = 500;
					rightMotor.motorTorque = 300;
					print("Veer right...");
				}

			}else{
				leftMotor.motorTorque = 400;
				rightMotor.motorTorque = 400;
			}

		}

		float timeSeek = timeCurrent + 5.0f;
		while(timeCurrent < timeSeek){
			timeSeek = Time.timeSinceLevelLoad;
    	}
    }

    void avoidWall(float ir_angle){
    	WheelCollider leftMotor = GameObject.Find("frontLeft").GetComponent<WheelCollider>();
		WheelCollider rightMotor = GameObject.Find("frontRight").GetComponent<WheelCollider>();
		float timeAvoid = timeCurrent + 5.0f;
		//IR ir_cone;//Do this the same was as with bricks in block smasher...
    	//ir_cone = GameObject.Find(name).GetComponent<IR>();
    	print("ir sensor detecting wall at: " + ir_angle.ToString());
    	print("avoiding from " + timeCurrent.ToString() + " until " + timeAvoid.ToString());
    	while(timeCurrent < timeAvoid){
			if(ir_angle <= 90 || ir_angle >= 270){
				leftMotor.motorTorque = -500f;
				rightMotor.motorTorque = 200f;
				print("Back up...");
			}else{
				leftMotor.motorTorque = 200f;
				rightMotor.motorTorque = 500f;
				print("Move forward...");
			}
    		timeAvoid = Time.timeSinceLevelLoad;
    	}
    }
}