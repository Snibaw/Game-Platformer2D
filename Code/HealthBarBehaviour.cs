using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
	public Slider slider;

	public Color low;

	public Color high;

	public Vector3 Offset;

	public void SetHealth(float health, float maxHealth) // Set the health of the slider
	{
		slider.gameObject.SetActive(health < maxHealth);
		slider.value = health;
		slider.maxValue = maxHealth;
		slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
	}

	private void Update()
	{
		slider.transform.position = Camera.main.WorldToScreenPoint(base.transform.parent.position + Offset);
	}
}
