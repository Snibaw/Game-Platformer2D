using UnityEngine;

public class CameraParallax : MonoBehaviour
{
	public Transform[] backgrounds;

	public float[] parallaxScales;

	public float smoothing;

	private Transform cam;

	private Vector3 previousCamPos;

	private void Start()
	{
		// Set up camera reference
		cam = Camera.main.transform;
		previousCamPos = cam.position;
		parallaxScales = new float[backgrounds.Length];
		for (int i = 0; i < backgrounds.Length; i++)
		{
			parallaxScales[i] = backgrounds[i].position.z * -1f;
			Debug.Log(parallaxScales[i]);
		}
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < backgrounds.Length; i++) // for each background
		{
			float num = (previousCamPos.x - cam.position.x) * parallaxScales[i];
			float x = backgrounds[i].position.x + num;
			Vector3 b = new Vector3(x, backgrounds[i].position.y, backgrounds[i].position.z);
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, b, smoothing * Time.deltaTime);
		}
		previousCamPos = cam.position;
	}
}
