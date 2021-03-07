using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : Character
{
    private Character_Stats charStats;
	private Enemy_Movement _enemyMovement;

	public enum ThreatLevel { Agressive, Friendly, Afraid };
	public ThreatLevel threatLevel;
	public enum EnemyType { Run, Dash, Jump, Fly, Ranged };
	public EnemyType enemyType;
	public enum EnemyState { Patrol, Chase, Dead, Idle };
	public EnemyState enemyState;

	[Header("Patrol")]
	public Vector2 originPos;
	public Vector2 nextPatrolPos;
	public float patrolArea;
	public float timeBetweenPatrolMin;
	public float timeBetweenPatrolMax;

	[Header("Attack")]
	public float enemyRange;
	public float agroArea;
	public float knockbackRate;
	public GameObject target = null;
	public CharacterType typeToAgro;

	private List<Character> characterList = new List<Character>();
	private Character character;

	private Vector2 savedPos;


	void Awake()
    {
        charStats = GetComponentInChildren<Character_Stats>();
		character = GetComponentInParent<Character>();
		_enemyMovement = GetComponentInChildren<Enemy_Movement>();

		originPos = gameObject.transform.position;
		nextPatrolPos = originPos;
	}

    private void Start()
    {
		StartCoroutine(ChangePatrolPos());
	}

    void Update()
    {
		if (enemyState != EnemyState.Dead)
        {
			IsTarget();
		}

		if ((Vector2)characterEntity.transform.position == nextPatrolPos && enemyState == EnemyState.Patrol)
        {
			enemyState = EnemyState.Idle;
        }
        if ((Vector2)characterEntity.transform.position != nextPatrolPos && enemyState != EnemyState.Patrol)
        {
			enemyState = EnemyState.Patrol;
        }
		if (target != null)
        {
			enemyState = EnemyState.Chase;
        }

		if (enemyState == EnemyState.Patrol)
        {
			if (savedPos != nextPatrolPos)
				UpdateAnimator(nextPatrolPos);
        }
		else if (enemyState == EnemyState.Chase)
        {
			if (savedPos != (Vector2)target.transform.position)
				UpdateAnimator(target.transform.position);
        }
		else if (enemyState == EnemyState.Idle)
        {
			CharacterAnimator.SetFloat("Speed", 0);
		}

		if (!CanMove)
        {
			CharacterAnimator.SetFloat("Speed", 0);
		}
    }

	void UpdateAnimator(Vector2 targetPos)
	{
		CharacterAnimator.SetFloat("Speed", (targetPos - (Vector2)characterEntity.transform.position).sqrMagnitude);
		if (_enemyMovement.disableMove)
        {
			return;
        }
		CharacterAnimator.SetFloat("MoveX", (targetPos.x - characterEntity.transform.position.x));
		CharacterAnimator.SetFloat("MoveY", (targetPos.y - characterEntity.transform.position.y));

		savedPos = targetPos;
	}

	IEnumerator ChangePatrolPos()
    {
		if (enemyState == EnemyState.Idle)
		{
			nextPatrolPos = originPos + Random.insideUnitCircle * patrolArea;
			enemyState = EnemyState.Patrol;
		}
		float randomTime = Random.Range(timeBetweenPatrolMin, timeBetweenPatrolMax);
		yield return new WaitForSeconds(randomTime);
		StartCoroutine(ChangePatrolPos());
	}

	void IsTarget()
	{
		characterList = Character_Manager.GetCharactersByType(characterList, typeToAgro);
		if (characterList.Count > 0)
		{
			GameObject _character = null;
			_character = Character_Manager.GetClosestCharacterType(character.transform, typeToAgro, _character, agroArea);
			if (_character != null)
			{
				target = _character.GetComponent<Character>().characterEntity;
			}
			else
            {
				target = null;
            }
		}
		else
        {
			target = null;
        }
	}

    public void TakeDamage(float damage, Transform otherTransform, float joltAmount, float timeToRecover)
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
		Hit(otherTransform, joltAmount, timeToRecover);
	}

	private void Hit(Transform otherTransform, float joltAmount, float timeToRecover)
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
		Debug.Log("KNOCKBACK");
		charRigid.AddForce(relativePos.normalized * joltAmount, ForceMode2D.Impulse);
	}

	private IEnumerator NoCharacterControl(float timeToRecover)
	{
		// Make the player not be able to control the character while the knockback is happening.
		CanMove = false;
		// We are currently being knockbacked.
		currentlyJolted = true;
		// Wait for 'HitAnimationTime' before being able to control the character again.
		yield return new WaitForSeconds(timeToRecover);
		// Stop the knockback.
		characterEntity.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		// We can now move the character.
		CanMove = true;
		// We are not being jolted anymore.
		currentlyJolted = false;
	}

	// Debug
	private void OnDrawGizmos()
	{
		//Patrol area
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(originPos, patrolArea);
		//Agro area
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(characterEntity.transform.position, agroArea);
		//Next patrol position
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(nextPatrolPos, 0.1f);
	}
}
