using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    //Manages dialogue for the game

    private List<DialogueInteractable.Sentence> sentences;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Canvas textBoxC;
    public GameObject textBox;
    public GameObject weakChoiceButton;
    //public GameObject strongChoiceButton;

    GameObject[] buttons;
    TextMeshProUGUI[] buttonsText;

    int currentSentence;

    [SerializeField]
    private Image blinkIcon;

    //List<string> lastingImpacts;

    [SerializeField]
    private Player player;
    private Movement playerM;
    [SerializeField]
    private ColdMeter gameColdMeter;
    bool coldMeterWasDepleting;

    private bool canMoveOn;
    private bool waitingForPlayerToContinue;

    int nextSentence;

    bool isPressingButton;

    bool dialogueRunning;

    void Start()
    {
        blinkIcon.enabled = false;

        buttons = new GameObject[1];
        buttonsText = new TextMeshProUGUI[1];

        //lastingImpacts = new List<string>();

        playerM = player.GetComponent<Movement>();

        canMoveOn = false;
        waitingForPlayerToContinue = false;

        nextSentence = 0;
        
        isPressingButton = false;

        dialogueRunning = false;
    }

    private void Update() 
    {
        if((Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && canMoveOn && waitingForPlayerToContinue && !isPressingButton)
        {
            isPressingButton = true;
            if(nextSentence < sentences.Count)
            DisplayNextSentence(nextSentence);

            else EndDialogue();
        }    

        if((Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.Space)) && canMoveOn && waitingForPlayerToContinue)
        isPressingButton = false;
    }

    public void StartDialogue(DialogueInteractable.Dialogue dialogue)
    {
        if(!dialogueRunning)
        {
            dialogueRunning = true;
        
            sentences = new List<DialogueInteractable.Sentence>();
            coldMeterWasDepleting = gameColdMeter.depleting;
            gameColdMeter.depleting = false;
            playerM.canMove = false;

            textBoxC.enabled = true;
            
            sentences.Clear();
            foreach(GameObject button in buttons) if(button != null) Destroy(button);

            foreach(DialogueInteractable.Sentence sentence in dialogue.sentences)
            {
                sentences.Add(sentence);
            }

            DisplayNextSentence(0);
        }
    }

    public void DisplayNextSentence(int sentenceID)
    {
        currentSentence = sentenceID;

        foreach(GameObject button in buttons) if(button != null) Destroy(button);
        if(sentenceID >= sentences.Count)
        {
            dialogueText.text = "";
            //GrantStrongChoice(dialogueToDisplay.strongChoice.Length, 
                                //dialogueToDisplay.strongChoice);
            return;
        }

        DialogueInteractable.Sentence sentence = sentences[sentenceID];

        if(sentence.weakChoice.Length == 1)
        {
            nextSentence = sentence.weakChoice[0].nextTextID;
        }

        nameText.text = sentence.speaker;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        
    }

    public void GrantWeakChoice(int numberOfChoices, DialogueInteractable.WeakChoice[] weakChoice)
    {
        foreach(GameObject button in buttons) if(button != null) Destroy(button);
        buttons = new GameObject[numberOfChoices];
        buttonsText = new TextMeshProUGUI[numberOfChoices];
        for(int i = 0; i < numberOfChoices; i++)
        {
            buttons[i] = Instantiate(weakChoiceButton) as GameObject;
            buttons[i].transform.SetParent(textBox.transform, false);
            buttons[i].transform.position = new Vector3(buttons[i].transform.position.x, buttons[i].transform.position.y + 30 * i, buttons[i].transform.position.z);
            Button button = buttons[i].GetComponent<Button>();
            buttonsText[i] = buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            buttonsText[i].text = weakChoice[i].choiceText;

            int weakChoiceID = i;
            if(weakChoice[weakChoiceID].nextTextID != 0) 
            {
                button.onClick.AddListener(delegate{DisplayNextSentence(weakChoice[weakChoiceID].nextTextID);});
                EventSystem.current.SetSelectedGameObject(button.gameObject);
            }
            // else if(sentences[currentSentence].gameImpact.Length > 0)
            // {
            //     foreach(Impact impact in sentences[currentSentence].gameImpact)
            //     {
            //         if(lastingImpacts.Contains(impact.impact)) button.onClick.AddListener(delegate{DisplayNextSentence(impact.nextTextID);});
            //     }
            // }
        }

        if(buttons.Length > 0 && buttons[0] != null)
        EventSystem.current.SetSelectedGameObject(buttons[0]);
    }

    // void GrantStrongChoice(int numberOfChoices, StrongChoice[] strongChoice)
    // {
    //     if(numberOfChoices == 1 && strongChoice[0].choiceText.Equals(""))
    //     {
    //         EndDialogue();
    //     }

    //     foreach(GameObject button in buttons) if(button != null) Destroy(button);
    //     buttons = new GameObject[numberOfChoices];
    //     buttonsText = new TextMeshProUGUI[numberOfChoices];
    //     for(int i = 0; i < numberOfChoices; i++)
    //     {
    //         buttons[i] = Instantiate(strongChoiceButton) as GameObject;

    //         Button button = buttons[i].GetComponent<Button>();
    //         buttons[i].transform.SetParent(textBoxC.gameObject.transform, false);
    //         float numberOfChoicesF = (float) numberOfChoices;
    //         buttons[i].transform.position = new Vector3(Screen.width/2 + ((i - ((numberOfChoicesF-1)/2)) * (Screen.width/4 + 5)), Screen.height/3 * 2, 0);
    //         buttonsText[i] = buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    //         buttonsText[i].text = strongChoice[i].choiceText;
    //         int strongChoiceID = i;
    //         button.onClick.AddListener(delegate{StrongChoiceMade(strongChoice[strongChoiceID].nextDialogueID, strongChoice[strongChoiceID].lastingImpact);});
    //     }

    // }

    // public void StrongChoiceMade(int nextDialogueID, string lastingImpact)
    // {
    //     if (lastingImpact != null) lastingImpacts.Add(lastingImpact);
    // }

    private void EndDialogue()
    {
        if(coldMeterWasDepleting) gameColdMeter.depleting = true;
        playerM.canMove = true;
        dialogueRunning = false;
        textBoxC.enabled = false;
    }

    IEnumerator TypeSentence(DialogueInteractable.Sentence sentence)
    {
        dialogueText.text = "";
        foreach (char character in sentence.text.ToCharArray())
        {
            dialogueText.text += character;
            yield return new WaitForSeconds(.025f);
        }

        if(sentence.weakChoice != null && sentence.weakChoice.Length > 1)
        GrantWeakChoice(sentence.weakChoice.Length, sentence.weakChoice);
        

        canMoveOn = true;

        if(canMoveOn && (sentence.weakChoice == null || sentence.weakChoice.Length <= 1))
        StartCoroutine(BlinkIcon());

        StopCoroutine(TypeSentence(sentence));
    }

    IEnumerator BlinkIcon()
    {
        waitingForPlayerToContinue = true;
        while(waitingForPlayerToContinue) 
        {
            blinkIcon.enabled = true;
            yield return new WaitForSeconds(.5f);
            blinkIcon.enabled = false;
            yield return new WaitForSeconds(.3f);
        }
        waitingForPlayerToContinue = false;

        StopCoroutine(BlinkIcon());
    }
}