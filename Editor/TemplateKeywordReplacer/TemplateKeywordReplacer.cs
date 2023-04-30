/*
TemplateKeywordReplacer.cs
Author : Koo
Date   : 2023-04-29 00:15:03
*/

using UnityEngine;
using UnityEditor;
using System.Collections;

public class TemplateKeywordReplacer : UnityEditor.AssetModificationProcessor
{
    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        int index = path.LastIndexOf(".");

        if (index < 0) { return; }

        string file = path.Substring(index);
        if (file != ".cs") { return; }

        index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;
        if (!System.IO.File.Exists(path)) { return; }

        string fileContent = System.IO.File.ReadAllText(path);

        fileContent = fileContent.Replace("#DATE#", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CreateSpecificCulture("ko-KR")));
        SOTemplateKeywordReplacer information = AssetDatabase.LoadAssetAtPath<SOTemplateKeywordReplacer>("Assets/Editor/TemplateKeywordReplacer/SOTemplateKeyword.asset");
        if (information)
        {
            fileContent = fileContent.Replace("#NAMESPACE#", information.Namespace);
            fileContent = fileContent.Replace("#AUTHOR#", information.Author);
        }
        else
        {
            Debug.LogError("Failed to load DeveloperInformation");
        }

        // 대체가 끝나면 다시 파일에 쓰기
        System.IO.File.WriteAllText(path, fileContent);

        // 다하고 나면 꼭 호출
        AssetDatabase.Refresh();
    }
}