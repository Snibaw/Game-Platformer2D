using UnityEngine;

public class CameraEndless : MonoBehaviour
{
	public Transform player;

	public float increasePerSecond;

	public float speedStart;

	private float secondsElapsed;

	public PlatformGenerator platformGenerator;

	private Vector3 velocity;

	private void Start()
	{
		base.transform.position = new Vector3(player.position.x, player.position.y, -20f);
	}

	private void Update()
	{
		if (PlayerHealth.instance.currentHealth > 0f)
		{
			if (base.transform.position.x - player.position.x + 4f < 1.5f) // 1.5f is the distance between the camera and the player
			{
				base.transform.position = Vector3.SmoothDamp(base.transform.position, new Vector3(player.position.x, platformGenerator.PlatformPosition.y, -20f), ref velocity, 1f);
			}
			base.transform.Translate(Vector3.right * Time.deltaTime * (increasePerSecond * secondsElapsed + speedStart));
			secondsElapsed += Time.deltaTime;
			base.transform.position = Vector3.SmoothDamp(base.transform.position, new Vector3(base.transform.position.x, platformGenerator.PlatformPosition.y, -20f), ref velocity, 3f);
			if (base.transform.position.x - player.position.x > 15f) // 15f is the distance between the camera and the player
			{
				platformGenerator.PlayerOutOfGame();
			}
		}
	}
}
