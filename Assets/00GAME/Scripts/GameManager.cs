using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Vector3 _cameraPosPlay = new Vector3(0, 0, -10);
    Vector3 _cameraPosTuto1 = new Vector3(-50, 72, -10);
    Vector3 _cameraPosTuto2 = new Vector3(-30, 72, -10);
    Vector3 _cameraPosTuto3 = new Vector3(-70, 72, -10);
    Vector3 _cameraPosMenu = new Vector3(-30, 30, -10);
    Vector3 _cameraPosCutScene = new Vector3(0, 72, -10);

    Vector2 _spawnPosPlay = new Vector2(-5,-2.2f);
    Vector2 _spawnPosTuto1 = new Vector2(-54, 71.8f);
    Vector2 _spawnPosTuto2 = new Vector2(-34, 71.8f);
    Vector2 _spawnPosTuto3 = new Vector2(-76, 71.8f);
    Vector2 _spawnPosMenu = new Vector2(-35.8f, 28.8f);
    Vector2 _spawnPosCutScene = new Vector2(-7.8f,69.8f);
    Vector2 _spawnPosOver = new Vector2(-0.75f, 71.8f);

    public GAME_STATE _gameState;

    public enum GAME_STATE
    {
        TUTO1 = 0, 
        TUTO2 = 1,
        TUTO3 = 2,
        MENU = 3,
        PLAY = 4,
        PAUSE = 5,
        CUTSCENE = 6,
        OVER = 7

    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(GAME_STATE.MENU);
        AudioManager.instance.PlaySound(AudioManager.instance.BGMusicClip, 0, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeState(GAME_STATE state)
    {
        if (state == this._gameState)
            return;

        if (state == GAME_STATE.PLAY)
        {
            PlayerController.instance.gameObject.transform.position = _spawnPosPlay;
            CameraController.instance.gameObject.transform.position = _cameraPosPlay;
            UIController.instance.HideMenu();
            UIController.instance.HideTutoandCutScene();
            PlayerController.instance.Init();
            StartCoroutine(UIController.instance.PlayDialog());
        }
        if (state == GAME_STATE.TUTO1)
        {
            Debug.Log("dddddd");
            PlayerController.instance.gameObject.transform.position = _spawnPosTuto1;
            CameraController.instance.gameObject.transform.position = _cameraPosTuto1;
            UIController.instance.ShowTutoUI(1);
            UIController.instance.HideMenu();
        }
        if (state == GAME_STATE.TUTO2)
        {
            PlayerController.instance.gameObject.transform.position = _spawnPosTuto2;
            CameraController.instance.gameObject.transform.position = _cameraPosTuto2;
            UIController.instance.ShowTutoUI(2);
            PlayerController.instance.Init();
        }
        if (state == GAME_STATE.TUTO3)
        {
            PlayerController.instance.gameObject.transform.position = _spawnPosTuto3;
            CameraController.instance.gameObject.transform.position = _cameraPosTuto3;
            UIController.instance.ShowTutoUI(3);
            PlayerController.instance.Init();
        }
        if (state == GAME_STATE.MENU)
        {
            PlayerController.instance.gameObject.transform.position = _spawnPosMenu;
            CameraController.instance.gameObject.transform.position = _cameraPosMenu;
            UIController.instance.ShowMenu();
            PlayerController.instance.Init();
        }
        if(state == GAME_STATE.CUTSCENE)
        {
            PlayerController.instance.gameObject.transform.position = _spawnPosCutScene;
            CameraController.instance.gameObject.transform.position = _cameraPosCutScene;
            UIController.instance.HideMenu();
            UIController.instance.HideTutoandCutScene();
            UIController.instance.ShowCutScene();
            PlayerController.instance.Init();
        }
        if (state == GAME_STATE.OVER)
        {
            PlayerController.instance.gameObject.transform.position = _spawnPosOver;
            CameraController.instance.gameObject.transform.position = _cameraPosCutScene;
            UIController.instance.HideMenu();
            UIController.instance.HideTutoandCutScene();
            PlayerController.instance.Init();
        }

        _gameState = state;
    }
}
