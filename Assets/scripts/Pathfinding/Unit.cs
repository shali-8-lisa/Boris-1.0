using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	const float minPathUpdateTime = 0.2f;
	const float pathUpdateMoveThreshold = 0.5f;

	public Transform BorisTransform;
	public Transform FishTransform;
	public Transform TestTransform;
	public float speed = 15;
	public float turnSpeed = 3;
	public float turnDst = 5;
	public float stoppingDst = 10;

	public static int penguinTarget;

	Path path;

	private Transform target;

	void Start() 
	{
		StartCoroutine (UpdatePath ());
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

		//set up target for the Penguin
		if (penguinTarget == 0)
			target = gameObject.transform;
		else if (penguinTarget == 1)
			target = FishTransform;
		else if (penguinTarget == 2)
			target = BorisTransform;
		else
			target = TestTransform;
				
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
	}

	public void OnDrawGizmos() 
	{
		if (path != null) 
			path.DrawWithGizmos ();
	}
}
