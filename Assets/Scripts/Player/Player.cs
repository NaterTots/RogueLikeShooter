using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour 
{
	NavMeshAgent agent;

	bool inSustainedMoveMode = false;
	bool isAttacking = false;

	private InputController inputController;

	private Animation weaponAnimation;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.speed = GameController.GetController<ConfigurationController>().PlayerConfig.Movement.speed;

		inputController = GameController.GetController<InputController>();
		inputController.AddKeyCodeListener(KeyCode.Space, OnSwing);

		weaponAnimation = GetComponentInChildren<Animation>();
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

	void OnSwing()
	{
		if (!isAttacking)
		{
			isAttacking = true;

			weaponAnimation.Play("SwordSwing");
		}
	}

	public void OnSwingComplete()
	{
		isAttacking = false;
	}
}
