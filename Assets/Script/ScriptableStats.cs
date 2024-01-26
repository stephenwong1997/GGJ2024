using UnityEngine;
[CreateAssetMenu]
public class ScriptableStats : ScriptableObject
{
    public LayerMask PlayerLayer;
    public LayerMask GroundLayer;
    public LayerMask OneWayPlatformLayer;
    public float MaxSpeed = 14;

    public float Acceleration = 120;

    public float GroundDeceleration = 60;

    public float AirDeceleration = 30;

    public float GroundingForce = -1.5f;

    public float GrounderDistance = 0.05f;

    public float JumpPower = 36;

    public float MaxFallSpeed = 40;

    public float FallAcceleration = 110;

    public float JumpEndEarlyGravityModifier = 3;

    public float CoyoteTime = .15f;

    public float JumpBuffer = .2f;
}