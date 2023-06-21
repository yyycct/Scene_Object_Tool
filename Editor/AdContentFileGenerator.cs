using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
#if UNITY_EDITOR
namespace SceneObjectTool
{
    public class AdContentFileGenerator : MonoBehaviour
    {
        public ContentInfo[] adContents = new ContentInfo[24];
        [Space(10)]
        [Header("Add Ad content to list")]
        public int startTime = -1;
        public int endTime = -1;
        public ContentInfo AdContent;
        [HideInInspector]
        public string savePath = "Assets/";
        [HideInInspector]
        public string nameOfFile;
        [HideInInspector]
        public string NewUUID;
        [HideInInspector]
        public string GeneratedObjectFile;

        public void ReplaceAdcontents()
        {
            if (startTime < 0 || endTime < 0)
            {
                return;
            }
            if (endTime < startTime)
            {
                return;
            }
            if (endTime > 24)
            {
                return;
            }
            for (int i = startTime; i < endTime; i++)
            {
                ContentInfo contentInfo = new ContentInfo(AdContent.contentUUID, AdContent.contentCATID, AdContent.InteractionType, AdContent.InteractionInformation, new SerializableVector3(transform.localPosition), new SerializableVector3(transform.localEulerAngles),AdContent.AdvertiserName);
                adContents[i] = contentInfo;
            }
        }
        public string GetNewUUID()
        {
            return System.Guid.NewGuid().ToString();
        }
        public string GenerateAdContentFile()
        {
            if (string.IsNullOrEmpty(nameOfFile))
            {
                return "File Name cannot be empty";
            }
            if (AssetDatabase.IsValidFolder(savePath))
            {
                string nameUUID = nameOfFile;
                List<string> info = new List<string>();
                foreach (ContentInfo c in adContents)
                {
                    info.Add(JsonConvert.SerializeObject(c));
                }
                string result = Newtonsoft.Json.JsonConvert.SerializeObject(info);
                nameUUID += "_config.json";
                System.IO.File.WriteAllText(savePath + nameUUID, result);
                AssetDatabase.Refresh();
                return savePath + nameUUID;
            }
            return "";
        }
    }

    [System.Serializable]
    public class ContentInfo
    {
        [JsonProperty("contentUUID")]
        public string contentUUID = "c2c346fe-5f37-49ff-9ded-c8897b352cfa";
        [JsonProperty("contentCATID")]
        public string contentCATID = "7000";
        [JsonProperty("AdvertiserName")]
        public string AdvertiserName = "";
        [JsonProperty("contentPosition")]
        public string contentPosition;
        [JsonProperty("contentRotation")]
        public string contentRotation;
        [JsonProperty("InteractionType")]
        public string InteractionType = "0";
        [JsonProperty("InteractionInformation")]
        public string InteractionInformation = "https://i3m.tv/";
        

        public ContentInfo(string _contentUUID, string _contentCATID, string _InteractionType, string _InteractionInfo, SerializableVector3 position, SerializableVector3 rotation,string advertiserName = "")
        {
            contentCATID = _contentCATID;
            contentUUID = _contentUUID;
            InteractionType = _InteractionType;
            InteractionInformation = _InteractionInfo;
            contentPosition = Newtonsoft.Json.JsonConvert.SerializeObject(position);
            contentRotation = Newtonsoft.Json.JsonConvert.SerializeObject(rotation);
            AdvertiserName = advertiserName;
            if (!string.IsNullOrEmpty(AdvertiserName))
            {
                string[] ads = AdvertiserName.Split('/');
                Debug.Log("ads" + ads);
                if (ads.Length >= 2)
                {
                    AdvertiserName = ads[0];
                    for (int i = 1; i < ads.Length; i++)
                    {
                        AdvertiserName += "\n" + ads[i];
                    }
                }
            }
            Debug.Log("AdvertiserName" + AdvertiserName);
        }

    }


    [CustomEditor(typeof(AdContentFileGenerator))]
    public class AdContentFileGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();
            var adContent = target as AdContentFileGenerator;

            if (GUILayout.Button("Add Content", GUILayout.Height(30)))
            {
                adContent.ReplaceAdcontents();
            }

            adContent.savePath = EditorGUILayout.TextField("Save Path", adContent.savePath);
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Define the name of this configuration file, if for Template, the file name should be Scene Addressable UUID, if for customer, the file name should be customer UUID", EditorStyles.wordWrappedMiniLabel);
            EditorGUILayout.EndVertical();
            adContent.nameOfFile = EditorGUILayout.TextField("File Name", adContent.nameOfFile);
            if (GUILayout.Button("Generate A New UUID for New Customer"))
            {
                //generateObject.NewUUID = System.Guid.NewGuid().ToString();
                adContent.NewUUID = adContent.GetNewUUID();
            }
            EditorGUILayout.TextArea(adContent.NewUUID);
            GUIStyle textStyle = EditorStyles.label;
            if (GUILayout.Button("Generate"))
            {
                adContent.GeneratedObjectFile = adContent.GenerateAdContentFile();

            }
            textStyle.wordWrap = true;
            EditorGUILayout.TextArea(adContent.GeneratedObjectFile, textStyle);
        }
    }
}
#endif
