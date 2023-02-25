using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSpecificScene : MonoBehaviour
{
	public AudioClip NextSceneSound;

	public string sceneName;

	private Animator fadeSystem;

	private void Awake()
	{
		fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			StartCoroutine(loadNextScene());
			AudioManager.instance.PlayClipAt(NextSceneSound, base.transform.position);
		}
	}

	public IEnumerator loadNextScene()
	{
		// Load scene and save data
		LoadAndSaveData.instance.SaveData();
		fadeSystem.SetTrigger("FadeIn");
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene(sceneName);
	}
}
