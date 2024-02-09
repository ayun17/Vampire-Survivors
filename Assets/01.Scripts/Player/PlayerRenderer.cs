using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    protected SpriteRenderer _spriteRenderer;

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FaceDirection(Vector2 movementInput)
    {
        if (movementInput.x < 0)
            _spriteRenderer.flipX = true;
        else if (movementInput.x > 0)
            _spriteRenderer.flipX = false;
    }
}
