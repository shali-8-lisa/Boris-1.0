using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;

public class PenguinController : MonoBehaviour {

	public static int Hunger;

	private Blackboard blackboard;
	private Root behaviorTree;

	void Start()
	{
		behaviorTree = CreateBehaviourTree();
		blackboard = behaviorTree.Blackboard;

		#if UNITY_EDITOR
		Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
		debugger.BehaviorTree = behaviorTree;
		#endif

		behaviorTree.Start();
	}

	private Root CreateBehaviourTree()
	{
		return new Root (

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
		behaviorTree.Blackboard["full"] = false;
		behaviorTree.Blackboard["fishGo"] = true;
		behaviorTree.Blackboard["BorisGo"] = false;
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


	/*
	void Start ()
	{
		Hunger = UnityEngine.Random.Range (90, 120);
	}

	void Update ()
	{
		float distance = Vector3.Distance (transform.position, Boris.transform.position);
		if (distance < 15.0f)
			Unit.penguinTarget = 1;

		Debug.Log ("Hunger: " + Hunger);
	
	}*/
}
