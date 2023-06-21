using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
namespace SceneObjectTool
{

    public class AdStation : MonoBehaviour
    {
        public AdStationInfo stationInfo;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public string GenerateAdStationInfo()
        {
            stationInfo.position = new SerializableVector3(gameObject.transform.position);
            stationInfo.rotation = new SerializableVector3(gameObject.transform.eulerAngles);
            string res = "";
            res = Newtonsoft.Json.JsonConvert.SerializeObject(stationInfo);
            return res;
        }
    }
    [System.Serializable]
    public class AdStationInfo
    {
        public string stationUUID;
        public string stationCATID;
        public string contentUUID = "6d5540d4-4c21-4f15-93b2-1d24f0aac93a";
        //public string contentUUID;
        //public string contentCATID;
        public SerializableVector3 position;
        public SerializableVector3 rotation;
        //public SerializableVector3 contentPos;
        //public SerializableVector3 contentRot;
    }

}
#endif