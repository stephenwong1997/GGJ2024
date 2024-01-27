using UnityEngine;
[CreateAssetMenu]
public class ScriptableStats : ScriptableObject
{
    public LayerMask PlayerLayer;
    public LayerMask GroundLayer;
    public LayerMask OneWayPlatformLayer;
    public float MoveSpeed = 14;

    public float SlowedSpeed = 7;

    public float Acceleration = 120;

    public float Decceleration = 120;

    public float velPower = 0.5f;

    public float GrounderDistance = 0.05f;

    public float JumpPower = 36;

    public float CoyoteTime = .15f;

    public float JumpBuffer = .2f;

    public float DashSpeed = 50f;

    public float DashDuration = 0.2f;

    public float DashCooldown = 1f;

}