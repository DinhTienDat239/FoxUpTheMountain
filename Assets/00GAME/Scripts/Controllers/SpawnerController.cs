using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : Singleton<SpawnerController>
{
    [SerializeField] GameObject _bullet;
    // Start is called before the first frame update
    void Start()
    {
        ObjectPooling.instance.CreatePool(_bullet,10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBullet(Vector2 dir,Vector2 pos)
    {
        BulletController bullet = ObjectPooling.instance.GetObject(_bullet).GetComponent<BulletController>();
        bullet.Init(dir);
        bullet.gameObject.SetActive(true);
        bullet.transform.position = pos;
    }
}
