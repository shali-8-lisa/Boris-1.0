using UnityEngine;
using System.Collections;

public class PenguinAnimation : MonoBehaviour {
	
	public GameObject penguin;
	public GameObject leftFin;
	public GameObject rightFin;
	public GameObject leftFeet;
	public GameObject rightFeet;

	private Quaternion penguinStartPose;
	private Quaternion leftFinStartPose;
	private Quaternion rightFinStartPose;
	private Quaternion leftFeetStartPose;
	private Quaternion rightFeetStartPose;
	private int isWalking;
	private int isDroning;
	private int BodyRotateDir;
	private int FinFlapDir;
	private int FeetFlapDir;

	void Start () 
	{
		penguinStartPose = penguin.transform.rotation;
		leftFinStartPose = leftFin.transform.rotation;
		rightFinStartPose = rightFin.transform.rotation;
		leftFeetStartPose = leftFeet.transform.rotation;
		rightFeetStartPose = rightFeet.transform.rotation;

		isWalking = 0;
		isDroning = 0;
		BodyRotateDir = 1;
		FinFlapDir = 1;
		FeetFlapDir = 1;
	}

	void Update () 
	{
		if (Input.GetKey ("1"))
			resetPose ();

		if (Input.GetKey ("2")) 
		{
			resetPose ();
			jumpPose ();
		}

		if (Input.GetKey ("3")) 
		{
			resetPose ();
			isWalking = 1;
		}

		if (Input.GetKey ("4")) 
		{
			resetPose ();
			isDroning = 1;
		}

		if (isWalking == 1) 
		{
			//body
			float bodyRotateSpeed = Time.deltaTime * 150.0f;
			penguin.transform.Rotate (Vector3.up * BodyRotateDir * bodyRotateSpeed);
			if (penguin.transform.rotation.y > 0.1f)
				BodyRotateDir = -1;
			if (penguin.transform.rotation.y < -0.1f)
				BodyRotateDir = 1;
		
			//fin
			float finFlapSpeed = Time.deltaTime * 400.0f;
			leftFin.transform.Rotate (Vector3.forward * FinFlapDir * finFlapSpeed);
			rightFin.transform.Rotate (Vector3.forward * (-FinFlapDir) * finFlapSpeed);
			if (leftFin.transform.rotation.z > 0.5f)
				FinFlapDir = -1;
			if (leftFin.transform.rotation.z < -0.3f)
				FinFlapDir = 1;
		
			//feet
			float feetFlapSpeed = Time.deltaTime * 400.0f;
			leftFeet.transform.Rotate (Vector3.right * FeetFlapDir * feetFlapSpeed);
			rightFeet.transform.Rotate (Vector3.right * (-FeetFlapDir) * feetFlapSpeed);
			if (leftFeet.transform.rotation.x > 0.3f)
				FeetFlapDir = -1;
			if (leftFeet.transform.rotation.x < -0.2f)
				FeetFlapDir = 1;
		}

		if (isDroning == 1) 
		{
			//body
			float bodyRotateSpeed = Time.deltaTime * 250.0f;
			penguin.transform.Rotate (Vector3.up * BodyRotateDir * bodyRotateSpeed);
			if (penguin.transform.rotation.y > 0.2f)
				BodyRotateDir = -1;
			if (penguin.transform.rotation.y < -0.2f)
				BodyRotateDir = 1;
			
			//fin
			float finFlapSpeed = Time.deltaTime * 800.0f;
			leftFin.transform.Rotate (Vector3.forward * FinFlapDir * finFlapSpeed);
			rightFin.transform.Rotate (Vector3.forward * FinFlapDir * finFlapSpeed);
			if (leftFin.transform.rotation.z > 0.5f)
				FinFlapDir = -1;
			if (leftFin.transform.rotation.z < -0.5f)
				FinFlapDir = 1;
			
			//feet
			float feetFlapSpeed = Time.deltaTime * 700.0f;
			leftFeet.transform.Rotate (Vector3.right * FeetFlapDir * feetFlapSpeed);
			rightFeet.transform.Rotate (Vector3.right * (-FeetFlapDir) * feetFlapSpeed);
			if (leftFeet.transform.rotation.x > 0.3f)
				FeetFlapDir = -1;
			if (leftFeet.transform.rotation.x < -0.3f)
				FeetFlapDir = 1;
		}
	
	}

	void jumpPose ()
	{
		Quaternion leftFinJumpPose = new Quaternion(0.0f, 0.0f, 0.6f, 1.0f);
		Quaternion rightFinJumpPose = new Quaternion(0.0f, 0.0f, -0.6f, 1.0f);
		Quaternion leftFeetJumpPose = new Quaternion(-1.2f, 0.0f, 0.0f, 1.0f);
		Quaternion rightFeetJumpPose = new Quaternion(-1.1f, 0.0f, 0.0f, 1.0f);

		leftFin.transform.rotation = leftFinJumpPose;
		rightFin.transform.rotation = rightFinJumpPose;
		leftFeet.transform.rotation = leftFeetJumpPose;
		rightFeet.transform.rotation = rightFeetJumpPose;
	}

	void resetPose ()
	{
		penguin.transform.rotation = penguinStartPose;
		leftFin.transform.rotation = leftFinStartPose;
		rightFin.transform.rotation = rightFinStartPose;
		leftFeet.transform.rotation = leftFeetStartPose;
		rightFeet.transform.rotation = rightFeetStartPose;

		isWalking = 0;
		isDroning = 0;
		BodyRotateDir = 1;
		FinFlapDir = 1;
		FeetFlapDir = 1;
	}
}
