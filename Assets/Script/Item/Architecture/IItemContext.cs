using UnityEngine;

public interface IItemContext
{
    public Vector2 SpawnPosition { get; }
    public bool IsFacingLeft { get; }
}