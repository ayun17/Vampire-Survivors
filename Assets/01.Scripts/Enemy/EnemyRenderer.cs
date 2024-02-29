using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRenderer : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private PlayerAnimator _playerAnimator;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _playerAnimator = GetComponent<PlayerAnimator>();
    }

    public void FaceDirection(Vector2 vector)
    {
        Vector3 direction = (Vector3)vector - transform.position;
        Vector3 result = Vector3.Cross(transform.up, direction);

        _sprite.flipX = result.z > 0;
    }

    public void SelfMovementFaceDirection(Vector2 vector)
    {
        Vector3 result = Vector3.Cross(transform.up, vector);

        _sprite.flipX = result.z > 0;
    }
}
