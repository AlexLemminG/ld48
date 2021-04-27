using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextPrinter {
    TMP_Text mesh;
    string text;
    float totalTime = 0f;
    public float timePerChar = 0.0333f;

    public TextPrinter (TMP_Text mesh, string text) {
        this.mesh = mesh;
        this.text = text;
        mesh.text = "";
    }
    public void Update (float deltaTime) {
        totalTime += deltaTime;
        mesh.text = text.Substring (0, Mathf.Clamp (Mathf.RoundToInt (totalTime / timePerChar), 0, text.Length));
    }
    public void Finish () {
        totalTime = 100000;
        Update (0f);
    }
    public bool IsFinished () {
        return mesh.text == text;
    }
}

public class ScrollPanel : MonoBehaviour {
    public GameObject rays;
    public string finalQuote;
    public string finalDesc;
    public List<string> quotes = new List<string> ();
    List<string> unusedQuotes = new List<string> ();

    public TMP_Text deepMesh;
    public TMP_Text quoteMesh;
    public TMP_Text scrollMesh;
    public TMP_Text continueMesh;

    string continueMessage = "press \"space\" to continue";
    string continueDeathMessage = "press \"space\" to restart";
    public string deathQuote = "death is opposite of life, but it's part of it";

    bool isDeath = false;
    public void Show (string message, bool isFinal = false, bool isDeath = false, bool notDeepEnough = false, string quoteOverride = "") {
        Time.timeScale = 0f;
        this.isFinal = isFinal;
        this.isDeath = isDeath;

        showTime = Time.unscaledTime;

        scrollPanel.transform.localPosition = new Vector3 (0f, scrollInitialOffset, 0f);
        background.color = Color.clear;

        quotePrinter = new TextPrinter (quoteMesh, quoteOverride == "" ? PopRandomQuote () : quoteOverride);
        scrollPrinter = new TextPrinter (scrollMesh, message);
        continuePrinter = new TextPrinter (continueMesh, isFinal ? "" : isDeath?continueDeathMessage : continueMessage);

        gameObject.SetActive (true);
        rays.SetActive (isFinal);
        deepMesh.text = isFinal ? "#deepest" : notDeepEnough ? "#notdeep" : "#deep";
    }
    bool isFinal = false;
    TextPrinter quotePrinter;
    TextPrinter scrollPrinter;
    TextPrinter continuePrinter;

    [EditorButton]
    public void ShowDeath () {
        Show ("", false, true);
    }

    [EditorButton]
    public void ShowFinal () {
        Show (finalDesc, true);
    }

    [EditorButton]
    void DbgShow () {
        Show ("desg fdgdf df g");
    }

    string PopRandomQuote () {
        if (isFinal) {
            return finalQuote;
        }
        if (isDeath) {
            return deathQuote;
        }
        if (unusedQuotes.Count == 0) {
            unusedQuotes.AddRange (quotes);
        }
        int idx = Random.Range (0, unusedQuotes.Count);
        string quote = unusedQuotes[idx];
        unusedQuotes.RemoveAt (idx);
        return quote;
    }

    void Hide () {
        Time.timeScale = 1f;
        gameObject.SetActive (false);
    }

    public Image background;
    public Color backgroundColor = new Color (0f, 0f, 0f, 0.5f);
    float backgroundFadeSpeed = 1f;
    float showTime;
    public float scrollInitialOffset = 40f;
    public float scrollPositionSpeed = 5f;
    public GameObject scrollPanel;

    float delayBetweenMessages = 0.3f;
    float timeQuoteFinished;
    float timeScrollFinished;

    void Update () {
        background.color = Color.Lerp (Color.clear, backgroundColor, (Time.unscaledTime - showTime) * backgroundFadeSpeed);
        scrollPanel.transform.localPosition = Vector3.Lerp (scrollPanel.transform.localPosition, new Vector3 (0, 0, 0), Time.unscaledDeltaTime * scrollPositionSpeed);

        if (Time.unscaledTime - showTime < 0.5f) {
            return;
        }

        bool isShownAll = continuePrinter.IsFinished ();
        if (!isShownAll && Input.GetButtonDown ("Jump")) {
            continuePrinter.Finish ();
            quotePrinter.Finish ();
            scrollPrinter.Finish ();
            return;
        }

        if (!quotePrinter.IsFinished ()) {
            quotePrinter.Update (Time.unscaledDeltaTime);
            timeQuoteFinished = Time.unscaledTime;
            return;
        }

        if (!scrollPrinter.IsFinished ()) {
            if (Time.unscaledTime - timeQuoteFinished > delayBetweenMessages) {
                scrollPrinter.Update (Time.unscaledDeltaTime);
                timeScrollFinished = Time.unscaledTime;
            }
            return;
        }

        if (!continuePrinter.IsFinished ()) {
            if (Time.unscaledTime - timeScrollFinished > delayBetweenMessages) {
                continuePrinter.Update (Time.unscaledDeltaTime);
            }
            return;
        }

        if (Input.GetButtonDown ("Jump") && !isFinal) {
            Hide ();
            if (isDeath) {
                Session.instance.NewGame ();
            }
        }
    }
}