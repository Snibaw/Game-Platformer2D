using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public int coinsCount;

	public Text coinsCountText;

	public static Inventory instance;

	public Image itemImageUI;

	public Sprite emptyItemImage;

	public Text itemNameUI;

	public List<Item> content = new List<Item>();

	private int contentCurrentIndex;

	public Text PreviousButton;

	public Text NextButton;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instancede Inventory dans la scène");
			return;
		}
		UpdateInvotoryUI();
		instance = this;
	}

	public void ConsumeItem()
	{
		// If there is an item in the inventory
		if (content.Count != 0)
		{
			Item item = content[contentCurrentIndex];
			PlayerHealth.instance.HealPlayer(item.hpGiven);
			PlayerEffect.instance.AddSpeed(item.speedGiven, item.duration);
			PlayerEffect.instance.AddJump(item.jumpGiven, item.duration);
			content.Remove(item);
			GetNextItem();
			UpdateInvotoryUI();
		}
	}

	public void GetNextItem()
	{
		// If there is an item in the inventory
		if (content.Count != 0)
		{
			contentCurrentIndex++;
			if (contentCurrentIndex > content.Count - 1)
			{
				contentCurrentIndex = 0;
			}
			UpdateInvotoryUI();
		}
	}

	public void GetPreviousItem()
	{
		// If there is an item in the inventory
		if (content.Count != 0)
		{
			contentCurrentIndex--;
			if (contentCurrentIndex < 0)
			{
				contentCurrentIndex = content.Count - 1;
			}
			UpdateInvotoryUI();
		}
	}

	public void UpdateInvotoryUI()
	{
		// If there is an item in the inventory
		if (content.Count > 0)
		{
			itemImageUI.sprite = content[contentCurrentIndex].image;
			itemNameUI.text = content[contentCurrentIndex].name;
			PreviousButton.enabled = true;
			NextButton.enabled = true;
		}
		else // If there is no item in the inventory
		{
			itemImageUI.sprite = emptyItemImage;
			itemNameUI.text = "";
			PreviousButton.enabled = false;
			NextButton.enabled = false;
		}
	}

	public void AddCoins(int count)
	{
		coinsCount += count;
		UpdateTextUI();
	}

	public void RemoveCoins(int count)
	{
		coinsCount -= count;
		UpdateTextUI();
	}

	public void UpdateTextUI()
	{
		coinsCountText.text = coinsCount.ToString();
		StartCoroutine(ChangeFrontSize(coinsCountText.fontSize, coinsCountText.fontSize + 15));
	}

	public IEnumerator ChangeFrontSize(int originalFront, int newFront)
	{
		// Change the front size of the text when the player get coins
		yield return new WaitForSeconds(0.1f);
		coinsCountText.fontSize = newFront;
		yield return new WaitForSeconds(0.2f);
		coinsCountText.fontSize = originalFront;
	}
}
