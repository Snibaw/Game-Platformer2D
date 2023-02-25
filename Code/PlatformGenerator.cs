using UnityEngine;
using UnityEngine.UI;

public class PlatformGenerator : MonoBehaviour
{
	public Text pointText;

	public int point;

	public GameObject[] platform;

	public GameObject[] EASYEnnemies;

	public GameObject[] NORMALEnnemies;

	public GameObject[] HARDEnnemies;

	public float[] pointEnnemies = new float[3] { 250f, 500f, 1000f };

	public GameObject spike;

	public GameObject heart;

	private int maxY;

	private float lastLength;

	public Vector2 PlatformPosition = new Vector2(0f, 0f);

	private Vector2 lastPlaformPosition;

	public GameObject player;

	public GameObject mainCamera;

	private void Start()
	{
		lastLength = 3.5f;
		CreatePlatforme(lastLength);
	}

	private void Update()
	{
		if (player.transform.position.x / 3f > (float)point) // 3f is the distance between each platform
		{
			point = Mathf.RoundToInt(player.transform.position.x / 3f);
			pointText.text = point.ToString();
		}
		if (Mathf.Abs(PlatformPosition.x - mainCamera.transform.position.x) < 10f) // 10f is the distance between the camera and the last platform
		{
			CreatePlatforme(lastLength);
		}
		if (player.transform.position.y < PlatformPosition.y - 5f) // 5f is the distance between the player and the last platform
		{
			PlayerOutOfGame();
		}
	}

	private void CreatePlatforme(float _lastLength) // Create a new platform
	{
		int num = Random.Range(0, platform.Length);
		int num2 = Mathf.RoundToInt(platform[num].GetComponent<BoxCollider2D>().size.x);
		int num3 = Random.Range(3, 9);
		if (num3 >= 7)
		{
			maxY = 1;
		}
		else
		{
			maxY = 3;
		}
		int num4 = Random.Range(-2, maxY);
		lastPlaformPosition = PlatformPosition;
		PlatformPosition = new Vector3(PlatformPosition.x + 3.5f + (float)(num2 / 2) + (float)num3, PlatformPosition.y + (float)num4, 0f);
		Object.Instantiate(platform[num], PlatformPosition, Quaternion.identity);
		addExtra(num2);
		lastLength = num2;
	}

	private void addExtra(float _platformLength) // Add extra on the platform (ennemies, spikes, hearts)
	{
		if (!(_platformLength > 2f))
		{
			return;
		}
		int num = Random.Range(0, 9);
		float num2 = Random.Range((0f - _platformLength) / 2f + 1.5f, _platformLength / 2f - 1.5f);
		if (num == 8)
		{
			Object.Instantiate(heart, new Vector3(PlatformPosition.x + num2, PlatformPosition.y + 1.2f, 0f), Quaternion.identity);
		}
		else if (num <= 2)
		{
			Object.Instantiate(spike, new Vector3(PlatformPosition.x + num2, PlatformPosition.y + 1.1f, 0f), Quaternion.identity);
		}
		else if (num <= 5 && _platformLength > 3f)
		{
			if ((float)point <= pointEnnemies[0]) // Chose which ennemy is adapted to the player's level (low)
			{
				int num3 = Random.Range(0, EASYEnnemies.Length);
				GameObject obj = Object.Instantiate(EASYEnnemies[num3], new Vector3(PlatformPosition.x, PlatformPosition.y + 1.1f, 0f), Quaternion.identity);
				GameObject gameObject = obj.transform.GetChild(0).gameObject;
				GameObject gameObject2 = obj.transform.GetChild(1).gameObject;
				GameObject obj2 = obj.transform.GetChild(2).gameObject;
				gameObject.transform.position = new Vector3(PlatformPosition.x, PlatformPosition.y + 1.2f, 0f);
				gameObject2.transform.position = new Vector3(PlatformPosition.x - _platformLength / 2f + 0.8f, PlatformPosition.y + 1.2f, 0f);
				obj2.transform.position = new Vector3(PlatformPosition.x + _platformLength / 2f - 0.8f, PlatformPosition.y + 1.2f, 0f);
			}
			else if ((float)point <= pointEnnemies[1]) // Chose which ennemy is adapted to the player's level (medium)
			{
				int num4 = Random.Range(0, NORMALEnnemies.Length);
				GameObject obj3 = Object.Instantiate(NORMALEnnemies[num4], new Vector3(PlatformPosition.x, PlatformPosition.y + 1.1f, 0f), Quaternion.identity);
				GameObject gameObject3 = obj3.transform.GetChild(0).gameObject;
				GameObject gameObject4 = obj3.transform.GetChild(1).gameObject;
				GameObject obj4 = obj3.transform.GetChild(2).gameObject;
				gameObject3.transform.position = new Vector3(PlatformPosition.x, PlatformPosition.y + 1.2f, 0f);
				gameObject4.transform.position = new Vector3(PlatformPosition.x - _platformLength / 2f + 0.8f, PlatformPosition.y + 1.2f, 0f);
				obj4.transform.position = new Vector3(PlatformPosition.x + _platformLength / 2f - 0.8f, PlatformPosition.y + 1.2f, 0f);
			}
			else if ((float)point <= pointEnnemies[2]) // Chose which ennemy is adapted to the player's level (high)
			{
				int num5 = Random.Range(0, HARDEnnemies.Length);
				GameObject obj5 = Object.Instantiate(HARDEnnemies[num5], new Vector3(PlatformPosition.x, PlatformPosition.y + 1.1f, 0f), Quaternion.identity);
				GameObject gameObject5 = obj5.transform.GetChild(0).gameObject;
				GameObject gameObject6 = obj5.transform.GetChild(1).gameObject;
				GameObject obj6 = obj5.transform.GetChild(2).gameObject;
				gameObject5.transform.position = new Vector3(PlatformPosition.x, PlatformPosition.y + 1.2f, 0f);
				gameObject6.transform.position = new Vector3(PlatformPosition.x - _platformLength / 2f + 0.8f, PlatformPosition.y + 1.2f, 0f);
				obj6.transform.position = new Vector3(PlatformPosition.x + _platformLength / 2f - 0.8f, PlatformPosition.y + 1.2f, 0f);
				gameObject5.GetComponent<EnnemyHealth>().currentHealth += point / 2;
			}
		}
	}

	public void PlayerOutOfGame() // If the player is out of the game, he takes damage
	{
		player.transform.position = new Vector2(lastPlaformPosition.x, lastPlaformPosition.y + 1.5f);
		PlayerHealth.instance.TakeDamage(33);
	}
}
