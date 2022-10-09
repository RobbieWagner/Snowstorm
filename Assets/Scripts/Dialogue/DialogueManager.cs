using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{

    //Manages dialogue for the game

    public TextAsset textJSON;

    //Dialogue class stores a list of sentences and possible choices
    [System.Serializable]
    public class Dialogue
    {
        public int dialogueID;
        public Sentence[] sentences;
        public StrongChoice[] strongChoice;
    }

    //Sentence class holds properties for the person speaking, text, and choices
    [System.Serializable]
    public class Sentence
    {
        public int textID;
        public string text;
        public WeakChoice[] weakChoice;
        public string speaker;
        public Impact[] gameImpact;
    }

    //WeakChoice class, stores potential outcomes to choices made
    [System.Serializable]
    public class WeakChoice
    {
        public string choiceText;
        public int nextTextID;
    }

    //StrongChoice class, stores potential lasting impacts to choices made
    [System.Serializable]
    public class StrongChoice
    {
        public string choiceText;
        public int nextDialogueID;
        public string lastingImpact;
    }

    //Impact class. Keeps track of possible branching paths.
    [System.Serializable]
    public class Impact
    {
        public string impact;
        public int nextTextID;
    }

    public Dialogue dialogueToDisplay = new Dialogue();

    private List<Sentence> sentences;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Canvas textBoxC;
    public GameObject textBox;
    public GameObject weakChoiceButton;
    public GameObject strongChoiceButton;

    GameObject[] buttons;
    TextMeshProUGUI[] buttonsText;

    int currentScene;
    int currentDialogue;
    int currentSentence;

    List<string> lastingImpacts;

    [SerializeField]
    private Player player;
    private Movement playerM;
    [SerializeField]
    private ColdMeter gameColdMeter;
    bool coldMeterWasDepleting;

    void Start()
    {
        currentScene = 0;
        dialogueToDisplay = JsonUtility.FromJson<Dialogue>(textJSON.text);
        sentences = new List<Sentence>();
        buttons = new GameObject[1];
        buttonsText = new TextMeshProUGUI[1];

        lastingImpacts = new List<string>();

        player = GameObject.Find("Player").GetComponent<Player>();
        playerM = player.GetComponent<Movement>();
        gameColdMeter = GameObject.Find("ColdMeterCanvas").transform.Find("ColdMeter").GetComponent<ColdMeter>();

        StartDialogue(dialogueToDisplay);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        coldMeterWasDepleting = gameColdMeter.depleting;
        gameColdMeter.depleting = false;
        playerM.canMove = false;
        
        sentences.Clear();
        foreach(GameObject button in buttons) if(button != null) Destroy(button);

        foreach(Sentence sentence in dialogue.sentences)
        {
            sentences.Add(sentence);
        }

        DisplayNextSentence(0);
    }

    public void DisplayNextSentence(int sentenceID)
    {

        currentSentence = sentenceID;

        foreach(GameObject button in buttons) if(button != null) Destroy(button);
        if(sentenceID >= sentences.Count)
        {
            dialogueText.text = "";
            GrantStrongChoice(dialogueToDisplay.strongChoice.Length, 
                                dialogueToDisplay.strongChoice);
            return;
        }

        Sentence sentence = sentences[sentenceID];
        nameText.text = sentence.speaker;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        
    }

    public void GrantWeakChoice(int numberOfChoices, WeakChoice[] weakChoice)
    {
        foreach(GameObject button in buttons) if(button != null) Destroy(button);
        buttons = new GameObject[numberOfChoices];
        buttonsText = new TextMeshProUGUI[numberOfChoices];
        for(int i = 0; i < numberOfChoices; i++)
        {
            buttons[i] = Instantiate(weakChoiceButton) as GameObject;
            buttons[i].transform.SetParent(textBox.transform, false);
            buttons[i].transform.position = new Vector3(Screen.width, 20*i, 0);
            Button button = buttons[i].GetComponent<Button>();
            buttonsText[i] = buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            buttonsText[i].text = weakChoice[i].choiceText;

            int weakChoiceID = i;
            if(weakChoice[weakChoiceID].nextTextID != 0) button.onClick.AddListener(delegate{DisplayNextSentence(weakChoice[weakChoiceID].nextTextID);});
            else if(sentences[currentSentence].gameImpact.Length > 0)
            {
                foreach(Impact impact in sentences[currentSentence].gameImpact)
                {
                    if(lastingImpacts.Contains(impact.impact)) button.onClick.AddListener(delegate{DisplayNextSentence(impact.nextTextID);});
                }
            }
        }

        if(buttons.Length > 0 && buttons[0] != null)
        EventSystem.current.SetSelectedGameObject(buttons[0]);
    }

    void GrantStrongChoice(int numberOfChoices, StrongChoice[] strongChoice)
    {
        if(numberOfChoices == 1 && strongChoice[0].choiceText.Equals(""))
        {
            EndDialogue();
        }

        foreach(GameObject button in buttons) if(button != null) Destroy(button);
        buttons = new GameObject[numberOfChoices];
        buttonsText = new TextMeshProUGUI[numberOfChoices];
        for(int i = 0; i < numberOfChoices; i++)
        {
            buttons[i] = Instantiate(strongChoiceButton) as GameObject;

            Button button = buttons[i].GetComponent<Button>();
            buttons[i].transform.SetParent(textBoxC.gameObject.transform, false);
            float numberOfChoicesF = (float) numberOfChoices;
            buttons[i].transform.position = new Vector3(Screen.width/2 + ((i - ((numberOfChoicesF-1)/2)) * (Screen.width/4 + 5)), Screen.height/3 * 2, 0);
            buttonsText[i] = buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            buttonsText[i].text = strongChoice[i].choiceText;
            int strongChoiceID = i;
            button.onClick.AddListener(delegate{StrongChoiceMade(strongChoice[strongChoiceID].nextDialogueID, strongChoice[strongChoiceID].lastingImpact);});
        }

    }

    public void StrongChoiceMade(int nextDialogueID, string lastingImpact)
    {
        if (lastingImpact != null) lastingImpacts.Add(lastingImpact);
    }

    private void EndDialogue()
    {
        if(coldMeterWasDepleting) gameColdMeter.depleting = true;
        playerM.canMove = true;
        Destroy(gameObject);
    }

    IEnumerator TypeSentence(Sentence sentence)
    {
        dialogueText.text = "";
        foreach (char character in sentence.text.ToCharArray())
        {
            dialogueText.text += character;
            yield return new WaitForSeconds(.025f);
        }

        GrantWeakChoice(sentence.weakChoice.Length, sentence.weakChoice);

        StopCoroutine(TypeSentence(sentence));
    }
}