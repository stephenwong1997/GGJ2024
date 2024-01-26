using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketItem : IItem
{
    private Rocket _rocketPrefab;

    public RocketItem(Rocket rocketPrefab)
    {
        _rocketPrefab = rocketPrefab;
    }

    public void Use(IItemContext context)
    {
        Vector2 fireDirection = context.IsFacingLeft ? Vector2.left : Vector2.right;
        SpawnRocket(fireDirection);
    }

    private void SpawnRocket(Vector2 fireDirection)
    {
        Rocket rocketInstance = GameObject.Instantiate(_rocketPrefab);
        rocketInstance.SetDirection(fireDirection);
        rocketInstance.StartMoving();
    }
}

[RequireComponent(typeof(Rigidbody2D))]
public class Rocket : MonoBehaviour, IItemPrefab
{
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection = Vector2.zero;

    // MonoBehaviour METHODS
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // TODO : OnCollision handling

    // PUBLIC METHODS
    public void SetDirection(Vector2 direction)
    {
        _moveDirection = direction;
        // TODO : Update rotation
    }

    public void StartMoving()
    {
        _rigidbody.velocity = _speed * _moveDirection.normalized;
    }
}