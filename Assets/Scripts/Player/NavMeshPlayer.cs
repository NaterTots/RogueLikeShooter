using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshPlayer : MonoBehaviour 
{
	NavMeshAgent agent;

	bool inRightClickMode = false;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		bool rightButtonDown = Input.GetMouseButton(1);
		if (Input.GetMouseButtonDown(0) || rightButtonDown)
		{
			inRightClickMode = rightButtonDown;

			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
			{
				agent.destination = hit.point;
			}
		}
		else if (inRightClickMode && !rightButtonDown)
		{
			agent.destination = transform.position;
		}
	}
}
