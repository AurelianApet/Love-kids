//----------------------------------------------
// Panel Mesh Editor
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using CWALK;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MeshSprite : MonoBehaviour
{
	Vector3[] verticesArr ;
	Vector2[] uvsArr;
	int[] traingVerArr;
	private int verticesWidthNo = 0;
	private int verticesHeightNo = 0;
	private bool isAdjustForX = false;
	private bool isAdjustForY = false;
	
	private float fAdjustValueX = 0f;
	private float fAdjustValueY = 0f;
	
	float subHorWidth = 0f;
	float horTopHeight =0f;
	float horBottomHeight =0f;
	
	float subVerHeight = 0f;
	float verLeftWidth = 0f;
	float verRightWidth = 0f;

	int VerticeArrLength = 0;

	[HideInInspector][SerializeField]
	private MeshSpriteData m_MeshSpriteData;

	public void Init()
	{
		
	}
	
	public void Init(float width,float height,MeshSpriteMode mode,Texture tex)
	{
		this.m_MeshWidth = width;
		this.m_MeshHeight = height;
		this.m_MeshMode = mode;
		this.m_MainTexture = tex;
	}
	
	[HideInInspector][SerializeField]
	private MeshSpriteMaker m_MeshSpriteMaker;
	public MeshSpriteMaker meshSpriteMaker
	{
		get
		{
			return m_MeshSpriteMaker;
		}
		set
		{
			if(value != null)
			{
				m_MeshSpriteMaker = value;
			//	#if UNITY_EDITOR
				if(m_MeshSpriteMaker.textureList.Count>0)
				{
					mainTexture = m_MeshSpriteMaker.textureList[0];
					m_MeshSpriteData = m_MeshSpriteMaker.items[0];
					m_MeshMode = m_MeshSpriteData.Mode;
					if(m_MeshSpriteData !=null)
					{
						UpdateMesh(m_MeshSpriteData);
					}
				}
				else
				{
					Debug.LogWarning("The MeshMaker Perfab have null item,Please Edit it!");
				}

		//		#endif
				
			}
			else
			{
				m_MeshSpriteMaker = null;
				
				m_MeshSpriteData = null;
				mainTexture = null;
				
				Clear();
				
			}
		}
		
	}
	
	
	[HideInInspector][SerializeField]
	private float m_MeshWidth = 1;
	public float Width
	{
		get
		{
			return this.m_MeshWidth;
		}
		set
		{
			this.m_MeshWidth = value;
			
			if(m_MeshSpriteData != null)
			{
				UpdateMesh(m_MeshSpriteData);
			}else
			{
				if(mainTexture != null)
				{
					m_MeshSpriteData =  getItemDataByName(mainTexture.name);
					if(m_MeshSpriteData != null)
					{	
						UpdateMesh(m_MeshSpriteData);
					}
					else
					{
					
					}
				}
				
			}
		}
	}
	
	[HideInInspector][SerializeField]
	private float m_MeshHeight = 1;
	public float Height
	{
		get
		{
			return this.m_MeshHeight;
		}
		set
		{
			this.m_MeshHeight = value;
			if(m_MeshSpriteData != null)
			{
				UpdateMesh(m_MeshSpriteData);
			}else
			{
				if(mainTexture != null)
				{
					m_MeshSpriteData =  getItemDataByName(mainTexture.name);
					if(m_MeshSpriteData != null)
					{	
						UpdateMesh(m_MeshSpriteData);
					}
					else
					{
						Debug.Log("texture is null");
					}
				}
				
			}
		}
	}
	
	[HideInInspector][SerializeField]
	private MeshSpriteMode m_MeshMode;
	public MeshSpriteMode meshMode
	{
		get
		{
			return this.m_MeshMode;
		}
		set
		{
			MeshSpriteMode tempMode = m_MeshMode;
			this.m_MeshMode = value;
	
			if(m_MeshSpriteData != null) 
			{
				if((m_MeshSpriteData.Mode == MeshSpriteMode.NINE_SLICED|| m_MeshSpriteData.Mode == MeshSpriteMode.DEFAULT)&& 
				   value == MeshSpriteMode.TILED_SLICED)
				{
					m_MeshMode = tempMode;
					Debug.LogWarning("this Texture not support Tiled sliced type,please edit it in the SpriteMaker perfab!");
				}
				else if(m_MeshSpriteData.Mode == MeshSpriteMode.DEFAULT && value == MeshSpriteMode.NINE_SLICED)
				{
					m_MeshMode = tempMode;
					Debug.LogWarning("this Texture not support Nine Sliced sliced type,please edit it in the SpriteMaker perfab!");
				}
				else
				{
					UpdateMesh(m_MeshSpriteData);
				}
			}
			else
            {
				if(mainTexture != null)
				{
					m_MeshSpriteData =  getItemDataByName(mainTexture.name);
					if(m_MeshSpriteData != null)
					{
						UpdateMesh(m_MeshSpriteData);
					}
				}
				else
				{
					Debug.Log("texture is null");
				}
				
			}
		}
	}

	[HideInInspector][SerializeField]
	private Texture m_MainTexture;
	public Texture mainTexture
	{
		get
		{
			#if UNITY_EDITOR
			if(GetComponent<Renderer>().sharedMaterial != null)
			{
				m_MainTexture = GetComponent<Renderer>().sharedMaterial.mainTexture;
			}
			#elif UNITY_ANDROID||UNITY_IOS||UNITY_WEBPLAYER ||UNITY_WEBGL 
			if(GetComponent<Renderer>().material != null)
			{
				m_MainTexture = GetComponent<Renderer>().material.mainTexture;
			}
			#endif
			/*else    //add by common
			{
				m_MainTexture = null;
			}*/
			return this.m_MainTexture;
		}
		set
		{
			#if UNITY_EDITOR
			if(GetComponent<Renderer>().sharedMaterial != null)
			{
				GetComponent<Renderer>().sharedMaterial.mainTexture = value;
			}
			#elif UNITY_ANDROID||UNITY_IOS||UNITY_WEBPLAYER ||UNITY_WEBGL 
			if(GetComponent<Renderer>().material != null)
			{
				GetComponent<Renderer>().material.mainTexture = value;
			}
			#endif
			
			this.m_MainTexture = value;
			
			#if UNITY_EDITOR
			if(value != null)
			{
				m_MeshSpriteData =  getItemDataByName(value.name);
				if(m_MeshSpriteData != null)
				{
					if(m_MeshMode > m_MeshSpriteData.Mode)
					{
						m_MeshMode = m_MeshSpriteData.Mode;
					}
					UpdateMesh(m_MeshSpriteData);
				}
			}
			else 
			{
				Clear();
			}
			#endif
		}
	}

	private string m_SpriteName;
	public string SpriteName
	{
		get
		{
			return m_MeshSpriteData.name;
		}
		set
		{
			m_SpriteName = value;
			if( !string.IsNullOrEmpty( m_SpriteName))
			{
				m_MeshSpriteData =  getItemDataByName(m_SpriteName);
				setMaterialMainTex(getTextureByName(m_SpriteName));
				if(m_MeshSpriteData != null)
				{
					m_MeshMode = m_MeshSpriteData.Mode;
					UpdateMesh(m_MeshSpriteData);
				}
			}
			else 
			{
				Clear();
			}
		}
	}

	public void setMaterialMainTex(Texture tex)
	{
		m_MainTexture = tex;
#if UNITY_EDITOR

		GetComponent<Renderer>().sharedMaterial.mainTexture = tex;
		#elif UNITY_ANDROID||UNITY_IOS||UNITY_WEBPLAYER ||UNITY_WEBGL 
		GetComponent<Renderer>().material.mainTexture = tex;
#endif
	}
	
	public void setMeshMode(MeshSpriteMode mode)
	{
		m_MeshMode = mode;
	}

	public void selectDefaultTex()
	{
		for (int i = 0; i!= m_MeshSpriteMaker.textureList.Count; i++) {
			if(m_MeshSpriteMaker.textureList[i] != null)
			{
				mainTexture = m_MeshSpriteMaker.textureList[i];
			}
		}
	}

	public Material material
	{
		get
		{
			#if UNITY_EDITOR
			if(GetComponent<Renderer>().sharedMaterial != null)
			{
				return GetComponent<Renderer>().sharedMaterial;
			}
            #elif UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL || UNITY_WEBPLAYER
			if(GetComponent<Renderer>().material != null)
			{
				return GetComponent<Renderer>().material;
			}
            #endif
            /* add by common
			else
			{
				return null;
			}*/
            return null;
        }
	}
	

	
	void Awake() 
	{
		Init ();
	}
	
	public void Clear()
	{
		Mesh mesh = null;
		#if UNITY_EDITOR
		mesh = GetComponent<MeshFilter>().sharedMesh ;
		
		#elif UNITY_ANDROID||UNITY_IOS ||UNITY_WEBPLAYER
		mesh = GetComponent<MeshFilter>().mesh ;
		#endif
		if (mesh != null) {
			GetComponent<MeshFilter>().mesh = null;
		}
	}
	
	public void UpdateMesh(string texName)
	{
		MeshSpriteData data = getItemDataByName (texName);
		if (data != null) {
			m_MeshMode = data.Mode;
            m_MeshSpriteData = data;
			UpdateMesh (data);
		} else {
			Debug.Log("currieng mesh data is null");
		}
	}

	/// <summary>
	/// Update the Mesh
	/// </summary>
	/// <param name="data">Data. the custom class of frame data</param>
	/// <param name="fullscreen">If set to <c>true<s/c> is fullscreen ?.</param>
	public void UpdateMesh()
	{
		MeshSpriteData data = getItemDataByName (mainTexture.name);
		if (data != null) {
			if(m_MeshMode > data.Mode)
			{
				m_MeshMode = data.Mode;
			}
			m_MeshSpriteData = data;
			UpdateMesh (data);
		} else {
			Debug.Log("current mesh data is null");
		}
	}

	/// <summary>
	/// Creates the frame effect.
	/// </summary>
	/// <param name="data">Data. the custom class of frame data</param>
	/// <param name="fullscreen">If set to <c>true</c> is fullscreen ?.</param>
	void UpdateMesh(MeshSpriteData data)
	{
		if (m_MeshMode == MeshSpriteMode.NINE_SLICED) {

			if(data.LeftBorder<1||data.RightBorder<1||data.BottomBorder<1||data.TopBorder<1)
			{
				UpdateDefaultMesh();
			}
			else
			{
				UpdateSlicedMesh (data);
			}
		} else if (m_MeshMode == MeshSpriteMode.TILED_SLICED) {
			if(data.LeftBorder<1||data.RightBorder<1||data.BottomBorder<1||data.TopBorder<1)
			{
				UpdateDefaultMesh();
			}
			else
			{
				UpdateSlicedTileMeshSuper (data);
			}
		} else if (m_MeshMode == MeshSpriteMode.DEFAULT) {
			
			UpdateDefaultMesh();
		}
	}
	
	/// <summary>
	/// Updates the default mesh.  
	/// </summary>
	void UpdateDefaultMesh()
	{
		Mesh mesh = null;
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		
		//mesh.vertices  = new Vector3[] {
		verticesArr  = new Vector3[] {
			new Vector3(0, 0, 0), 
			new Vector3(m_MeshWidth, 0, 0), 
			new Vector3( 0, m_MeshHeight,0), 
			new Vector3(m_MeshWidth,m_MeshHeight, 0),
			
		};
		
		for (int i = 0; i!=verticesArr.Length; i++) {
			verticesArr[i] = new Vector3(verticesArr[i].x - m_MeshWidth/2.0f,verticesArr[i].y-m_MeshHeight/2.0f,verticesArr[i].z);
		}
		
		mesh.vertices = verticesArr;
		
		mesh.uv = new Vector2[] {
			new Vector2(0, 0), new Vector2(1, 0),
			new Vector2(0, 1), new Vector2(1, 1)
		};
		
		mesh.triangles = new int[] {
			0, 2, 3,
			0, 3, 1
		};
		mesh.RecalculateNormals ();
	}
	
	/// <summary>
	/// Updates the sliced mesh.
	/// </summary>
	/// <param name="data">Data.</param>
	void UpdateSlicedMesh(MeshSpriteData data)
	{
		if (data == null) {
			return ;
		}
		
		subHorWidth = (data.texWidth - data.RightBorder - data.LeftBorder)*1.0f/data.texWidth;
		horTopHeight = data.TopBorder*1.0f/data.texHeight;
		horBottomHeight = data.BottomBorder*1.0f/data.texHeight;
		
		subVerHeight = (data.texHeight - data.TopBorder - data.BottomBorder)*1.0f/data.texWidth;
		verLeftWidth = data.LeftBorder*1.0f/data.texWidth;
		verRightWidth = data.RightBorder*1.0f/data.texWidth;
		
		Mesh mesh = null;
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		
		//mesh.vertices  = new Vector3[] {
		verticesArr  = new Vector3[] {
			new Vector3(0, 0, 0), 
			new Vector3(verLeftWidth, 0, 0), 
			new Vector3(m_MeshWidth-verRightWidth, 0, 0), 
			new Vector3(m_MeshWidth, 0, 0),
			
			new Vector3(0, horTopHeight, 0), 
			new Vector3(verLeftWidth, horTopHeight, 0), 
			new Vector3(m_MeshWidth-verRightWidth,  horTopHeight, 0), 
			new Vector3(m_MeshWidth,  horTopHeight, 0),
			
			new Vector3(0, m_MeshHeight-horBottomHeight, 0), 
			new Vector3(verLeftWidth, m_MeshHeight-horBottomHeight, 0), 
			new Vector3(m_MeshWidth-verRightWidth, m_MeshHeight-horBottomHeight, 0), 
			new Vector3(m_MeshWidth, m_MeshHeight-horBottomHeight,0),
			
			new Vector3(0, m_MeshHeight, 0), 
			new Vector3(verLeftWidth, m_MeshHeight, 0), 
			new Vector3(m_MeshWidth-verRightWidth, m_MeshHeight, 0), 
			new Vector3(m_MeshWidth, m_MeshHeight, 0)
		};
		
		for (int i = 0; i!=verticesArr.Length; i++) {
			verticesArr[i] = new Vector3(verticesArr[i].x - m_MeshWidth/2.0f,verticesArr[i].y-m_MeshHeight/2.0f,verticesArr[i].z);
		}
		mesh.vertices = verticesArr;
		
		mesh.uv = new Vector2[] {
			new Vector2(0, 0), new Vector2(verLeftWidth, 0), new Vector2(1-verRightWidth, 0), new Vector2(1, 0),
			new Vector2(0, horTopHeight), new Vector2(verLeftWidth, horTopHeight), new Vector2(1-verRightWidth, horTopHeight), new Vector2(1, horTopHeight),
			new Vector2(0, 1-horBottomHeight), new Vector2(verLeftWidth, 1-horBottomHeight), new Vector2(1-verRightWidth, 1-horBottomHeight), new Vector2(1,1-horBottomHeight),
			new Vector2(0, 1), new Vector2(verLeftWidth, 1), new Vector2(1-verRightWidth, 1), new Vector2(1, 1)
		};
		
		mesh.triangles = new int[] {
			0, 4, 5,
			0, 5, 1,
			1, 5, 6,
			1, 6, 2,
			2, 6, 7,
			2, 7, 3,
			4, 8, 9,
			4, 9, 5, 
			5, 9, 10,
			5, 10, 6,
			6, 10, 11,
			6, 11, 7,
			8, 12, 13,
			8, 13, 9,
			9, 13, 14,
			9, 14, 10,
			10, 14, 15,
			10, 15, 11
		};
		mesh.RecalculateNormals ();
	}
	
	/// <summary>
	/// Updates the sliced tile mesh.
	/// </summary>
	/// <param name="data">Data.</param>
	void UpdateSlicedTileMesh(MeshSpriteData data)
	{
		if (data == null) {
			return ;
		}
		
		subHorWidth = (data.texWidth - data.RightBorder - data.LeftBorder)*1.0f/data.texWidth;
		horTopHeight = data.TopBorder*1.0f/data.texHeight;
		horBottomHeight = data.BottomBorder*1.0f/data.texHeight;
		
		subVerHeight = (data.texHeight - data.TopBorder - data.BottomBorder)*1.0f/data.texWidth;
		verLeftWidth = data.LeftBorder*1.0f/data.texWidth;
		verRightWidth = data.RightBorder*1.0f/data.texWidth;
		
		Mesh mesh = null;
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		
		isAdjustForX = false;
		isAdjustForY = false;
		
		verticesWidthNo = Mathf.CeilToInt ((m_MeshWidth - verLeftWidth -verRightWidth ) / subHorWidth) +3;
		verticesHeightNo = Mathf.CeilToInt ((m_MeshHeight - horTopHeight - horBottomHeight) / subVerHeight) +3;
		
		fAdjustValueX = ((m_MeshWidth - verLeftWidth -verRightWidth) / subHorWidth) - 
			Mathf.Floor ((m_MeshWidth - verLeftWidth -verRightWidth) / subHorWidth);
		fAdjustValueY = ((m_MeshHeight - horTopHeight - horBottomHeight) / subVerHeight) - 
			Mathf.Floor ((m_MeshHeight  - horTopHeight - horBottomHeight) / subVerHeight);
		
		verticesWidthNo = verticesWidthNo+1;
		verticesHeightNo = verticesHeightNo+1;
		if ((Mathf.CeilToInt ((m_MeshWidth - verLeftWidth -verRightWidth) / subHorWidth) % 2) == 0) {
			isAdjustForX = true;
		}
		
		if ((Mathf.CeilToInt ((m_MeshHeight - horTopHeight - horBottomHeight) / subVerHeight) % 2) == 0) {
			
			isAdjustForY = true;
		}
		
		createVertice ();
		createTriangles();
		CreateUV ();
		
		if (verticesArr.Length > 0) {
			mesh.vertices = verticesArr;	
		}
		if (uvsArr.Length > 0) {
			mesh.uv = uvsArr;	
		}
		if (traingVerArr.Length > 0) {
			mesh.triangles  = traingVerArr;
		}
		mesh.RecalculateNormals ();
	}

	/// <summary>
	/// Updates the sliced tile mesh.
	/// </summary>
	/// <param name="data">Data.</param>
	void UpdateSlicedTileMeshSuper(MeshSpriteData data)
	{
		if (data == null) {
			return ;
		}
		
		subHorWidth = (data.texWidth - data.RightBorder - data.LeftBorder)*1.0f/data.texWidth;
		horTopHeight = data.TopBorder*1.0f/data.texHeight;
		horBottomHeight = data.BottomBorder*1.0f/data.texHeight;
		
		subVerHeight = (data.texHeight - data.TopBorder - data.BottomBorder)*1.0f/data.texWidth;
		verLeftWidth = data.LeftBorder*1.0f/data.texWidth;
		verRightWidth = data.RightBorder*1.0f/data.texWidth;
		
		Mesh mesh = null;
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		
		isAdjustForX = false;
		isAdjustForY = false;
		
		verticesWidthNo = Mathf.CeilToInt ((m_MeshWidth - verLeftWidth -verRightWidth ) / subHorWidth) +3;
		verticesHeightNo = Mathf.CeilToInt ((m_MeshHeight - horTopHeight - horBottomHeight) / subVerHeight) +3;
		
		fAdjustValueX = ((m_MeshWidth - verLeftWidth -verRightWidth) / subHorWidth) - 
			Mathf.Floor ((m_MeshWidth - verLeftWidth -verRightWidth) / subHorWidth);
		fAdjustValueY = ((m_MeshHeight - horTopHeight - horBottomHeight) / subVerHeight) - 
			Mathf.Floor ((m_MeshHeight  - horTopHeight - horBottomHeight) / subVerHeight);
		
		verticesWidthNo = verticesWidthNo+1;
		verticesHeightNo = verticesHeightNo+1;
		if ((Mathf.CeilToInt ((m_MeshWidth - verLeftWidth -verRightWidth) / subHorWidth) % 2) == 0) {
			isAdjustForX = true;
		}
		
		if ((Mathf.CeilToInt ((m_MeshHeight - horTopHeight - horBottomHeight) / subVerHeight) % 2) == 0) {
			
			isAdjustForY = true;
		}
		VerticeArrLength = verticesWidthNo * 2 + verticesHeightNo * 2 + (verticesWidthNo - 4) * 2 + (verticesHeightNo - 4) * 2 +4;
		createVerticeSuper ();
		createTrianglesSuper();
		CreateUVSuper ();
		
		if (verticesArr.Length > 0) {
			mesh.vertices = verticesArr;	
		}
		if (uvsArr.Length > 0) {
			mesh.uv = uvsArr;	
		}
		if (traingVerArr.Length > 0) {
			mesh.triangles  = traingVerArr;
		}
		mesh.RecalculateNormals ();
	}

	
	void createVertice()
	{
		verticesArr = new Vector3[verticesWidthNo * verticesHeightNo];
		
		///top left
		for (int i = 0; i!= 2; i++) {//row
			for (int j = 0; j!= 2; j++) {//col
				verticesArr[j + i*verticesWidthNo] = new Vector3(j*verLeftWidth,i *horTopHeight,0);
			}
		}
		
		/// top right
		for (int i = 0; i!= 2; i++) {//row
			for (int j = verticesWidthNo -2,k=0; j!= verticesWidthNo; k++,j++) {//col
				verticesArr[j + i*verticesWidthNo] = new Vector3(m_MeshWidth - verRightWidth + verRightWidth*k,i *horTopHeight,0);
			}
		}
		
		/// bottom left
		
		for (int i = verticesHeightNo - 2,k=0; i!= verticesHeightNo; i++,k++) {//row
			for (int j = 0; j!= 2; j++) {//col
				verticesArr[j + i*verticesWidthNo] = new Vector3(j*verLeftWidth,m_MeshHeight-horBottomHeight + k*horBottomHeight,0);
			}
		}
		
		/// Bottom right
		for (int i = verticesHeightNo - 2,k=0; i!= verticesHeightNo; i++,k++) {//row
			for (int j = verticesWidthNo -2,h=0; j!= verticesWidthNo; j++,h++) {//col
				verticesArr[j + i*verticesWidthNo] = new Vector3(m_MeshWidth - verRightWidth + verRightWidth*h,m_MeshHeight-horBottomHeight + k*horBottomHeight,0);
			}
		}
		
		/// center hor
		for(int i = 0;i != verticesHeightNo;i++ )
		{
			for(int j = 2;j != verticesWidthNo-2;j++)
			{
				if(i<2)
				{
					verticesArr[j + i*verticesWidthNo] = 
						new Vector3(verLeftWidth+(j-1)*subHorWidth,i*horTopHeight,0);
				}
				else if(i >= verticesHeightNo-2)
				{
					verticesArr[j + i*verticesWidthNo] = 
						new Vector3(verLeftWidth+(j-1)*subHorWidth,
						            m_MeshHeight - horBottomHeight + horBottomHeight * (i - (verticesHeightNo-2)),
						            0);
				}
				else
				{
					verticesArr[j + i*verticesWidthNo] = 
						new Vector3(verLeftWidth+(j-1)*subHorWidth,
						            horTopHeight + (i-1)*subVerHeight,
						            0);
				}
				
			}
		}
		
		/// center ver
		for(int i = 2;i != verticesHeightNo-2;i++ )
		{
			for(int j = 0;j != verticesWidthNo;j++)
			{
				if(j<2)
				{
					verticesArr[j + i*verticesWidthNo] = 
						new Vector3(j*verLeftWidth,(i-1)* subVerHeight + horTopHeight,0);
				}
				else if(j >=verticesWidthNo-2)
				{
					verticesArr[j + i*verticesWidthNo] = 
						new Vector3(m_MeshWidth - verRightWidth +verRightWidth *(j-(verticesWidthNo-2)),
						            horTopHeight + (i - 1) *subVerHeight,
						            0);
				}
				else
				{
					verticesArr[j + i*verticesWidthNo] = 
						new Vector3(verLeftWidth+(j-1)*subHorWidth,
						            horTopHeight + (i-1)*subVerHeight,
						            0);
				}
			}
		}
		
		for(int i = 0;i != verticesHeightNo;i++ )
		{
			verticesArr[ verticesWidthNo - 3+ i*verticesWidthNo]
			= verticesArr[ verticesWidthNo - 2+ i*verticesWidthNo];
		}
		
		for(int j = 0;j != verticesWidthNo; j++ )
		{
			verticesArr[ (verticesHeightNo- 3) * verticesWidthNo + j]
			= verticesArr[ (verticesHeightNo- 2) * verticesWidthNo+ j];
		}
		
		// set the anchor is center
		for (int i = 0; i!=verticesArr.Length; i++) {
			verticesArr[i] = new Vector3(verticesArr[i].x - m_MeshWidth/2.0f,verticesArr[i].y-m_MeshHeight/2.0f,verticesArr[i].z);
		}
	}
	void createVerticeSuper()
	{
		int VerticeArrLength = verticesWidthNo * 2 + verticesWidthNo * 2 + (verticesHeightNo - 4) * 4 +4;
		
		verticesArr = new Vector3[VerticeArrLength];
		
		//		verticesArr = new Vector3[verticesWidthNo * verticesHeightNo];
		
		///top left
		for (int i = 0; i!= 2; i++) {//row
			for (int j = 0; j!= 2; j++) {//col
				verticesArr[j + i*verticesWidthNo] = new Vector3(j*verLeftWidth,i *this.horTopHeight,0);
			}
		}
		
		/// top right
		for (int i = 0; i!= 2; i++) {//row
			for (int j = verticesWidthNo -2,k=0; j!= verticesWidthNo; k++,j++) {//col
				verticesArr[j + i*verticesWidthNo] = new Vector3(m_MeshWidth - verRightWidth + verRightWidth*k,i *horTopHeight,0);
			}
		}
		
		//top Hor
		for(int i = 0;i != 2;i++ )
		{
			for(int j = 2;j != verticesWidthNo-2;j++)
			{
				verticesArr[j + i*verticesWidthNo] = 
					new Vector3(verLeftWidth+(j-1)*subHorWidth,i*horTopHeight,0);
			}
		}
		
		
		int startPos = verticesWidthNo * 2 + (verticesHeightNo - 4) * 4;
		
		
		/// bottom left
		for (int i = 0,k=0; i!= 2; i++,k++) {//row
			for (int j = 0; j!= 2; j++) {//col
				verticesArr[startPos + j + i*verticesWidthNo] = new Vector3(j*verLeftWidth,
				                                                            m_MeshHeight - horBottomHeight + horBottomHeight * i,
				                                                            0);
			}
		}
		
		/// Bottom right
		for (int i = 0,k=0; i!= 2; i++,k++) {//row
			for (int j = verticesWidthNo -2,h=0; j!= verticesWidthNo; j++,h++) {//col
				verticesArr[startPos + j + i*verticesWidthNo] = new Vector3(m_MeshWidth - verRightWidth + verRightWidth*h,
				                                                            m_MeshHeight - horBottomHeight + horBottomHeight * i,
				                                                            0);
			}
		}
		
		// bottom
		for(int i = 0;i != 2;i++ )
		{
			for(int j = 2;j != verticesWidthNo-2;j++)
			{
				verticesArr[ startPos + j + i*verticesWidthNo] = 
					new Vector3(verLeftWidth+(j-1)*subHorWidth,
					            m_MeshHeight - horBottomHeight + horBottomHeight * i,
					            0);
				
			}
		}
		
		startPos = verticesWidthNo * 2;
		int subLength = 4;
		
		/// left ver
		for(int i = 2,k=0;i != verticesHeightNo-2;i++,k++ )
		{
			for(int j = 0;j != 2;j++)
			{
				verticesArr[ startPos + j + k*subLength] = 
					new Vector3(j*verLeftWidth,
					            (k + 1)* subVerHeight + horTopHeight,
					            0);
			}
		}
		
		
		/// right ver
		for(int i = 2,k=0;i != verticesHeightNo-2;i++,k++ )
		{
			for(int j = subLength - 2;j != subLength;j++)
			{
				verticesArr[startPos + j + k*subLength] = 
					new Vector3(m_MeshWidth - verRightWidth +verRightWidth *(j-(subLength-2)),
					            (i-1)* subVerHeight + horTopHeight,
					            0);
			}
		}
		
		
		startPos = verticesWidthNo * 2 + (verticesHeightNo - 4) * 4;
		/////////	/////////	/////////	///////// adjust the ver vertice	/////////	/////////	/////////	/////////
		//top adjust
		for(int i = 0;i != 2;i++ )
		{
			verticesArr[ verticesWidthNo - 3+ i*verticesWidthNo]
			= verticesArr[ verticesWidthNo - 2+ i*verticesWidthNo];
		}
		
		// bootom adjust
		for(int i = 0;i != 2;i++ )
		{
			verticesArr[startPos + verticesWidthNo - 3+ i*verticesWidthNo]
			= verticesArr[ startPos +verticesWidthNo - 2+ i*verticesWidthNo];
		}
		
		/////	/////////	/////////	///////// end adjust the ver vertice	/////////	/////////	/////////	/////////
		startPos = verticesWidthNo * 2 + (verticesHeightNo - 5) * 4;
		/////////	/////////	/////////	///////// adjust the hor vertice	/////////	/////////	/////////	/////////
		
		for(int j = 0;j != 2; j++ )
		{
			verticesArr[startPos  + j]
			= verticesArr[ startPos + subLength + j];
		}
		
		startPos += 2;
		for(int j = 0;j != 2 ;j++ )
		{
			verticesArr[startPos  + j]
			= verticesArr[ startPos + verticesWidthNo+ j];
		}
		
		/////////	/////////	/////////	///////// end adjust the hor vertice	/////////	/////////	/////////	/////////
		verticesArr[verticesArr.Length - 4] = new Vector3(verLeftWidth,horTopHeight,0);//left top
		verticesArr[verticesArr.Length - 3] = new Vector3(verticesWidthNo - verRightWidth,horTopHeight,0); //right top
		verticesArr[verticesArr.Length - 2] = new Vector3(verLeftWidth,verticesHeightNo -  horBottomHeight,0);
		verticesArr[verticesArr.Length - 1] = new Vector3(verticesWidthNo - verRightWidth,verticesHeightNo -  horBottomHeight,0);
		
		//	verticesArr[j + i*verticesWidthNo] = new Vector3(meshWidth - verRightWidth + verRightWidth*k,i *horTopHeight,0);
		//  verticesArr[j + i*verticesWidthNo] = new Vector3(j*verLeftWidth,i *this.horTopHeight,0);
		// set the anchor is center
		for (int i = 0; i!=verticesArr.Length; i++) {
			verticesArr[i] = new Vector3(verticesArr[i].x - m_MeshWidth/2.0f,verticesArr[i].y-m_MeshHeight/2.0f,verticesArr[i].z);
		}
	}


	void createTriangles()
	{
		List<int> trainglesList = new List<int> ();
		for (int i=0; i != verticesHeightNo-1; i++) {
			for(int j = 0;j!=verticesWidthNo-1;j++)
			{
				int tempValue = i*verticesWidthNo +j;
				trainglesList.Add(tempValue);
				tempValue = tempValue + verticesWidthNo;
				trainglesList.Add(tempValue);
				tempValue = tempValue+1;
				trainglesList.Add(tempValue);
				
				//////////////////////////////////////
				tempValue = i*verticesWidthNo +j;
				trainglesList.Add(tempValue);
				tempValue = tempValue + verticesWidthNo+1;
				trainglesList.Add(tempValue);
				tempValue = i*verticesWidthNo +j;
				tempValue = tempValue+1;
				trainglesList.Add(tempValue);
			}
		}
		
		traingVerArr = new int[trainglesList.Count];
		for (int i = 0; i!=trainglesList.Count; i++) {
			traingVerArr[i] = trainglesList[i];
		}
	}

	void createTrianglesSuper()
	{
		int tempValue = 0;
		
		List<int> trainglesList = new List<int> ();
		for (int i=0; i != 1; i++) {
			for(int j = 0;j!=verticesWidthNo-1;j++)
			{
				tempValue = i*verticesWidthNo +j;
				trainglesList.Add(tempValue);
				tempValue = tempValue + verticesWidthNo;
				trainglesList.Add(tempValue);
				tempValue = tempValue+1;
				trainglesList.Add(tempValue);
				
				//////////////////////////////////////
				tempValue = i*verticesWidthNo +j;
				trainglesList.Add(tempValue);
				tempValue = tempValue + verticesWidthNo+1;
				trainglesList.Add(tempValue);
				tempValue = i*verticesWidthNo +j;
				tempValue = tempValue+1;
				
				trainglesList.Add(tempValue);
			}
		}
		
		int startPos = verticesWidthNo * 2 + (verticesHeightNo - 4) * 4;
		int subLength = 4;
		for (int i=0; i != 1; i++) {
			for(int j = 0;j!=verticesWidthNo-1;j++)
			{
				tempValue = startPos + i +j;
				trainglesList.Add(tempValue);
				tempValue = tempValue + verticesWidthNo;
				trainglesList.Add(tempValue);
				tempValue = tempValue+1;
				trainglesList.Add(tempValue);
				
				//////////////////////////////////////
				tempValue = startPos + i +j;
				trainglesList.Add(tempValue);
				tempValue = tempValue + verticesWidthNo+1;
				trainglesList.Add(tempValue);
				tempValue = startPos + i +j;
				tempValue = tempValue+1;
				
				trainglesList.Add(tempValue);
			}
		}
		
		//------------------------------------------------------------
		startPos =  verticesWidthNo * 2 ;
		
		tempValue =verticesWidthNo;
		trainglesList.Add(tempValue);
		tempValue = startPos;
		trainglesList.Add(tempValue);
		tempValue =startPos +1;
		trainglesList.Add(tempValue);
		
		//////////////////////////////////////
		tempValue = verticesWidthNo ;
		trainglesList.Add(tempValue);
		tempValue = startPos+1;
		trainglesList.Add(tempValue);
		tempValue = verticesWidthNo+1;
		trainglesList.Add(tempValue);
		//------------------------------------------------------------
		
		//------------------------------------------------------------
		startPos =  verticesWidthNo * 2 -2;
		
		tempValue =startPos;
		trainglesList.Add(tempValue);
		tempValue = startPos + subLength;
		trainglesList.Add(tempValue);
		tempValue =tempValue +1;
		trainglesList.Add(tempValue);
		
		//////////////////////////////////////
		tempValue = startPos ;
		trainglesList.Add(tempValue);
		tempValue = startPos+subLength +1;
		trainglesList.Add(tempValue);
		tempValue = startPos+1;
		trainglesList.Add(tempValue);
		//------------------------------------------------------------
		
		startPos = verticesWidthNo * 2;
		//center
		//left
		
		for (int i=0; i != verticesHeightNo-5; i++) {
			for(int j = 0;j!=1;j++)
			{
				tempValue = startPos  +j + i* subLength;
				trainglesList.Add(tempValue);
				tempValue = tempValue + subLength;
				trainglesList.Add(tempValue);
				tempValue = tempValue+1;
				trainglesList.Add(tempValue);
				
				//////////////////////////////////////
				tempValue =  startPos  +j+ i* subLength;
				trainglesList.Add(tempValue);
				tempValue = tempValue + subLength+1;
				trainglesList.Add(tempValue);
				//tempValue = i*verticesWidthNo +j+ i* subLength;
				tempValue = startPos  +j+ i* subLength  +1;
				trainglesList.Add(tempValue);
			}
		}
		
		//right
		for (int i=0; i != verticesHeightNo-5; i++) {
			for(int j = 0;j!=1;j++)
			{
				tempValue =startPos + 2  +j+ i* subLength;
				trainglesList.Add(tempValue);
				tempValue = tempValue + subLength;
				trainglesList.Add(tempValue);
				tempValue = tempValue+1;
				trainglesList.Add(tempValue);
				
				//////////////////////////////////////
				tempValue = startPos + 2  +j+ i* subLength;
				trainglesList.Add(tempValue);
				tempValue = tempValue + subLength+1;
				trainglesList.Add(tempValue);
				tempValue = startPos + 2  +j+ i* subLength+1;
				trainglesList.Add(tempValue);
			}
		}
		
		startPos = verticesWidthNo * 2 + (verticesHeightNo - 5) * 4;
		
		tempValue =verticesWidthNo + 1;
		trainglesList.Add(tempValue);
		tempValue = startPos +1;
		trainglesList.Add(tempValue);
		tempValue =startPos + verticesWidthNo+1;
		trainglesList.Add(tempValue);
		
		
		//////////////////////////////////////
		tempValue = verticesWidthNo + 1;
		trainglesList.Add(tempValue);
		tempValue =startPos + verticesWidthNo+1;
		trainglesList.Add(tempValue);
		tempValue = verticesWidthNo *2 -2;
		
		trainglesList.Add(tempValue);
		
		traingVerArr = new int[trainglesList.Count];
		for (int i = 0; i!=trainglesList.Count; i++) {
			traingVerArr[i] = trainglesList[i];
		}
		return ;
	}

	
	Vector2[] CreateUV()
	{
		uvsArr = new Vector2[verticesWidthNo * verticesHeightNo];
		///top left
		for (int i = 0; i!= 2; i++) {//row
			for (int j = 0; j!= 2; j++) {//col
				uvsArr[j + i*verticesWidthNo] = new Vector2(j*verLeftWidth,i *horTopHeight);
			}
		}
		/// top right
		for (int i = 0; i!= 2; i++) {//row
			for (int j = verticesWidthNo -2,k=0; j!= verticesWidthNo; j++,k++) {//col
				uvsArr[j + i*verticesWidthNo] = new Vector2(1 - verRightWidth + verRightWidth*k,i *horTopHeight);
			}
		}
		
		/// bottom left
		for (int i = verticesHeightNo - 2,k=0; i!= verticesHeightNo; i++,k++) {//row
			for (int j = 0; j!= 2; j++) {//col
				uvsArr[j + i*verticesWidthNo] = new Vector2(j*verLeftWidth,1-horBottomHeight + k*horBottomHeight);
			}
		}
		
		/// Bottom right
		for (int i = verticesHeightNo - 2,k=0; i!= verticesHeightNo; i++,k++) {//row
			for (int j = verticesWidthNo -2,h=0; j!= verticesWidthNo; j++,h++) {//col
				uvsArr[j + i*verticesWidthNo] = new Vector2(1 - verRightWidth + verRightWidth*h,1-horBottomHeight + k*horBottomHeight);
			}
		}
		
		/// top slider
		for(int j = 2;j != verticesWidthNo-2;j++)
		{
			uvsArr[j + 0*verticesWidthNo] = (j%2==0)?new Vector2(1-verRightWidth,0):new Vector2(verLeftWidth,0);
			uvsArr[j + 1*verticesWidthNo] = (j%2==0)?new Vector2(1-verRightWidth,horTopHeight):new Vector2(verLeftWidth,horTopHeight);
		}
		
		/// Bottom slider
		for(int j = 2;j != verticesWidthNo-2;j++)
		{
			uvsArr[j + (verticesHeightNo -2)*verticesWidthNo] = (j%2==0)?new Vector2(1-verRightWidth,1-horBottomHeight):new Vector2(verLeftWidth,1-horBottomHeight);
			uvsArr[j + (verticesHeightNo -1)*verticesWidthNo] = (j%2==0)?new Vector2(1-verRightWidth,1):new Vector2(verLeftWidth,1);
		}
		
		/// left slider
		for(int i = 2;i != verticesHeightNo-2;i++)
		{
			uvsArr[0 + i*verticesWidthNo] = (i%2==0)?new Vector2(0,1-horBottomHeight):new Vector2(0,horTopHeight);
			uvsArr[1 + i*verticesWidthNo] = (i%2==0)?new Vector2(verLeftWidth,1-horBottomHeight):new Vector2(verLeftWidth,horTopHeight);
		}
		
		/// right slider
		for(int i = 2;i != verticesHeightNo-2;i++)
		{
			uvsArr[verticesWidthNo-2 + i*verticesWidthNo] = (i%2==0)?new Vector2(1-verRightWidth,1-horBottomHeight):new Vector2(1-verRightWidth,horTopHeight);
			uvsArr[verticesWidthNo-1 + i*verticesWidthNo] = (i%2==0)?new Vector2(1,1-horBottomHeight):new Vector2(1,horTopHeight);
		}
		
		/// center ver
		for(int i = 2;i != verticesHeightNo-2;i++ )
		{
			bool rowChange = false;
			for(int j = 2;j != verticesWidthNo-2;j++)
			{
				rowChange = !rowChange;
				uvsArr[j + i*verticesWidthNo] =rowChange? new Vector2(verLeftWidth,horTopHeight):new Vector2(1-verRightWidth,1-horBottomHeight);
			}
		}
		
		if (fAdjustValueX > 0) {
			for(int i =0;i != verticesHeightNo;i++)
			{
				if(i>=2 && i<verticesHeightNo-2)
				{
					continue;
				}
				Vector2 tempVec2_1 = uvsArr[verticesWidthNo-3 + i*verticesWidthNo];
				float offectX = 0f;
				if(isAdjustForX)
				{
					offectX = (1 - verLeftWidth - verRightWidth) *(1- fAdjustValueX) + verLeftWidth;
				}
				else
				{
					offectX = (1 - verLeftWidth - verRightWidth) *(fAdjustValueX) + verLeftWidth;
				}
				uvsArr[verticesWidthNo-3 + i*verticesWidthNo] =
					new Vector2(offectX,tempVec2_1.y);
			}
		}
		
		if (fAdjustValueY > 0) {
			for(int j =0;j != verticesWidthNo;j++)
			{
				if(j >= 2 && j < verticesWidthNo-2)
				{
					continue;
				}
				float offectY = 0f;
				Vector2 tempVec2_1 = uvsArr[ j + (verticesHeightNo-3)*verticesWidthNo];
				if(isAdjustForY)
				{
					offectY = (1 - horTopHeight - horBottomHeight) *(1- fAdjustValueY) + horTopHeight;
				}
				else
				{
					offectY = (1 - horTopHeight - horBottomHeight) *(fAdjustValueY) + horTopHeight;
				}
				uvsArr[j +(verticesHeightNo-3) *verticesWidthNo] =
					new Vector2(tempVec2_1.x,offectY);
			}
		}
		
		return uvsArr;
	}

	
	Vector2[] CreateUVSuper()
	{
		uvsArr = new Vector2[VerticeArrLength];
		///top left
		for (int i = 0; i!= 2; i++) {//row
			for (int j = 0; j!= 2; j++) {//col
				uvsArr[j + i*verticesWidthNo] = new Vector2(j*verLeftWidth,i *horTopHeight);
			}
		}
		
		/// top right
		for (int i = 0; i!= 2; i++) {//row
			for (int j = verticesWidthNo -2,k=0; j!= verticesWidthNo; j++,k++) {//col
				uvsArr[j + i*verticesWidthNo] = new Vector2(1 - verRightWidth + verRightWidth*k,i *horTopHeight);
			}
		}
		
		int startPos = verticesWidthNo * 2 + (verticesHeightNo - 4) * 4;
		
		/// bottom left
		for (int i = 0; i!= 2; i++) {//row
			for (int j = 0; j!= 2; j++) {//col
				uvsArr[j + i*verticesWidthNo + startPos] = new Vector2(j*verLeftWidth,1-horBottomHeight + i*horBottomHeight);
			}
		}
		
		/// Bottom right
		for (int i = 0; i!= 2; i++) {//row
			for (int j = verticesWidthNo -2,h=0; j!= verticesWidthNo; j++,h++) {//col
				uvsArr[startPos + j + i*verticesWidthNo] = new Vector2(1 - verRightWidth + verRightWidth*h,
				                                                       1-horBottomHeight + i*horBottomHeight);
			}
		}
		
		/// top slider
		for(int j = 2;j != verticesWidthNo-2;j++)
		{
			uvsArr[j + 0*verticesWidthNo] = (j%2==0)?new Vector2(1-verRightWidth,0):new Vector2(verLeftWidth,0);
			uvsArr[j + 1*verticesWidthNo] = (j%2==0)?new Vector2(1-verRightWidth,horTopHeight):new Vector2(verLeftWidth,horTopHeight);
		}
		
		
		/// Bottom slider
		for(int j = 2,k=0;j != verticesWidthNo-2;k++,j++)
		{
			uvsArr[j  + startPos ] = (j%2==0)?new Vector2(1-verRightWidth,1-horBottomHeight):new Vector2(verLeftWidth,1-horBottomHeight);
			uvsArr[j + verticesWidthNo + startPos] = (j%2==0)?new Vector2(1-verRightWidth,1):new Vector2(verLeftWidth,1);
		}
		
		startPos = verticesWidthNo * 2 ;
		int subLength = 4;
		/// left slider
		for(int i = 2;i != verticesHeightNo-2;i++)
		{
			uvsArr[startPos + 0 + (i-2)*subLength] = (i%2==0)?new Vector2(0,1-horBottomHeight):new Vector2(0,horTopHeight);
			uvsArr[startPos + 1 + (i-2)*subLength] = (i%2==0)?new Vector2(verLeftWidth,1-horBottomHeight):new Vector2(verLeftWidth,horTopHeight);
		}
		
		/// right slider
		for(int i = 2;i != verticesHeightNo-2;i++)
		{
			uvsArr[startPos + subLength-2 + (i-2)*subLength] = (i%2==0)?new Vector2(1-verRightWidth,1-horBottomHeight):new Vector2(1-verRightWidth,horTopHeight);
			uvsArr[startPos + subLength-1 + (i-2)*subLength] = (i%2==0)?new Vector2(1,1-horBottomHeight):new Vector2(1,horTopHeight);
		}
		
		//center adjust
		uvsArr[uvsArr.Length - 4] = new Vector2(verLeftWidth,horTopHeight);//left top
		uvsArr[uvsArr.Length - 3] = new Vector2(1-verRightWidth,1-horBottomHeight);//right top
		uvsArr[uvsArr.Length - 2] = new Vector2(verLeftWidth,horTopHeight);
		uvsArr[uvsArr.Length - 1] = new Vector2(1-verRightWidth,1-horBottomHeight);
		
		startPos = verticesWidthNo * 2 ;
		
		if (fAdjustValueX > 0) {
			
			for(int i =0;i != 2;i++)
			{
				Vector2 tempVec2_1 = uvsArr[verticesWidthNo-3 + i*verticesWidthNo];
				float offectX = 0f;
				if(isAdjustForX)
				{
					offectX = (1 - verLeftWidth - verRightWidth) *(1- fAdjustValueX) + verLeftWidth;
				}
				else
				{
					offectX = (1 - verLeftWidth - verRightWidth) *(fAdjustValueX) + verLeftWidth;
				}
				uvsArr[verticesWidthNo-3 + i*verticesWidthNo] =
					new Vector2(offectX,tempVec2_1.y);
			}
			startPos = verticesWidthNo * 2 + (verticesHeightNo - 4) * 4;
			for(int i =0;i != 2;i++)
			{
				Vector2 tempVec2_1 = uvsArr[startPos + verticesWidthNo-3 + i*verticesWidthNo];
			

				float offectX = 0f;
				if(isAdjustForX)
				{
					offectX = (1 - verLeftWidth - verRightWidth) *(1- fAdjustValueX) + verLeftWidth;
				}
				else
				{
					offectX = (1 - verLeftWidth - verRightWidth) *(fAdjustValueX) + verLeftWidth;
				}
				uvsArr[startPos + verticesWidthNo-3 + i*verticesWidthNo] =
					new Vector2(offectX,tempVec2_1.y);
			}
			
		}
		
		
		if (fAdjustValueY > 0) {
			startPos = verticesWidthNo * 2 + (verticesHeightNo - 5) * 4;
			for(int j =0;j != 2;j++)
			{
				float offectY = 0f;
				Vector2 tempVec2_1 = uvsArr[ j + startPos];
				if(isAdjustForY)
				{
					offectY = (1 - horTopHeight - horBottomHeight) *(1- fAdjustValueY) + horTopHeight;
				}
				else
				{
					offectY = (1 - horTopHeight - horBottomHeight) *(fAdjustValueY) + horTopHeight;
				}
				uvsArr[j + startPos] =
					new Vector2(tempVec2_1.x,offectY);
			}

			startPos = verticesWidthNo * 2 + (verticesHeightNo - 4) * 4-2;

			for(int j =0;j != 2;j++)
			{
				float offectY = 0f;
				Vector2 tempVec2_1 = uvsArr[ j + startPos];
				if(isAdjustForY)
				{
					offectY = (1 - horTopHeight - horBottomHeight) *(1- fAdjustValueY) + horTopHeight;
				}
				else
				{
					offectY = (1 - horTopHeight - horBottomHeight) *(fAdjustValueY) + horTopHeight;
				}
				uvsArr[j + startPos] =
					new Vector2(tempVec2_1.x,offectY);
			}

			
		}
		return uvsArr;
	}
	
	
	private MeshSpriteData getFirstItemDataFromAssemble()
	{
		
		return null;
		
	}
	
	private MeshSpriteData getItemDataByName(string targetName)
	{
		if (m_MeshSpriteMaker == null) {
			Debug.Log("curreing list is length is null" + targetName);
		}
		MeshSpriteData itemdata = null;
		for (int i = 0;  m_MeshSpriteMaker != null &&i < m_MeshSpriteMaker.items.Count ; i++) {
			if(m_MeshSpriteMaker.items[i].name == targetName)
			{
				itemdata = m_MeshSpriteMaker.items[i];
			}
		}
		return itemdata;
	}

	private Texture2D getTextureByName(string targetName)
	{
		if (m_MeshSpriteMaker == null) {
			Debug.Log("curreing list is length is null" + targetName);
		}
		Texture2D itemdata = null;
		for (int i = 0;  m_MeshSpriteMaker != null &&i < m_MeshSpriteMaker.textureList.Count ; i++) {
			if(m_MeshSpriteMaker.textureList[i].name == targetName)
			{
				itemdata = m_MeshSpriteMaker.textureList[i];
			}
		}
		return itemdata;
	}

	void CreateFile(string path,string name,string info)
	{
		//file stream info
		StreamWriter sw;
		FileInfo t = new FileInfo(path+"//"+ name);
		if(!t.Exists)
		{
			//if the file not exsit ,need create new one
			sw = t.CreateText();
		}
		else
		{
			//open file
			sw = t.AppendText();
		}
		//write the file by line
		sw.WriteLine(info);
		//close the file stream
		sw.Close();
		//despose the file
		sw.Dispose();
	}
	
	
}