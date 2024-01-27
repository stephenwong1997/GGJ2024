using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineItem : IEquippedItem
{
    private LandMine _landMinePrefab;
    public EItemType GetItemType()
    {
        return EItemType.LandMine;
    }
    public LandMineItem(LandMine landMinePrefab)
    {
        _landMinePrefab = landMinePrefab;
    }

    public void Use(IItemContext context)
    {
        SpawnLandMine(context.SpawnPosition);
    }

    private void SpawnLandMine(Vector2 spawnPosition)
    {
        LandMine landMineInstance = GameObject.Instantiate(_landMinePrefab);
        landMineInstance.transform.position = spawnPosition;

    }
}
[RequireComponent(typeof(Rigidbody2D))]
public class LandMine : MonoBehaviour, IItemPrefab
{
    private Rigidbody2D _rigidbody;
    private bool activated = false;
    [SerializeField] LayerMask excludeLayerFirstMoment;
    [SerializeField] LayerMask excludeLayerFromExplosion;

    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private GameObject flashLight;
    [SerializeField] private GameObject explosionParticle;

    // MonoBehaviour METHODS
    private void Awake()
    {
        activated = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        Invoke("ChangeIgnoreLayer", 0.7f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (activated)
            return;
        if (!collision.collider.tag.Equals("Player"))
            return;
        activated = true;
        AudioManager.instance.PlayOnUnusedTrack("mine_nc212991", 0.7f);
        Invoke("CreateFlashLight", 0.23f);
        Destroy(this.gameObject, .9f);
    }
    void CreateFlashLight() => Instantiate(flashLight, transform.position, Quaternion.identity);
    void ChangeIgnoreLayer() => _rigidbody.excludeLayers = excludeLayerFirstMoment;
    private void Explode()
    {
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        Vector3 explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, _explosionRadius);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null)
                rb.AddExplosionForce(_explosionForce, explosionPos, _explosionRadius, 1.0F);
        }
    }
    private void OnDestroy()
    {
        Explode();
    }
}

