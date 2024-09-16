using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] float _movementSpeed;
    [SerializeField] float _jumpForce;
    [SerializeField] float _stunForce;
    [SerializeField] float _dashForce;
    [SerializeField] Vector3 _movementDir;
    [SerializeField] TrailRenderer _trailDash;

    float holdingMaxTime = 0.5f;
    float holdingMaxTimer = 0;
    float _lastVelocityX;

    float stunTime = 3f;
    float stunTimer = 0;
    float dashTime = 0.3f;
    float dashTimer = 0;

    [SerializeField] bool _isJump;
    [SerializeField] bool _isRun;
    [SerializeField] bool _isStun;
    [SerializeField] bool _isDash;
    [SerializeField] bool _canDash;
    [SerializeField] bool _doneTuto;
    [SerializeField] bool _doneGame;

    Rigidbody2D _rb;
    Animator _anim;

    public LayerMask groundLayer;
    public Vector2 groundCheckSize;
    public float groundCastDis;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        _movementDir = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {


    }
    private void FixedUpdate()
    {

        if (GameManager.instance._gameState == GameManager.GAME_STATE.MENU || GameManager.instance._gameState == GameManager.GAME_STATE.CUTSCENE || GameManager.instance._gameState == GameManager.GAME_STATE.OVER)
            return;
        else
        {

            ProcessInput();
        }


        ProcessAnim();
    }

    public void Init()
    {
        _isJump = false;
        _isRun = false;
        _isStun = false;
        _isDash = false;
        _canDash = false;
        _doneTuto = false;
        _doneGame = false;
        this.transform.localScale = Vector3.one;

        if (_rb != null)
            _rb.velocity = Vector2.zero;

        if (_anim != null)
            ProcessAnim();
    }

    void ProcessInput()
    {


        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
        }
        else
        {
            _isStun = false;
        }

        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
        else
        {
            _isDash = false;
            _trailDash.emitting = false;
        }

        if (_isStun || _isDash)
            return;

        _movementDir.x = Input.GetAxisRaw("Horizontal");
        if (_movementDir.x < 0)
            this.transform.localScale = new Vector3(-1, 1, 1);
        else if (_movementDir.x > 0)
            this.transform.localScale = Vector3.one;


        Debug.Log(isGrounded());

        if (isGrounded())
        {
            if (holdingMaxTimer < holdingMaxTime)
                holdingMaxTimer += Time.deltaTime;
            if (holdingMaxTimer > holdingMaxTime)
                holdingMaxTimer = holdingMaxTime;
            _rb.velocity = new Vector3(_movementDir.x * (_movementSpeed * holdingMaxTimer) / holdingMaxTime, _rb.velocity.y);
            _lastVelocityX = _rb.velocity.x;
        }
        else
        {

            _rb.velocity = new Vector3(_lastVelocityX + (_movementDir.x * _movementSpeed / 4), _rb.velocity.y);
        }

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && isGrounded())
        {
            _isJump = true;
            _rb.AddForce(Vector2.up * _jumpForce);
            holdingMaxTimer = 0;
            AudioManager.instance.PlaySound(AudioManager.instance.UIClips[2], 0, false);
        }

        if (Input.GetKey(KeyCode.Space) && !_isDash && _canDash)
        {
            _lastVelocityX = 0;
            _rb.AddForce(new Vector2(this.transform.localScale.x, 0) * _dashForce);
            AudioManager.instance.PlaySound(AudioManager.instance.UIClips[2], 0, false);
            _isDash = true;
            _canDash = false;
            _trailDash.emitting = true;
            dashTimer = dashTime;
        }

        if (_movementDir.x == 0)
            holdingMaxTimer = 0;
    }

    bool isGrounded()
    {
        if (Physics2D.BoxCast(this.transform.position, groundCheckSize, 0, -transform.up, groundCastDis, groundLayer))
            return true;
        else
            return false;
    }

    void ProcessAnim()
    {
        if (_rb.velocity.x != 0)
            _isRun = true;
        else
            _isRun = false;

        _anim.SetBool("Run", _isRun);
        _anim.SetBool("Jump", _isJump);
        _anim.SetBool("Stun", _isStun);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Ground" && isGrounded())
        {
            _isJump = false;
            _canDash = true;
            holdingMaxTimer = 0;
        }

        if (collision.gameObject.tag == "Spike")
        {
            _rb.AddForce(new Vector2(-(_rb.velocity.x / 3) + Random.Range(-0.5f, 0.5f), 1) * _stunForce);
            _isStun = true;
            stunTimer = stunTime;
            AudioManager.instance.PlaySound(AudioManager.instance.UIClips[1], 0, false);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && isGrounded())
        {
            _canDash = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position - transform.up * groundCastDis, groundCheckSize);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CTuto1")
            GameManager.instance.ChangeState(GameManager.GAME_STATE.TUTO2);
        if (collision.tag == "CTuto2")
            GameManager.instance.ChangeState(GameManager.GAME_STATE.TUTO3);
        if (collision.tag == "CTuto3")
        {
            if(_doneTuto == false)
            {
                StartCoroutine(UIController.instance.FromTuToToMenu());
                _doneTuto = true;
            }
        }
        if (collision.tag == "Finish")
        {
            if (_doneGame == false)
            {

                StartCoroutine(UIController.instance.OverDialog());
                _doneGame = true;
            }
        }
    }

    internal void CutSceneJump()
    {
        Vector2 dir = new Vector2(0.15f, 1f);
        _rb.AddForce(dir.normalized * 500);
    }

}
