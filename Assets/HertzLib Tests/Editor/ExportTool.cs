using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ExportTool
{
    [MenuItem("Tools/Export")]
    private static void Export()
    {
        string[] allPaths = AssetDatabase.GetAllAssetPaths();
        List<string> validPaths = new List<string>();

        for (int i = 0; i < allPaths.Length; i++)
        {
            if (allPaths[i].StartsWith("Assets/"))
            {
                if (allPaths[i].StartsWith("Assets/Editor"))
                    continue;
                if (allPaths[i].ToLower().Contains("probuilder"))
                    continue;
                if (allPaths[i].ToLower().Contains("hertzlib tests"))
                    continue;
                if (allPaths[i].ToLower().Contains("textmesh pro"))
                    continue;

                validPaths.Add(allPaths[i]);
            }
        }

        string exportPath = EditorUtility.SaveFilePanel("Export package", Application.dataPath + "/../Exports", "HertzLib", "unitypackage");

        if (!string.IsNullOrEmpty(exportPath))
            AssetDatabase.ExportPackage(validPaths.ToArray(), exportPath, ExportPackageOptions.Interactive);
    }
}
