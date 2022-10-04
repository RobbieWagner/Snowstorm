using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Journal : MonoBehaviour
{
    [SerializeField]
    private Canvas journalCanvas;

    [SerializeField]
    private CanvasSwap journalCS;

    [HideInInspector]
    public bool hasUnreadEntries;

    private JournalEntry blankPage;

    public JournalEntryList journalEntries = new JournalEntryList();
    public List<JournalEntry> entriesInJournal;

    [SerializeField]
    private AudioSource pageFlipSound;

    private bool canFlipPage;
    private bool flippingPage;
    private int currentPage;

    private Player player;

    [SerializeField]
    private TextMeshProUGUI journalText;

    [SerializeField]
    public TextAsset textJSON;

    [System.Serializable]
    public class JournalEntryList
    {
        public JournalEntry[] journalEntries;
    }

    [System.Serializable]
    public class JournalEntry
    {
        public int entryID;
        public string text;
        public bool entryInJournal;
        public int tileRequirement;
        public string requirement;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        journalEntries = JsonUtility.FromJson<JournalEntryList>(textJSON.text);
        entriesInJournal = new List<JournalEntry>();

        hasUnreadEntries = false;

        player = GameObject.Find("Player").GetComponent<Player>();

        currentPage = 0;
        canFlipPage = false;
        flippingPage = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Page adding checks
        //Adds wandering pages to journal
        for(int i = 0; i < journalEntries.journalEntries.Length; i++)
        {
            if(!journalEntries.journalEntries[i].entryInJournal)
            {
                if(journalEntries.journalEntries[i].tileRequirement != 0 && player.tilesGenerated > journalEntries.journalEntries[i].tileRequirement)
                {
                    AddPage(journalEntries.journalEntries[i]);
                }

                //Adds fire lighting page to journal
                else if(journalEntries.journalEntries[i].requirement != null)
                {
                    if(journalEntries.journalEntries[i].requirement.Equals("lightFire") && player.hasSeenFireLightingTutorial && player.hasSeenWarmthTutorial)
                    {
                        AddPage(journalEntries.journalEntries[i]);
                    }
                    if(journalEntries.journalEntries[i].requirement.Equals("enterLogCabin") && player.playerHasEneteredCabin)
                    {
                        StartCoroutine(WaitToAddPage(journalEntries.journalEntries[i]));
                    }
                    if(journalEntries.journalEntries[i].requirement.Equals("enterTown") && player.playerhasEnteredTown)
                    {
                        StartCoroutine(WaitToAddPage(journalEntries.journalEntries[i]));
                    }
                }
            }
        }

        if(!canFlipPage && !flippingPage && journalCanvas.enabled)
        {
            canFlipPage = true;
        }

        //adds the blank page at the end of the journal
        if(entriesInJournal.Count == 0)
        {
            blankPage = new JournalEntry();
            blankPage.entryID = -1;
            blankPage.text = "";
            blankPage.entryInJournal = true;

            entriesInJournal.Insert(0, blankPage);
        }
    }

    private void OnGUI() 
    {
        if(Input.GetKeyDown(KeyCode.Escape) && journalCanvas.enabled)
        {
            journalCS.SwapCanvases();
            hasUnreadEntries = false;
        }

        //handle page turning input
        if(entriesInJournal.Count > 0 && journalCanvas.enabled && canFlipPage && !flippingPage && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)))
        {
            pageFlipSound.Stop();

            if(currentPage == 0)
            {
                flippingPage = true;
                currentPage = entriesInJournal.Count - 1;
                FlipPage(currentPage);
            }
            else 
            {
                flippingPage = true;
                currentPage--;
                FlipPage(currentPage);
            }
        }

        if(entriesInJournal.Count > 0 && journalCanvas.enabled && canFlipPage && !flippingPage && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)))
        {
            pageFlipSound.Stop();

            if(currentPage == entriesInJournal.Count - 1)
            {
                flippingPage = true;
                currentPage = 0;
                FlipPage(currentPage);
            }
            else 
            {
                flippingPage = true;
                currentPage++;
                FlipPage(currentPage);
            }
        }

        if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            flippingPage = false;
        }

    }

    public void AddPage(JournalEntry journalEntry)
    {
        entriesInJournal.Add(journalEntry);
        journalEntry.entryInJournal = true;
        currentPage = entriesInJournal.Count-1;
        hasUnreadEntries = true;
    }

    public void FlipPage(int page)
    {
        //sets the text of the journal to the correct page
        if(page < 0 && entriesInJournal.Count > 0)
        {
            journalText.text = string.Empty;
            journalText.text = "\n" + entriesInJournal[currentPage].text;
        }
        else if(page >= 0)
        {
            journalText.text = string.Empty;
            journalText.text = "\n" + entriesInJournal[page].text;
        }
        pageFlipSound.Play();
    }

    IEnumerator WaitToAddPage(JournalEntry page)
    {
        yield return new WaitForSeconds(1f);
        AddPage(page);
        StopCoroutine(WaitToAddPage(page));
    }
}