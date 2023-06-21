using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace SceneObjectTool
{
    public class NPCSceneTool : MonoBehaviour
    {

        // gizmos common infomation
        public Vector3 centerOffset = new Vector3(0f, 0f, 0f);
        public Vector3 NPCSize = new Vector3(0f, 0f, 0f);
        public float arrowSize = 0.2f;
        public NPCInfo npcInfos;
        [HideInInspector]
        public string jsonResult;
        [HideInInspector]
        public string readJson;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.cyan;
            //Draw gizmos for npc
            Gizmos.DrawWireSphere(centerOffset, NPCSize.x);
            Gizmos.DrawWireCube(new Vector3(centerOffset.x, centerOffset.y + NPCSize.y / 2, centerOffset.z), NPCSize);
            Gizmos.DrawWireSphere(new Vector3(centerOffset.x, centerOffset.y + NPCSize.y, centerOffset.z), NPCSize.x);
            //Draw eye indication
            Vector3 arrowStart = new Vector3(centerOffset.x, centerOffset.y + NPCSize.y, centerOffset.z);
            Vector3 arrowEnd = arrowStart + Vector3.forward * arrowSize;
            DrawArrow(arrowStart, arrowEnd, Color.cyan);

        }
        private void DrawArrow(Vector3 arrowStart, Vector3 arrowEnd, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(arrowStart, 0.01f);
            Gizmos.DrawRay(arrowStart, Vector3.forward * arrowSize);
            Vector3 right = Quaternion.LookRotation(Vector3.forward) * Quaternion.Euler(0, 180 + 30, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(Vector3.forward) * Quaternion.Euler(0, 180 - 30, 0) * new Vector3(0, 0, 1);
            Vector3 Bottom = Quaternion.LookRotation(Vector3.forward) * Quaternion.Euler(180 - 30, 0, 0) * new Vector3(0, 0, 1);
            Vector3 Top = Quaternion.LookRotation(Vector3.forward) * Quaternion.Euler(180 + 30, 0, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(arrowEnd, right * arrowSize / 3);
            Gizmos.DrawRay(arrowEnd, left * arrowSize / 3);
            Gizmos.DrawRay(arrowEnd, Top * arrowSize / 3);
            Gizmos.DrawRay(arrowEnd, Bottom * arrowSize / 3);
            Gizmos.DrawSphere(arrowEnd, 0.02f);
            Gizmos.DrawSphere(arrowEnd + right * arrowSize / 3, arrowSize / 30);
            Gizmos.DrawSphere(arrowEnd + left * arrowSize / 3, arrowSize / 30);
            Gizmos.DrawSphere(arrowEnd + Top * arrowSize / 3, arrowSize / 30);
            Gizmos.DrawSphere(arrowEnd + Bottom * arrowSize / 3, arrowSize / 30);
        }
        public string GenerateNpcJson()
        {
            string result = string.Empty;
            result = Newtonsoft.Json.JsonConvert.SerializeObject(npcInfos);
            return result;
        }
        public void ReadNPCJson()
        {
            try
            {
                npcInfos = JsonUtility.FromJson<NPCInfo>(readJson);
            }
            catch
            {
                Debug.Log("fail to generate");
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(NPCSceneTool))]
    public class NPCSceneToolEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var npcTool = target as NPCSceneTool;
            if (GUILayout.Button("Generate"))
            {
                npcTool.jsonResult = npcTool.GenerateNpcJson();
            }
            GUIStyle textStyle = EditorStyles.label;
            textStyle.wordWrap = true;
            GUILayout.TextArea(npcTool.jsonResult, textStyle);
            if (GUILayout.Button("Read Json"))
            {
                npcTool.ReadNPCJson();
            }
            npcTool.readJson = GUILayout.TextField(npcTool.readJson);
        }
    }

#endif
    [System.Serializable]
    public class NPCInfo
    {
        public string i3m_npc_uuid;
        public string i3m_npc_audio_uuid;
        public SerializableVector3 i3m_npc_posoff;
        public SerializableVector3 i3m_npc_rotoff;
        public string i3m_npc_type;
        public string i3m_npc_catid;
        public string i3m_npc_fitness_set;
    }
}
