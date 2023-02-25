using System.Collections;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
	public static PlayerEffect instance;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instancede PlayerEffect dans la scène");
		}
		else
		{
			instance = this;
		}
	}

	public void AddSpeed(int speedGiven, float speedDuration) // Add speed effect
	{
		PlayerMovement.instance.moveSpeed += speedGiven;
		StartCoroutine(RemoveSpeed(speedGiven, speedDuration));
	}

	private IEnumerator RemoveSpeed(int speedGiven, float duration) // Remove speed effect
	{
		yield return new WaitForSeconds(duration);
		PlayerMovement.instance.moveSpeed -= speedGiven;
	}

	public void AddJump(int jumpGiven, float duration) // Add jump effect
	{
		PlayerMovement.instance.jumpForce += jumpGiven;
		StartCoroutine(RemoveJump(jumpGiven, duration));
	}

	private IEnumerator RemoveJump(int jumpGiven, float duration) // Remove jump effect
	{
		yield return new WaitForSeconds(duration);
		PlayerMovement.instance.jumpForce -= jumpGiven;
	}
}
