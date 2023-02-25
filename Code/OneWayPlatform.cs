using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
	private PlatformEffector2D effector;

	private float waitTime;

	private float fallTime;

	private Animator animator;

	private void Start()
	{
		effector = GetComponent<PlatformEffector2D>();
		animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
	}

	private void Update()
	{
		// The platform is a one way platform
		fallTime -= Time.deltaTime;
		// If he wants to go down and the wait time is over
		if (Input.GetAxis("Vertical") < 0f)
		{
			if (waitTime <= 0f)
			{
				effector.rotationalOffset = 180f;
				fallTime = 0.3f;
				waitTime = 0.2f;
				animator.SetBool("isGrounded", value: false);
			}
			else
			{
				waitTime -= Time.deltaTime;
			}
		}
		else if (fallTime <= 0f)
		{
			waitTime = 0.2f;
			effector.rotationalOffset = 0f;
		}
	}
}
