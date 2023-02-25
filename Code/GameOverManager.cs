using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
	public GameObject gameOverUI;

	public static GameOverManager instance;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instancede GameOverManager dans la scène");
		}
		else
		{
			instance = this;
		}
	}

	public void OnPlayerDeath()
	{
		StartCoroutine(WaitScreen());
	}

	public void RetryButton()
	{
		// Reset the player's health, coins, and the scene
		Inventory.instance.RemoveCoins(CurrentSceneManager.instance.coinsPickedUpInThisSceneCount);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		PlayerHealth.instance.Respawn();
		gameOverUI.SetActive(value: false);
	}

	public void MainMenuButton()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void QuitButton()
	{
		Application.Quit();
	}

	public IEnumerator WaitScreen()
	{
		yield return new WaitForSeconds(1f);
		gameOverUI.SetActive(value: true);
	}
}
