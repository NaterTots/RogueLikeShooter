using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour 
{
	Player target;
	NavMeshAgent agent;

	public int Health { get; set; }

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

	public void TakeDamage(int damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			Die();
		}
	}

	public void Die()
	{
		Destroy(this);
	}
}
