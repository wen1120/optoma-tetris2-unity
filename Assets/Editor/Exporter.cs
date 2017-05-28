using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class Exporter {
	public static void export() {

		string[] args = System.Environment.GetCommandLineArgs();
		Debug.Log ("args: "+string.Join (", ", args));
		string path = args [args.Length - 1];

		BuildPlayerOptions options = new BuildPlayerOptions();

		options.scenes = getScenes();
		options.target = BuildTarget.Android;
		options.locationPathName = path;
		options.options = BuildOptions.AcceptExternalModificationsToPlayer;

		BuildPipeline.BuildPlayer (options);
	}

	private static string[] getScenes() {
		// from http://answers.unity3d.com/questions/10482/accessing-buildsettings-from-buildsettingsasset.html
		return (from scene in EditorBuildSettings.scenes
		        where scene.enabled
		        select scene.path).ToArray ();
	}
}
