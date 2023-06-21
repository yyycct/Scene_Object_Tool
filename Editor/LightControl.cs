using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class LightControl : MonoBehaviour
{
    // Start is called before the first frame update
    public string jsonResult;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string GenerateLightData()
    {
        string result = "";
        AllLightVariable allLightData = new AllLightVariable();
        foreach (Transform child in transform)
        {
            Light light = child.GetComponent<Light>();
            if (light != null)
            {
                LightVariable newlight = new LightVariable(light.color, light.colorTemperature, light.intensity, light.range, light.bounceIntensity);
                allLightData.allLights.Add(newlight);
            }
        }
        jsonResult = JsonUtility.ToJson(allLightData);
        Debug.Log(JsonUtility.ToJson(allLightData));
        return result;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(LightControl))]
public class LightControlEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var lightControl = target as LightControl;
        if (GUILayout.Button("Generate", GUILayout.Height(30)))
        {
            lightControl.jsonResult = lightControl.GenerateLightData();
        }
        GUILayout.TextArea(lightControl.jsonResult);
    }
}
#endif
[System.Serializable]
public class LightVariable
{
    public Color filterColor;
    public float temperature;
    public float intensity;
    public float range;
    public float indirect_mutiplier;

    public LightVariable(Color _filterColor, float _temperature, float _internsity, float _range, float _indirect_mutiplier)
    {
        /*        filterColor[0] = _filterColor.r;
                filterColor[1] = _filterColor.g;
                filterColor[2] = _filterColor.b;
                filterColor[3] = _filterColor.a;*/
        filterColor = _filterColor;
        temperature = _temperature;
        intensity = _internsity;
        range = _range;
        indirect_mutiplier = _indirect_mutiplier;
    }
}

public class AllLightVariable
{
    public List<LightVariable> allLights = new List<LightVariable>();
}
