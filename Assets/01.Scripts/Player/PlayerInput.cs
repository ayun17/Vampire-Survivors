using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector2> onMovementKeyPressed = null;

    private void Update()
    {
        GetMovementInput();
    }

    private void GetMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(x, y);
        onMovementKeyPressed?.Invoke(direction.normalized);
    }
}
