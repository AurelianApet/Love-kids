#if !(UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7)
#define UNITY_5_OR_LATER
#endif

using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;


[InitializeOnLoad]
public static class managerAssets
{
	static managerAssets()
	{
		#if UNITY_EDITOR_OSX|| UNITY_IOS
		if (!AssetDatabase.IsValidFolder ("Assets/Plugins")) {
			AssetDatabase.CreateFolder("Assets","Plugins");
		}

		if (AssetDatabase.LoadAssetAtPath<TextAsset> ("Assets/Plugins/ScreenShotIOS.js") == null &&
		    AssetDatabase.LoadAssetAtPath<TextAsset> ("Assets/FunnyCamera/Plugins/ScreenShotIOS.js") != null) {
			AssetDatabase.MoveAsset ("Assets/FunnyCamera/Plugins/ScreenShotIOS.js", "Assets/Plugins/ScreenShotIOS.js");
		}
		AssetDatabase.Refresh ();
	#endif
	}

}
