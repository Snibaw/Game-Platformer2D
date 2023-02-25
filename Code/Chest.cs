using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
	private bool isInRange;

	private Text interactUI;

	public Animator animator;

	public int coinsToAdd;

	public AudioClip soundToPlay;

	private void Awake()
	{
		interactUI = GameObject.FindGameObjectWithTag("InteractUI").GetComponent<Text>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E) && isInRange)
		{
			OpenChest();
		}
	}

	private void OpenChest() // This is the method that is called when the player presses E
	{
		animator.SetTrigger("OpenChest");
		Inventory.instance.AddCoins(coinsToAdd);
		AudioManager.instance.PlayClipAt(soundToPlay, base.transform.position);
		GetComponent<BoxCollider2D>().enabled = false;
		interactUI.enabled = false;
	}

	private void OnTriggerEnter2D(Collider2D collision) // When player enters the trigger
	{
		if (collision.CompareTag("Player"))
		{
			interactUI.enabled = true;
			isInRange = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision) // When player exits the trigger
	{
		if (collision.CompareTag("Player"))
		{
			interactUI.enabled = false;
			isInRange = false;
		}
	}
}
