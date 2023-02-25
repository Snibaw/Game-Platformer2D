using UnityEngine;
using UnityEngine.UI;

public class SellButtonItem : MonoBehaviour
{
	public Text itemName;

	public Image itemImage;

	public Text itemPrice;

	public Item item;

	public void BuyItem() 
	{
		
		Inventory instance = Inventory.instance;
		if (instance.coinsCount >= item.price) // if we have enough money
		{
			instance.content.Add(item);
			instance.UpdateInvotoryUI();
			instance.coinsCount -= item.price;
			instance.UpdateTextUI();
		}
	}
}
