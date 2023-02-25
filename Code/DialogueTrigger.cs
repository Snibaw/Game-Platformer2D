using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
	public Dialogue dialogue;

	public bool isInRange;

	private Text interactUI;

	private PlayerMovement playerMovement;

	public float timerText;

	private float timeBTWNextSentences = 0.2f;

	private void Start()
	{
		playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		interactUI = GameObject.FindGameObjectWithTag("InteractUI").GetComponent<Text>();
		interactUI.enabled = false;
	}

	private void Update()
	{
		if (isInRange && Input.GetKeyDown(KeyCode.E))
		{
			if (playerMovement.isTalking)
			{
				if (timerText <= 0f) // If the timer is less than or equal to 0 then display the next sentence
				{
					timerText = timeBTWNextSentences;
					DialogueManager.instance.DisplayNextSentence();
					Debug.Log("Prochaine sentence stp");
				}
			}
			else
			{
				playerMovement.rb.velocity = Vector3.zero;
				playerMovement.isTalking = true;
				TriggerDialogue();
			}
		}
		else if (Input.GetKeyDown(KeyCode.Space) && playerMovement.isTalking) // If the player is talking and press space then end the dialogue
		{
			DialogueManager.instance.EndDialogue();
		}
		timerText -= Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			isInRange = true;
			interactUI.enabled = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			isInRange = false;
			interactUI.enabled = false;
		}
	}

	private void TriggerDialogue()
	{
		DialogueManager.instance.StartDialogue(dialogue);
	}
}
