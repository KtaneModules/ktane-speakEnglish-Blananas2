using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class speakEnglishScript : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMNeedyModule needyModule;
    public KMAudio Audio;

    public TextMesh screenText;
    public TextMesh[] buttonText;
    public KMSelectable[] buttons;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    public List<string> phrases = new List<string> {
        "Hello", "Привет", "γεια σας", "Bonjour", "Բարեւ", "Hello",
        "Goodbye", "До Свидания", "αντίο", "Au revoir", "ցտեսություն", "Faida",
        "My name is Cody", "Меня Зовут Коди", "Το όνομά μου είναι Κοδι", "Mon nom est cody", "իմ անունը cody է", "Jina langu ni Cody",
        "Death is painful", "Смерть болезненна", "Ο θάνατος είναι επώδυνος", "La mort est douloureuse", "մահը ցավալի է", "Kifo ni chungu",
        "This bomb is going to explode", "эта бомба взорвется", "αυτή η βόμβα θα εκραγεί", "Cette bombe va exploser", "այս ռումբը մտադիր է պայթել", "Bomu hili litakuja",
        "Confusion", "Hеразбериха", "Σύγχυση", "Confusion", "Խառնաշփոթություն", "mkanganyiko",
        "I don't speak English", "Я не говорю по английски", "Δεν μιλώ αγγλικά", "Je ne parle pas anglais", "Ես անգլերեն չեմ խոսում", "Sizungumzi Kiingereza",
        "I hate English", "Я ненавижу английский", "Μισώ τα αγγλικά", "Je déteste l'anglais", "Ես ատում եմ անգլերենը", "Ninapenda Kiingereza",
        "How are you?", "как твои дела", "Πώς είσαι", "Comment vas-tu", "Ինչպես ես", "Habari yako"
    };
    public List<int> setNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
    public List<int> indexNumbers = new List<int> { 1, 2, 3, 4, 5 };
    public List<string> selected = new List<string> { };
    public List<int> order = new List<int> { 0, 1, 2 };
    
    void Awake () {
        moduleId = moduleIdCounter++;
        needyModule = GetComponent<KMNeedyModule>();
        needyModule.OnNeedyActivation += OnNeedyActivation;
        needyModule.OnNeedyDeactivation += OnNeedyDeactivation;
        needyModule.OnTimerExpired += OnTimerExpired;

        foreach (KMSelectable button in buttons) {
            KMSelectable pressedButton = button;
            button.OnInteract += delegate () { buttonPress(pressedButton); return false; };
        }   
    }

    // Use this for initialization
    void Start () {
        moduleSolved = true;
    }

    void OnNeedyActivation()
    {
        moduleSolved = false;
        selected.Clear();
        setNumbers.Shuffle();
        indexNumbers.Shuffle();
        screenText.text = phrases[6*setNumbers[0]];
        selected.Add(phrases[6*setNumbers[0] + indexNumbers[0]]);
        selected.Add(phrases[6 * setNumbers[1] + indexNumbers[1]]);
        selected.Add(phrases[6 * setNumbers[2] + indexNumbers[2]]);
        order.Shuffle();
        buttonText[0].text = selected[order[0]];
        buttonText[1].text = selected[order[1]];
        buttonText[2].text = selected[order[2]];
        Debug.LogFormat("[Speak English #{0}] Display says: \'{1}\'", moduleId, phrases[6 * setNumbers[0]]);
        Debug.LogFormat("[Speak English #{0}] Button 1 says: \'{1}\'", moduleId, selected[order[0]]);
        Debug.LogFormat("[Speak English #{0}] Button 2 says: \'{1}\'", moduleId, selected[order[1]]);
        Debug.LogFormat("[Speak English #{0}] Button 3 says: \'{1}\'", moduleId, selected[order[2]]);
        if (order[0] == 0)
        {
            Debug.LogFormat("[Speak English #{0}] The correct answer is Button 1 (\'{1}\')", moduleId, selected[order[0]]);
        } else if (order[1] == 0)
        {
            Debug.LogFormat("[Speak English #{0}] The correct answer is Button 2 (\'{1}\')", moduleId, selected[order[1]]);
        } else if (order[2] == 0)
        {
            Debug.LogFormat("[Speak English #{0}] The correct answer is Button 3 (\'{1}\')", moduleId, selected[order[2]]);
        }
    }

    void OnNeedyDeactivation()
    {
        moduleSolved = true;
        screenText.text = "speak english";
        buttonText[0].text = "-";
        buttonText[1].text = "-";
        buttonText[2].text = "-";
    }

    void OnTimerExpired()
    {
        needyModule.HandleStrike();
        moduleSolved = true;
        screenText.text = "speak english";
        buttonText[0].text = "-";
        buttonText[1].text = "-";
        buttonText[2].text = "-";
    }

    void buttonPress (KMSelectable button)
    {
        button.AddInteractionPunch();
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        if (button == buttons[0]) {
            if (order[0] != 0)
            {
                needyModule.HandleStrike();
                Debug.LogFormat("[Speak English #{0}] Button 1 pressed, which was incorrect. Needy striked.", moduleId);
            } else
            {
                Debug.LogFormat("[Speak English #{0}] Button 1 pressed, which was correct. Needy deactivated.", moduleId);
            }
        } else if (button == buttons[1]) {
            if (order[1] != 0)
            {
                needyModule.HandleStrike();
                Debug.LogFormat("[Speak English #{0}] Button 2 pressed, which was incorrect. Needy striked.", moduleId);
            } else
            {
                Debug.LogFormat("[Speak English #{0}] Button 2 pressed, which was correct. Needy deactivated.", moduleId);
            }
        } else if (button == buttons[2])
        {
            if (order[2] != 0)
            {
                needyModule.HandleStrike();
                Debug.LogFormat("[Speak English #{0}] Button 3 pressed, which was incorrect. Needy striked.", moduleId);
            } else
            {
                Debug.LogFormat("[Speak English #{0}] Button 3 pressed, which was correct. Needy deactivated.", moduleId);
            }
        }
        needyModule.HandlePass();
        Debug.LogFormat("[Speak English #{0}] ===", moduleId);
    }
}
