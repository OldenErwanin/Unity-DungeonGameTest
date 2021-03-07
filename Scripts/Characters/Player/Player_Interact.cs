using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interact : MonoBehaviour
{
    private CircleCollider2D interactCollider;
    public float interactRange = 1.2f;
    public List<PickableItem> pickableList = new List<PickableItem>();

    void Awake()
    {
        interactCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PickableItem>() != null)
        {
            pickableList.Add(collision.gameObject.GetComponent<PickableItem>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PickableItem>() != null && pickableList.Contains(collision.gameObject.GetComponent<PickableItem>()))
        {
            pickableList.Remove(collision.gameObject.GetComponent<PickableItem>());
        }
    }
}
