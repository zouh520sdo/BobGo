using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Game manager class
/// </summary>

public class GameManager : MonoBehaviour {

    public enum GameState
    {
        prepare,
        started,
        win,
        lose
    }

    public static GameState gameState;

    // Static make this field survive through different scenes
    // use this field to open all levels you already exlored
    public static int exploredLevelIndex = 0;
    public static Dictionary<int, int> highestStars;
    public static Image standardStarGO;

    // Collection of pads
    static List<PadBase> pads;

    // Use to keep tracking touch finger
    static Dictionary<int, PadBase> fingerIdToPad;

    // Collection of launcher
    static List<LauncherBase> launchers;

    // UI recouses
    float timeUntilHint = 10f;
    Animator buttonAnimator;
    public Sprite[] stars;
    public Text countdownText;
    public Image summaryImage;
    public Button pauseButton;
    public Button startButton;
    public Button nextButton;
    public RectTransform summaryPanel;
    Vector3 panelStartPos;
    Vector3 panelEndPos;
    bool isShowing;
    public Sprite[] levelRelated;
    public Image[] levelButtonImages;

    public RectTransform levelSelectionPanel;
    Vector3 levelPanelStartPos;
    Vector3 levelPanelEndPos;
    bool isLevelPanelShowing;
    public RectTransform notImplementPanel;

    // Countdown related
    bool isEnd = false;
    int totalTime = 15;
    float startTime;

    void Awake()
    {
        // Activate level button that you've explored to you
        print("exploredLevelIndex " + exploredLevelIndex);
        Time.timeScale = 1f;
        if (highestStars == null)
            highestStars = new Dictionary<int, int>();
        if (standardStarGO == null)
        {
            standardStarGO = levelButtonImages[0].gameObject.GetComponentsInChildren<Image>()[1];
        }
        // Update Level button images
        for (int i=0; i<levelButtonImages.Length; i++)
        {
            if (!highestStars.ContainsKey(i))
            {
                highestStars[i] = 0;
            }

            // Add star image to Button
            if (levelButtonImages[i].gameObject.GetComponentsInChildren<Image>().Length <= 1)
            {
                Image starGO = Instantiate(standardStarGO);
                starGO.name = "StarImage";
                starGO.rectTransform.SetParent(levelButtonImages[i].rectTransform, false);
            }

            levelButtonImages[i].gameObject.GetComponentsInChildren<Image>()[1].sprite = stars[highestStars[i]];

            levelButtonImages[i].sprite = levelRelated[0];
            if (i <= exploredLevelIndex)
            {
                levelButtonImages[i].gameObject.SetActive(true);
            }
            else
            {
                levelButtonImages[i].gameObject.SetActive(false);
            }
            /*Button button = levelButtonImages[i].GetComponent<Button>();
            button.onClick.AddListener(delegate { LoadLevel(i); });*/
        }
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        levelButtonImages[levelIndex].sprite = levelRelated[1];

        // Animator 
        buttonAnimator = startButton.GetComponent<Animator>();
        StartCoroutine(WatchForHint());
    }

    // Use this for initialization
    void Start() {
        // Initialize game state with prepare state
        gameState = GameState.prepare;

        ////////////////////////
        GameObject[] padObjects = GameObject.FindGameObjectsWithTag("Pad");

        if (pads == null)
            pads = new List<PadBase>();
        pads.Clear();

        foreach (GameObject go in padObjects)
        {
            PadBase pad = go.GetComponent<PadBase>();
            if (!pads.Contains(pad))
                pads.Add(pad);
        }
        /////////////////////////

        //////////////
        GameObject[] launcherObjects = GameObject.FindGameObjectsWithTag("Launcher");

        if (launchers == null)
            launchers = new List<LauncherBase>();
        launchers.Clear();

        foreach (GameObject go in launcherObjects)
        {
            LauncherBase launcher = go.GetComponent<LauncherBase>();
            if (!launchers.Contains(launcher))
                launchers.Add(launcher);
        }
        //////////////

        // UI Initialization
        nextButton.gameObject.SetActive(false);
        summaryImage.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        notImplementPanel.gameObject.SetActive(false);

        if (fingerIdToPad == null)
        {
            fingerIdToPad = new Dictionary<int, PadBase>();
        }
        fingerIdToPad.Clear();

        // Timer
        countdownText.text = totalTime.ToString();

        // Hide menu
        panelStartPos = summaryPanel.position;
        summaryPanel.position += Vector3.left * summaryPanel.position.x * 2;
        panelEndPos = summaryPanel.position;
        isShowing = false;

        levelPanelStartPos = levelSelectionPanel.position;
        levelSelectionPanel.position += Vector3.up * levelSelectionPanel.position.y;
        levelPanelEndPos = levelSelectionPanel.position;
        isLevelPanelShowing = false;
    }

