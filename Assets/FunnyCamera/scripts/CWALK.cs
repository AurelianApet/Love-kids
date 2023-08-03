//----------------------------------------------
// FunnyCamera - realtime camera effects
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;

using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

namespace CWALK
{
	public enum CameraMode
	{
		DEFAULT,
		FRONT,
		BACK,
		NONE
	}

	public enum PoivtMode {
		TLMode,
		TRMode,
		BLMode,
		BRMode,
		NONE
	}

	public enum CWALK_BORDER_MODE {
		M1,
		M4,
		M41,
		M108,
		M111,
		M119,
		M120,
		M163,
		NONE
	}

	public enum PAGEMODE
	{
		FLITER_MENU,
		FLITER_PREVIEW,
		DECORATE_MENU,
		DECORATE_FRAME,
		DECORATE_STICKER,
		FLITER_RECO,
		DECORATE_TEXT,
        DECORATE_PAINT,
		NONE
	}

	public class ToolBar
	{
		public static float  ToolBarModelMinSize = 1f;
		public  static float ToolBarModelMaxSize = 3f;

		public static  string audioSpritePath = "Sounds/3DEffect/";
		public static string audioWordPath = "Sounds/Word/";
		public static string audioSpriteBgPath = "Sounds/Bg/";

		public static string ADSID = "a1114b46209b1af4";
		public static  string ADSSKT = "8d53a77ba11c22c0";
	}

	public enum MeshSpriteMode
	{
		DEFAULT,
		NINE_SLICED,
		TILED_SLICED,
		NONE
	}

	[System.Serializable]
	public class MeshSpriteData
	{
		//Lets make two string variables that will hold the name and description of the trophy
	
		public string name ="texture Name";

		public MeshSpriteMode Mode = MeshSpriteMode.DEFAULT;

		public int texWidth =0;

		public int texHeight =0;

		public int LeftBorder =0;

		public int RightBorder =0;

		public int TopBorder =0;

		public int BottomBorder = 0;


	}


}
