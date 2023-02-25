using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public GameObject player;

	public float timeoffset;

	public Vector3 posoffset;

	private Vector3 Offset;

	private Vector3 velocity;

	private bool flipx;

	private void Update()
	{
		if (PlayerMovement.instance.spriteRenderer.flipX)
		{
			Offset = new Vector3(0f - posoffset.x, posoffset.y, posoffset.z);
		}
		else
		{
			Offset = posoffset;
		}
		base.transform.position = Vector3.SmoothDamp(base.transform.position, player.transform.position + Offset, ref velocity, timeoffset);
	}
}
