using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData
{
    private string text_en;
    private string portrait_id;
    private string portrait_pos;
    private string position;

    public DialogueData(string text_en, string portrait_id, string position, string portrait_pos)
    {
        this.text_en = text_en;
        this.portrait_id = portrait_id;
        this.portrait_pos = portrait_pos;
        this.position = position;
    }

    public string getTextByKey(EnumDialogueKey key)
    {
        switch(key)
        {
            case EnumDialogueKey.EN:
                if (text_en != null) return text_en;
                else return "No EN Translation for this text.";
            default:
                return "Text Not Found Error";
        }
    }

    public string getPortraitID()
    {
        // This should eventually not return a string, but the actual portrait to use.
        return portrait_id;
    }

    public string getPortraitPosition()
    {
        // Probably return a Vector2 (or maybe just x-coord?)
        return portrait_pos;
    }

    public string getPosition()
    {
        // It'd sure be nice to do all the fancy math for position here for the basic types :)
        return position;
    }

    public void setStringEN(string string_en)
    {
        text_en = string_en;
    }

    public void setPortraitID(string portraitID)
    {
        portrait_id = portraitID;
    }

    public void setPortraitPos(string portraitPos)
    {
        portrait_pos = portraitPos;
    }

    public void setPosition(string pos)
    {
        position = pos;
    }
}
