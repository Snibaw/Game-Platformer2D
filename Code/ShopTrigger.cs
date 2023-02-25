using UnityEngine;
using UnityEngine.UI;

public class ShopTrigger : MonoBehaviour
{
	public bool isInRange;

	private Text interactUI;

	private PlayerMovement playerMovement;

	public Item[] itemsToSell;

	public string pnjName;

	private void Awake()
	{
		interactUI = GameObject.FindGameObjectWithTag("InteractUI").GetComponent<Text>();
	}

	private void Update()
	{
		// If the player is in range and press E, open the shop
		if (isInRange && Input.GetKeyDown(KeyCode.E))
		{
			ShopManager.instance.OpenShop(itemsToSell, pnjName);
			PlayerMovement.instance.rb.velocity = Vector3.zero;
			PlayerMovement.instance.isTalking = true;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) // When the player enter the trigger
	{
		if (collision.CompareTag("Player"))
		{
			isInRange = true;
			interactUI.enabled = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision) // When the player exit the trigger
	{
		if (collision.CompareTag("Player"))
		{
			isInRange = false;
			interactUI.enabled = false;
			ShopManager.instance.CloseShop();
		}
	}
}
