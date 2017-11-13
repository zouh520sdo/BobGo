using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This is a manager to control when and what dialogs happen
/// </summary>

public class Chitchat : MonoBehaviour {

    private static Chitchat _instance;
    public static Chitchat instance { get { return _instance; } }

    // Dialog manager to three circle pads
    public DialogManager pad1;
    public DialogManager pad2;
    public DialogManager pad3;

    // Pad reference to three pads
    PadBase pPad1;
    PadBase pPad2;
    PadBase pPad3;

    // Collection of all text to be appearred
    public Dictionary<DialogManager, string> names;
    public Dictionary<DialogManager, List<string>> greeting;
    public Dictionary<DialogManager, List<string>> tips;
    public Dictionary<DialogManager, List<string>> progressiveTips;
    public Dictionary<DialogManager, List<string>> start;//
    public Dictionary<DialogManager, List<string>> dead;
    public Dictionary<DialogManager, List<string>> win;
    public Dictionary<DialogManager, List<string>> afterDeath;
    public Dictionary<DialogManager, List<string>> lose;

    // Flag to determine if cirtain catagory's dialog is just appearing to trigger
    // some special dialogs
    bool isFirst;

    // Similar to isFirst flag, but specific to dead catagory for each pads
    Dictionary<DialogManager, bool> isDeadFirst;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        isFirst = true;
        isDeadFirst = new Dictionary<DialogManager, bool>();
        isDeadFirst[pad1] = true;
        isDeadFirst[pad2] = true;
        isDeadFirst[pad3] = true;

        pPad1 = pad1.GetComponent<PadBase>();
        pPad2 = pad2.GetComponent<PadBase>();
        pPad3 = pad3.GetComponent<PadBase>();

        names = new Dictionary<DialogManager, string>();
        names[pad1] = "Bob Alpha";
        names[pad2] = "Bob Beta";
        names[pad3] = "Bob Gamma";

        greeting = new Dictionary<DialogManager, List<string>>();

        greeting[pad1] = new List<string>();
        greeting[pad1].Add("I'm Bob Alpha.");
        greeting[pad1].Add("How do we get here?");
        greeting[pad1].Add("I forget everything.");

        greeting[pad2] = new List<string>();
        greeting[pad2].Add("I'm Bob Beta.");
        greeting[pad2].Add("We are in trouble.");
        greeting[pad2].Add("Need to find a way.");

        greeting[pad3] = new List<string>();
        greeting[pad3].Add("I'm Bob Gamma.");
        greeting[pad3].Add("Are we gonna die here?");
        greeting[pad3].Add("Those things look dangerous.");


        // Tips
        tips = new Dictionary<DialogManager, List<string>>();

        if (SceneManager.GetActiveScene().buildIndex < 4)
        {
            tips[pad1] = new List<string>();
            tips[pad1].Add("I hate laser.");
            tips[pad1].Add("I look pretty in mirror.");
            tips[pad1].Add("Move me.");

            tips[pad2] = new List<string>();
            tips[pad2].Add("Save them first!");
            tips[pad2].Add("I'm fine.");
            tips[pad2].Add("It's better to survive.");

            tips[pad3] = new List<string>();
            tips[pad3].Add("What's happaning here?");
            tips[pad3].Add("Help me!");
            tips[pad3].Add("Don't leave me.");
        }
        else
        {
            tips[pad1] = new List<string>();
            tips[pad1].Add("I hate laser.");
            tips[pad1].Add("We've walked long time.");
            tips[pad1].Add("Do you where is the exit?");

            tips[pad2] = new List<string>();
            tips[pad2].Add("We can find a way out.");
            tips[pad2].Add("We need to stick on it.");
            tips[pad2].Add("Believe me!");

            tips[pad3] = new List<string>();
            tips[pad3].Add("We will be trapped forever.");
            tips[pad3].Add("Help me!");
            tips[pad3].Add("Don't leave me.");
        }

        // Progressive Tips
        progressiveTips = new Dictionary<DialogManager, List<string>>();

        progressiveTips[pad1] = new List<string>();
        progressiveTips[pad1].Add("Ready to press GO.");
        progressiveTips[pad1].Add("There is GO button.");

        progressiveTips[pad2] = new List<string>();
        progressiveTips[pad2].Add("Find a good position to get started.");
        progressiveTips[pad2].Add("Predict laser's trajectory.");

        progressiveTips[pad3] = new List<string>();
        progressiveTips[pad3].Add("It's easier to give up.");
        progressiveTips[pad3].Add("No one can learn so fast.");

