using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;

public class PenguinController : MonoBehaviour {

	public static int Hunger;

	private Blackboard sharedBlackboard;
	private Blackboard ownBlackboard;
	private Root behaviorTree;

	void Start()
	{
		Hunger = UnityEngine.Random.Range (90, 120);

		sharedBlackboard = UnityContext.GetSharedBlackboard ("penguin-union");
		ownBlackboard = new Blackboard (sharedBlackboard, UnityContext.GetClock());

		behaviorTree = CreateBehaviourTree();

		#if UNITY_EDITOR
		Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
		debugger.BehaviorTree = behaviorTree;
		#endif

		behaviorTree.Start();
	}

	private Root CreateBehaviourTree()
	{
		return new Root (ownBlackboard,

			new Service (0.125f, UpdatePenguinState,

				new Selector(

					new BlackboardCondition ("full", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART,

						new Sequence (

							new Action(() => PrintInfo(0)) { Label = "Penguin is full" },

							new Action((bool _shouldCancel) =>
								{
									if (!_shouldCancel)
									{
										LieDown();
										return Action.Result.PROGRESS;
									}
									else
									{
										return Action.Result.FAILED;
									}
								}){ Label = "Penguin lies down" }
						)
					),

					new Selector(

						new BlackboardCondition ("fishGo", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART,

							new Sequence (

								new Action(() => PrintInfo(1)) { Label = "Fish mission" },

								new Action((bool _shouldCancel) =>
									{
										if (!_shouldCancel)
										{
											MoveTowardsFish();
											return Action.Result.PROGRESS;
										}
										else
										{
											return Action.Result.FAILED;
										}
									}){ Label = "Penguin moves to fish" }
							)
						),

						new Selector (
						
							new BlackboardCondition ("BorisGo", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART,

								new Sequence(
							
									new Action(() => PrintInfo(2)) { Label = "Boris mission" },

									new Action((bool _shouldCancel) =>
										{
											if (!_shouldCancel)
											{
												MoveTowardsBoris();
												return Action.Result.PROGRESS;
											}
											else
											{
												return Action.Result.FAILED;
											}
										}){ Label = "Penguin moves to Boris" }
								)
							),

							new Sequence (

								new Action(() => PrintInfo(3)) { Label = "Wander mission" },

								new Action((bool _shouldCancel) =>
									{
										if (!_shouldCancel)
										{
											Wander();
											return Action.Result.PROGRESS;
										}
										else
										{
											return Action.Result.FAILED;
										}
									}){ Label = "Penguin is wandering" }
							
							)
						)
					
					)
				
				)
			)

		);
	}

	private void UpdatePenguinState ()
	{

		GameObject fishFound = FindClosestFish ();
		Unit.myFish = fishFound;

		Vector3 fishLocalPos = this.transform.InverseTransformPoint(fishFound.transform.position);
		Vector3 BorisLocalPos = this.transform.InverseTransformPoint(GameObject.FindGameObjectWithTag("Boris").transform.position);

		if (Hunger == 0)
			ownBlackboard["full"] = true;
		else
			ownBlackboard["full"] = false;

		if (Hunger > 0 && fishLocalPos.magnitude < 15.0f)
			ownBlackboard["fishGo"] = true;
		else
			ownBlackboard["fishGo"] = false;

		if (Hunger > 0 && fishLocalPos.magnitude > 15.0f && BorisLocalPos.magnitude < 10.0f)
			ownBlackboard["BorisGo"] = true;
		else
			ownBlackboard["BorisGo"] = false;
	}

	private void PrintInfo (int flag)
	{
		if (flag == 0)
			Debug.Log ("Penguin lies down");
		else if (flag == 1)
			Debug.Log ("Penguin moves to fish.");
		else if (flag == 2)
			Debug.Log ("Penguin moves to Boris.");
		else
			Debug.Log ("Penguin is wandering.");
	}
		
	private void LieDown ()
	{
		Unit.penguinTarget = 0;
	}

	private void MoveTowardsFish ()
	{
		Unit.penguinTarget = 1;
	}

	private void MoveTowardsBoris ()
	{
		Unit.penguinTarget = 2;
	}

	private void Wander ()
	{
		Unit.penguinTarget = 3;
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
