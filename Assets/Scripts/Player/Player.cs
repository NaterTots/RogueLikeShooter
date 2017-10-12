using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour 
{
	NavMeshAgent agent;

	bool inSustainedMoveMode = false;

	private InputController inputController;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.speed = GameController.GetController<ConfigurationController>().PlayerConfig.Movement.speed;

		inputController = GameController.GetController<InputController>();
	}

	void Update()
	{
		bool sustainedMode = inputController.IsSustainedMove();
		if (inputController.IsAttemptSingleMove() || sustainedMode)
		{
			inSustainedMoveMode = sustainedMode;

			RaycastHit hit;
			if (Physics.Raycast(inputController.GetMoveAttemptRay(), out hit, 100))
			{
				agent.destination = hit.point;
			}
		}
		else if (inSustainedMoveMode && !sustainedMode)
		{
			agent.destination = transform.position;
			inSustainedMoveMode = false;
		}
	}
}
