using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
	public float ZRotate;

	private void Update()
	{
		base.transform.Rotate(new Vector3(0f, 0f, ZRotate * Time.deltaTime));
	}
}
