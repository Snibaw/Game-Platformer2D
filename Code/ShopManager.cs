using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
	public Animator animator;

	public Text pnjNameText;

	public static ShopManager instance;

	public GameObject sellButtonPrefab;

	public Transform sellButtonParent;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instance de ShopManager dans la scène");
		}
		else
		{
			instance = this;
		}
	}

	public void OpenShop(Item[] items, string pnjName)
	{
		pnjNameText.text = pnjName;
		UpdateItemsToSell(items);
		animator.SetBool("isOpen", value: true);
	}

	private void UpdateItemsToSell(Item[] items) // This is the function that is called when you open the shop
	{
		for (int i = 0; i < sellButtonParent.childCount; i++)
		{
			Object.Destroy(sellButtonParent.GetChild(i).gameObject);
		}
		for (int j = 0; j < items.Length; j++)
		{
			//Create the button
			GameObject gameObject = Object.Instantiate(sellButtonPrefab, sellButtonParent);
			SellButtonItem buttonScript = gameObject.GetComponent<SellButtonItem>();
			buttonScript.itemName.text = items[j].name;
			buttonScript.itemImage.sprite = items[j].image;
			buttonScript.itemPrice.text = items[j].price.ToString();
			buttonScript.item = items[j];
			gameObject.GetComponent<Button>().onClick.AddListener(delegate
			{
				buttonScript.BuyItem();
			});
			gameObject.GetComponent<Button>().enabled = true;
			gameObject.GetComponent<Image>().enabled = true;
			// Enable the text
			GameObject[] array = GameObject.FindGameObjectsWithTag("Image");
			GameObject[] array2 = GameObject.FindGameObjectsWithTag("Text");
			GameObject[] array3 = array;
			for (int k = 0; k < array3.Length; k++)
			{
				array3[k].GetComponent<Image>().enabled = true;
			}
			array3 = array2;
			for (int k = 0; k < array3.Length; k++)
			{
				array3[k].GetComponent<Text>().enabled = true;
			}
		}
	}

	public void CloseShop()
	{
		animator.SetBool("isOpen", value: false);
		PlayerMovement.instance.isTalking = false;
	}
}
