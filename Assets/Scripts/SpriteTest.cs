using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTest : MonoBehaviour 
{
	void Awake()
	{
		Sprite newSprite;
		if (GameController.GetController<ResourceController>().TryGetSprite("test", out newSprite))
		{
			GetComponent<SpriteRenderer>().sprite = newSprite;
		}
	}

	void Start()
	{
		AudioClip soundEffect;
		if (GameController.GetController<ResourceController>().TryGetSoundEffect("testsoundeffect", out soundEffect))
		{
			GameController.GetController<SoundController>().PlayClipOneShot(soundEffect);
		}

		GameController.GetController<InputController>().AddKeyCodeListener(KeyCode.T, OnToggleRotation);
	}

	void OnRotateLeft()
	{
		gameObject.transform.Rotate(0, 0, 90, Space.Self);
		GameController.GetController<ConfigurationController>().GetSetting(SingleGameStats.Points).IncrementValueAsLong(10L);
	}

	void OnRotateRight()
	{
		gameObject.transform.Rotate(0, 0, -90, Space.Self);
		GameController.GetController<ConfigurationController>().GetSetting(SingleGameStats.Points).IncrementValueAsLong(-10L);
	}

	bool canRotate = false;

	void OnToggleRotation()
	{
		if (!canRotate)
		{
			GameController.GetController<InputController>().AddKeyCodeListener(KeyCode.G, OnRotateLeft);
			GameController.GetController<InputController>().AddKeyCodeListener(KeyCode.H, OnRotateRight);

			canRotate = true;
		}
		else
		{
			GameController.GetController<InputController>().RemoveKeyCodeListener(KeyCode.G, OnRotateLeft);
			GameController.GetController<InputController>().RemoveKeyCodeListener(KeyCode.H, OnRotateRight);

			canRotate = false;
		}
	}
}
