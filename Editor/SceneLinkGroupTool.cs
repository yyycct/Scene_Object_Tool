using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class SceneLinkGroupTool : MonoBehaviour
{
    public Dictionary<string, string> AllSceneLinkGroupInfo = new Dictionary<string, string>();
#if UNITY_EDITOR
    public string GenerateSceneLinkJson()
    {
        string res = string.Empty;
        res = Newtonsoft.Json.JsonConvert.SerializeObject(AllSceneLinkGroupInfo);
        return res;
    }

    public void AddNewSceneLink()
    {
        GameObject scenelinkPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SceneObjectEditorTool/Prefab/SceneLink_1.prefab");

        if (scenelinkPrefab != null)
        {
            GameObject newscenelink = PrefabUtility.InstantiatePrefab(scenelinkPrefab, transform) as GameObject;
            newscenelink.name = transform.childCount.ToString();
        }
        else
        {
            Debug.Log("Unable to find prefab at Assets/SceneObjectEditorTool/Prefab/SceneLink_1.prefab");
        }
    }
#endif
}
#if UNITY_EDITOR
[CustomEditor(typeof(SceneLinkGroupTool))]
public class SceneLinkGroupToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var scenelinkGroup = target as SceneLinkGroupTool;
        if(GUILayout.Button("Add a new Scene Link"))
        {
            scenelinkGroup.AddNewSceneLink();
        }
    }



}
#endif