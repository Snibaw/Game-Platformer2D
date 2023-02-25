using UnityEngine;

public class WeakSpot : MonoBehaviour
{
	public AudioClip killSound;

	public GameObject objectToDestroy;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player")) // If the player hits the weak spot
		{
			AudioManager.instance.PlayClipAt(killSound, base.transform.position);
			Object.Destroy(objectToDestroy);
		}
	}
}
