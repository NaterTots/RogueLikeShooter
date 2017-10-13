using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemyConfiguration
{
	public int enemyid;
	public string name;
	public string model;
	public float speed;

	public GameObject Prefab { get; set; }
}