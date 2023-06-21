using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
public class PaintingSetUp : MonoBehaviour
{
    public void ValidatePaintingSetUp()
    {
        Debug.Log("Validation Started");
        if (gameObject.name != "Paintings")
        {
            gameObject.name = "Paintings";
        }
        foreach(Transform painting in gameObject.transform)
        {
            //check if painting name is a uuid;
            GUID uuid;
            if (!GUID.TryParse(painting.name, out uuid)){
                painting.name = GUID.Generate().ToString();
            }
            //check if child contains Picture, Frame
            if (!painting.Find("Picture"))
            {
                Debug.Log($"Painting {painting.name} does not contain Picutre child object");
            }
            if (!painting.Find("Frame"))
            {
                Debug.Log($"Painting {painting.name} does not contain Frame child object");
            }
        }
        Debug.Log("Validation Finished");
    }
}

[CustomEditor(typeof(PaintingSetUp))]
public class PaintingSetUpEditer : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var edit = target as PaintingSetUp;
        EditorGUILayout.LabelField("Click validate to check if painting object name is UUID, will change to UUID if it is not, and check if each painting object have Picture and Frame child object");
        if (GUILayout.Button("Validate Paintings SetUP"))
        {
            edit.ValidatePaintingSetUp();
        }
       

    }
}
#endif