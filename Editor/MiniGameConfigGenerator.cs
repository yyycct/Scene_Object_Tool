using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
namespace SceneObjectTool
{
    public class MiniGameConfigGenerator : MonoBehaviour
    {
        public enum GameType
        {
            Dart = 0
        }
        public string GameUUID;
        public string GameCatID;
        public GameType type;
        [HideInInspector]
        public DartVariable dartVariable;
        public Dictionary<string, object> GenerateConfig()
        {
            Dictionary<string, object> value = new Dictionary<string, object>();
            value.Add("gameUUID", GameUUID);
            value.Add("gameCatID", GameCatID);
            value.Add("gameType", type);
            value.Add("position", new SerializableVector3(gameObject.transform.position));
            value.Add("rotation", new SerializableVector3(gameObject.transform.eulerAngles));
            switch (type)
            {
                case GameType.Dart:
                    value.Add("variable", dartVariable);
                    break;
            }
            return value;
        }

    }
    [CustomEditor(typeof(MiniGameConfigGenerator))]
    public class MiniGameConfigGeneratorEditer : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var generator = target as MiniGameConfigGenerator;
            switch (generator.type)
            {
                case MiniGameConfigGenerator.GameType.Dart:
                    GUILayout.Label("Dart Mini Game Variable");
                    EditorGUI.indentLevel++;
                    GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Dart Amount");
                    generator.dartVariable.DartAmount = EditorGUILayout.IntField(generator.dartVariable.DartAmount);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Max Speed");
                    generator.dartVariable.MaxSpeed = EditorGUILayout.FloatField(generator.dartVariable.MaxSpeed);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Min Speed");
                    generator.dartVariable.MinSpeed = EditorGUILayout.FloatField(generator.dartVariable.MinSpeed);
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    EditorGUI.indentLevel--;
                    break;
                default:
                    break;
            }
           


        }
    }

[System.Serializable]
    public class DartVariable
    {
        public int DartAmount;
        public float MaxSpeed;
        public float MinSpeed;
    }
}
#endif