using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndlessEnnemy : MonoBehaviour
{
	public int ennemyClass;

	private EnnemyHealth ennemyHealth;

	public Text bonusText;

	private bool hasGivenBonus;

	private void Start()
	{
		bonusText = GameObject.FindGameObjectWithTag("Bonus").GetComponent<Text>();
		bonusText.enabled = false;
		ennemyHealth = base.gameObject.GetComponent<EnnemyHealth>();
	}

	private void Update()
	{
		if (ennemyHealth.currentHealth <= 0 && !hasGivenBonus)
		{
			giveBonusToPlayer();
		}
	}

	private void giveBonusToPlayer() // Give a random bonus to the player when the ennemy is dead
	{
		hasGivenBonus = true;
		switch (Random.Range(0, 4))
		{
		case 0: // Speed
		{
			int num4 = 20 * ennemyClass;
			PlayerMovement.instance.vitesse += num4;
			PlayerMovement.instance.moveSpeed = PlayerMovement.instance.vitesse;
			StopAllCoroutines();
			StartCoroutine(changeText(("Vitesse + " + num4).ToString()));
			break;
		}
		case 1: // Health
		{
			int num3 = 20 * ennemyClass;
			PlayerHealth.instance.currentHealth += num3;
			PlayerHealth.instance.maxHealth += num3;
			PlayerHealth.instance.healthBar.SetHealth(PlayerHealth.instance.currentHealth);
			PlayerHealth.instance.healthBar.SetMaxHealth(PlayerHealth.instance.maxHealth);
			StopAllCoroutines();
			StartCoroutine(changeText(("Point de vie + " + num3).ToString()));
			break;
		}
		case 2: // Damage
		{
			int num2 = 15 * ennemyClass;
			PlayerAttack.instance.damage += num2;
			StopAllCoroutines();
			StartCoroutine(changeText(("Dégâts + " + num2).ToString()));
			break;
		}
		case 3: // Regen
		{
			float num = 0.001f * (float)ennemyClass;
			PlayerHealth.instance.regen += num;
			StopAllCoroutines();
			StartCoroutine(changeText("regen +".ToString()));
			break;
		}
		}
	}

	private IEnumerator changeText(string _text)
	{
		bonusText.enabled = true;
		bonusText.text = _text;
		yield return new WaitForSeconds(1f);
		bonusText.enabled = false;
	}
}
