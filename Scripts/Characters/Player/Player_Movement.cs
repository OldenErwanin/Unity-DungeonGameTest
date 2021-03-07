using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player_Movement : MonoBehaviour
{
    // Vector direction we are moving.
    private Vector2 movement;
    // The GameObjects Rigidbody.
    private Rigidbody2D rb;
    // The Player State
    private Player_Manager _playerManager;
    // The Character Stats.
    private Character_Stats charStats;
    // Holder for the movements.
    private Vector2 move;

    public FixedJoystick joystick;

    void Awake()
    {
        // Get the Player State in the parent.
        _playerManager = GetComponentInParent<Player_Manager>();
        // Get the Players Stats as we use that to potentially alter movement.
        charStats = _playerManager.GetComponentInChildren<Character_Stats>();
        // Get the Rigidbody2D Component.
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_playerManager.CanMove && !_playerManager.isDodgeRoll)
        {
            //move.x = Input.GetAxisRaw("Horizontal");
            //move.y = Input.GetAxisRaw("Vertical");

            if ((joystick.Horizontal == 1f && joystick.Vertical == 1f) || (joystick.Horizontal == -1f && joystick.Vertical == -1f) || (joystick.Horizontal == -1f && joystick.Vertical == 1f) || (joystick.Horizontal == 1f && joystick.Vertical == -1f))
            {
                move.x = joystick.Horizontal * 0.75f;
                move.y = joystick.Vertical * 0.75f;
            }
            else
            {
                move.x = joystick.Horizontal;
                move.y = joystick.Vertical;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (_playerManager.canDodgeRoll)
            {
                StartCoroutine(SetDodgeRoll());
                _playerManager.isDodgeRoll = true;
                if (move.sqrMagnitude == 2)
                {
                    rb.AddForce(move * (charStats.dodgerollSpeed / 2), ForceMode2D.Impulse);
                }
                else
                {
                    rb.AddForce(move * charStats.dodgerollSpeed, ForceMode2D.Impulse);
                }
                _playerManager.isDodgeRoll = false;
            }
        }
    }


    IEnumerator SetDodgeRoll()
    {
        _playerManager.CharacterAnimator.SetTrigger("Dash");
        _playerManager.canDodgeRoll = false;
        yield return new WaitForSeconds(2f);
        _playerManager.canDodgeRoll = true;
    }



    void FixedUpdate()
    {
        // IF we are able to move.
        // ELSE IF we are not able to move.
        if (_playerManager.CanMove && !_playerManager.isDodgeRoll)
        {
            // Get Vector2 direction.
            movement = new Vector2(move.x * _playerManager.PlayerInvertX, move.y * _playerManager.PlayerInvertY);
            // Apply direction with speed and alterspeed.
            movement *= charStats.CurrentMoveSpeed * _playerManager.AlterSpeed;

            
            // Apply the force for movement.
            
            rb.AddForce(movement);
            

            if (rb.velocity != Vector2.zero)
            {
                // IF the user has an animation set.
                PlayAnimation(movement);
            }
        }
        else if (!_playerManager.currentlyJolted)
        {
            rb.velocity = Vector2.zero;
            _playerManager.CharacterAnimator.SetFloat("Speed", 0f);
        }
    }

    void PlayAnimation(Vector2 _movement)
    {
        // IF the user has an animation set and ready to go.
        if (_playerManager.CharacterAnimator != null)
        {
            _playerManager.CharacterAnimator.SetFloat("Speed", _movement.sqrMagnitude);
            _playerManager.CharacterAnimator.SetFloat("MoveX", _movement.x);
            _playerManager.CharacterAnimator.SetFloat("MoveY", _movement.y);

            if (_movement.x > 0 && _movement.y < 0)
            {
                // Set the down right animation.
                _playerManager.CharacterAnimator.SetFloat("Direction", 2f);
                _playerManager.facingDirection = 2;
            }
            else if (_movement.x < 0 && _movement.y < 0)
            {
                // Set the down left animation.
                _playerManager.CharacterAnimator.SetFloat("Direction", 1f);
                _playerManager.facingDirection = 1;
            }
            else if (_movement.x < 0 && _movement.y > 0)
            {
                // Set the up left animation.
                _playerManager.CharacterAnimator.SetFloat("Direction", 1f);
                _playerManager.facingDirection = 1;
            }
            else if (_movement.x > 0 && _movement.y > 0)
            {
                // Set the up right animation.
                _playerManager.CharacterAnimator.SetFloat("Direction", 2f);
                _playerManager.facingDirection = 2;
            }
            else if (_movement.x > 0)
            {
                // Set the right animation.
                _playerManager.CharacterAnimator.SetFloat("Direction", 2f);
                _playerManager.facingDirection = 2;
            }
            else if (_movement.y < 0)
            {
                // Set the down animation.
                _playerManager.CharacterAnimator.SetFloat("Direction", 4f);
                _playerManager.facingDirection = 4;
            }
            else if (_movement.x < 0)
            {
                // Set the left animation.
                _playerManager.CharacterAnimator.SetFloat("Direction", 1f);
                _playerManager.facingDirection = 1;
            }
            else if (_movement.y > 0)
            {
                // Set the up animation.
                _playerManager.CharacterAnimator.SetFloat("Direction", 3f);
                _playerManager.facingDirection = 3;
            }

        }
    }
}
