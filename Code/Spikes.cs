using UnityEngine;

public class Spikes : MonoBehaviour
{
	public int SpikesDamage;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			PlayerHealth.instance.TakeDamage(SpikesDamage);
		}
	}
}
