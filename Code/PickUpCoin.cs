using UnityEngine;

public class PickUpCoin : MonoBehaviour
{
	public AudioClip sound;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			// Add the coin to the inventory, play the sound, and destroy the coin
			AudioManager.instance.PlayClipAt(sound, base.transform.position);
			Inventory.instance.AddCoins(1);
			CurrentSceneManager.instance.coinsPickedUpInThisSceneCount++;
			Object.Destroy(base.gameObject);
		}
	}
}
