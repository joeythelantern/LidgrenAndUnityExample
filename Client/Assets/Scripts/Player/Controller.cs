using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Controller : MonoBehaviour
{
    Movement movement;

    void Awake()
    {
        movement = GetComponent<Movement>();
    }

    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var clickedPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Debug.Log(clickedPos);

            StaticManager.NetworkManager.SendPosition(clickedPos.x, clickedPos.y);
        }
	}
}