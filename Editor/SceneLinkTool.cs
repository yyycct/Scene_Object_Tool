using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SceneObjectTool
{
    public class SceneLinkTool : MonoBehaviour
    {
        public SceneLinkInfo sceneLinkInfo;

        public string GetSceneLinkInfo()
        {
            sceneLinkInfo.position = new SerializableVector3(transform.position);
            sceneLinkInfo.rotation = new SerializableVector3(transform.eulerAngles);
            string res = "";
            res = Newtonsoft.Json.JsonConvert.SerializeObject(sceneLinkInfo);
            return res;
        }
    }
    [System.Serializable]
    public class SceneLinkInfo
    {
        public string tripUUID;
        public string doorCatID;
        public string doorUUID;
        public string address;
        public string zoneNum;
        public SerializableVector3 position;
        public SerializableVector3 rotation;
    }
}