using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
namespace SceneObjectTool
{
    public class GenerateObjectConfigFile : MonoBehaviour
    {

        public enum TransportationTypes
        {
            Drone = 0,
            Bike = 1,
            Scooter = 2,
            Jeep = 3,
            Horse = 4,
            Car = 5
        }
        [HideInInspector]
        public string GeneratedObjectFile;
        public Dictionary<string, object> fileToGenerate = new Dictionary<string, object>();

        public string skyboxName;
        public bool GenerateFile = true;
        [HideInInspector]
        public string savePath = "Assets/";
        [HideInInspector]
        public string nameOfFile;
        [HideInInspector]
        public string NewUUID;

        [Header("Interaction Target")]
        public List<InteractionTarget> interactionTargets = new List<InteractionTarget>();

        [Header("Loading Screen Image")]
        public List<string> loadingImageURL = new List<string>();

        [Header("Transportation Types")]
        public List<TransportationTypes> transportationTypes = new List<TransportationTypes>();
        public string GenerateEntrieObjectConfigFile()
        {
            fileToGenerate.Clear();
            string result = string.Empty;
            foreach (Transform child in transform)
            {
                if (child.gameObject.name == "NPCSpawner")
                {
                    fileToGenerate.Add("npc_spawn_info", GenerateSceneNPCScetion(child.gameObject));
                }
                else if (child.gameObject.name == "DynamicObject")
                {
                    fileToGenerate.Add("scene_object_info", GenerateSceneObjectSection(child.gameObject));
                }
                else if (child.gameObject.name == "SceneLinkGroup")
                {
                    fileToGenerate.Add("scene_link_group_info", GenerateSceneLinkSection(child.gameObject));
                }
                else if (child.gameObject.name == "SpawnPositions")
                {
                    fileToGenerate.Add("spawn_point_info", child.gameObject.GetComponent<SpawnPointGenerator>().GetAllSpawnPointInfo());
                }
                else if (child.gameObject.name == "AdStations")
                {
                    fileToGenerate.Add("adstation_info", GenerateAdStationSection(child.gameObject));
                }
                else if(child.gameObject.name == "FilmSections")
                {
                    fileToGenerate.Add("film_section_info", GenerateFilmSection(child.gameObject));
                }
                else if (child.gameObject.name == "MiniGame")
                {
                    fileToGenerate.Add("minigame_section_info", GenerateMiniGameSection(child.gameObject));
                }
            }
            fileToGenerate.Add("interaction_target_info", GenerateInteractionSection());
            fileToGenerate.Add("loading_image_info", loadingImageURL);

            fileToGenerate.Add("transportation_type_info", GenerateTransportationTypes());
            fileToGenerate.Add("skyboc_info", skyboxName);
            result = Newtonsoft.Json.JsonConvert.SerializeObject(fileToGenerate);

            if (GenerateFile)
            {
                if (string.IsNullOrEmpty(nameOfFile))
                {
                    return "File Name cannot be empty";
                }
                if (AssetDatabase.IsValidFolder(savePath))
                {
                    string nameUUID = nameOfFile;
                    /*                if (ifAutoAssignNewUUID)
                                    {
                                        nameUUID = System.Guid.NewGuid().ToString();
                                    }*/
                    nameUUID += "_config.json";
                    System.IO.File.WriteAllText(savePath + nameUUID, result);
                    AssetDatabase.Refresh();
                    return savePath + nameUUID;
                }
            }

            return result;
        }
        public Dictionary<string, NPCInfo> GenerateSceneNPCScetion(GameObject objectParent)
        {
            string grp = string.Empty;
            string num = string.Empty;
            Dictionary<string, NPCInfo> temp = new Dictionary<string, NPCInfo>();
            //add scene object info
            foreach (Transform child in objectParent.transform)
            {
                GameObject group = child.gameObject;
                grp = child.gameObject.name;
                foreach (Transform grandchild in group.transform)
                {
                    num = grandchild.gameObject.name;
                    string key = grp + "," + num;
                    if (grandchild.GetComponent<NPCSceneTool>() != null)
                    {
                        NPCInfo value = grandchild.GetComponent<NPCSceneTool>().npcInfos;
                        if (!string.IsNullOrEmpty(key) && value != null)
                        {
                            temp.Add(key, value);
                        }
                        else
                        {
                            Debug.Log($"{key} does not have npc info");

                        }
                    }
                    else
                    {
                        Debug.Log($"{key} does not have NPCSceneTool Attached");
                    }

                }
            }

            return temp;
        }
        public Dictionary<string, AllAvaliableSceneObject> GenerateSceneObjectSection(GameObject objectParent)
        {
            string grp = string.Empty;
            string num = string.Empty;
            Dictionary<string, AllAvaliableSceneObject> temp = new Dictionary<string, AllAvaliableSceneObject>();
            //add scene object info
            foreach (Transform child in objectParent.transform)
            {
                GameObject group = child.gameObject;
                grp = child.gameObject.name;
                foreach (Transform grandchild in group.transform)
                {
                    num = grandchild.gameObject.name;
                    string key = grp + "," + num;
                    if (grandchild.GetComponent<SceneObjectTool>() != null)
                    {
                        AllAvaliableSceneObject value = grandchild.GetComponent<SceneObjectTool>().GenerateJsonAuto();
                        if (!string.IsNullOrEmpty(key) && value != null)
                        {
                            temp.Add(key, value);
                        }
                        else
                        {
                            Debug.Log($" {key}'s index is out of bound");
                        }
                    }
                    else
                    {
                        Debug.Log($"{key} does not have sceneObjectToolAttached");
                    }

                }
            }

            return temp;
        }
        public List<string> GenerateSceneLinkSection(GameObject objectParent)
        {
            string grp = string.Empty;
            List<string> temp = new List<string>();
            foreach (Transform child in objectParent.transform)
            {
                if (child.GetComponent<SceneLinkTool>() != null)
                {
                    string res = child.gameObject.GetComponent<SceneLinkTool>().GetSceneLinkInfo();
                    temp.Add(res);
                }
            }
            return temp;
        }
        public List<string> GenerateAdStationSection(GameObject objectParent)
        {
            string grp = string.Empty;
            List<string> temp = new List<string>();
            foreach (Transform child in objectParent.transform)
            {
                if (child.GetComponent<AdStation>() != null)
                {
                    string res = child.gameObject.GetComponent<AdStation>().GenerateAdStationInfo();
                    temp.Add(res);
                }
            }
            return temp;
        }
        public Dictionary<string, FilmSectionItemInfo> GenerateFilmSection(GameObject objectParent)
        {
            string grp = string.Empty;
            Dictionary<string, FilmSectionItemInfo> temp = new Dictionary<string, FilmSectionItemInfo>();
            for(int i = 0;i<objectParent.transform.childCount;i++)
            {
                if (objectParent.transform.GetChild(i).GetComponent<FilmSectionItem>() != null)
                {
                    FilmSectionItemInfo res = objectParent.transform.GetChild(i).gameObject.GetComponent<FilmSectionItem>().info;
                    temp.Add(objectParent.transform.GetChild(i).name+"_"+i,res);
                }
            }
            return temp;
        }
        public List<Dictionary<string,object>> GenerateMiniGameSection(GameObject objectParent)
        {
            List<Dictionary<string, object>> temp = new List<Dictionary<string, object>>();
            for (int i = 0; i < objectParent.transform.childCount; i++)
            {
                if (objectParent.transform.GetChild(i).GetComponent<MiniGameConfigGenerator>() != null)
                {
                    Dictionary<string, object> res = objectParent.transform.GetChild(i).gameObject.GetComponent<MiniGameConfigGenerator>().GenerateConfig();
                    temp.Add(res);
                }
            }
            return temp;
        }
        public List<string> GenerateInteractionSection()
        {
            List<string> temp = new List<string>();
            foreach (InteractionTarget target in interactionTargets)
            {
                Dictionary<string, string> info = new Dictionary<string, string>();
                info.Add("name", target.name);
                info.Add("ID",((int)target.ID).ToString());
                temp.Add(Newtonsoft.Json.JsonConvert.SerializeObject(info));
            }

            return temp;
        }
        public List<int> GenerateTransportationTypes()
        {
            List<int> temp = new List<int>();
            foreach (TransportationTypes target in transportationTypes)
            {
                temp.Add((int)target);
            }

            return temp;
        }

