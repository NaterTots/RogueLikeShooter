using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
	public float playerSpeed = 4f;

	private InputController inputController;

	private enum Facing
	{
		Up,
		Down,
		Left,
		Right
	};

	private Facing myFacing = Facing.Up;

	private GameObject renderingGameObject;

	void Start()
	{
		inputController = GameController.GetController<InputController>();

		renderingGameObject = transform.Find("PlayerRendering").gameObject;

	}

	void FixedUpdate()
	{
		Vector2 targetVelocity = new Vector2(inputController.GetAxis(InputController.Axis.Horizontal), inputController.GetAxis(InputController.Axis.Vertical));
		GetComponent<Rigidbody2D>().velocity = targetVelocity * playerSpeed;

		/*
         * Figure out direction we're facing based on the position of the mouse relative to the character.  The screen is divided up into quadrants:
         * |\          /|
         * | \        / |
         * |  \      /  |
         * |   \    /   |
         * |    \  /    |
         * |     \/     |
         * |     /\     |
         * |    /  \    |
         * |   /    \   |
         * |  /      \  |
         * | /        \ |
         * |/          \|
         * 
         * We can do some cheater math to figure out where in that grid (it's supposed to be a square, bear with me) the cursor is located using the fact that
         * along the lines, abs(xdiff) == abs(ydiff).  so if abs(xdiff) > abs(ydiff), for example, we know it's either in the left or right section.
         */

		Facing oldFacing = myFacing;

		Vector2 pointerPosition = inputController.GetMouseLocation();

		float xDiff = pointerPosition.x - transform.position.x;
		float yDiff = pointerPosition.y - transform.position.y;

		if (Mathf.Abs(xDiff) >= Mathf.Abs(yDiff))
		{
			myFacing = ((xDiff > 0) ? Facing.Right : Facing.Left);
		}
		else
		{
			myFacing = ((yDiff > 0) ? Facing.Up : Facing.Down);
		}


		if (oldFacing != myFacing)
		{
			SetNewRotation();
		}
	}

	private void SetNewRotation()
	{
		switch (myFacing)
		{
			case Facing.Up:
				renderingGameObject.transform.rotation = Quaternion.Euler(0f, 0f, 00f);
				break;
			case Facing.Down:
				renderingGameObject.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
				break;
			case Facing.Left:
				renderingGameObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
				break;
			case Facing.Right:
				renderingGameObject.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
				break;
		}
	}
}
