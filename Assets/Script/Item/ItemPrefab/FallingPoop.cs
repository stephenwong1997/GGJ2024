using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FallingPoopItem : IEquippedItem
{
    private FallingPoop _fallingPoopPrefab;
    public FallingPoopItem(FallingPoop fallingPoopPrefab)
    {
        _fallingPoopPrefab = fallingPoopPrefab;
    }
    public EItemType GetItemType()
    {
        return EItemType.Poop;
    }

    public void Use(IItemContext context)
    {
        Vector2 fireDirection = context.IsFacingLeft ? Vector2.left + Vector2.up : Vector2.one;
        SpawnPoop(context.SpawnPosition);
    }
    private void SpawnPoop(Vector2 spawnPosition)
    {
        FallingPoop poopInstance = GameObject.Instantiate(_fallingPoopPrefab);
        poopInstance.transform.position = spawnPosition;
        AudioManager.instance.PlayOnUnusedTrack("poop", 1f);
    }
}
[RequireComponent(typeof(Rigidbody2D))]
public class FallingPoop : MonoBehaviour, IItemPrefab
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject poopGround;
    private void Awake()
    {
        AudioManager.instance.PlayOnUnusedTrack("poop", 0.7f);
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(poopGround, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
