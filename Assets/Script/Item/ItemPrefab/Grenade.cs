using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeItem : IEquippedItem
{
    private Grenade _grenadePrefab;
    public GrenadeItem(Grenade grenadePrefab)
    {
        _grenadePrefab = grenadePrefab;
    }
    public void Use(IItemContext context)
    {
        Vector2 fireDirection = context.IsFacingLeft ? Vector2.left + Vector2.up : Vector2.one;
        SpawnGrenade(context.SpawnPosition, fireDirection);
    }
    private void SpawnGrenade(Vector2 spawnPosition, Vector2 fireDirection)
    {
        Grenade grenadeInstance = GameObject.Instantiate(_grenadePrefab);

        grenadeInstance.transform.position = spawnPosition;
        grenadeInstance.SetMoveDirection(fireDirection);
    }
}

[RequireComponent(typeof(Rigidbody2D))]
public class Grenade : MonoBehaviour, IItemPrefab
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject blackHoleParticle;
    [SerializeField] private float forwardForce = 500;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(blackHoleParticle, transform.position, Quaternion.identity);
        BlackHole();
        Destroy(this.gameObject);
    }
    private void BlackHole()
    {

    }
    public void SetMoveDirection(Vector2 direction)
    {
        _rigidbody.AddForce(direction * forwardForce);
    }
}