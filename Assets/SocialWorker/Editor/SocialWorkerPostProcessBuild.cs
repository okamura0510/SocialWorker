using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.IO;
using System.Text;

public static class SocialWorkerPostProcessBuild
{
	[PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
	{
		if(target != BuildTarget.iOS) { return; }

        string path = Path.Combine(pathToBuiltProject, "Info.plist");
		string text = File.ReadAllText (path, Encoding.UTF8).Replace (
			"  </dict>\n" +
			"</plist>", 

			"    <key>LSApplicationQueriesSchemes</key>\n" +
			"    <array>\n" +
			"      <string>line</string>\n" +
			"      <string>instagram</string>\n" +
			"    </array>\n" +
			"  </dict>\n" +
			"</plist>");
		File.WriteAllText (path, text, Encoding.UTF8);
	}
}