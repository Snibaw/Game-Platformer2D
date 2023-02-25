using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public string levelToLoad;

	public GameObject settingsWindow;

	public void StartGame()
	{
		SceneManager.LoadScene(levelToLoad);
	}

	public void EndlessGame()
	{
		SceneManager.LoadScene("Endless");
	}

	public void SettingsButton()
	{
		settingsWindow.SetActive(value: true);
	}

	public void CloseSettingsWindow()
	{
		settingsWindow.SetActive(value: false);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void LoadCreditsScene()
	{
		SceneManager.LoadScene("Credits");
	}
}