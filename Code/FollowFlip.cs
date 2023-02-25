using UnityEngine;

public class FollowFlip : MonoBehaviour
{
	private Vector3 playerPosition;

	public Vector3 vector3;

	private void Awake()
	{
	}

	private void Update()
	{
		playerPosition = PlayerMovement.instance.transform.position;
		if (PlayerMovement.instance.spriteRenderer.flipX)
		{
			base.transform.position = playerPosition - vector3;
		}
		else
		{
			base.transform.position = playerPosition + vector3;
		}
	}
}
