using UnityEngine;

public class FloatingTextHandler : MonoBehaviour
{
	private void Start()
	{
		Object.Destroy(base.gameObject, 1f);
		base.transform.localPosition += new Vector3(0f, 0.5f, 0f);
	}

	private void Update()
	{
	}
}
