using System.Linq;
using UnityEngine;

public class LoadAndSaveData : MonoBehaviour
{
	public static LoadAndSaveData instance;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instancede LoadAndSaveData dans la scène");
		}
		else
		{
			instance = this;
		}
	}

	private void Start()
	{
		// Gather all the data from the PlayerPrefs
		Inventory.instance.coinsCount = PlayerPrefs.GetInt("coinsCount", 0);
		Inventory.instance.UpdateTextUI();
		string[] array = PlayerPrefs.GetString("inventoryItems", "").Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != "")
			{
				int id = int.Parse(array[i]);
				Item item = ItemsDatabase.instance.allItems.Single((Item x) => x.id == id);
				Inventory.instance.content.Add(item);
			}
		}
		Inventory.instance.UpdateInvotoryUI();
	}

	public void SaveData()
	{
		// Save all the data to the PlayerPrefs
		PlayerPrefs.SetInt("coinsCount", Inventory.instance.coinsCount);
		if (CurrentSceneManager.instance.levelToUnlock > PlayerPrefs.GetInt("levelReached", 1))
		{
			PlayerPrefs.SetInt("levelReached", CurrentSceneManager.instance.levelToUnlock);
		}
		string value = string.Join(",", Inventory.instance.content.Select((Item x) => x.id));
		PlayerPrefs.SetString("inventoryItems", value);
	}
}
