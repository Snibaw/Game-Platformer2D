using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
	public AudioMixer audioMixer;

	public Dropdown resolutionDropdown;

	private Resolution[] resolutions;

	public Slider musicSlider;

	public Slider soundSlider;

	public void Start()
	{
		// Set the default values
		audioMixer.GetFloat("Music", out var value);
		musicSlider.value = value;
		audioMixer.GetFloat("Sound", out var value2);
		soundSlider.value = value2;
		// Set the resolution options
		resolutions = Screen.resolutions.Select(delegate(Resolution Resolution)
		{
			Resolution result = default(Resolution);
			result.width = Resolution.width;
			result.height = Resolution.height;
			return result;
		}).Distinct().ToArray();
		resolutionDropdown.ClearOptions();
		List<string> list = new List<string>();
		int value3 = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			string item = resolutions[i].width + "x" + resolutions[i].height;
			list.Add(item);
			if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
			{
				value3 = i;
			}
		}
		// Set the default resolution
		resolutionDropdown.AddOptions(list);
		resolutionDropdown.value = value3;
		resolutionDropdown.RefreshShownValue();
		Screen.fullScreen = true;
	}

	public void SetVolume(float volume)
	{
		audioMixer.SetFloat("Music", volume);
	}

	public void SetSoundVolume(float volume)
	{
		audioMixer.SetFloat("Sound", volume);
	}

	public void SetFullScreen(bool isFullScreen)
	{
		Screen.fullScreen = isFullScreen;
	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}

	public void ClearSavedDate()
	{
		PlayerPrefs.DeleteAll();
	}
}
