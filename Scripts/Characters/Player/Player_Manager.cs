using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : Character
{
	[Header("Keybinds")]
	public KeyCode InteractionKey;
	public KeyCode AttackKey;

	[Header("Movement")]
	public int PlayerInvertX = 1;
	public int PlayerInvertY = 1;
	public int facingDirection = 1;
	private Vector2 rawInput;
	public bool isDodgeRoll = false;
	public bool canDodgeRoll = true;

	// Components
	private Weapon_Manager weaponManager;
	private Character_Stats charStats;
	private Collider2D playerCollider;
	private Rigidbody2D playerRb;

	[Header("Attack")]
	public float MainAttackCooldown = 0.2f;
	public AudioClip AttackSound;

	void Awake()
	{
		playerCollider = characterEntity.GetComponent<Collider2D>();
		CharacterAnimator = characterEntity.GetComponent<Animator>();
		charStats = GetComponentInChildren<Character_Stats>();
		playerRb = characterEntity.GetComponent<Rigidbody2D>();
		weaponManager = GetComponentInChildren<Weapon_Manager>();

		if (facingDirection == 0)
			facingDirection = 2;
	}

    void Update()
    {
		//rawInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		rawInput = new Vector2(characterEntity.GetComponent<Player_Movement>().joystick.Horizontal, characterEntity.GetComponent<Player_Movement>().joystick.Vertical);

		if (rawInput == new Vector2(1, 1))
        {
			rawInput = new Vector2(1, 0);
        }
		else if (rawInput == new Vector2(1, -1))
		{
			rawInput = new Vector2(1, 0);
		}
		else if (rawInput == new Vector2(-1, -1))
		{
			rawInput = new Vector2(-1, 0);
		}
		else if (rawInput == new Vector2(-1, 1))
		{
			rawInput = new Vector2(-1, 0);
		}

		if (rawInput != Vector2.zero && CanMove)
			weaponManager.SetWeaponBasePosition(rawInput, facingDirection);
    }

	public void TakeDamage(float damage, Transform otherTransform, float joltAmount, float timeToRecover)
	{
		// Remove HP.
		charStats.CurrentHealth -= damage;
		// IF we are dead.
		if (charStats.CurrentHealth <= 0f)
		{
			// We DIEDEDED!!! NOOOOOOOO....
			//Death();
			// We are dead so lets leave.
			return;
		}
		// We are hit.
		Hit(otherTransform, joltAmount, timeToRecover);
	}

	private void Hit(Transform otherTransform, float joltAmount, float timeToRecover)
	{
		// Play the sound from getting hit.
		if (GetHitSound != null)
		{
			//Grid.soundManager.PlaySound(GetHitSound, transform.position, 1f, 1f);
		}

		// IF the character that we collided with can be knockedback.
		if (CanBeJolted)
		{
			// Knock GameObject back.
			Knockback(otherTransform, joltAmount);
			// Make the Hero not be able to control the character while being knockedback.
			StartCoroutine(NoCharacterControl(timeToRecover));
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

	private IEnumerator NoCharacterControl(float timeToRecover)
	{
		CanMove = false;
		currentlyJolted = true;
		yield return new WaitForSeconds(timeToRecover);
		characterEntity.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		CanMove = true;
		currentlyJolted = false;
	}
}
