using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveWithMapItem : MonoBehaviour
{
    [SerializeField] private PlatformSpawnerSettingsSO _settings;
    [SerializeField] private Rigidbody2D _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        if (_settings!=null) {
            _settings = PlatformSpawner.Setting;
        }
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TryUpdatePosition();
    }

    // PRIVATE METHODS
    private void TryUpdatePosition()
    {

        // Update on every frame for easier adjustment of speed
        if (_rigidbody != null && _settings!=null)
        {
            _rigidbody.velocity = new Vector2(-1, _settings.PlatformSpeed);
            Vector2 targetPosition = transform.position;
            targetPosition.x -= _settings.PlatformSpeed * Time.fixedDeltaTime;

            _rigidbody.MovePosition(targetPosition);
        }



    }
}
