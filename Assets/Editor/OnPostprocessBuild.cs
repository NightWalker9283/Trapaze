#if UNITY_IPHONE
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
using System.Collections;

public class XcodeSettingsPostProcesser
{
    [PostProcessBuildAttribute(0)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
        // iOS以外のプラットフォームは処理を行わない
        if (buildTarget != BuildTarget.iOS) return;

        // PBXProjectの初期化
        var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);
        string targetGuid = pbxProject.GetUnityFrameworkTargetGuid();//.TargetGuidByName("Unity-iPhone");

        // ここに自動化の処理を記述する
        pbxProject.AddFrameworkToProject(targetGuid, "UserNotifications.framework", false);
        // 設定を反映
        File.WriteAllText(projectPath, pbxProject.WriteToString());

    }
    
}
#endif