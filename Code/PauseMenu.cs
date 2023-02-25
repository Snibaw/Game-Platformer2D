using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public static bool gameisPaused;

	public GameObject pauseMenuUI;

	public GameObject settingsWindow;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (gameisPaused)
			{
				Resume();
			}
			else
			{
				Paused();
			}
		}
	}

	private void Paused()
	{
		PlayerMovement.instance.enabled = false;
		pauseMenuUI.SetActive(value: true);
		Time.timeScale = 0f;
		gameisPaused = true;
	}

	public void Resume()
	{
		PlayerMovement.instance.enabled = true;
		pauseMenuUI.SetActive(value: false);
		Time.timeScale = 1f;
		gameisPaused = false;
	}

	public void OpenSettingsWindow()
	{
		settingsWindow.SetActive(value: true);
	}

	public void CloseSettingsWindow()
	{
		settingsWindow.SetActive(value: false);
	}

	public void LoadMainMenu()
	{
		Resume();
		SceneManager.LoadScene("MainMenu");
	}
}
