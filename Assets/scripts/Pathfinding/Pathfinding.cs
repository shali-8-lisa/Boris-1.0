using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		//OPEN (the set of nodes to be evaluated)
		List<Node> openSet = new List<Node>();
		//CLOSED (the set of nodes already evaluated)
		HashSet<Node> closedSet = new HashSet<Node>();
		//add the start node to OPEN
		openSet.Add(startNode);

		//loop
		while (openSet.Count > 0) 
		{
			//current = node in OPEN with the lowest f_cost
			Node node = openSet[0];
			for (int i = 1; i < openSet.Count; i++) 
			{
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) 
				{
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}

			//remove current from OPEN
			openSet.Remove(node);
			//add current to CLOSED
			closedSet.Add(node);

			//if current is the target node (path has been found)
			//	return
			if (node == targetNode) 
			{
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
					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
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
