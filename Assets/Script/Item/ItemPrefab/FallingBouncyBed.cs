using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBedItem : IEquippedItem
{
    [SerializeField] private FallingBouncyBed _bouncyBedPrefab;

    public BouncyBedItem(FallingBouncyBed bouncyBed)
    {
        _bouncyBedPrefab = bouncyBed;
    }
    public EItemType GetItemType()
    {
        return EItemType.BouncyBed;
    }

    public void Use(IItemContext context)
    {
        SpawnBouncyBed(context.SpawnPosition);
    }

    private void SpawnBouncyBed(Vector2 spawnPosition)
    {
        FallingBouncyBed bouncyBedInstance = GameObject.Instantiate(_bouncyBedPrefab);
        bouncyBedInstance.transform.position = spawnPosition;
    }
}
[RequireComponent(typeof(Rigidbody2D))]
public class FallingBouncyBed : MonoBehaviour, IItemPrefab
{
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private Animator _anim;
    private bool activated = false;
    [SerializeField] LayerMask excludeLayerAfterMoment;
    //[SerializeField] LayerMask excludeLayerAfter;

    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    private void Awake()
    {
        activated = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _anim = GetComponent<Animator>();
        Invoke(nameof(ChangeIgnoreLayer), 0.7f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated)
        {
            return;
        }
        if (!collision.gameObject.tag.Equals("Player"))
        {
            return;
        }
        activated = true;
        Explode();
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (activated)

    //        return;

    //    if (!collision.collider.tag.Equals("Player"))

    //        return;

    //    activated = true;
    //    Explode();
    //    //Destroy(this.gameObject);
    //}
    void ChangeIgnoreLayer()
    {
        _collider.excludeLayers = excludeLayerAfterMoment;
    }
    //void ChangeIgnoreLayer2() => _rigidbody.excludeLayers = excludeLayerAfter;
    private void Explode()
    {
        GetComponent<MoveWithMapItem>().enabled = true;
        //_rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        gameObject.layer = LayerMask.NameToLayer("Ground");
        _anim.ResetTrigger("Bounce");
        _anim.SetTrigger("Bounce");
        Vector3 explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, _explosionRadius);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null && hit.tag.Equals("Player"))
                rb.AddForce((new Vector2(-1, 1)) * _explosionForce);
        }
    }
}