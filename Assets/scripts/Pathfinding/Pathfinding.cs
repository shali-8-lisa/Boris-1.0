using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour {

	public Transform seeker, target;
	Grid grid;

	void Awake() 
	{
		grid = GetComponent<Grid> ();
	}

	void Update() 
	{
		FindPath (seeker.position, target.position);
	}

	void FindPath(Vector3 startPos, Vector3 targetPos) 
	{
		Stopwatch sw = new Stopwatch ();
		sw.Start ();
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		//OPEN (the set of nodes to be evaluated)
		Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
		//CLOSED (the set of nodes already evaluated)
		HashSet<Node> closedSet = new HashSet<Node>();
		//add the start node to OPEN
		openSet.Add(startNode);

		//loop
		while (openSet.Count > 0) 
		{
			//current = node in OPEN with the lowest f_cost
			Node node = openSet.RemoveFirst();
			closedSet.Add (node);

			if (node == targetNode) 
			{
				sw.Stop ();
				print ("Path found: " + sw.ElapsedMilliseconds + "ms");
				RetracePath(startNode,targetNode);
				return;
			}

			//foreach neighbour of the current node
			foreach (Node neighbour in grid.GetNeighbours(node)) 
			{
				//if neighbour is not traversable or neighbour is in CLOSED
				//	skip to the next neighbour
				if (!neighbour.walkable || closedSet.Contains(neighbour)) 
				{
					continue;
				}

				//if new path to neighbour is shorter or neighbour is not in OPEN
				//if neighbour is not in OPEN
				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) 
				{
					//set f_cost of neighbour
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					//set parent of neighbour to current
					neighbour.parent = node;

					//add neighbour to OPEN
					if (!openSet.Contains (neighbour))
						openSet.Add (neighbour);
					else
						openSet.UpdateItem (neighbour);
				}
			}
		}
	}

	void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		grid.path = path;

	}

	int GetDistance(Node nodeA, Node nodeB) 
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}
