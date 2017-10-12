using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour 
{
	public Transform target;

	public Vector3 difference;

	private void Start()
	{
		difference = transform.position - target.position;
	}

	void Update()
	{
		transform.position = new Vector3(target.position.x, target.position.y, target.position.z) + difference;
	}
}
