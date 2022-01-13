using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class GameManager : Singleton<GameManager>, IWinObserver, ILoseObserver, ILevelEndObserver
{
    public static GameManager Instance;

    [SerializeField] TextMeshProUGUI levelTMP;
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject succesPanel;
    [SerializeField] GameObject failPanel;
    [SerializeField] GameObject pointPanel;
    [SerializeField] Button nextLevelButton;
    [SerializeField] Button restartButton;
    [SerializeField] TextMeshProUGUI multipliedPointTMP;

    // Level System
    [SerializeField] LevelDefinition[] levels;
    int levelIndex;

    // Coroutines
    bool a, b;

    private void Awake()
    {
        Instance = this;
    }

    #region Game Start Functions
    void Start()
    {
        LevelOperations();
        PreparingPanels();
        AddObserver();

        // Physics.gravity = new Vector3(0, -9.8F, 0);

        // Globals.isLevelEnd = false;
    }
    
    void LevelOperations()
    {
        // PlayerPrefs.SetInt("level", 0);
        levelIndex = PlayerPrefs.GetInt("level");
        levelTMP.enabled = true;
        levelTMP.text = "LEVEL " + (levelIndex + 1).ToString();

        SettingLevelInfo();
    }
    void SettingLevelInfo()
    {
        int currentIndex = levelIndex % levels.Length;
        GameObject _level = Instantiate(levels[currentIndex].levelPrefab, Vector3.zero, Quaternion.identity);
        // InputHandler.Instance.speed = levels[currentIndex].speed;
        // InputHandler.Instance.swipeSpeed = levels[currentIndex].swipeSpeed;
    }
    void PreparingPanels()
    {
        startPanel.SetActive(true);
        succesPanel.SetActive(false);
        failPanel.SetActive(false);
    }
    void AddObserver()
    {
        Observers.Instance.Add_WinObserver(this);
        Observers.Instance.Add_LoseObserver(this);
        Observers.Instance.Add_LevelEndObserver(this);
    }

    #endregion

    #region Level Start Funtions

    public void StartPanel()
    {
        startPanel.SetActive(false);
        Observers.Instance.Notify_LevelStartObservers();
    }
    #endregion

    #region Buttons

    public void FailPanel()
    {
        SceneManager.LoadScene(0);
    }
    public void SuccessPanel()
    {
        SceneManager.LoadScene(0);
    }

    #endregion

    public void WinScenario()
    {
        // Confeeties maybe;
    }

    public void LoseScenario()
    {
        failPanel.SetActive(true);
    }

    public void LevelEnd()
    {
        levelIndex++;
        PlayerPrefs.SetInt("level", levelIndex);
        Invoke("SuccesPanelActivate", 1);
    }
    void SuccesPanelActivate()
    {
        succesPanel.SetActive(true);
        StartCoroutine(PointPanelOperations());
    }
    IEnumerator PointPanelOperations()
    {
        nextLevelButton.interactable = false;
        pointPanel.transform.localScale = Vector3.one * 0.3f;
        yield return new WaitForSeconds(0.2f);
        pointPanel.transform.DOScale(Vector3.one, 1);
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(ShowTotalPoint());
    }

    IEnumerator ShowTotalPoint()
    {
        yield return new WaitForSeconds(0.1f);
        int initPoint = 0;
        int targetPoint = Globals.totalPoint;
        DOTween.To(() => initPoint, x => initPoint = x, targetPoint, 1.2f);
        a = true;
        while (a)
        {
            multipliedPointTMP.text = initPoint.ToString() + "$";
            if (b == false)
            {
                b = true;
                StartCoroutine(NextLevelButtonActivate());
            }
            yield return null;
        }
    }

    IEnumerator NextLevelButtonActivate()
    {
        yield return new WaitForSeconds(2.1f);
        nextLevelButton.interactable = true;
        a = false;
    }
}