using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Manager : Character
{
    private Character_Stats charStats;

    void Awake()
    {
        charStats = GetComponentInChildren<Character_Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public void TakeDamage(float damage, Transform otherTransform, float joltAmount)
	{
		// Remove HP.
		charStats.CurrentHealth -= damage;
		// IF the current health is 0 or below.
		if (charStats.CurrentHealth <= 0f)
		{
			// Do all the stuff you want when this GameObject Dies.
			//Death();
			// GTFO.
			return;
		}
		// We are hit.
		Hit(otherTransform, joltAmount);
	}

	private void Hit(Transform otherTransform, float joltAmount)
	{
		// Play the sound from getting hit.
		if (GetHitSound != null)
		{
			//Grid.soundManager.PlaySound(GetHitSound, transform.position, 1f, 1f);
		}

		// IF this animator has a state that represents your getting Hit animation.
		if (CharacterAnimator.HasState(0, Animator.StringToHash("Hit")))
		{
			//StartCoroutine(HitAnimation());
		}
		// IF the character that we collided with can be knockedback.
		if (CanBeJolted)
		{
			// Knock GameObject back.
			Knockback(otherTransform, joltAmount);
			// Make the Hero not be able to control the character while being knockedback.
			StartCoroutine(NoCharacterControl());
		}
	}

	public void Knockback(Transform otherTransform, float joltAmount)
	{
		// Get the relative position.
		Vector2 relativePos = characterEntity.transform.position - otherTransform.position;
		// Get the rigidbody2D
		Rigidbody2D charRigid = characterEntity.GetComponent<Rigidbody2D>();
		// Stop the colliding objects velocity.
		charRigid.velocity = Vector3.zero;
		// Apply knockback.
		charRigid.AddForce(relativePos.normalized * joltAmount, ForceMode2D.Impulse);
	}

	private IEnumerator NoCharacterControl()
	{
		// Make the player not be able to control the character while the knockback is happening.
		CanMove = false;
		// We are currently being knockbacked.
		currentlyJolted = true;
		// Wait for 'HitAnimationTime' before being able to control the character again.
		yield return new WaitForSeconds(2f);
		// Stop the knockback.
		characterEntity.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		// We can now move the character.
		CanMove = true;
		// We are not being jolted anymore.
		currentlyJolted = false;
	}
}
