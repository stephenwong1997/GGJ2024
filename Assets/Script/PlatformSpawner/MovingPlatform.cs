using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private bool _isMoving = false;
    private PlatformSpawnerSettingsSO _settings;
    private float _destroyXPosition = 0;

    // MonoBehaviour 
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        TryUpdatePosition();
    }

    // PUBLIC METHODS
    public void StartMoving()
    {
        _isMoving = true;
    }

    public void SetSettings(PlatformSpawnerSettingsSO settings)
    {
        _settings = settings;
    }

    public void SetDestroyPosition(Vector2 position)
    {
        _destroyXPosition = position.x;
    }

    // PRIVATE METHODS
    private void TryUpdatePosition()
    {
        if (!_isMoving)
            return;

        // Update on every frame for easier adjustment of speed
        _rigidbody.velocity = new Vector2(-1, _settings.PlatformSpeed);

        Vector2 targetPosition = transform.position;
        targetPosition.x -= _settings.PlatformSpeed * Time.fixedDeltaTime;

        _rigidbody.MovePosition(targetPosition);

        if (targetPosition.x <= _destroyXPosition)
            // Use object pool.... later on if have time😂
            Destroy(this.gameObject);
    }
}