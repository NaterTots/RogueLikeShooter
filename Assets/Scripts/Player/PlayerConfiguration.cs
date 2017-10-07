using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerConfiguration
{
	[System.Serializable]
	public class MovementConfiguration
	{
		public float speed = 4.0f;
	}

	public MovementConfiguration Movement = new MovementConfiguration();
}