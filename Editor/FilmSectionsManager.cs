using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
namespace SceneObjectTool
{
    public class FilmSectionsManager : MonoBehaviour
    {
        public string InteractionType;
        public string InteractionInfo;

        public void AddFilmSectionItems()
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    if (child.gameObject.GetComponent<FilmSectionItem>() == null)
                    {
                        FilmSectionItem item = child.gameObject.AddComponent<FilmSectionItem>();
                        item.info.InteractionInfo = InteractionInfo;
                        item.info.InteractionType = InteractionType;
                        Transform fn = item.transform.Find("FilmName");
                        if (fn != null)
                        {
                            if(fn.GetChild(1) != null)
                            {
                                item.info.VideoUUID = fn.GetChild(1).gameObject.name;
                            }  
                        }
                        Transform camera = item.transform.Find("Camera");
                        if(camera != null)
                        {
                            if (camera.GetChild(0) != null)
                            {
                                GameObject interactionTarget = camera.GetChild(0).gameObject;
                                if (interactionTarget.name == "ItemInteractionTarget"){
                                    Transform info = interactionTarget.transform.GetChild(0);
                                    if (info != null)
                                    {
                                        if(info.name == "ItemInfo")
                                        {
                                            item.info.InteractionType = "2";
                                            item.info.InteractionInfo = info.GetComponent<TMP_Text>().text;

                                        }
                                        else if(info.name == "ItemUUID")
                                        {
                                            item.info.InteractionType = "1";
                                            item.info.InteractionInfo = info.GetComponent<TMP_Text>().text;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ReplaceFilmSectionItems()
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    if (child.gameObject.GetComponent<FilmSectionItem>() != null)
                    {
                        FilmSectionItem item = child.gameObject.GetComponent<FilmSectionItem>();
                        item.info.InteractionInfo = InteractionInfo;
                        item.info.InteractionType = InteractionType;
                        Transform fn = item.transform.Find("FilmName");
                        if (fn != null)
                        {
                            if (fn.GetChild(1) != null)
                            {
                                item.info.VideoUUID = fn.GetChild(1).gameObject.name;
                            }
                        }
                        Transform camera = item.transform.Find("Camera");
                        if (camera != null)
                        {
                            if (camera.GetChild(0) != null)
                            {
                                GameObject interactionTarget = camera.GetChild(0).gameObject;
                                if (interactionTarget.name == "ItemInteractionTarget")
                                {
                                    Transform info = interactionTarget.transform.GetChild(0);
                                    if (info != null)
                                    {
                                        if (info.name == "ItemInfo")
                                        {
                                            item.info.InteractionType = "2";
                                            item.info.InteractionInfo = info.GetComponent<TMP_Text>().text;

                                        }
                                        else if (info.name == "ItemUUID")
                                        {
                                            item.info.InteractionType = "1";
                                            item.info.InteractionInfo = info.GetComponent<TMP_Text>().text;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void ReplaceInteraction()
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    if (child.gameObject.GetComponent<FilmSectionItem>() != null)
                    {
                        FilmSectionItem item = child.gameObject.GetComponent<FilmSectionItem>();
                        item.info.InteractionInfo = InteractionInfo;
                        item.info.InteractionType = InteractionType;  
                    }
                }
            }
        }
    }

    [CustomEditor(typeof(FilmSectionsManager))]
    public class FilmSectionsManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var filmSectionsManager = target as FilmSectionsManager;
            base.OnInspectorGUI();
            if (GUILayout.Button("Add Film Section Items", GUILayout.Height(30)))
            {
                filmSectionsManager.AddFilmSectionItems();
            }
            if (GUILayout.Button("Replace Film Section Items Info", GUILayout.Height(30)))
            {
                filmSectionsManager.ReplaceFilmSectionItems();
            }
            if (GUILayout.Button("Replace All Interactions", GUILayout.Height(30)))
            {
                filmSectionsManager.ReplaceInteraction();
            }
        }
    }
}
#endif