using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The speed that the player will move.")]
    private float speed = 5f;

    Vector3 movePosition;

    void Awake()
    {
        movePosition = transform.position;
    }

    void Update()
    {
        if (speed != 0f)
            transform.position = Vector3.MoveTowards(transform.position, movePosition, speed * Time.deltaTime);
    }

    public void SetMovePosition(Vector3 newPosition)
    {
        movePosition = newPosition;
    }
}
