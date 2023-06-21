using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayfabJson : MonoBehaviour
{
    public string UUID;
    public string itemClass = "internal";
    public string Cat_Version = "CAT_TRIPS";
    public string displayName;
    public string description;
    public Dictionary<string, int> virtualCurrency = new Dictionary<string, int>();
    public Dictionary<string, int> realCurrencyPrice = new Dictionary<string, int>();
    public string[] tags;
    public Dictionary<string, object> playfabJson = new Dictionary<string, object>();

    [Header("Custom Data Attribute")]
    public string catId;
    public string scene_to_load;

    [HideInInspector]
    public string playfabjsonResult;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string GenerateCustumData()
    {
        string res = string.Empty;
        Dictionary<string, object> customdata = new Dictionary<string, object>();
        customdata.Add("3tr_cat_id", catId);
        customdata.Add("3tr_metafile", UUID + "_m");
        customdata.Add("i3m_scene_toload", scene_to_load);
        res = Newtonsoft.Json.JsonConvert.SerializeObject(customdata);

        return res;
    }
    public void GeneratePlayfabJson()
    {
        try { 
        playfabJson.Add("ItemId", UUID);
        playfabJson.Add("ItemClass", itemClass);
        playfabJson.Add("CatalogVersion", Cat_Version);
        playfabJson.Add("DisplayName", displayName);
        playfabJson.Add("Description", description);
        virtualCurrency.Add("CO", 0);
        playfabJson.Add("VirtualCurrencyPrices", virtualCurrency);
        playfabJson.Add("RealCurrencyPrices", realCurrencyPrice);
        playfabJson.Add("Tags", tags);
        playfabJson.Add("CustomData", GenerateCustumData());
        playfabJson.Add("Consumable", new Dictionary<string, object>(){
            {"UsageCount",null },
            {"UsagePeriod",null },
            {"UsagePeriodGroup",null }}
        );
        playfabJson.Add("Container", null);
        playfabJson.Add("Bundle", null);
        playfabJson.Add("CanBecomeCharacter", false);
        playfabJson.Add("IsStackable", false);
        playfabJson.Add("IsTradable", false);
        playfabJson.Add("ItemImageUrl", null);
        playfabJson.Add("IsLimitedEdition", false);
        playfabJson.Add("InitialLimitedEditionCount", 0);
        playfabJson.Add("ActivatedMembership", null);
        }
        catch
        {
            Debug.Log("fail to generate playfab json");
        }
        playfabjsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(playfabJson);
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(PlayfabJson))]
public class PlayfabJsonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var playfabJson = target as PlayfabJson;
        if(GUILayout.Button("Generate Playfab Json")){
            playfabJson.GeneratePlayfabJson();
        }
        GUIStyle textStyle = EditorStyles.label;
        textStyle.wordWrap = true;
        EditorGUILayout.TextArea(playfabJson.playfabjsonResult, textStyle);
    }
}
#endif
