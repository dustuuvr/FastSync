using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Dustuu.VRChat.Uutils.VideoQuuSystem.Editor
{
    // TODO: instead of getting components (causes bugs when haven't been selected/viewed), get UdonSharpBehaviours and extract the script from there
    public static class DataImportExportTools
    {
        // TODO: Prevent and warn about duplicates
        [MenuItem("Tools/Import Data")]
        private static void ImportData()
        {
            string videoCollectionPrefabAssetPath = "Assets/_VideoQuu/Prefabs/VideoCollection.prefab";
            string videoDetailPrefabAssetPath = "Assets/_VideoQuu/Prefabs/VideoDetail.prefab";
            GameObject videoCollectionPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(videoCollectionPrefabAssetPath);
            GameObject videoDetailPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(videoDetailPrefabAssetPath);

            GameObject selectedGameObject = Selection.activeGameObject;
            if (selectedGameObject == null)
            {
                Debug.LogError("No GameObject selected!");
                return;
            }

            VideoCollection selectedVideoCollection = selectedGameObject.GetComponent<VideoCollection>();
            if (selectedVideoCollection == null)
            {
                Debug.LogError("Selected GameObject did not have a VideoCollection component!");
                return;
            }

            // Destroy all children, iterate backwards to avoid modifying collection order while running
            GameObject selectedVideoCollectionGameObject = selectedVideoCollection.gameObject;
            for (int i = selectedVideoCollectionGameObject.transform.childCount - 1; i >= 0; i--)
            { GameObject.DestroyImmediate(selectedVideoCollectionGameObject.transform.GetChild(i).gameObject); }

            // Create objects for each line
            string filePath = EditorUtility.OpenFilePanel("Select Data to Import", "", "txt");
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] lineParts = line.Split(',');
                if (lineParts.Length >= 2)
                {
                    string[] collectionTitles = lineParts.Take(lineParts.Length - 2).ToArray();
                    string detailTitle = lineParts[lineParts.Length - 2];
                    string detailUrl = lineParts[lineParts.Length - 1];

                    Transform last = selectedVideoCollectionGameObject.transform;
                    for (int i = 0; i < collectionTitles.Length; i++)
                    {
                        string collectionTitle = collectionTitles[i];

                        Transform next = last.Find(collectionTitle);
                        if ( next == null )
                        {
                            next = (PrefabUtility.InstantiatePrefab(videoCollectionPrefab, last.transform) as GameObject).transform;
                            next.name = collectionTitle;
                            UdonBehaviour nextUdonBehaviour = next.GetComponent<UdonBehaviour>();
                            IUdonVariableTable nextVariableTable = nextUdonBehaviour.publicVariables;
                            nextVariableTable.TrySetVariableValue<string>("title", collectionTitle);
                        }
                        last = next;
                    }
                    GameObject detail = PrefabUtility.InstantiatePrefab(videoDetailPrefab, last.transform) as GameObject;
                    detail.name = detailTitle;
                    UdonBehaviour detailUdonBehaviour = detail.GetComponent<UdonBehaviour>();
                    IUdonVariableTable detailVariableTable = detailUdonBehaviour.publicVariables;
                    detailVariableTable.TrySetVariableValue<string>("title", detailTitle);
                    detailVariableTable.TrySetVariableValue<VRCUrl>("url", new VRCUrl(detailUrl));
                }
                else { Debug.LogError($"Invalid line: {line}"); }
            }
        }
    }
}