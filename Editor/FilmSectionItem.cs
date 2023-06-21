using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilmSectionItem : MonoBehaviour
{
    public FilmSectionItemInfo info;
    public string GetFilmSectionInfo()
    {
        string result = "";
        result = Newtonsoft.Json.JsonConvert.SerializeObject(info);
        return result;
    }
}

[System.Serializable]
public class FilmSectionItemInfo
{
    public string InteractionType;
    public string InteractionInfo;
    public string GroupName1;
    public string GroupName2;
    public string VideoUUID;
}