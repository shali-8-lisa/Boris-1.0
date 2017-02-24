﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	const float minPathUpdateTime = 0.2f;
	const float pathUpdateMoveThreshold = 0.5f;

	public float speed = 15;
	public float turnSpeed = 3;
	public float turnDst = 5;
	public float stoppingDst = 10;

	public static int penguinTarget;
	public static GameObject myFish;

	Path path;

	private GameObject Boris;
	private GameObject Test;
	private Transform target;

	void Start() 
	{
		Boris = GameObject.FindGameObjectWithTag ("Boris");
		Test = GameObject.FindGameObjectWithTag("Test");
		myFish = gameObject;

		StartCoroutine (UpdatePath ());
	}

	void Update()
	{
		//set up target for the Penguin
		if (penguinTarget == 0)
			target = gameObject.transform;
		else if (penguinTarget == 1)
			target = myFish.transform;
		else if (penguinTarget == 2)
			target = Boris.transform;
		else
			target = Test.transform;
	}

	public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) 
	{
		if (pathSuccessful) 
		{
			path = new Path(waypoints, transform.position, turnDst, stoppingDst);

			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}
		
	IEnumerator UpdatePath() 
	{
		if (Time.timeSinceLevelLoad < 0.3f) 
			yield return new WaitForSeconds (0.3f);
				
		PathRequestManager.RequestPath (new PathRequest(transform.position, target.position, OnPathFound));

		float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
		Vector3 targetPosOld = target.position;

		while (true) 
		{
			yield return new WaitForSeconds (minPathUpdateTime);
			if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold) 
			{
				PathRequestManager.RequestPath (new PathRequest(transform.position, target.position, OnPathFound));
				targetPosOld = target.position;
			}
		}
	}

	IEnumerator FollowPath() 
	{
		bool followingPath = true;
		int pathIndex = 0;
		transform.LookAt (path.lookPoints [0]);

		float speedPercent = 1;

		while (followingPath) 
		{
			Vector2 pos2D = new Vector2 (transform.position.x, transform.position.z);
			while (path.turnBoundaries [pathIndex].HasCrossedLine (pos2D)) 
			{
				if (pathIndex == path.finishLineIndex) 
				{
					followingPath = false;
					break;
				} 
				else 
					pathIndex++;
			}

			if (followingPath) 
			{
				
				//slow down when gets close
				if (pathIndex >= path.slowDownIndex && stoppingDst > 0) 
				{
					speedPercent = Mathf.Clamp01 (path.turnBoundaries [path.finishLineIndex].DistanceFromPoint (pos2D) / stoppingDst);
					if (speedPercent < 0.01f)
						followingPath = false;
				}

				Quaternion targetRotation = Quaternion.LookRotation (path.lookPoints [pathIndex] - transform.position);
				transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
				transform.Translate (Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
			}

			yield return null;
		}

		if (target.gameObject.tag == "Fish")
			Destroy (target.gameObject);
		else if (target.gameObject.tag == "Boris")
			PlayerController.BorisSpeed -= 10;
	}

	public void OnDrawGizmos() 
	{
		if (path != null) 
			path.DrawWithGizmos ();
	}

	GameObject FindClosestFish() 
	{
		GameObject[] fishCollection;
		fishCollection = GameObject.FindGameObjectsWithTag("Fish");
		GameObject closestFish = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;

		foreach (GameObject currentFish in fishCollection) 
		{
			Vector3 diff = currentFish.transform.position - position;
			float currentDistance = diff.sqrMagnitude;

			if (currentDistance < distance) 
			{
				closestFish = currentFish;
				distance = currentDistance;
			}
		}

		return closestFish;
	}
}
