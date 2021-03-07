using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Movement : MonoBehaviour
{
    // Vector direction we are moving.
    private Vector2 movement;
    // The GameObjects Rigidbody.
    private Rigidbody2D rb;
    // The Player State
    private NPC_Manager _npcManager;
    // The Character Stats.
    private Character_Stats charStats;

    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _npcManager = GetComponentInParent<NPC_Manager>();
        charStats = GetComponentInChildren<Character_Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
