using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public static class SocialWorkerPostProcessBuild
{
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
		if (target != BuildTarget.iOS) return;

        string path = Path.Combine(pathToBuiltProject, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(path);

        PlistElementArray schemes = plist.root["LSApplicationQueriesSchemes"] as PlistElementArray;
        if (schemes == null)
        {
            schemes = plist.root.CreateArray("LSApplicationQueriesSchemes");
        }
        string[] scheveValues = schemes.values.Select(e => (e as PlistElementString).value).ToArray();
        if (!scheveValues.Contains("line"))
        {
            schemes.AddString("line");
        }
        if (!scheveValues.Contains("instagram"))
        {
            schemes.AddString("instagram");
        }
        plist.WriteToFile(path);
    }
}