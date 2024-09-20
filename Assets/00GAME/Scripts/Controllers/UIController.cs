using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [SerializeField] GameObject _menuUI;
    [SerializeField] GameObject _pauseUI;
    [SerializeField] GameObject _playUI;
    [SerializeField] Image _blankDark;

    [SerializeField] GameObject _dialogBox;
    [SerializeField] Text _dialogTxt;
    [SerializeField] Text _spaceTxt;

    Vector3 _dialogPosTuto = new Vector3(0, -380,0);
    Vector3 _dialogPosCutScene = new Vector3(0, 250,0);
    bool _isCutScene1 = false;

    string _tuto1 = "THis is the tutorial you need to climb up the\r\nmountain. First, know how to move:\r\nA/Left and D/Right - to run!     W/Up - to jump!\r\nNow try to reach the Tree!";
    string _tuto2 = "GREAT! You know how to move!\r\nNow always remember that:\r\nThe longer you press moving keys, the faster you\r\ngo. Try to run and jump farther to that tree!";
    string _tuto3 = "AWESOME! one last thing:\r\nPRESS *SPACE* to dash much much farther!!\r\nREACH THE TREE again!";

    string _cutscene1 = "Hey fox! get over here with me so we can\r\ncelebrate our lovely relation! i'm so happy to\r\nhave you with me, you make me laugh every single\r\nday! Come on, le't's sign our name on this tree.";
    string _cutscene2 = "Damn it! I need to get back to the top\r\nwith samoyed...";
    string _cutscene3 = "Fox: I have let you waited, it's been \r\ncrazy down there... So.. Happy our love. *moaaa*";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space) && _isCutScene1)
        {
            StartCoroutine(CutSceneFlow());
        }

        if(Input.GetKey(KeyCode.Escape) && (GameManager.instance._gameState != GameManager.GAME_STATE.MENU && GameManager.instance._gameState != GameManager.GAME_STATE.PAUSE))
        {
            _pauseUI.gameObject.SetActive(true);
        }
    }

    public void ContinueBtn()
    {
        _pauseUI.gameObject.SetActive(false);
    }
    public void ToMenuBtn()
    {
        _pauseUI.gameObject.SetActive(false);
        HideTutoandCutScene();
        GameManager.instance.ChangeState(GameManager.GAME_STATE.MENU);
    }

    public void PlayTutoBtn()
    {
        GameManager.instance.ChangeState(GameManager.GAME_STATE.TUTO1);
        _menuUI.gameObject.SetActive(false);
        AudioManager.instance.PlaySound(AudioManager.instance.UIClips[0], 0, false);
    }

    public void PlayBtn()
    {
        GameManager.instance.ChangeState(GameManager.GAME_STATE.CUTSCENE);
        _menuUI.gameObject.SetActive(false);
        AudioManager.instance.PlaySound(AudioManager.instance.UIClips[0], 0, false);
    }

    public void ShowTutoUI(int tuto)
    {
        _dialogBox.SetActive(true);
        switch (tuto)
        {
            case 1:
                StopAllCoroutines();
                StartCoroutine(TypingTxt(_tuto1));
                break;
            case 2:
                StopAllCoroutines();
                StartCoroutine(TypingTxt(_tuto2));
                break;
            case 3:
                StopAllCoroutines();
                StartCoroutine(TypingTxt(_tuto3));
                break;
            default:
                break;
        }
        _dialogBox.transform.localPosition = _dialogPosTuto;
    }
    public void ShowCutScene()
    {
        _isCutScene1 = true;
        _dialogBox.SetActive(true);
        _dialogBox.transform.localPosition = _dialogPosCutScene;
        _spaceTxt.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(TypingTxt(_cutscene1));
    }

    public void HideTutoandCutScene()
    {
        _dialogBox.SetActive(false);
        _spaceTxt.gameObject.SetActive(false);
        _isCutScene1 = false;
    }
    public void ShowMenu()
    {
        _menuUI.gameObject.SetActive(true);
        _dialogBox.SetActive(false);
    }
    public void HideMenu()
    {
        _menuUI.gameObject.SetActive(false);
    }
    IEnumerator TypingTxt(string chat)
    {
        
        _dialogTxt.text = "";
        foreach (var i in chat)
        {
            yield return new WaitForSeconds(0.02f);
            _dialogTxt.text += i;
        }
    }
    IEnumerator CutSceneFlow()
    {
        _isCutScene1 = false;
        yield return new WaitForSeconds(2f);
        PlayerController.instance.CutSceneJump();
        Debug.Log("a");
        StartCoroutine(SceneSwitch());
        yield return new WaitForSeconds(5f);
        GameManager.instance.ChangeState(GameManager.GAME_STATE.PLAY);
    }

    public IEnumerator SceneSwitch()
    {
        yield return new WaitForSeconds(1f);
        _blankDark.transform.localPosition = new Vector2(0, 1200);
        _blankDark.gameObject.SetActive(true);
        while (_blankDark.transform.localPosition.y > 0)
        {
            yield return new WaitForSeconds(0.01f);
            _blankDark.transform.localPosition = new Vector2(0, _blankDark.transform.localPosition.y - 12f);
        }
        _blankDark.transform.localPosition = new Vector2(0, 0);
        yield return new WaitForSeconds(3f);
        _blankDark.gameObject.SetActive(false);
    }

    public IEnumerator FromTuToToMenu()
    {
        StartCoroutine(SceneSwitch());
        yield return new WaitForSeconds(5f);
        GameManager.instance.ChangeState(GameManager.GAME_STATE.MENU);
    }
    public IEnumerator PlayDialog()
    {
        yield return new WaitForSeconds(5f);
        _dialogBox.SetActive(true);
        _dialogBox.transform.localPosition = _dialogPosCutScene;
        _dialogTxt.text = "";
        StartCoroutine(TypingTxt(_cutscene2));
        yield return new WaitForSeconds(5f);
        _dialogBox.SetActive(false);
    }

    public IEnumerator OverDialog()
    {
        StartCoroutine(SceneSwitch());
        yield return new WaitForSeconds(5f);
        GameManager.instance.ChangeState(GameManager.GAME_STATE.OVER);
        yield return new WaitForSeconds(5f);
        _dialogBox.SetActive(true);
        _dialogBox.transform.localPosition = _dialogPosCutScene;
        _dialogTxt.text = "";
        StartCoroutine(TypingTxt(_cutscene3));
        yield return new WaitForSeconds(8f);
        _dialogBox.SetActive(false);
        yield return new WaitForSeconds(2f);
        StartCoroutine(SceneSwitch());
        yield return new WaitForSeconds(5f);
        GameManager.instance.ChangeState(GameManager.GAME_STATE.MENU);
    }
}
