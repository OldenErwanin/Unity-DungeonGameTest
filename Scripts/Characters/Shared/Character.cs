using UnityEngine;

public enum CharacterType { Hero, Enemy, NPC }
public abstract class Character : MonoBehaviour
{
	[Header("Components")]
	// The GameObject that represents the character entity.
	public GameObject characterEntity;
	// The Character Animator.
	public Animator CharacterAnimator;


	[Header("General data")]
	// The type of character this is. The hero (Player), Enemy (normal mobs / bosses) or Neutral.
	public CharacterType characterType;
	public bool CanMove = true;
	public float AlterSpeed = 1f;
	public bool CanAttack = true;
	public bool CanBeJolted;
	public bool currentlyJolted = false;
	//public bool Interactable = false;

	

	
	[Header("Effects")]
	// The audio clip for when this character gets hit.
	public AudioClip GetHitSound;
	// The audio clip for when this character dies.
	public AudioClip DieSound;
	// The after effects from dying. (Think of like a poof of clouds when something dies.)
	public GameObject afterDeathVisual;

	void OnEnable()
	{
		Character_Manager.Register(this);
	}

	void OnDisable()
	{
		Character_Manager.Unregister(this);
	}
}
