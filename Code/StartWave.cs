using System.Collections;
using UnityEngine;

public class StartWave : MonoBehaviour
{
	public GameObject PNJ;

	public GameObject platform;

	public GameObject[] Wave1Ennemies;

	public GameObject[] Wave2Ennemies;

	public GameObject[] Wave3Ennemies;

	public GameObject[] healPowerUp;

	public bool isStarting;

	public GameObject[] ObjectToHide;

	public int Battues;

	public static StartWave instance;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instancede PlayerMovement dans la scène");
		}
		else
		{
			instance = this;
		}
	}

	private void Start()
	{
		// Hide all objects
		GameObject[] objectToHide = ObjectToHide;
		for (int i = 0; i < objectToHide.Length; i++)
		{
			objectToHide[i].SetActive(value: false);
		}
		for (int j = 0; j < Wave1Ennemies.Length; j++)
		{
			Wave1Ennemies[j].SetActive(value: false);
		}
		for (int k = 0; k < healPowerUp.Length; k++)
		{
			healPowerUp[k].SetActive(value: false);
		}
		for (int l = 0; l < Wave2Ennemies.Length; l++)
		{
			Wave2Ennemies[l].SetActive(value: false);
		}
		for (int m = 0; m < Wave3Ennemies.Length; m++)
		{
			Wave3Ennemies[m].SetActive(value: false);
		}
	}

	public void Update()
	{
		if (Battues == 10) // 10 = number of ennemies
		{
			GameObject[] objectToHide = ObjectToHide;
			for (int i = 0; i < objectToHide.Length; i++)
			{
				objectToHide[i].SetActive(value: true);
			}
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!isStarting) 
		{
			// Start the wave
			isStarting = true;
			PlayerMovement.instance.jumpForce += 100f;
			PNJ.SetActive(value: false);
			platform.SetActive(value: false);
			StartCoroutine(TimingWave());
		}
	}

	public IEnumerator TimingWave() 
	{
		// Wait 5 seconds before starting the wave
		yield return new WaitForSeconds(5f);
		for (int i = 0; i < Wave1Ennemies.Length; i++)
		{
			Wave1Ennemies[i].SetActive(value: true);
		}
		for (int j = 0; j < healPowerUp.Length; j++)
		{
			healPowerUp[j].SetActive(value: true);
		}
		yield return new WaitForSeconds(15f);
		for (int k = 0; k < Wave2Ennemies.Length; k++)
		{
			Wave2Ennemies[k].SetActive(value: true);
		}
		yield return new WaitForSeconds(15f);
		for (int l = 0; l < Wave3Ennemies.Length; l++)
		{
			Wave3Ennemies[l].SetActive(value: true);
		}
	}
}
