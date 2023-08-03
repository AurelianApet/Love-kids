//----------------------------------------------
// FunnyCamera - realtime camera effects
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using CWALK;

public class ItemPlaneController : MonoBehaviour {

	float perItemSplitWidth = 0.0f;
	float perItemSplitHeight = 0.0f;

	public PoivtMode e_ItemPoivtMode;
	public float e_DurationTime;
	public float e_DelayTime;
	private Vector3 orgScaleVec;
	private Vector3 orgPosVec;
	private Vector3 targetPlaneScale;
	private Vector3 targetPlaneShowScale;
	private Vector3 targetPlanePos;
	float itemWidth;
	float itemHeight;
	bool isSelected = false;

	// Use this for initialization
	void Start () {

		GameObject targetPlane = GameObject.Find ("MainPlane");
		targetPlaneScale = targetPlane.transform.localScale;
        targetPlaneScale = new Vector3(0.5f, targetPlaneScale.y, 0.65f);
		
		#if UNITY_EDITOR||UNITY_WEBPLAYER
		targetPlaneShowScale.Set (-1 * targetPlaneScale.x, targetPlaneScale.y, targetPlaneScale.z);
		#elif UNITY_ANDROID
		targetPlaneShowScale.Set (targetPlaneScale.z, targetPlaneScale.y,  -1 * targetPlaneScale.x);
		#elif UNITY_IOS
		targetPlaneShowScale.Set (targetPlaneScale.z, targetPlaneScale.y, targetPlaneScale.x);
		#endif
		
		targetPlanePos = targetPlane.transform.position;
		targetPlanePos.Set (targetPlanePos.x, targetPlanePos.y, transform.position.z-1);
		
		float Screenheight = (float)Camera.main.orthographicSize* 2.0f; 
		float Screenwidth = Screenheight * Screen.width / Screen.height;
		float height = Screenheight / 10.0f;
		float width = Screenwidth / 10.0f;
		
		perItemSplitWidth = width / 20.0f;
		perItemSplitHeight = perItemSplitWidth;
		
		itemWidth = (width - perItemSplitWidth) / 2.0f;
		itemHeight = (height - perItemSplitHeight) / 2.0f;
		
		/*#if UNITY_EDITOR||UNITY_WEBPLAYER
		transform.localScale = new Vector3( -1 * itemWidth, 1.0f, itemHeight);
		transform.localEulerAngles = new Vector3 (90, -180, 0);
		#elif UNITY_ANDROID
		transform.localScale = new Vector3(itemHeight, 1.0f, -1 * itemWidth);
		transform.localEulerAngles = new Vector3 (0, 90, -90);
		#elif UNITY_IOS
		transform.localScale = new Vector3(itemHeight, 1.0f, itemWidth);
		transform.localEulerAngles = new Vector3 (0, -90, 90);
		#endif
         * */
		float posx;
		float posy;
		
		perItemSplitWidth = Screenwidth / 20.0f;
		perItemSplitHeight = perItemSplitWidth;
		
		if (e_ItemPoivtMode == PoivtMode.TLMode) {
			posx = (Screenwidth / 2.0f )/2.0f * -1;
			posy = (Screenheight / 2.0f )/2.0f;
		} else if (e_ItemPoivtMode == PoivtMode.TRMode) {
			posx = (Screenwidth / 2.0f)/2.0f;
			posy = (Screenheight / 2.0f )/2.0f;
		} else if (e_ItemPoivtMode == PoivtMode.BLMode) {
			posx = (Screenwidth / 2.0f )/2.0f * -1;
			posy = (Screenheight / 2.0f)/2.0f * -1;
		} else if (e_ItemPoivtMode == PoivtMode.BRMode) {
			posx = (Screenwidth / 2.0f )/2.0f;
			posy = (Screenheight / 2.0f )/2.0f * -1;
		} else {
			posx =0;
			posy = 0;
		}
		
		//this.transform.localPosition = new Vector3(posx,posy,0f);
		orgScaleVec = transform.localScale;
		orgPosVec = transform.position;

        showItem();

        
	}

