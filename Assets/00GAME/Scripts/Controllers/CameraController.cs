using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance._gameState != GameManager.GAME_STATE.PLAY)
            return;

        if (PlayerController.instance.transform.position.y + 1.5f < 0)
        {
            this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            return;
        }
        this.transform.position = new Vector3(this.transform.position.x, PlayerController.instance.transform.position.y + 1.5f,this.transform.position.z);
    }
}
