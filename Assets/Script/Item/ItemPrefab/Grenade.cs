using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeItem : IEquippedItem
{
    private Grenade _grenadePrefab;
    public GrenadeItem(Grenade grenadePrefab)
    {
        _grenadePrefab = grenadePrefab;
    }

    public EItemType GetItemType()
    {
        return EItemType.Grenade;
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
    private bool isActivated = false;
    [SerializeField] private float forwardForce = 500;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        isActivated = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActivated)
            return;
        print("Create Bomb bomb");
        Instantiate(blackHoleParticle, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        isActivated = true;
    }

    // private void Update()
    // {
    //     if (isActivated) { BlackHole(); }
    // }
    // private void BlackHole()
    // {
    //     Vector3 explosionPos = transform.position;
    //     Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, _explosionRadius);
    //     foreach (Collider2D hit in colliders)
    //     {
    //         Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

    //         if (rb != null)
    //             rb.AddExplosionForce(_explosionForce, explosionPos, _explosionRadius, 3.0F);
    //     }
    // }
    public void SetMoveDirection(Vector2 direction)
    {
        _rigidbody.AddForce(direction * forwardForce);
    }
}