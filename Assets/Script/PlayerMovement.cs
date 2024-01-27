using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HappyFunTimes;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
    public bool IsChicken { get; private set; }

    [Header("Stats")]
    [SerializeField] private ScriptableStats _stats;
    [SerializeField] private List<GameObject> chickenOrEgg; //chicken = 0, egg = 1

    [SerializeField] private CharacterDisplayController _displayController;
    [SerializeField] private PlayerItemController _itemController;

    private Rigidbody2D _rb;
    private CircleCollider2D _col;
    private FrameInput _frameInput;
    private Vector2 _frameVelocity;
    public bool Facingleft;
    private bool _cachedQueryStartInColliders;
    private float _time;


    [Header("HFT")]
    public Transform nameTransform;
    private HFTInput m_hftInput;
    public bool deleteWhenDisconnected = true;
    private HFTGamepad m_gamepad;

    [Header("Name")]
    private string m_playerName;
    private static int m_playerNumber = 0;
    public TextMeshProUGUI NameUI;
    private void Awake()
    {
        m_playerNumber++;

        int index = m_playerNumber % 2;
        chickenOrEgg[index].SetActive(true);
        // Index 0 => Chicken
        // Index 1 => Egg
        IsChicken = index == 0;

        _displayController = GetComponentInChildren<CharacterDisplayController>();

        _itemController = GetComponentInChildren<PlayerItemController>();
        _itemController.InjectDependencies(new PlayerItemControllerDependencies()
        {
            IsFacingLeftGetter = () => Facingleft
        });

        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CircleCollider2D>();

        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;

        m_hftInput = GetComponent<HFTInput>();
        m_gamepad = GetComponent<HFTGamepad>();

        SetName(m_gamepad.Name);

        m_gamepad.OnNameChange += ChangeName;
        m_gamepad.OnDisconnect += Remove;
        _displayController.RandomColor(m_playerNumber);
        m_gamepad.color = _displayController.preferredColor;

    }

    private void Start()
    {
        // In Start, not Awake, because the components may need to initalize themselves in Awake first
        RespawnManager.AddToTargetTransforms(this.transform);
    }

    private void Update()
    {

        _time += Time.deltaTime;
        GatherInput();
        UpdateDisplayController();
    }

    public void PlayerRespawn()
    {
        if (IsChicken)
        {
            AudioManager.instance.PlayOnUnusedTrack("chicken_nc148163", 0.7f);
        }
        else
        {
            AudioManager.instance.PlayOnUnusedTrack("egg_nc261748", 0.7f);
        }

    }

    private void UpdateDisplayController()
    {
        _displayController.SetBool("IsGrounded", _grounded);
        _displayController.SetBool("IsRunning", _rb.velocity.x < -0.5f || _rb.velocity.x > 0.5f);
        _displayController.flipX = Facingleft;
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        HandleJump();
        HandleDirection();
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
            _itemController.OnSkillDown();
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
        _rb.AddForce(new Vector2(0, _stats.JumpPower), ForceMode2D.Impulse);
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

        Vector2 dashForce = new Vector2((Facingleft ? -1 : 1) * _stats.DashSpeed, 0);
        float orgGS = _rb.gravityScale;


        _rb.velocity = new Vector2(0, 0);
        _rb.gravityScale = 0f;
        _rb.AddForce(dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(_stats.DashDuration);
        _rb.velocity = new Vector2(0, _rb.velocity.y);
        _rb.gravityScale = orgGS;
        isDashing = false;

        // Wait for dash cooldown
        yield return new WaitForSeconds(_stats.DashCooldown);

        CanDash = true;
    }

    #endregion


    public bool isSlowed = false;

    public void GetSlowed()
    {
        //Debug.Log("GetSlowed");
        AudioManager.instance.PlayOnUnusedTrack("StepShit", 1f);
        isSlowed = true;
    }
    public void GetOffSlowed()
    {
        //  Debug.Log("GetOffSlowed");
        isSlowed = false;
    }

    private void HandleDirection()
    {
        float targetSpeed;
        if (isSlowed)
        {
            targetSpeed = _frameInput.Move.x * _stats.SlowedSpeed;
        }
        else
        {
            targetSpeed = _frameInput.Move.x * _stats.MoveSpeed;
        }


        float speedDif = targetSpeed - _rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _stats.Acceleration : _stats.Decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _stats.velPower) * Mathf.Sign(speedDif);


        if (targetSpeed != 0)
        {
            Facingleft = targetSpeed < 0;
        }

        _rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }




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
        NameUI.text = m_playerName;
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
