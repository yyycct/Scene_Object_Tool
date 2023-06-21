using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace SceneObjectTool
{
    public class SpawnPointGenerator : MonoBehaviour
    {
        public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
        public bool hasZone;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public List<string> GetAllSpawnPointInfo()
        {
            List<string> res = new List<string>();
            spawnPoints.Clear();
            foreach (Transform child in transform)
            {
                SpawnPoint spawnPoint = new SpawnPoint();
                if (hasZone)
                {
                    SpawnPointInfo spawnPointInfo = child.gameObject.GetComponent<SpawnPointInfo>();
                    if (spawnPointInfo != null && !string.IsNullOrEmpty(spawnPointInfo.zoneName))
                    {
                        spawnPoint.position = new SerializableVector3(child.position);
                        spawnPoint.rotation = new SerializableVector3(child.eulerAngles);
                        spawnPoint.ZR = spawnPointInfo.zoneName;
                        res.Add(Newtonsoft.Json.JsonConvert.SerializeObject(spawnPoint));
                        spawnPoints.Add(spawnPoint);
                    }
                }
                else
                {
                    spawnPoint.position = new SerializableVector3(child.position);
                    spawnPoint.rotation = new SerializableVector3(child.eulerAngles);
                    spawnPoint.ZR = "N/A";
                    spawnPoints.Add(spawnPoint);
                    res.Add(Newtonsoft.Json.JsonConvert.SerializeObject(spawnPoint));
                }
            }
            //Debug.Log(res);
            return res;
        }
        public void AddSpawnPoint()
        {
            GameObject newPosition = new GameObject("SpawnPoint");
            newPosition.AddComponent<SpawnPointInfo>();
            newPosition.transform.parent = this.transform;
            newPosition.transform.localPosition = Vector3.zero;
            newPosition.transform.localRotation = Quaternion.identity;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(SpawnPointGenerator))]
    public class SpawnPointGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var spawnPointGen = target as SpawnPointGenerator;
            base.OnInspectorGUI();
            GUILayout.Label("To add a spawn point, add a child object to this object and attach the spawn point info component to the object, and fill out the zone information in there if needed", EditorStyles.wordWrappedLabel);
            if (GUILayout.Button("Add Spawn Position"))
            {
                spawnPointGen.AddSpawnPoint();
            }
            if (GUILayout.Button("Spawn Position"))
            {
                spawnPointGen.GetAllSpawnPointInfo();
            }
        }
    }
#endif
    public class SpawnPoint
    {
        public SerializableVector3 position;
        public SerializableVector3 rotation;
        public string ZR;
    }
}