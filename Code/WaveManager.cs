using UnityEngine;

public class WaveManager : MonoBehaviour
{
	public EnnemyHealth vie;

	private bool Done;

	public void Update()
	{
		if (vie.currentHealth <= 0 && !Done) // If the ennemy is dead and the wave is not done
		{
			Done = true;
			StartWave.instance.Battues++;
		}
	}
}
