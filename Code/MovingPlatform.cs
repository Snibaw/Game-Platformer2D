using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	public Transform pos1;

	public Transform pos2;

	public float speed;

	public Transform startPos;

	private Vector3 nextPos;

	private void Start()
	{
		nextPos = startPos.position;
	}

	private void Update()
	{
		if (base.transform.position == pos1.position)
		{
			nextPos = pos2.position;
		}
		if (base.transform.position == pos2.position)
		{
			nextPos = pos1.position;
		}
		base.transform.position = Vector3.MoveTowards(base.transform.position, nextPos, speed * Time.deltaTime);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(pos1.position, pos2.position);
	}
}
