using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
namespace SceneObjectTool
{
    public class SceneObjectGroupEdit : MonoBehaviour
    {
        public int index;
        public string catID;
        public string UUID;
        public void ChangeIndexForAll()
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<SceneObjectTool>() != null)
                {
                    child.gameObject.GetComponent<SceneObjectTool>().ChangeIndex(index);
                }
            }
        }

        public void ChangeCatIDForAll()
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<SceneObjectTool>() != null)
                {
                    child.gameObject.GetComponent<SceneObjectTool>().ChangeAllCatIDAndUUID(catID);
                }
            }
        }
        public void ChangeUUIDForIndex()
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<SceneObjectTool>() != null)
                {
                    child.gameObject.GetComponent<SceneObjectTool>().sceneObjects[index].i3m_object_uuid = UUID;
                }
            }
        }
        public void ReplaceInfoForAll()
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<SceneObjectTool>() != null)
                {
                    child.gameObject.GetComponent<SceneObjectTool>().FindAllChildObejctAndLoadInfo();
                }
            }
        }
        public void AddInfoForAll()
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<SceneObjectTool>() != null)
                {
                    child.gameObject.GetComponent<SceneObjectTool>().sceneObjects.Clear();
                    child.gameObject.GetComponent<SceneObjectTool>().FindAllChildObejctAndLoadInfo();
                }
            }
        }
        public void ToggleAllGrandChildGameObject(bool enable)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<SceneObjectTool>() != null)
                {
                    child.gameObject.GetComponent<SceneObjectTool>().ToggleAllChildObejct(enable);
                }
            }
        }
    }
    [CustomEditor(typeof(SceneObjectGroupEdit))]
    public class SceneObjectGroupEditer : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var edit = target as SceneObjectGroupEdit;
            if (GUILayout.Button("Change Cat ID For All"))
            {
                edit.ChangeCatIDForAll();
            }
            if (GUILayout.Button("Change UUID ID For Index"))
            {
                edit.ChangeUUIDForIndex();
            }
            if (GUILayout.Button("Change Index For All"))
            {
                edit.ChangeIndexForAll();
            }
            if (GUILayout.Button("Replace For All"))
            {
                edit.ReplaceInfoForAll();
            }
            if (GUILayout.Button("Add For All"))
            {
                edit.AddInfoForAll();
            }
            if (GUILayout.Button(new GUIContent("Disable All Child Gameobject", "Use this disable all Child Gameobject under each scene object parent")))
            {
                edit.ToggleAllGrandChildGameObject(false);
            }
            if (GUILayout.Button(new GUIContent("Enable All Child Gameobject", "Use this Enable all Child Gameobject under each scene object parent")))
            {
                edit.ToggleAllGrandChildGameObject(true);
            }
            GUILayout.Label("Child Object Overview");

        }

    }


}
#endif