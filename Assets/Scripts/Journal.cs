using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Journal : MonoBehaviour
{
    [HideInInspector]
    public bool hasUnreadEntries;

    public JournalEntryList journalEntries = new JournalEntryList();

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
    }
    

    // Start is called before the first frame update
    void Start()
    {
        journalEntries = JsonUtility.FromJson<JournalEntryList>(textJSON.text);

        hasUnreadEntries = false;

        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!journalEntries.journalEntries[0].entryInJournal && player.tilesGenerated > 10){
            journalText.text += "\n" + journalEntries.journalEntries[0].text;
            journalEntries.journalEntries[0].entryInJournal = true;
        }
    }
}