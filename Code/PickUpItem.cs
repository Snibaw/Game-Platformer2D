using UnityEngine;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour
{
	private bool isInRange;

	private Text interactUI;

	public Item item;

	public AudioClip soundToPlay;

	private void Awake()
	{
		interactUI = GameObject.FindGameObjectWithTag("InteractUI").GetComponent<Text>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E) && isInRange)
		{
			TakeItem();
		}
	}

	private void TakeItem() // This is the method that is called when the player presses E
	{
		// Add the item to the inventory, update the inventory UI, play the sound and destroy the item
		PlayerMovement.instance.animator.SetTrigger("isPicking");
		Inventory.instance.content.Add(item);
		Inventory.instance.UpdateInvotoryUI();
		AudioManager.instance.PlayClipAt(soundToPlay, base.transform.position);
		interactUI.enabled = false;
		PlayerMovement.instance.animator.SetTrigger("isnotPicking");
		Object.Destroy(base.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			interactUI.enabled = true;
			isInRange = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			interactUI.enabled = false;
			isInRange = false;
		}
	}
}
