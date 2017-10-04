using UnityEngine;
using System.Collections;

public class SingleGameStats : MonoBehaviour
{
	public static string Points = "Points";
	public static string Money = "Money";

	private void Awake()
	{
		GameController.GetController<ConfigurationController>().AddSetting(Points, 0L);
		GameController.GetController<ConfigurationController>().AddSetting(Money, 0L);
	}

	private void OnDestroy()
	{
		GameController.GetController<ConfigurationController>().RemoveSetting(Points);
		GameController.GetController<ConfigurationController>().RemoveSetting(Money);
	}
}