        start = new Dictionary<DialogManager, List<string>>();

        start[pad1] = new List<string>();
        start[pad1].Add("What!?");
        start[pad1].Add("It starts!");

        start[pad2] = new List<string>();
        start[pad2].Add("Watch out!");
        start[pad2].Add("Hang on for 15 seconds!");

        start[pad3] = new List<string>();
        start[pad3].Add("!!!");
        start[pad3].Add("Run!");

        dead = new Dictionary<DialogManager, List<string>>();

        dead[pad1] = new List<string>();
        dead[pad1].Add("I hate laser!!!");
        dead[pad1].Add("Oh my pretty dress.");
        dead[pad1].Add("It hurts.");

        dead[pad2] = new List<string>();
        dead[pad2].Add("I will come back.");
        dead[pad2].Add("I can do better.");
        dead[pad2].Add("Save them!");

        dead[pad3] = new List<string>();
        dead[pad3].Add("It's over.");
        dead[pad3].Add("We can't survive.");
        dead[pad3].Add("No way to escape.");

        win = new Dictionary<DialogManager, List<string>>();

        win[pad1] = new List<string>();
        win[pad1].Add("How good we are!");
        win[pad1].Add("Unbelievable!");
        win[pad1].Add("Easy game!");

        win[pad2] = new List<string>();
        win[pad2].Add("We did it.");
        win[pad2].Add("Let's prepare for next level.");
        win[pad2].Add("Nice work.");

        win[pad3] = new List<string>();
        win[pad3].Add("I'm good.");
        win[pad3].Add("I'm afraid of next level.");
        win[pad3].Add("Thank you.");

        // After Death
        afterDeath = new Dictionary<DialogManager, List<string>>();

        afterDeath[pad1] = new List<string>();
        afterDeath[pad1].Add("I can still talk.");
        afterDeath[pad1].Add("Come on! Gamma.");
        afterDeath[pad1].Add("you can do it!");

        afterDeath[pad2] = new List<string>();
        afterDeath[pad2].Add("I wish I could help");
        afterDeath[pad2].Add("Be careful to the reflaction.");
        afterDeath[pad2].Add("Hide behind the mirror.");

        afterDeath[pad3] = new List<string>();
        afterDeath[pad3].Add("We can't survive.");
        afterDeath[pad3].Add("So hard.");
        afterDeath[pad3].Add("I'm tired.");

        // Lose
        lose = new Dictionary<DialogManager, List<string>>();

        lose[pad1] = new List<string>();
        lose[pad1].Add("Now we all dead body.");
        lose[pad1].Add("Let's give another try.");
        lose[pad1].Add("We can do better.");

        lose[pad2] = new List<string>();
        lose[pad2].Add("Mirror is good place to hide.");
        lose[pad2].Add("We can do better.");
        lose[pad2].Add("We should learn something.");

        lose[pad3] = new List<string>();
        lose[pad3].Add("I don't want more.");
        lose[pad3].Add("This is so hard.");
        lose[pad3].Add("I don't think ...");

