using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public Player_Manager _playerManager;
    public Inventory _inventory;

    private Player_Movement _playerMovement;
    private Weapon_Manager _weaponManager;

    private void Awake()
    {
        _playerMovement = _playerManager.GetComponentInChildren<Player_Movement>();
        _weaponManager = _playerMovement.GetComponentInChildren<Weapon_Manager>();
    }

    public void PressButton_Attack()
    {
        if (_playerManager.CanAttack)
        {
            _weaponManager.weaponAnimator.SetTrigger("Attack");
            _weaponManager.StartCoroutine("SetCanAttack");
            _playerManager.CharacterAnimator.SetTrigger("Attack");
        }
    }

    public void PressButton_Inventory()
    {
        _inventory.ShowHideInventory();
    }
}