        public string GetNewUUID()
        {
            return System.Guid.NewGuid().ToString();
        }
        public string GetAddressableUUIDForCurrentScene()
        {
            string res = "";
            Scene thisScene = SceneManager.GetActiveScene();
            return res;
        }

    }

    [CustomEditor(typeof(GenerateObjectConfigFile))]
    public class GenerateObjectConfigFileEditor : Editor
    {
        SerializedProperty fileName;
        
        void OnEnable()
        {
            fileName = serializedObject.FindProperty("nameOfFile");
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var generateObject = target as GenerateObjectConfigFile;
            if (generateObject.GenerateFile)
            {

                generateObject.savePath = EditorGUILayout.TextField("Save Path", generateObject.savePath);
                EditorGUILayout.BeginVertical("HelpBox");
                GUILayout.Label("Define the name of this configuration file, if for Template, the file name should be Scene Addressable UUID, if for customer, the file name should be customer UUID", EditorStyles.wordWrappedMiniLabel);
                EditorGUILayout.EndVertical();
                fileName.stringValue = EditorGUILayout.TextField("File Name", generateObject.nameOfFile);
                if (GUILayout.Button("Generate A New UUID for New Customer"))
                {
                    //generateObject.NewUUID = System.Guid.NewGuid().ToString();
                    generateObject.NewUUID = generateObject.GetNewUUID();
                }
                EditorGUILayout.TextArea(generateObject.NewUUID);
            }
            GUIStyle textStyle = EditorStyles.label;
            if (GUILayout.Button("Generate"))
            {
                generateObject.GeneratedObjectFile = generateObject.GenerateEntrieObjectConfigFile();

            }
            textStyle.wordWrap = true;
            EditorGUILayout.TextArea(generateObject.GeneratedObjectFile, textStyle);
            serializedObject.ApplyModifiedProperties();
        }
    }

    [System.Serializable]
    public class InteractionTarget
    {
        public enum InteractionTypes
        {
            Seat = 2,
            ItemInteraction = 5,
            AppointmentInteraction = 6,
            VideoCharInteraction = 7,
            HouseSaleInteraction = 8
        }
        public string name;
        public InteractionTypes ID;
    }

}
#endif