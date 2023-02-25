using UnityEngine;
using UnityEngine.UI;

public class Ladder : MonoBehaviour
{
	private bool isInRange;

	private PlayerMovement playerMovement;

	public BoxCollider2D topCollider;

	private Text interactUI;

	private void Awake()
	{
		playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		interactUI = GameObject.FindGameObjectWithTag("InteractUI").GetComponent<Text>();
	}

	private void Update()
	{
		// If the player is in range and is climbing, then disable the collider on the top of the ladder
		if (isInRange && playerMovement.isClimbing && Input.GetKeyDown(KeyCode.E))
		{
			playerMovement.isClimbing = false;
			topCollider.isTrigger = false;
		}
		else if (isInRange && Input.GetKeyDown(KeyCode.E))
		{
			playerMovement.isClimbing = true;
			topCollider.isTrigger = true;
		}
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
			isInRange = false;
			playerMovement.isClimbing = false;
			topCollider.isTrigger = false;
			interactUI.enabled = false;
		}
	}
}