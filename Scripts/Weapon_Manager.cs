using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon_Manager : MonoBehaviour
{
    public Animator weaponAnimator;
    [SerializeField] private AnimatorOverrideController aocontroller;
    private SpriteRenderer weaponSprite;
    private BoxCollider2D weaponCollider;
    private Player_Manager playerManager;
    private Player_Movement playerMovement;

    public Weapon currentWeapon;

    [Header("Animator Override Controller settings")]
    [SerializeField] private const string ATTACK_TRIGGER = "Attack";
    [SerializeField] private const string DEFAULT_ATTACK = "Sword01";


    void Awake()
    {
        weaponAnimator = GetComponent<Animator>();
        weaponSprite = GetComponent<SpriteRenderer>();
        weaponCollider = GetComponent<BoxCollider2D>();
        playerManager = GetComponentInParent<Player_Manager>();
        playerMovement = GetComponentInParent<Player_Movement>();

        weaponSprite.enabled = false;
        weaponCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            UpdateWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && playerManager.CanAttack)
        {
            weaponAnimator.SetTrigger("Attack");
            StartCoroutine(SetCanAttack());
            playerManager.CharacterAnimator.SetTrigger("Attack");
        }
    }

    public void ExecuteAttack()
    {
        //playerManager.CanMove = !playerManager.CanMove;
        weaponCollider.enabled = !weaponCollider.enabled;
        weaponSprite.enabled = !weaponSprite.enabled;
    }

    public IEnumerator SetCanAttack()
    {
        playerManager.CanAttack = false;
        playerManager.CanMove = false;
        yield return new WaitForSeconds(currentWeapon.weaponTimeBetweenAttacks);
        playerManager.CanAttack = true;
        playerManager.CanMove = true;
    }

    public void SetWeaponBasePosition(Vector2 direction, int rotation)
    {
        gameObject.transform.localPosition = direction / 1.5f;
        if (rotation == 1)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (rotation == 2)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (rotation == 3)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (rotation == 4)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
    }

    public void UpdateWeapon()
    {
        aocontroller[DEFAULT_ATTACK] = currentWeapon.weaponAnimation;
        weaponCollider.size = currentWeapon.weaponColliderSize;
        weaponCollider.offset = currentWeapon.weaponColliderOffset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy_Manager hitElement = collision.gameObject.GetComponentInParent<Enemy_Manager>();
        if (hitElement && hitElement.characterType == CharacterType.Enemy)
        {
            Debug.Log("Hit enemy!");
            hitElement.TakeDamage(currentWeapon.weaponDamage, playerMovement.transform, currentWeapon.weaponKnockbackRate, 1.5f);
        }
    }
}
