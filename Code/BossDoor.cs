using System.Collections;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
	private EnnemyHealth ennemyHealth;

	public GameObject door;

	private void Start()
	{
		ennemyHealth = base.gameObject.GetComponent<EnnemyHealth>();
		door.SetActive(value: false);
	}

	private void Update()
	{
		if (ennemyHealth.currentHealth <= 0)
		{
			StartCoroutine(DoorAppear());
		}
	}

	private IEnumerator DoorAppear()
	{
		door.transform.position = base.transform.position;
		yield return new WaitForSeconds(1f);
		door.SetActive(value: true);
	}
}
