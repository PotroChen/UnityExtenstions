using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class AssetDatabaseExtensions
{
    /// <summary>
    /// Copy 序列化信息到 原本的资源内，不然引用会有问题(Unity引擎的问题)，
    /// 然后使用返回的新引用
    /// </summary>
    /// <param name="asset"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Object CreateOrReplaceAsset(Object asset, string path)
    {
        var serializedAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
        if (serializedAsset != null)
        {
            EditorUtility.CopySerialized(asset, serializedAsset);//Copy 序列化信息到 原本的资源内，不然引用会有问题(Unity引擎的问题)
            return serializedAsset;
        }
        else
        {
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }
    }

    /*
     * Unity 2019.4.40f1:
     * 用该函数更改prefab后,调用Assetdatabase.Refresh时的UnityEditor.PrefabUtility.prefabInstanceUpdated中,不要加
     * 载对应的prefab，不然会使prefab文件损坏
     */
    public static void SetAssetGUID(string path,string guid)
    {
        //fileFormatVersion: 2 是可以的
        int lastIndexOfAssets = Application.dataPath.LastIndexOf("Assets");
        string metaFilePath = Application.dataPath.Remove(lastIndexOfAssets) + path + ".meta";

        var lines = File.ReadAllLines(metaFilePath);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line.StartsWith("guid: "))
            {
                lines[i] = string.Format("guid: {0}", guid);
                break;
            }
        }
        File.WriteAllLines(metaFilePath, lines);
    }
}