	public void changeTranformByCameraMode(CameraMode mode)
	{
		if (CameraMode.FRONT == mode||
		    CameraMode.DEFAULT == mode) {


			#if UNITY_EDITOR||UNITY_WEBPLAYER
			targetPlaneShowScale.Set (-1 * targetPlaneScale.x, targetPlaneScale.y, targetPlaneScale.z);
			#elif UNITY_ANDROID
			targetPlaneShowScale.Set (targetPlaneScale.z, targetPlaneScale.y,  -1 * targetPlaneScale.x);
			#elif UNITY_IOS
			targetPlaneShowScale.Set (targetPlaneScale.z, targetPlaneScale.y, targetPlaneScale.x);
			#endif

			if(isSelected)
			{
				#if UNITY_EDITOR||UNITY_WEBPLAYER
				transform.localScale = targetPlaneShowScale;
				transform.localEulerAngles = new Vector3 (90, -180, 0);
				orgScaleVec = new Vector3 (-1 * itemWidth, 1.0f, -1 * itemHeight);
				#elif UNITY_ANDROID
				transform.localScale = targetPlaneShowScale;
				transform.localEulerAngles = new Vector3 (0, 90, -90);
				orgScaleVec = new Vector3(itemHeight, 1.0f, -1 * itemWidth);
				#elif UNITY_IOS
				transform.localScale = targetPlaneShowScale;
				transform.localEulerAngles = new Vector3 (0, -90, 90);
				orgScaleVec = new Vector3(itemHeight, 1.0f, itemWidth);
				#endif
			}
			else
			{
				#if UNITY_EDITOR||UNITY_WEBPLAYER
				transform.localScale = new Vector3 (-1 * itemWidth, 1.0f, -1 * itemHeight);
				transform.localEulerAngles = new Vector3 (90, -180, 0);
				#elif UNITY_ANDROID
				transform.localScale = new Vector3(itemHeight, 1.0f, -1 * itemWidth);
				transform.localEulerAngles = new Vector3 (0, 90, -90);
				#elif UNITY_IOS
				transform.localScale = new Vector3(itemHeight, 1.0f, itemWidth);
				transform.localEulerAngles = new Vector3 (0, -90, 90);
				
				#endif
				orgScaleVec = transform.localScale;
			}


		} else {

			#if UNITY_EDITOR||UNITY_WEBPLAYER
			targetPlaneShowScale.Set (-1 * targetPlaneScale.x, targetPlaneScale.y, targetPlaneScale.z);
			#elif UNITY_ANDROID
			targetPlaneShowScale.Set (targetPlaneScale.z, targetPlaneScale.y, targetPlaneScale.x);
			#elif UNITY_IOS
			targetPlaneShowScale.Set (targetPlaneScale.z, targetPlaneScale.y, -1* targetPlaneScale.x);
			#endif

			if(isSelected)
			{
				#if UNITY_EDITOR||UNITY_WEBPLAYER
				transform.localScale = targetPlaneShowScale;
				transform.localEulerAngles = new Vector3 (90, -180, 0);
				orgScaleVec = new Vector3 (-1 * itemWidth, 1.0f, itemHeight);
				#elif UNITY_ANDROID
				transform.localScale = targetPlaneShowScale;
				transform.localEulerAngles = new Vector3 (0,-90, 90);
				orgScaleVec = new Vector3(itemHeight, 1.0f, itemWidth);
				#elif UNITY_IOS
				transform.localScale = targetPlaneShowScale;
				transform.localEulerAngles = new Vector3 (0, -90, 90);
				orgScaleVec = new Vector3(itemHeight, 1.0f, -1 * itemWidth);
				#endif

			}
			else
			{
				#if UNITY_EDITOR||UNITY_WEBPLAYER
				transform.localScale = new Vector3 (-1 * itemWidth, 1.0f, itemHeight);
				transform.localEulerAngles = new Vector3 (90, -180, 0);
				#elif UNITY_ANDROID
				transform.localScale = new Vector3(itemHeight, 1.0f, itemWidth);
				transform.localEulerAngles = new Vector3 (0,-90, 90);
				#elif UNITY_IOS
				transform.localScale = new Vector3(itemHeight, 1.0f, -1 * itemWidth);
				transform.localEulerAngles = new Vector3 (0, -90, 90);
				#endif

				orgScaleVec = transform.localScale;
			}

		}
        //this.transform.localScale = new Vector3(-0.75f, this.transform.localScale.y, this.transform.localScale.z);
	}

	public void showItem()
	{
		this.gameObject.transform.DOScale (targetPlaneShowScale, e_DurationTime).SetEase (Ease.InOutQuad);
		this.gameObject.transform.DOMove (targetPlanePos, e_DurationTime).SetEase (Ease.InOutQuad);

        
		isSelected = true;
	}

	public void hideItem()
	{
		this.gameObject.transform.DOScale (orgScaleVec, e_DurationTime).SetEase (Ease.InOutQuad);
		this.gameObject.transform.DOMove (orgPosVec, e_DurationTime).SetEase (Ease.InOutQuad);


		isSelected = false;
	}


}
