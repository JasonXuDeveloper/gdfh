using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ExportConfig : EditorWindow
{
    [MenuItem("编辑器工具/导表 %#X")]
    private static void ShowWindow()
    {
        var window = GetWindow<ExportConfig>();
        window.titleContent = new GUIContent("导表工具");
        window.minSize = new Vector2(500, 350);
        window.Show();
        _outPath = Application.dataPath + "/HotUpdateResources/TextAsset/";
    }

    private static string _excelPath;
    private static string _outPath;
    private static string _outFileName;

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUIStyle textStyle = new GUIStyle
        {
            normal = {textColor = GUI.skin.label.normal.textColor},
            fontSize = 24,
            alignment = TextAnchor.MiddleCenter
        };
        GUILayout.Label("导表工具", textStyle);
        GUILayout.Space(10);

        MakeHorizontal(GetSpace(0.1f),
            () =>
            {
                EditorGUILayout.HelpBox("导表工具是通过C#的Process去调用命令行然后运行python程序，请确保有python环境且安装了openpyxl",
                    MessageType.Info);
            });
        GUILayout.Space(10);

        MakeHorizontal(GetSpace(0.1f), () =>
        {
            if (GUILayout.Button("导出链式任务",GUILayout.Height(50)))
            {
                _excelPath = Application.dataPath + "/../Config/链式任务.xlsx";
                _outPath = Application.dataPath + "/HotUpdateResources/TextAsset/";
                _outFileName = "task_cfg";
            }
            if (GUILayout.Button("导出任务目标",GUILayout.Height(50)))
            {
                _excelPath = Application.dataPath + "/../Config/任务目标.xlsx";
                _outPath = Application.dataPath + "/HotUpdateResources/TextAsset/";
                _outFileName = "task_desc";
            }
            if (GUILayout.Button("导出道具配置",GUILayout.Height(50)))
            {
                _excelPath = Application.dataPath + "/../Config/道具配置.xlsx";
                _outPath = Application.dataPath + "/HotUpdateResources/TextAsset/";
                _outFileName = "prop_cfg";
            }
            if (GUILayout.Button("导出产业配置",GUILayout.Height(50)))
            {
                _excelPath = Application.dataPath + "/../Config/产业配置.xlsx";
                _outPath = Application.dataPath + "/HotUpdateResources/TextAsset/";
                _outFileName = "property_cfg";
            }
        });
        
        
        GUILayout.Space(10);

        MakeHorizontal(GetSpace(0.1f), () =>
        {
            GUI.enabled = false;
            EditorGUILayout.LabelField("表格文件路径");
            GUI.enabled = true;
        });
        MakeHorizontal(GetSpace(0.1f), () =>
        {
            GUI.enabled = false;
            EditorGUILayout.TextField(_excelPath);
            GUI.enabled = true;
        });
        MakeHorizontal(GetSpace(0.1f), () =>
        {
            if (GUILayout.Button("选择文件"))
            {
                _excelPath =
                    EditorUtility.OpenFilePanel("选择表格文件",
                        string.IsNullOrEmpty(_excelPath) ? Application.dataPath + "/../Config/" : _excelPath, "xlsx");
            }
        });
        GUILayout.Space(10);

        MakeHorizontal(GetSpace(0.1f), () =>
        {
            GUI.enabled = false;
            EditorGUILayout.LabelField("输出文件夹");
            GUI.enabled = true;
        });
        MakeHorizontal(GetSpace(0.1f), () =>
        {
            GUI.enabled = false;
            EditorGUILayout.TextField(_outPath);
            GUI.enabled = true;
        });
        MakeHorizontal(GetSpace(0.1f), () =>
        {
            if (GUILayout.Button("选择路径"))
            {
                _outPath =
                    EditorUtility.OpenFolderPanel("选择输出路径",
                        string.IsNullOrEmpty(_outPath)
                            ? Application.dataPath + "/HotUpdateResources/TextAsset/"
                            : _outPath, "");
            }
        });
        GUILayout.Space(10);

        MakeHorizontal(GetSpace(0.1f), () => { _outFileName = EditorGUILayout.TextField("输出文件名", _outFileName); });
        GUILayout.Space(30);

        MakeHorizontal(GetSpace(0.1f), async () =>
        {
            if (GUILayout.Button("导出",GUILayout.Height(30)))
            {
                var arg1 = _excelPath;
                var arg2 = new DirectoryInfo(Application.dataPath + "/../Server/Config/").FullName + "/" +
                           _outFileName + (_outFileName.EndsWith(".json") ? "" : ".json");
                var arg3 = _outPath + "/" + _outFileName + (_outFileName.EndsWith(".json") ? "" : ".json");
                var fullArg = $" {arg1} {arg2} {arg3}";
                var path = new DirectoryInfo(Application.dataPath + "/../Config/").FullName;
                //因为垃圾mac带参数去调python脚本有问题，这里直接创bash去调用py
                using (StreamWriter sw = new StreamWriter(path + "export.sh"))
                {
                    sw.Write(
                        $@"#!/bin/bash
python3 {path}export.py {fullArg}");
                }

                //执行一个Python脚本
                Process p = new Process();
                p.StartInfo.FileName = "/System/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal";
                p.StartInfo.Arguments = path + "export.sh";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WorkingDirectory = path;
                p.Start();
                EditorUtility.DisplayDialog("注意", "关闭终端弹窗后Unity会响应", "关闭");
                await Task.Delay(500);
                p.WaitForExit();
                AssetDatabase.Refresh();
            }
        });
    }


    private void MakeHorizontal(int space, Action act)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(space);
        act();
        GUILayout.Space(space);
        GUILayout.EndHorizontal();
    }
        
    private int GetSpace(float percentage)
    {
        int result = (int) (position.width * percentage);
        return result;
    }
}