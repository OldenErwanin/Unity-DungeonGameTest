using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("General parameters")]
    public Transform followTarget;
    [SerializeField] private float smoothFollowRate = 1.5f;

    // Follow the target with a smooth delay
    void FixedUpdate()
    {
        this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, followTarget.transform.position.x, Time.deltaTime * smoothFollowRate),
                                              Mathf.Lerp(this.transform.position.y, followTarget.transform.position.y, Time.deltaTime * smoothFollowRate), -10);
    }
}
