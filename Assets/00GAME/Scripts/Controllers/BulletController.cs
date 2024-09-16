using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    Vector2 _moveDir;

    Rigidbody2D _rb;
    Animator _anim;

    public bool _isHit;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    public void Init(Vector2 moveDir)
    {
        _moveDir = moveDir;
        this.transform.localScale = new Vector3(-moveDir.x, 1, 1);
        _isHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isHit)
        {
            _rb.velocity = _moveDir * _moveSpeed;
        }
        if(this.transform.position.x > 15f || this.transform.position.x < -15f)
        {
            Destroy();
        }
        AnimProcess();
    }

    void AnimProcess() 
    {
        _anim.SetBool("Hit", _isHit);
    }

    public void Destroy()
    {
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isHit = true;
        _rb.velocity = Vector2.zero;
    }
}
