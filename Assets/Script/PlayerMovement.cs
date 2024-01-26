using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HappyFunTimes;

public class PlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private ScriptableStats _stats;
    [SerializeField] private List<GameObject> chickenOrEgg; //chicken = 0, egg = 1

    [SerializeField] private CharacterDisplayController _displayController;
    private Rigidbody2D _rb;
    private CircleCollider2D _col;
    private FrameInput _frameInput;
    private Vector2 _frameVelocity;
    public bool Facingleft;
    private bool _cachedQueryStartInColliders;
    private float _time;
    private SpriteRenderer m_spriteRenderer;


    [Header("HFT")]
    public Transform nameTransform;
    public Color baseColor;
    private HFTInput m_hftInput;
    public bool deleteWhenDisconnected = true;
    private HFTGamepad m_gamepad;

    [Header("Name")]
    private GUIStyle m_guiStyle = new GUIStyle();
    private GUIContent m_guiName = new GUIContent("");
    private Rect m_nameRect = new Rect(0, 0, 0, 0);
    private string m_playerName;
    private static int m_playerNumber = 0;


    private void Start()
    {
        m_playerNumber++;
        chickenOrEgg[m_playerNumber % 2].SetActive(true);
        _displayController = GetComponentInChildren<CharacterDisplayController>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CircleCollider2D>();
        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        m_hftInput = GetComponent<HFTInput>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_gamepad = GetComponent<HFTGamepad>();
        SetName(m_gamepad.Name);
        m_gamepad.OnNameChange += ChangeName;
        m_gamepad.OnDisconnect += Remove;
        baseColor = m_gamepad.color;
        m_spriteRenderer.color = baseColor;
    }
    private void Update()
    {
        _time += Time.deltaTime;
        GatherInput();
        UpdateDisplayController();
    }

    private void UpdateDisplayController()
    {
        _displayController.SetBool("IsGrounded", _grounded);
        _displayController.SetBool("IsRunning", _frameVelocity.x != 0);
        if (_frameVelocity.x != 0)
            _displayController.flipX = _frameVelocity.x < 0;
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        HandleJump();
        HandleDirection();
        ApplyMovement();
        HandleGravity();
    }

    #region Input
    private void GatherInput()
    {

        if (isDashing)
        {
            return;
        }


        bool jumpButtonDown = Input.GetKeyDown(KeyCode.Space) || m_hftInput.GetButtonDown("fire2");
        bool jumpButtonHeld = Input.GetKey(KeyCode.Space) || m_hftInput.GetButton("fire2");
        bool skillButtonDown = Input.GetButtonDown("Fire1") || m_hftInput.GetButtonDown("fire3");
        bool DashButtonDown = Input.GetButtonDown("Fire2") || m_hftInput.GetButtonDown("fire1");
        float horizontalInput = Input.GetAxisRaw("Horizontal") + m_hftInput.GetAxis("Horizontal");



        _frameInput = new FrameInput
        {
            JumpDown = jumpButtonDown,
            JumpHeld = jumpButtonHeld,
            SkillDown = skillButtonDown,
            DashDown = DashButtonDown,
            Move = new Vector2(horizontalInput, Input.GetAxisRaw("Vertical") + m_hftInput.GetAxis("Vertical"))
        };

        if (_frameInput.JumpDown)
        {
            Debug.Log("JumpDown");
            _jumpToConsume = true;
            _timeJumpWasPressed = _time;
        }

        if (_frameInput.SkillDown)
        {
            Debug.Log("SkillDown");
        }

        if (_frameInput.DashDown)
        {
            Debug.Log("DashDown");
            if (CanDash) StartCoroutine(Dash());
        }

    }
    #endregion

    #region Collisions

    private float _frameLeftGrounded = float.MinValue;
    [SerializeField] private bool _grounded;

    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;
        //public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask, float minDepth);
        //CapsuleCollider2D a = null;
        //bool ceilindgHit = Physics2D.CapsuleCast(a.bounds.center, a.size, a.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.OneWayPlatformLayer);
        float radius = _col.bounds.extents.x;
        bool ceilingHit = Physics2D.CircleCast(_col.bounds.center, radius, Vector2.up, _stats.GrounderDistance, ~_stats.OneWayPlatformLayer);
        bool groundHit = Physics2D.CircleCast(_col.bounds.center, radius, Vector2.down, _stats.GrounderDistance, _stats.GroundLayer) ||
                 Physics2D.CircleCast(_col.bounds.center, radius, Vector2.down, _stats.GrounderDistance, _stats.OneWayPlatformLayer) ||
                 Physics2D.CircleCast(_col.bounds.center, radius, Vector2.down, _stats.GrounderDistance, _stats.PlayerLayer);

        if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

        if (!_grounded && (groundHit))
        {
            _grounded = true;
            _coyoteUsable = true;
            _bufferedJumpUsable = true;
            _endedJumpEarly = false;
        }
        else if (_grounded && !(groundHit))
        {
            _grounded = false;
            _frameLeftGrounded = _time;
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
    }

    #endregion

    #region Jumping

    private bool _jumpToConsume;
    private bool _bufferedJumpUsable;
    private bool _endedJumpEarly;
    private bool _coyoteUsable;
    private float _timeJumpWasPressed;

    private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
    private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

    private void HandleJump()
    {

        if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;

        if (!_jumpToConsume && !HasBufferedJump) return;

        if (_grounded || CanUseCoyote) ExecuteJump();

        _jumpToConsume = false;
    }

    private void ExecuteJump()
    {
        _endedJumpEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;
        _frameVelocity.y = _stats.JumpPower;
        //_rb.AddForce(new Vector2(0, _stats.JumpPower), ForceMode2D.Impulse);
    }

    #endregion

    #region Dashing
    public bool CanDash = true;
    public bool isDashing;

    private IEnumerator Dash()
    {
        Debug.Log("Dash");
        CanDash = false;
        isDashing = true;

        float targetDashSpeed = _stats.DashSpeed * (Facingleft ? -1 : 1);
        float originalSpeed = _rb.velocity.x;
        float dashTime = 0;

        while (dashTime < _stats.DashDuration)
        {
            dashTime += Time.fixedDeltaTime;
            _frameVelocity.x = Mathf.MoveTowards(originalSpeed, targetDashSpeed, dashTime / _stats.DashDuration * _stats.DashSpeed);
            _rb.velocity = new Vector2(_frameVelocity.x, 0);
            yield return new WaitForFixedUpdate();
        }

        isDashing = false;
        _frameVelocity.x = originalSpeed;
        yield return new WaitForSeconds(_stats.DashCooldown);
        CanDash = true;
    }

    #endregion

    private void HandleDirection()
    {
        if (_frameInput.Move.x == 0)
        {
            var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
        }

        if (_frameVelocity.x != 0)
        {
            //m_spriteRenderer.flipX = _frameVelocity.x < 0; //Flip
            Facingleft = _frameVelocity.x < 0;
        }


    }
    private void HandleGravity()
    {
        if (_grounded && _frameVelocity.y <= 0f)
        {
            _frameVelocity.y = _stats.GroundingForce;
        }
        else
        {
            if (isDashing)
            {
                return;
            }
            var inAirGravity = _stats.FallAcceleration;
            if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
        }
    }
    private void ApplyMovement() => _rb.velocity = _frameVelocity;

    #region HFT
    void Remove()
    {
        if (deleteWhenDisconnected)
        {
            Destroy(gameObject);
        }
    }

    void SetName(string name)
    {
        m_playerName = name;
        gameObject.name = "Player-" + m_playerName;
        m_guiName = new GUIContent(m_playerName);
        Vector2 size = m_guiStyle.CalcSize(m_guiName);
        m_nameRect.width = size.x + 12;
        m_nameRect.height = size.y + 5;
    }

    void OnGUI()
    {
        Vector2 size = m_guiStyle.CalcSize(m_guiName);
        Vector3 coords = Camera.main.WorldToScreenPoint(nameTransform.position);
        m_nameRect.x = coords.x - size.x * 0.5f - 5f;
        m_nameRect.y = Screen.height - coords.y;
        m_guiStyle.normal.textColor = Color.black;
        m_guiStyle.contentOffset = new Vector2(4, 2);
        GUI.Box(m_nameRect, m_playerName, m_guiStyle);
    }

    void ChangeName(object sender, System.EventArgs e)
    {
        SetName(m_gamepad.Name);
    }

    #endregion

}

public struct FrameInput
{
    public bool JumpDown;
    public bool JumpHeld;
    public bool SkillDown;
    public bool DashDown;
    public Vector2 Move;
}
