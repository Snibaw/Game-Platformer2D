using UnityEngine;

public class TutorialSnake : MonoBehaviour
{
	public EnnemyHealth ennemyHealth;

	public GameObject PNJ;

	public GameObject platform;

	private void Start()
	{
		PNJ.SetActive(value: false);
		platform.SetActive(value: false);
	}

	private void Update()
	{
		if (ennemyHealth.isDead) // If the ennemy is dead
		{
			PNJ.SetActive(value: true);
			platform.SetActive(value: true);
		}
	}
}
