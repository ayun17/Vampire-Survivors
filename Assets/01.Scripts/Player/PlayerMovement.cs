using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public UnityEvent<float> onSpeedChanged;

    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float accel = 50f, deAccel = 50f;

    private float _currentSpeed = 0;

    private Vector2 _moveDirection;
    private Rigidbody2D _rigid;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        onSpeedChanged?.Invoke(_currentSpeed);
        _rigid.velocity = _moveDirection * _currentSpeed;
    }

    public void Movement(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0) // A�� D�� ���� ������ ���
        {
            if (Vector2.Dot(_moveDirection, direction) < 0)
                _currentSpeed = 0;
        }

        _moveDirection = direction;
        _currentSpeed = CalCulateSpeed(direction);
    }

    private float CalCulateSpeed(Vector2 direction) // speed �ڿ������� ����
    {
        if (direction.sqrMagnitude > 0)
            _currentSpeed += accel * Time.deltaTime;
        else
            _currentSpeed -= deAccel * Time.deltaTime;

        return Mathf.Clamp(_currentSpeed, 0, maxSpeed);
    }

    private void StopPlayer()
    {
        _moveDirection = Vector2.zero;
        _currentSpeed = 0;
    }
}
