using UnityEngine;

public class folCam : MonoBehaviour
{
	private float camerax;

	private float cameray;

	private void Start()
	{
	}

	private void LateUpdate()
	{
		camerax = Camera.main.transform.position.x;
		cameray = Camera.main.transform.position.y;
		base.transform.position = new Vector3(camerax + 43.5f, cameray + 10f, base.transform.position.z);
	}
}
