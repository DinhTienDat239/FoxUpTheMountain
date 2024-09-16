using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTrapController : MonoBehaviour
{
    [SerializeField] float _shootDelayTime;
    [SerializeField] float _shootDelayTimer;
    Vector2 _shootDir;

    public bool _isAttack;
    public bool _isHit;

    Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _shootDir = new Vector2(-this.transform.localScale.x,0);
        _shootDelayTimer = _shootDelayTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!(PlayerController.instance.transform.position.y + 6 >= this.transform.position.y))
            return;

        if (!_isAttack)
        {
            _shootDelayTimer -= Time.deltaTime;
            if (_shootDelayTimer < 0)
            {
                _isAttack = true;
            }
        }

        AnimProcess();
    }

    public void Shoot()
    {
        SpawnerController.instance.SpawnBullet(_shootDir, new Vector2(this.transform.localScale.x < 0 ? this.transform.position.x + 0.7f : this.transform.position.x - 0.7f, this.transform.position.y-0.4f));
        _shootDelayTimer = _shootDelayTime;
    }

    void AnimProcess()
    {
        _anim.SetBool("Attack", _isAttack);
        _anim.SetBool("Hit", _isHit);
    }
}
