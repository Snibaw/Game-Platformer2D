using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	public Animator animator;

	public Text nameText;

	public Text dialogueText;

	private PlayerMovement playerMovement;

	private Queue<string> sentences;

	public static DialogueManager instance;

	private bool entireDialogue;

	private bool dialogueEnded;

	private bool tempo;

	private string phrase2;

	private string phrase1;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instance e DialogueManager dans la scène");
			return;
		}
		instance = this;
		playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		sentences = new Queue<string>();
	}

	public void StartDialogue(Dialogue dialogue) // Start a dialogue with a NPC when the player press E on him
	{
		animator.SetBool("isOpen", value: true);
		nameText.text = dialogue.name;
		sentences.Clear();
		string[] array = dialogue.sentences;
		foreach (string item in array)
		{
			sentences.Enqueue(item);
		}
		phrase1 = sentences.Dequeue();
		StartCoroutine(TypeSentence(phrase1));
	}

	public void DisplayNextSentence() // Display the next sentence of the dialogue when player press E
	{
		if (sentences.Count == 0 && dialogueEnded)
		{
			EndDialogue();
			StartCoroutine(JumpButton());
		}
		else if (dialogueEnded)
		{
			phrase1 = sentences.Dequeue();
			StopAllCoroutines();
			StartCoroutine(TypeSentence(phrase1));
		}
		else if (!dialogueEnded)
		{
			dialogueText.text = phrase1;
			StopAllCoroutines();
			dialogueEnded = true;
		}
	}

	private IEnumerator TypeSentence(string sentence) // Type the sentence letter by letter
	{
		dialogueEnded = false;
		dialogueText.text = "";
		char[] array = sentence.ToCharArray();
		foreach (char c in array)
		{
			dialogueText.text += c;
			yield return new WaitForSeconds(0.025f);
		}
		dialogueEnded = true;
		playerMovement.isTalking = false;
		Debug.Log("fin de phrase");
	}

	public void EndDialogue()
	{
		animator.SetBool("isOpen", value: false);
	}

	private IEnumerator JumpButton()
	{
		yield return new WaitForSeconds(0.2f);
		playerMovement.isTalking = false;
	}
}
