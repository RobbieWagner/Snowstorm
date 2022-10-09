using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cutscene : MonoBehaviour
{
    [SerializeField]
    private TextAsset textJSON;

    [System.Serializable]
    public class Dialogue
    {
        public Sentence[] sentences;
    }

    [System.Serializable]
    public class Sentence
    {
        public int textID;
        public string text;
        public bool togglesMusic;
        public int imageIndex;
        public bool turnsImageOff;
    }


    [SerializeField]
    private GameObject sceneChanger;

    [SerializeField]
    private Canvas parentCanvas;

    private bool runningDialogueReader;

    [SerializeField]
    private Dialogue dialogue;
    [SerializeField]
    private Sprite[] cutsceneImages;
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI cutsceneText;
    [SerializeField]
    private AudioSource cutsceneMusic;

    private bool displayImageRunning;
    private bool runDisplayImage;
    private bool sentenceReaderRunning;

    private int currentImage;

    public float textDelay;

    [SerializeField]
    private CanvasSwap canvasSwap;

    // Start is called before the first frame update
    void Start()
    {
        runningDialogueReader = false;

        dialogue = JsonUtility.FromJson<Dialogue>(textJSON.text);

        currentImage = 0;

        runDisplayImage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(parentCanvas.enabled && !runningDialogueReader) 
        {
            runningDialogueReader = true;
            StartCoroutine(ReadDialogue());
        }
    }

    IEnumerator ReadDialogue()
    {
        foreach(Sentence sentence in dialogue.sentences)
        {
            yield return StartCoroutine(StartNextSentence(sentence));
            if(sentence.turnsImageOff) yield return new WaitForSeconds(1f);
            else yield return new WaitForSeconds(3f);
        }
        StopCoroutine(ReadDialogue());
        canvasSwap.SwapCanvases();
    }

    IEnumerator StartNextSentence(Sentence sentence)
    {
        if(sentence.togglesMusic && cutsceneMusic.isPlaying) cutsceneMusic.Stop();
        else if(sentence.togglesMusic) cutsceneMusic.Play();

        if(sentence.imageIndex != 0) 
        {
            image.sprite = cutsceneImages[sentence.imageIndex - 1];
            runDisplayImage = true;
        }
        else runDisplayImage = false;

        StartCoroutine(ReadSentence(sentence));
        if(runDisplayImage)StartCoroutine(DisplayImage(image));

        yield return new WaitForSeconds(.3f);

        while(sentenceReaderRunning || displayImageRunning) yield return null;
        
        if(sentence.turnsImageOff) yield return StartCoroutine(HideImage(image));

        StopCoroutine(StartNextSentence(sentence));
    }

    IEnumerator ReadSentence(Sentence sentence)
    {
        sentenceReaderRunning = true;
        cutsceneText.text = "";
        foreach(char character in sentence.text.ToCharArray())
        {
            cutsceneText.text += character;
            yield return new WaitForSeconds(textDelay);
        }
        sentenceReaderRunning = false;
        StopCoroutine(ReadSentence(sentence));
    }

    IEnumerator DisplayImage(Image uiImage)
    {
        displayImageRunning = true;
        while(uiImage.color.a < 1f)
        {
            uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, uiImage.color.a + .1f);
            yield return new WaitForSeconds(.25f);
        }
        displayImageRunning = false;
        StopCoroutine(DisplayImage(uiImage));
    }

    IEnumerator HideImage(Image uiImage)
    {
        while(uiImage.color.a > 0f)
        {
            uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, uiImage.color.a - .1f);
            yield return new WaitForSeconds(.25f);
        }

        StopCoroutine(HideImage(uiImage));
    }

}
