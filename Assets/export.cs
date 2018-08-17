using UnityEngine;
using System.Collections;
using UnityEditor;

public static class ExportWithLayers
{

    [MenuItem("Asset Store Tools/Export package with tags and physics layers")]
    public static void ExportPackage()
    {
        string[] projectContent = new string[] { "Assets/SimpleCraft", "ProjectSettings/TagManager.asset" };
        AssetDatabase.ExportPackage(projectContent, "SimpleCraft.unitypackage", ExportPackageOptions.Interactive | ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
        Debug.Log("Project Exported");
    }

}
