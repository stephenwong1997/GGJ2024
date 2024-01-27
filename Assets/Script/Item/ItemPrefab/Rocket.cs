using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketItem : IEquippedItem
{
    private Rocket _rocketPrefab;

    public RocketItem(Rocket rocketPrefab)
    {
        _rocketPrefab = rocketPrefab;
    }

    public EItemType GetItemType()
    {
        return EItemType.Rocket;
    }

    public void Use(IItemContext context)
    {
        Vector2 fireDirection = context.IsFacingLeft ? Vector2.left : Vector2.right;
        SpawnRocket(context.SpawnPosition, fireDirection);
    }

    private void SpawnRocket(Vector2 spawnPosition, Vector2 fireDirection)
    {
        Rocket rocketInstance = GameObject.Instantiate(_rocketPrefab);

        rocketInstance.transform.position = spawnPosition;

        rocketInstance.SetMoveDirection(fireDirection);
        rocketInstance.StartMoving();
    }
}

[RequireComponent(typeof(Rigidbody2D))]
public class Rocket : MonoBehaviour, IItemPrefab
{
    [SerializeField] private float _speed;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;

    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection = Vector2.zero;
    [SerializeField] private GameObject explosionParticle;

    // MonoBehaviour METHODS
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // TODO : OnCollision handling
    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.instance.PlayOnUnusedTrack("explode_nc273971",0.7f);
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        Explode();
        Destroy(this.gameObject);
    }
    private void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, _explosionRadius);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null)
                rb.AddExplosionForce(_explosionForce, explosionPos, _explosionRadius, 1.0F);
        }
    }
    // PUBLIC METHODS
    public void SetMoveDirection(Vector2 direction)
    {
        _moveDirection = direction;
        Vector2 velocity = _moveDirection;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
    private void Update()
    {

        Vector2 velocity = _rigidbody.velocity.normalized;
        _rigidbody.AddForce(velocity * 5.5f);
    }

    public void StartMoving()
    {
        _rigidbody.velocity = _speed * _moveDirection.normalized;
    }
}
public static class Rigidbody2DExtension
{
    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }

    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        Vector3 baseForce = dir.normalized * explosionForce * wearoff;
        body.AddForce(baseForce);

        float upliftWearoff = 1 - upliftModifier / explosionRadius;
        Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
        body.AddForce(upliftForce);
    }
}