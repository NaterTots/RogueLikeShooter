using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModel : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		Debug.Log("Weapon OnTriggerEnter");
		if (other.tag.Equals("Enemy"))
		{
			other.gameObject.GetComponent<Enemy>().TakeDamage(1, this.transform.position);
		}
	}
}