        // Deactive UI first
        // Hide Text boxes first first
        pad1.dialog.gameObject.SetActive(false);
        pad2.dialog.gameObject.SetActive(false);
        pad3.dialog.gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        // Start greeting first 
        say(pad1, greeting, Random.Range(1f, 2f), 4f, true);
        say(pad2, greeting, Random.Range(1f, 2f), 4f, true);
        say(pad3, greeting, Random.Range(1f, 2f), 4f, true);
    } 

    // Update is called once per frame
    void Update () {
        if (GameManager.gameState == GameManager.GameState.prepare)
        {
            StartCoroutine(prepareSay());
        }
        else if (GameManager.gameState == GameManager.GameState.lose)
        {
            StartCoroutine(loseSay());
        }
        else if (GameManager.gameState == GameManager.GameState.win)
        {
            StartCoroutine(winSay());
        }
        else
        {
            if (pPad1)
                say(pad1, tips, Random.Range(5f, 15f), 5f, false);
            else
                StartCoroutine(deadSay(pad1));

            if (pPad2)
                say(pad2, tips, Random.Range(8f, 20f), 5f, false);
            else
                StartCoroutine(deadSay(pad2));

            if (pPad3)
                say(pad3, tips, Random.Range(8f, 25f), 5f, false);
            else
                StartCoroutine(deadSay(pad3));
        }
    }

    public void say(DialogManager who, Dictionary<DialogManager, List<string>> catagory, float predelay, float delay, bool bFlush)
    {
        StartCoroutine(_say(who, catagory, predelay, delay, bFlush));
    }

    IEnumerator _say(DialogManager who, Dictionary<DialogManager, List<string>> catagory, float predelay, float delay, bool bFlush)
    {
        if (bFlush)
        {
            who.bInFlush = true;
            who.flushIndex++;
            int flushIndex = who.flushIndex;
            //yield return null;
            float timestamp = Time.time;
            while (Time.time - timestamp <= predelay && flushIndex >= who.flushIndex)
            {
                yield return null;
            }
            if (flushIndex >= who.flushIndex)
            {
                int index = Random.Range(0, catagory[who].Count);
                who.showDialog(catagory[who][index]);
            }
            timestamp = Time.time;
            while (Time.time - timestamp <= delay && flushIndex >= who.flushIndex)
            {
                yield return null;
            }
            if (flushIndex >= who.flushIndex)
            {
                who.hideDialog();
                who.bInFlush = false;
            }
        }

        if (!who.bInProcess)
        {
            who.bInProcess = true;
            float timestamp = Time.time;
            while (Time.time - timestamp <= predelay && !who.bInFlush)
            {
                yield return null;
            }
            if (!who.bInFlush)
            {
                int index = Random.Range(0, catagory[who].Count);
                who.showDialog(catagory[who][index]);
            }
            timestamp = Time.time;
            while (Time.time - timestamp <= delay && !who.bInFlush)
            {
                yield return null;
            }
            if (!who.bInFlush)
                who.hideDialog();
            who.bInProcess = false;
        }
    }

    IEnumerator prepareSay()
    {
        if (isFirst)
        {
            isFirst = false;

            float startTime = Time.time;
            while (Time.time - startTime < 7.5f && GameManager.gameState == GameManager.GameState.prepare) 
            {
                say(pad1, tips, Random.Range(5f, 13f), Random.Range(4f, 6f), false);
                say(pad2, tips, Random.Range(8f, 13f), Random.Range(4f, 7f), false);
                say(pad3, tips, Random.Range(10f, 15f), Random.Range(5f, 7f), false);
                yield return null;
            }

            startTime = Time.time;
            while (GameManager.gameState == GameManager.GameState.prepare)
            {
                say(pad1, progressiveTips, Random.Range(5f, 10f), Random.Range(4f, 6f), false);
                say(pad2, progressiveTips, Random.Range(8f, 13f), Random.Range(4f, 7f), false);
                say(pad3, progressiveTips, Random.Range(10f, 20f), Random.Range(5f, 7f), false);
                yield return null;
            }

            isFirst = true;
        }
    }

    IEnumerator loseSay()
    {
        if (isFirst)
        {
            isFirst = false;

            say(pad1, lose, 0.1f, 5f, true);
            say(pad2, lose, 0.2f, 5f, true);
            say(pad3, lose, 0.3f, 5f, true);

            while (GameManager.gameState == GameManager.GameState.lose)
            {
                say(pad1, lose, Random.Range(5f, 15f), 5f, false);
                say(pad2, lose, Random.Range(8f, 15f), 5f, false);
                say(pad3, lose, Random.Range(10f, 20f), 5f, false);
                yield return null;
            }

            isFirst = true;
        }
    }

    IEnumerator winSay()
    {
        if (isFirst)
        {
            isFirst = false;

            say(pad1, win, 0.1f, 5f, true);
            say(pad2, win, 0.2f, 5f, true);
            say(pad3, win, 0.3f, 5f, true);

            while (GameManager.gameState == GameManager.GameState.win)
            {
                say(pad1, win, Random.Range(5f, 15f), 5f, false);
                say(pad2, win, Random.Range(8f, 15f), 5f, false);
                say(pad3, win, Random.Range(10f, 20f), 5f, false);
                yield return null;
            }

            isFirst = true;
        }
    }

    IEnumerator deadSay(DialogManager who)
    {
        if (isDeadFirst[who])
        {
            isDeadFirst[who] = false;

            say(who, dead, 0.1f, 5f, true);

            while (GameManager.gameState == GameManager.GameState.started)
            {
                say(pad1, afterDeath, Random.Range(5f, 15f), 5f, false);
                yield return null;
            }

            isDeadFirst[who] = true;
        }
    }

    // Will be called when "Start" button pressed
    public void sayWhenStart()
    {
        say(pad1, start, 0, 3.5f, true);
        say(pad2, start, 0, 3.5f, true);
        say(pad3, start, 0, 3.5f, true);
    }
}