    // Update is called once per frame
    void Update() {

        if (gameState == GameState.started)
        {
            // Lose condition
            if (pads != null && pads.Count == 0 && !isEnd)
            {
                isEnd = true;
                pauseButton.gameObject.SetActive(false);
                summaryImage.gameObject.SetActive(true);
                summaryImage.sprite = stars[0];
                isShowing = true;
                StartCoroutine(StartToShowSummaryPanel());
                gameState = GameState.lose;
            }
        }

        if (gameState != GameState.prepare)
        {
            
        }

        // Handle touching in Mobile device
        foreach (Touch touch in Input.touches)
        {
            // When touch begins
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

                // Choose the closed circle pads to interact
                Collider2D col = null; // = Physics2D.OverlapCircle(touchPos, 1f, 1 << LayerMask.NameToLayer("Pad"));
                Collider2D[] cols = Physics2D.OverlapCircleAll(touchPos, 1f, 1 << LayerMask.NameToLayer("Pad"));
                if (cols != null && cols.Length != 0)
                {
                    if (cols.Length == 1)
                        col = cols[0];
                    else
                    {
                        float minDis = float.PositiveInfinity;
                        int index = 0;
                        for (int i=0; i<cols.Length; i++)
                        {
                            float dis = Vector2.Distance(touchPos, cols[i].transform.position);
                            if ( dis < minDis)
                            {
                                minDis = dis;
                                index = i;
                            }
                        }
                        col = cols[index];
                    }
                }

                if (col && col.tag == "Pad")
                {
                    PadBase pad = col.GetComponent<PadBase>();
                    if (pad && !pad.isSelected)
                    {
                        pad.isSelected = true;
                        fingerIdToPad.Add(touch.fingerId, pad);
                    }
                }
                else
                {

                }
            }

            // When touch released
            if (touch.phase == TouchPhase.Ended)
            {
                if (fingerIdToPad.ContainsKey(touch.fingerId))
                {
                    fingerIdToPad[touch.fingerId].isSelected = false;
                    fingerIdToPad.Remove(touch.fingerId);
                }
            }

            //if (touch.phase == TouchPhase.Moved)
            //{
            if (fingerIdToPad.ContainsKey(touch.fingerId))
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                fingerIdToPad[touch.fingerId].Move(touchPos);
            }
            //}
        }
    }

    public static void RemovePad(PadBase pad)
    {
        pads.Remove(pad);
        Destroy(pad.gameObject);
    }

    IEnumerator WatchForHint()
    {
        float startTime = Time.time;
        while (Time.time - startTime < timeUntilHint)
        {
            yield return null;
        }
        buttonAnimator.SetBool("needHint", true);
    }

    IEnumerator StartCountDown()
    {
        startTime = Time.time;
        while (Time.time - startTime <= totalTime)
        {
            float elapsedTime = Time.time - startTime;
            countdownText.text = ((int)(totalTime - elapsedTime + 0.99f)).ToString();
            yield return null;
        }
        countdownText.text = "0";
        isEnd = true;
        foreach (PadBase pad in pads)
        {
            pad.isInvulnerable = true;
        }
        // Winning condition
        if (pads != null && pads.Count != 0)
        {
            pauseButton.gameObject.SetActive(false);
            isShowing = true;
            nextButton.gameObject.SetActive(true);
            summaryImage.gameObject.SetActive(true);
            summaryImage.sprite = stars[pads.Count];
            int levelIndex = SceneManager.GetActiveScene().buildIndex;
            highestStars[levelIndex] = Mathf.Max(highestStars[levelIndex], pads.Count);
            levelButtonImages[levelIndex].gameObject.GetComponentsInChildren<Image>()[1].sprite = stars[highestStars[levelIndex]];
            StartCoroutine(StartToShowSummaryPanel());
            gameState = GameState.win;

            // Update Level selection panel to open all available level button
            exploredLevelIndex = Mathf.Max(exploredLevelIndex, SceneManager.GetActiveScene().buildIndex + 1);
            for (int i=0; i<levelButtonImages.Length; i++)
            {
                if (i <= exploredLevelIndex)
                {
                    levelButtonImages[i].gameObject.SetActive(true);
                }
                else
                {
                    levelButtonImages[i].gameObject.SetActive(false);
                }
            }
        }
    }

    //////////// Start of Button reatction function /////////////////////
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TriggerMenu()
    {
        if (isShowing)
        {
            Time.timeScale = 1f;
            isShowing = false;
            StartCoroutine(StartToHideSummaryPanel());
            isLevelPanelShowing = false;
            StartCoroutine(StartToHideLevelPanel());
        }
        else
        {
            Time.timeScale = 0f;
            isShowing = true;
            StartCoroutine(StartToShowSummaryPanel());
        }
    }

    public void TriggerLevelSelection()
    {
        if (isLevelPanelShowing)
        {
            isLevelPanelShowing = false;
            StartCoroutine(StartToHideLevelPanel());
        }
        else
        {
            isLevelPanelShowing = true;
            StartCoroutine(StartToShowLevelPanel());
        }
    }

    public void StartGame()
    {
        startButton.gameObject.SetActive(false);
        buttonAnimator.SetBool("needHint", false);
        gameState = GameState.started;

        // Fire all launcher
        foreach (LauncherBase launcher in launchers)
        {
            if (launcher.lasersFire != null)
                launcher.lasersFire();
        }

        // Start counting down
        StartCoroutine(StartCountDown());
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void GoToNextLevel()
    {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        print("SceneManager.sceneCount " + SceneManager.sceneCountInBuildSettings);
        if (SceneManager.sceneCountInBuildSettings <= nextLevelIndex)
        {
            notImplementPanel.gameObject.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
    }
    ///////////// End of Button reaction function //////////////////////////////////

    IEnumerator StartToShowSummaryPanel()
    {
        while (Vector3.Distance(summaryPanel.position, panelStartPos) > 0.00001f && isShowing)
        {
            summaryPanel.position = Vector3.Lerp(summaryPanel.position, panelStartPos, 0.1f);
            yield return null;
        }
        //summaryPanel.position = panelStartPos;
    }

    IEnumerator StartToHideSummaryPanel()
    {
        while (Vector3.Distance(summaryPanel.position, panelEndPos) > 0.00001f && !isShowing)
        {
            summaryPanel.position = Vector3.Lerp(summaryPanel.position, panelEndPos, 0.1f);
            yield return null;
        }
        //summaryPanel.position = panelEndPos;
    }

    IEnumerator StartToShowLevelPanel()
    {
        while (Vector3.Distance(levelSelectionPanel.position, levelPanelStartPos) > 0.00001f && isLevelPanelShowing)
        {
            levelSelectionPanel.position = Vector3.Lerp(levelSelectionPanel.position, levelPanelStartPos, 0.1f);
            yield return null;
        }
    }

    IEnumerator StartToHideLevelPanel()
    {
        while (Vector3.Distance(levelSelectionPanel.position, levelPanelEndPos) > 0.00001f && !isLevelPanelShowing)
        {
            levelSelectionPanel.position = Vector3.Lerp(levelSelectionPanel.position, levelPanelEndPos, 0.1f);
            yield return null;
        }
    }
}