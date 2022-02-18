using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueManager
{
    private static Dictionary<string, DialogueData> activeDialogue = new Dictionary<string, DialogueData>();
    private static Dictionary<string, DialogueData> universalDialogue = new Dictionary<string, DialogueData>();

    private static ArrayList registry = new ArrayList();

    private static void DialogueRegistry()
    {
        // Reasonably, since you're just adding more dialogue, you should only ever
        // need to add it to this registry. Only touch other things if DialogueData
        // itself needs to be updated.

        // The string should be the filepath to the dialogue CSV,
        // and the object is the dialogue holder.

        registry.Add("text/postgame");
    }

    private static void loadDialogueByFilepathToActive(string filepath)
    {
        if(registry.Count <= 0)
        {
            DialogueRegistry();
            loadUniversalDialogue();
        }
        if(registry.Contains(filepath))
        {
            activeDialogue.Clear();

            TextAsset dialogue = Resources.Load<TextAsset>(filepath);

            string[] data = dialogue.text.Split(new char[] { '\n' });

            for (int i = 1; i < data.Length - 1; i++)
            {
                string[] row = data[i].Split(new char[] { ',' });

                int tempTest;
                if (int.TryParse(row[0], out tempTest) && row.Length >= 6)
                {
                    DialogueData dData = new DialogueData(row[2], row[3], row[4], row[5]);
                    activeDialogue.Add(row[1], dData);
                }
            }
        }
        else
        {
            Debug.LogError("Invalid Filepath or Dialogue not in Registry");
        }
    }

    private static void loadUniversalDialogue()
    {
        universalDialogue.Clear();

        TextAsset dialogue = Resources.Load<TextAsset>("text/universal");

        string[] data = dialogue.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            int tempTest;
            if (int.TryParse(row[0], out tempTest) && row.Length >= 6)
            {
                DialogueData dData = new DialogueData(row[2], row[3], row[4], row[5]);
                universalDialogue.Add(row[1], dData);
            }
        }
    }

    public static DialogueData getDialogueByID(string s)
    {
        if (activeDialogue.ContainsKey(s))
        {
            return activeDialogue[s];
        }
        else if (universalDialogue.ContainsKey(s))
        {
            return universalDialogue[s];
        }
        else
        {
            Debug.LogError("No such Dialogue ID found. Did you make a typo?");
            return new DialogueData(s, "none", "top", "left");
        }
    }
}

