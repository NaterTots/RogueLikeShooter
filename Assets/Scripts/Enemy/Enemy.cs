using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour 
{
	Player target;
	NavMeshAgent agent;

	// Use this for initialization
	void Start () 
	{
		target = FindObjectOfType<Player>();
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		agent.SetDestination(target.transform.position);
	}
}
