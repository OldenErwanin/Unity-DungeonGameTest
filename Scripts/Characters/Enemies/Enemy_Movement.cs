using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{

    // Components
    private Character_Stats charStats;
    private Enemy_Manager _enemyManager;
    private Rigidbody2D enemyRb;

    // Jump movement
    private bool isJumping;
    private bool canJump;
    private bool isJumpHandler;
    private Vector2 jumpPos;
    public bool disableMove;

    void Awake()
    {
        _enemyManager = GetComponentInParent<Enemy_Manager>();
        charStats = _enemyManager.GetComponentInChildren<Character_Stats>();
        enemyRb = GetComponent<Rigidbody2D>();

        canJump = true;
        disableMove = false;
    }

    void Update()
    {
        // Running enemy movement
        if (_enemyManager.enemyType == Enemy_Manager.EnemyType.Run)
        {
            RunMovement();
        }
        // Jumping enemy movement
        else if (_enemyManager.enemyType == Enemy_Manager.EnemyType.Jump)
        {
            JumpMovement();
        }
        // Dashing enemy movement
        else if (_enemyManager.enemyType == Enemy_Manager.EnemyType.Dash)
        {
            DashMovement();
        }
        // Flying enemy movement
        else if (_enemyManager.enemyType == Enemy_Manager.EnemyType.Fly)
        {
            FlyMovement();
        }
    }

    void RunMovement()
    {
        // Patrol
        if (_enemyManager.enemyState == Enemy_Manager.EnemyState.Patrol && _enemyManager.CanMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, _enemyManager.nextPatrolPos, Time.deltaTime * charStats.CurrentMoveSpeed);
        }
        // Chase
        else if (_enemyManager.enemyState == Enemy_Manager.EnemyState.Chase && _enemyManager.CanMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, _enemyManager.target.transform.position, Time.deltaTime * charStats.CurrentMoveSpeed);
        }
    }

    void JumpMovement()
    {
        // Patrol
        if (_enemyManager.enemyState == Enemy_Manager.EnemyState.Patrol && _enemyManager.CanMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, _enemyManager.nextPatrolPos, Time.deltaTime * charStats.CurrentMoveSpeed);
        }
        // Chase
        else if (_enemyManager.enemyState == Enemy_Manager.EnemyState.Chase && _enemyManager.CanMove)
        {
            float dist = Vector3.Distance(_enemyManager.target.transform.position, transform.position);
            if (dist <= _enemyManager.enemyRange && canJump)
            {
                if (!isJumpHandler)
                {
                    StartCoroutine(JumpMovementHandler());
                }
                if (isJumping)
                    transform.position = Vector2.MoveTowards(transform.position, jumpPos, Time.deltaTime * charStats.CurrentMoveSpeed * 10);
            }
            else
            {
                if (isJumpHandler && !canJump)
                {
                    isJumpHandler = false;
                    isJumping = false;
                    StopCoroutine(JumpMovementHandler());
                }
                transform.position = Vector2.MoveTowards(transform.position, _enemyManager.target.transform.position, Time.deltaTime * charStats.CurrentMoveSpeed * 2);
            }
        }
    }

    void DashMovement()
    {
        // Patrol
        if (_enemyManager.enemyState == Enemy_Manager.EnemyState.Patrol && _enemyManager.CanMove && !isJumping && !disableMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, _enemyManager.nextPatrolPos, Time.deltaTime * charStats.CurrentMoveSpeed);
        }
        // Chase

        else if (_enemyManager.enemyState == Enemy_Manager.EnemyState.Patrol && isJumping)
        {
            transform.position = Vector2.MoveTowards(transform.position, jumpPos, Time.deltaTime * charStats.CurrentMoveSpeed * 6);
        }

        else if (_enemyManager.enemyState == Enemy_Manager.EnemyState.Chase && _enemyManager.CanMove) 
        {
            float dist = Vector3.Distance(_enemyManager.target.transform.position, transform.position);
            if (dist <= _enemyManager.enemyRange)
            {
                if (!isJumpHandler)
                {
                    StartCoroutine(DashMovementHandler());
                }
            }

            
            if (isJumping)
            {
                transform.position = Vector2.MoveTowards(transform.position, jumpPos, Time.deltaTime * charStats.CurrentMoveSpeed * 6);
            }
            else if (!isJumping && !disableMove)
            {
                transform.position = Vector2.MoveTowards(transform.position, _enemyManager.target.transform.position, Time.deltaTime * charStats.CurrentMoveSpeed * 2);
            }
            





            /*
            if (dist <= _enemyManager.enemyRange && canJump)
            {
                if (!isJumpHandler)
                {
                    StartCoroutine(DashMovementHandler());
                }
                if (isJumping)
                {
                    //asdasd = (jumpPos - (Vector2)transform.position).normalized * 4 + (Vector2)transform.position;
                    //Vector2 directionTest = (nextPatrolPos - (Vector2)characterEntity.transform.position) * 2 + (Vector2)characterEntity.transform.position;
                    transform.position = Vector2.MoveTowards(transform.position, jumpPos, Time.deltaTime * charStats.CurrentMoveSpeed * 6);
                    //enemyRb.AddForce(dir * Time.fixedDeltaTime * charStats.CurrentMoveSpeed * 10f, ForceMode2D.Impulse);
                    //enemyRb.velocity = dir * Time.fixedDeltaTime * charStats.CurrentMoveSpeed * 350f;
                }
                    
            }
            else
            {
                if (isJumpHandler && !canJump)
                {
                    isJumpHandler = false;
                    isJumping = false;
                    StopCoroutine(DashMovementHandler());
                }
                
                transform.position = Vector2.MoveTowards(transform.position, _enemyManager.target.transform.position, Time.deltaTime * charStats.CurrentMoveSpeed * 2);
            }*/
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, jumpPos);
    }

    void FlyMovement()
    {
        // Patrol
        if (_enemyManager.enemyState == Enemy_Manager.EnemyState.Patrol && _enemyManager.CanMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, _enemyManager.nextPatrolPos, Time.deltaTime * charStats.CurrentMoveSpeed);
        }
        // Chase
        else if (_enemyManager.enemyState == Enemy_Manager.EnemyState.Chase && _enemyManager.CanMove)
        {
            Vector2 dir = (_enemyManager.target.transform.position - transform.position).normalized;
            enemyRb.AddForce(dir * Time.fixedDeltaTime * charStats.CurrentMoveSpeed * 0.5f, ForceMode2D.Impulse);
        }
    }

    IEnumerator JumpMovementHandler()
    {
        isJumpHandler = true;
        jumpPos = _enemyManager.target.transform.position;
        canJump = true;
        disableMove = true;
        yield return new WaitForSeconds(0.5f);
        isJumping = true;
        yield return new WaitForSeconds(0.3f);
        canJump = false;
        isJumping = false;
        yield return new WaitForSeconds(1f);
        disableMove = false;
        yield return new WaitForSeconds(4f);
        canJump = true;
        isJumpHandler = false;
    }

    IEnumerator DashMovementHandler()
    {
        isJumpHandler = true;
        jumpPos = ((Vector2)_enemyManager.target.transform.position - (Vector2)transform.position) * 5 + (Vector2)transform.position;
        canJump = true;
        disableMove = true;
        yield return new WaitForSeconds(2f);
        isJumping = true;
        yield return new WaitForSeconds(0.7f);
        canJump = false;
        isJumping = false;
        yield return new WaitForSeconds(1f);
        disableMove = false;
        yield return new WaitForSeconds(4f);
        canJump = true;
        isJumpHandler = false;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Player_Manager hit = collision.gameObject.GetComponent<Player_Movement>().GetComponentInParent<Player_Manager>();
        if (hit)
        {
            _enemyManager.CharacterAnimator.SetTrigger("Attack");
            _enemyManager.TakeDamage(0, hit.gameObject.transform, 0.1f, 1f);
            hit.TakeDamage(charStats.CurrentDamage, gameObject.transform, 20, 1.5f);
        }
    }
}
