using UnityEngine;

public class Parallax : MonoBehaviour
{
	private float length;

	private float startpos;

	public GameObject cam;

	public float parallaxEffect;

	public bool isIsland;

	private void Start()
	{
		startpos = base.transform.position.x;
		length = GetComponent<SpriteRenderer>().bounds.size.x;
	}

	private void Update()
	{
		// Doing the parallax effect
		float num = cam.transform.position.x * (1f - parallaxEffect);
		float num2 = cam.transform.position.x * parallaxEffect;
		base.transform.position = new Vector3(startpos + num2, base.transform.position.y, base.transform.position.z);
		if (num > startpos + length && !isIsland)
		{
			startpos += length;
		}
		else if (num < startpos - length && !isIsland)
		{
			startpos -= length;
		}
		else if (num > startpos + 1.5f * length && isIsland)
		{
			startpos += length * 2.5f;
		}
		else if (num < startpos - 1.5f * length && isIsland)
		{
			startpos -= length * 2.5f;
		}
	}
}
