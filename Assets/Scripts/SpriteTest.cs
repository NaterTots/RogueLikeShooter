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
	}
}
