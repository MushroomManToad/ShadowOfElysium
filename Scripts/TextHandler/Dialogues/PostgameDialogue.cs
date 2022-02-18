using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostgameDialogue : MonoBehaviour
{
    Dictionary<string, DialogueData> postgameDialogue_en = new Dictionary<string, DialogueData>();

    // Start is called before the first frame update
    void Start()
    {
        TextAsset postgameDialogue = Resources.Load<TextAsset>("text/postgame");

        string[] data = postgameDialogue.text.Split(new char[] { '\n' });

        for(int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            int tempTest;
            if(int.TryParse(row[0], out tempTest) && row.Length >= 6)
            {
                DialogueData dData = new DialogueData(row[2], row[3], row[4], row[5]);
                postgameDialogue_en.Add(row[1], dData);
            }
        }
    }

    public DialogueData getDialogueByID(string s)
    {
        if(postgameDialogue_en.ContainsKey(s))
        {
            return postgameDialogue_en[s];
        }
        else
        {
            Debug.LogError("No such Dialogue ID found. Did you make a typo?");
            return null;
        }
    }
}
