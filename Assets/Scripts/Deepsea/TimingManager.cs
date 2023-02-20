using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimingManager : MonoBehaviour
{
    public static TimingManager instance { get; private set; }

    [Header("Control Panel")]
    public GameObject canvas;
    public TMP_Text roundText;
    public GameObject startMenuPanel;
    public GameObject restartPanel;
    public TMP_Text roundTextInWrist;
    public GameObject congratsPanel;

    [Header("Player")]
    public GameObject XROrigin;
    public Transform resetTransform;
    public Camera playerHead;

    [Header("Fish Enemies")]
    public GameObject level1Fish;
    public GameObject level2Fish;
    public GameObject level3Fish;

    [Header("Count Down Numbers")]
    public GameObject oneObj;
    public GameObject twoObj;
    public GameObject threeObj;

    [Header("Audio")]
    public AudioSource startAudio;
    public AudioSource winAudio;
    public AudioSource lostAudio;
    public AudioSource winSoundEffect;
    public AudioSource lostSoundEffect;


    private int curRound;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        } else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        curRound = 1;
        OnTriggerStartMenu(curRound);
    }

    // When end for this level
    public void EndCurLevel(bool isDead)
    {
        if (isDead)
        {
            // if totally lost, lost effect will play!
            if (HealthManager.instance.currentHealth <= 1)
            {
                lostSoundEffect.Play();
            }

            StartCoroutine(FailedThenRestart());
        } else
        {
            // enter into next level
            // if totally win, win effect will play!
            if (curRound > 2)
            {
                winSoundEffect.Play();
            }

            StartCoroutine(startNextLevel());
        }
    }

    
    public void OnTriggerStartMenu(int roundNum)
    {
        curRound = roundNum;
        OpenStartMenu();
        roundText.text = roundNum.ToString();
    }

    public void OnTriggerCountDown()
    {
        startAudio.Play();
        OpenCountdown();
        StartCoroutine(Countdown());
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerRestartMenu()
    {
        lostAudio.Play();
        OpenRestartMenu();
    }

    private void OnTriggerCongratsPanel()
    {
        winAudio.Play();
        OpenCongratsPanel();
    }

    private void ResetToOrigin()
    {
        float rotationAngleY = playerHead.transform.rotation.eulerAngles.y - resetTransform.transform.eulerAngles.y;
        XROrigin.transform.Rotate(0, -rotationAngleY, 0);

        Vector3 distanceDiff = resetTransform.position - playerHead.transform.position;
        XROrigin.transform.position += distanceDiff;
    }

    private void OpenCanvas()
    {
        canvas.SetActive(true);
    }

    private void CloseCanvas()
    {
        canvas.SetActive(false);
    }

    private void OpenStartMenu()
    {
        OpenCanvas();
        startMenuPanel.SetActive(true);
        restartPanel.SetActive(false);
        congratsPanel.SetActive(false);
    }

    private void OpenCountdown()
    {
        //OpenCanvas();
        startMenuPanel.SetActive(false);
        restartPanel.SetActive(false);
        congratsPanel.SetActive(false);
    }

    private void OpenCongratsPanel()
    {
        OpenCanvas();
        startMenuPanel.SetActive(false);
        restartPanel.SetActive(false);
        congratsPanel.SetActive(true);
    }

    private void OpenRestartMenu()
    {
        OpenCanvas();
        startMenuPanel.SetActive(false);
        restartPanel.SetActive(true);
    }

    IEnumerator Countdown()
    {
        // the process of count down numbers showing
        DisableAllEnemies();
        CloseCanvas();

        yield return new WaitForSeconds(2);

        threeObj.SetActive(true);

        yield return new WaitForSeconds(1);
        threeObj.SetActive(false);
        twoObj.SetActive(true);

        yield return new WaitForSeconds(1);
        twoObj.SetActive(false);
        oneObj.SetActive(true);

        yield return new WaitForSeconds(1);
        oneObj.SetActive(false);

        // trigger of fishes
        switch (curRound)
        {
            case 1:
                level1Fish.SetActive(true);
                level1Fish.GetComponent<FishEnemy>().OnTriggerStart();
                break;
            case 2:
                level2Fish.SetActive(true);
                level2Fish.GetComponent<FishEnemy>().OnTriggerStart();
                break;
            case 3:
                level3Fish.SetActive(true);
                level3Fish.GetComponent<FishEnemy>().OnTriggerStart();
                break;
            default:
                break;
        }

    }

    IEnumerator FailedThenRestart()
    {
        FadeScreen.instance.FadeOut(2f);

        yield return new WaitForSeconds(2f);
        FadeScreen.instance.FadeIn(3f);

        ResetToOrigin();

        HealthManager.instance.DecreaseHealth();
        // totally lose!
        if (HealthManager.instance.currentHealth <= 0)
        {
            OnTriggerRestartMenu();
        } else
        {
            OnTriggerStartMenu(curRound);
            roundTextInWrist.text = curRound.ToString();
        }
    }

    IEnumerator startNextLevel()
    {
        yield return new WaitForSeconds(2f); // watch die animation of enemy

        FadeScreen.instance.FadeOut(2f);

        yield return new WaitForSeconds(2f);
        FadeScreen.instance.FadeIn(3f);

        ResetToOrigin();

        curRound += 1;
        // totally win!
        if (curRound > 3)
        {
            //curRound = 1;
            OnTriggerCongratsPanel();
        } else
        {
            roundTextInWrist.text = curRound.ToString();
            OnTriggerStartMenu(curRound);
        }
    }

    private void DisableAllEnemies()
    {
        level1Fish.SetActive(false);
        level2Fish.SetActive(false);
        level3Fish.SetActive(false);
    }

}
