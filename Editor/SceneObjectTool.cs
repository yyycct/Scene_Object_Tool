using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace SceneObjectTool
{
    public class SceneObjectTool : MonoBehaviour
    {
        public enum ObjectType
        {
            DefaultCube,
            DefaultSpere,
            Chair,
            Table,
            NPC
        }

        // gizmos common infomation
        [HideInInspector]
        public Vector3 GizmocenterOffset = new Vector3(0f, 0f, 0f);
        [HideInInspector]
        public ObjectType objectType;

        /*    public Vector3 ColliderSize = new Vector3(0f, 0f, 0f);
            public Vector3 ColliderCenter = new Vector3(0f, 0f, 0f);*/
        //default cube gizmos
        [HideInInspector]
        public Vector3 cubesize = new Vector3(1, 1, 1);

        //default sphere gizmos
        [HideInInspector]
        public float spheresize = 1f;

        // gizmos for chair
        [HideInInspector]
        public Vector3 seatArea = new Vector3(0.5f, 0.5f, 0.5f);
        [HideInInspector]
        public float seatBackHeight = 0.5f;
        [HideInInspector]
        public float seatBackWidth = 0.1f;
        [HideInInspector]
        public Vector3 seatDirection = new Vector3(0, 0.2f, 0);

        //gizmos for table
        [HideInInspector]
        public Vector3 tableLeg = new Vector3(0.15f, 0.15f, 0.7f);
        [HideInInspector]
        public Vector3 tableSurface = new Vector3(0.5f, 0.5f, 0.02f);


        //Generating block for configuration file
        [HideInInspector]
        public string itemUUID = null;
        [HideInInspector]
        public string catID = null;
        [HideInInspector]
        public string jsonResult;
        [HideInInspector]
        public string readJsonInput;
        [HideInInspector]
        public string readJsonMessage;

        public int chosenIndex;
        public List<SceneObject> sceneObjects = new List<SceneObject>();
        [HideInInspector]
        public Vector3 MaterialColor = new Vector3(0f, 0f, 0f);

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
            //Chair body
            Gizmos.color = Color.cyan;
            switch (objectType)
            {
                case ObjectType.DefaultCube:
                    Gizmos.DrawWireCube(Vector3.zero + GizmocenterOffset, cubesize);
                    Gizmos.DrawCube(new Vector3(0, 0, cubesize.z / 2) + GizmocenterOffset, new Vector3(cubesize.x, cubesize.y, 0));
                    Gizmos.DrawRay(new Vector3(0, 0, cubesize.z / 2) + GizmocenterOffset, new Vector3(0, 0, cubesize.z / 2 + cubesize.z * 0.3f) + GizmocenterOffset);
                    break;
                case ObjectType.DefaultSpere:
                    Gizmos.DrawWireSphere(Vector3.zero + GizmocenterOffset, spheresize);
                    Gizmos.DrawCube(new Vector3(0, 0, spheresize) + GizmocenterOffset, new Vector3(spheresize, spheresize, 0));
                    Gizmos.DrawRay(new Vector3(0, 0, spheresize) + GizmocenterOffset, new Vector3(0, 0, spheresize + spheresize * 0.3f) + GizmocenterOffset);
                    break;
                case ObjectType.Chair:
                    //Seat of the Chair
                    Gizmos.DrawWireCube(new Vector3(0f, 0f, -seatArea.z / 2) + GizmocenterOffset, seatArea);
                    //Back of the Chair
                    Vector3 backCenter = new Vector3(0f, (seatBackWidth / 2) - seatArea.y / 2, seatBackHeight / 2);
                    Gizmos.DrawWireCube(backCenter + GizmocenterOffset, new Vector3(seatArea.x, seatBackWidth, seatBackHeight));
                    //Arrow for seating direction
                    DrawArrow(GizmocenterOffset, GizmocenterOffset + seatDirection, Color.cyan);
                    break;
                case ObjectType.Table:
                    //collider
                    Gizmos.color = Color.green;
                    //Gizmos.DrawWireCube(ColliderCenter, ColliderSize);
                    Gizmos.color = Color.cyan;
                    //Leg of the table
                    Gizmos.DrawWireCube(new Vector3(0f, 0f, tableLeg.z / 2) + GizmocenterOffset, tableLeg);
                    //Surface of the table
                    Vector3 surfaceCenter = new Vector3(0f, 0f, tableLeg.z + tableSurface.z / 2);
                    Gizmos.DrawWireCube(surfaceCenter + GizmocenterOffset, tableSurface);
                    break;
                default:
                    break;
            }

        }
        private void DrawArrow(Vector3 arrowStart, Vector3 arrowEnd, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(arrowStart, 0.01f);
            Gizmos.DrawRay(arrowStart, seatDirection);
            Vector3 right = Quaternion.LookRotation(seatDirection) * Quaternion.Euler(0, 180 + 30, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(seatDirection) * Quaternion.Euler(0, 180 - 30, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(arrowEnd, right * 0.05f);
            Gizmos.DrawRay(arrowEnd, left * 0.05f);
        }
        public string GenerateJson()
        {
            string result = "";
            if (chosenIndex >= sceneObjects.Count)
            {
                result = "ERROR-Index is out of scene object bound";
            }
            else
            {
                AllAvaliableSceneObject allObject = new AllAvaliableSceneObject(sceneObjects, chosenIndex);
                result = JsonUtility.ToJson(allObject);
            }
            return result;
        }
        public AllAvaliableSceneObject GenerateJsonAuto()
        {

            if (chosenIndex >= sceneObjects.Count)
            {
                Debug.Log($" Group {gameObject.transform.parent.name} position {gameObject.name}'s index is out of bound");
                return null;
            }
            else
            {
                AllAvaliableSceneObject allObject = new AllAvaliableSceneObject(sceneObjects, chosenIndex);
                return allObject;
            }
        }
        public bool ReadJson()
        {
            try
            {
                AllAvaliableSceneObject allObject = JsonUtility.FromJson<AllAvaliableSceneObject>(readJsonInput);
                chosenIndex = allObject.current_object_index;
                sceneObjects = allObject.all_object_info;
            }
            catch
            {
                Debug.Log("Invalid Input");
                return false;
            }
            return true;
        }

        public void FindAllChildObejctAndLoadInfo()
        {
            foreach (Transform child in transform)
            {
                SceneObject newSceneObject = new SceneObject();
                SerializableVector3 posoff = new SerializableVector3(child.localPosition);
                SerializableVector3 rotoff = new SerializableVector3(child.localEulerAngles);
                SerializableVector3 scaleoff = new SerializableVector3(child.localScale);
                newSceneObject.i3m_object_posoff = posoff;
                newSceneObject.i3m_object_rotoff = rotoff;
                newSceneObject.i3m_object_scaleoff = scaleoff;
                newSceneObject.i3m_object_uuid = child.gameObject.name;
                sceneObjects.Add(newSceneObject);
            }
        }
        public void DeleteAllChildObejct()
        {
            for (int i = transform.childCount; i > 0; i--)
                DestroyImmediate(transform.GetChild(0).gameObject);
        }

        public void ToggleAllChildObejct(bool enable)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(enable);
            }
        }
        public void ChangeAllCatIDAndUUID(string catid)
        {
            foreach (SceneObject sceneObject in sceneObjects)
            {
                sceneObject.i3m_object_catid = catid;
            }
        }
        public void ChangeIndex(int index)
        {
            if (index >= sceneObjects.Count)
            {
                Debug.Log($"Item {gameObject.name} in Group {gameObject.transform.parent.name}'s index is out of bound, unable to change");
            }
            else
            {
                chosenIndex = index;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SceneObjectTool))]
    public class SceneObjectToolEditor : Editor
    {
        public override void OnInspectorGUI()
        {


            var sceneObjectTool = target as SceneObjectTool;

            //Gizmos
            EditorGUILayout.BeginVertical("OL BOX");
            EditorGUILayout.LabelField("Gizmo Setting", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("This information is for display only, changing the value here will not affect how gameobject is spawned at this location", EditorStyles.wordWrappedMiniLabel);
            //sceneObjectTool.gizmosSize = EditorGUILayout.Vector3Field("Gizmo Size", sceneObjectTool.gizmosSize);
            sceneObjectTool.GizmocenterOffset = EditorGUILayout.Vector3Field("Center offset", sceneObjectTool.GizmocenterOffset);
            sceneObjectTool.objectType = (SceneObjectTool.ObjectType)EditorGUILayout.EnumPopup("Type of Object:", sceneObjectTool.objectType);
            switch (sceneObjectTool.objectType)
            {
                case SceneObjectTool.ObjectType.DefaultCube:
                    sceneObjectTool.cubesize = EditorGUILayout.Vector3Field("Cube Size", sceneObjectTool.cubesize);
                    break;
                case SceneObjectTool.ObjectType.DefaultSpere:
                    sceneObjectTool.spheresize = EditorGUILayout.FloatField("Sphere Size", sceneObjectTool.spheresize);
                    break;
                case SceneObjectTool.ObjectType.Chair:
                    sceneObjectTool.seatArea = EditorGUILayout.Vector3Field("Seat Area", sceneObjectTool.seatArea);
                    sceneObjectTool.seatBackWidth = EditorGUILayout.FloatField("Seat Back Width:", sceneObjectTool.seatBackWidth);
                    sceneObjectTool.seatBackHeight = EditorGUILayout.FloatField("Seat Back Height:", sceneObjectTool.seatBackHeight);
                    sceneObjectTool.seatDirection = EditorGUILayout.Vector3Field("Seat Direaction", sceneObjectTool.seatDirection);
                    break;
                case SceneObjectTool.ObjectType.Table:
                    sceneObjectTool.tableLeg = EditorGUILayout.Vector3Field("Table Leg Area", sceneObjectTool.tableLeg);
                    sceneObjectTool.tableSurface = EditorGUILayout.Vector3Field("Table Surface Area", sceneObjectTool.tableSurface);
                    break;
                default:
                    break;
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(10);
            base.OnInspectorGUI();
            GUILayout.Space(10);

            //EditorGUILayout.TextArea("Use this to add to Scene Obejcts list from the child object attached to this object", EditorStyles.wordWrappedMiniLabel);
            if (GUILayout.Button(new GUIContent("Add to SceneObjects from Child Objects", "Use this to add to Scene Obejcts list from the child object attached to this object")))
            {
                sceneObjectTool.FindAllChildObejctAndLoadInfo();
            }
            if (GUILayout.Button(new GUIContent("Replace SceneObjects from Child Objects", "Use this to replace the current item in Scene Obejcts list from the child object attached to this object")))
            {
                sceneObjectTool.sceneObjects.Clear();
                sceneObjectTool.FindAllChildObejctAndLoadInfo();
            }
            if (GUILayout.Button(new GUIContent("Delete All Child Gameobject", "Use this delete all Child Gameobject from this object")))
            {
                sceneObjectTool.DeleteAllChildObejct();
            }
            if (GUILayout.Button(new GUIContent("Disable All Child Gameobject", "Use this disable all Child Gameobject from this object")))
            {
                sceneObjectTool.ToggleAllChildObejct(false);
            }
            if (GUILayout.Button(new GUIContent("Enable All Child Gameobject", "Use this Enable all Child Gameobject from this object")))
            {
                sceneObjectTool.ToggleAllChildObejct(true);
            }
            GUILayout.Space(10);
            GUIStyle textStyle = EditorStyles.label;
            textStyle.wordWrap = true;
            EditorGUILayout.LabelField("Generation Json Block with {Chosen index} and scene object in {Scene Objects}", textStyle);
            /* sceneObjectTool.itemUUID = EditorGUILayout.TextField("Item UUID", sceneObjectTool.itemUUID);
             sceneObjectTool.catID = EditorGUILayout.TextField("Item Catalog ID", sceneObjectTool.catID);*/
            if (GUILayout.Button("Generate", GUILayout.Height(30)))
            {
                sceneObjectTool.jsonResult = sceneObjectTool.GenerateJson();
            }
            GUILayout.Space(10);
            GUILayout.TextArea(sceneObjectTool.jsonResult);
            GUILayout.Space(10);

            EditorGUILayout.LabelField("Read in Json Block", textStyle);
            if (GUILayout.Button("Read Json", GUILayout.Height(30)))
            {
                if (sceneObjectTool.ReadJson())
                {
                    sceneObjectTool.readJsonMessage = "Read Json Succeed";
                }
                else
                {
                    sceneObjectTool.readJsonMessage = "Invalid Input";
                }
            }
            sceneObjectTool.readJsonInput = GUILayout.TextField(sceneObjectTool.readJsonInput);
            EditorGUILayout.LabelField(sceneObjectTool.readJsonMessage);
            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
    [System.Serializable]
    public class SceneObject
    {
        public string i3m_object_catid;
        public string i3m_object_uuid;
        public SerializableVector3 i3m_object_posoff;
        public SerializableVector3 i3m_object_rotoff;
        public SerializableVector3 i3m_object_scaleoff = new SerializableVector3(new Vector3(1f, 1f, 1f));
        public string i3m_object_type;
        public SerializableVector3 i3m_object_color;
        public bool i3m_need_lod;
        public SceneObject(string catID, string uuid, SerializableVector3 pos, SerializableVector3 rot, SerializableVector3 scale, SceneObjectTool.ObjectType type, SerializableVector3 color, bool needLod)
        {
            i3m_object_catid = catID;
            i3m_object_uuid = uuid;
            i3m_object_posoff = pos;
            i3m_object_rotoff = rot;
            i3m_object_scaleoff = scale;
            i3m_object_type = type.ToString();
            i3m_object_color = color;
            i3m_need_lod = needLod;
        }
        public SceneObject()
        {

        }

    }

    [System.Serializable]
    public class AllAvaliableSceneObject
    {
        public int current_object_index;
        public List<SceneObject> all_object_info;
        public AllAvaliableSceneObject(List<SceneObject> sceneObjects, int index)
        {
            all_object_info = sceneObjects;
            current_object_index = index;
        }
    }


}
