using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticForceLineArrowDrawer : MonoBehaviour {

	[SerializeField]private BarMagnetMagneticForceLinesDrawer magneticForceLinesDrawer = null; 

	[SerializeField]private GameObject arrowSrcObject = null;

	[SerializeField]private int arrowDrawInterval = 20;

	private List<List<GameObject>> allArrowObjectList = new List<List<GameObject>>();

	[SerializeField]private bool isValidDraw = true;
#if UNITY_EDITOR
	private bool isPrevValidDraw = true;
#endif

	// Use this for initialization
	void Start()
	{
		if( null == magneticForceLinesDrawer ){
			magneticForceLinesDrawer = GetComponent<BarMagnetMagneticForceLinesDrawer>();
		}
		if( null != magneticForceLinesDrawer ){
			for( int i = 0; i < magneticForceLinesDrawer.FetchMagnetForceLineNum(); i ++ ){
				List<GameObject> arrowObjectList = new List<GameObject>();
				Vector3[] posArray = magneticForceLinesDrawer.FetchMagnetForceLinePositionList(i);
				for( int j = 0; j < posArray.Length; j ++ ){
					if( 0 == (j % arrowDrawInterval) ){
						Quaternion arrowDirection = Quaternion.identity;

						arrowDirection = Quaternion.FromToRotation(posArray[Math.Min(j + (arrowDrawInterval / 2), posArray.Length - 1)] - posArray[j], Vector3.up);
//						arrowDirection = Quaternion.FromToRotation(posArray[Math.Min(j + (arrowDrawInterval / 2), posArray.Length - 1)] - posArray[Math.Max(j - (arrowDrawInterval / 2), 0)], Vector3.up);
						arrowObjectList.Add(Instantiate(arrowSrcObject, posArray[j], arrowDirection, transform));
					}
				}
				allArrowObjectList.Add(arrowObjectList);
			}
		}

		return;
	}
	
	// Update is called once per frame
	void Update () {
		
		UpdateArrowTransform( );
		DrawArrow( );

		return;
	}

	//各磁力線情報から、矢印の場所や向きを更新する.
	private void UpdateArrowTransform( )
	{
		if( null == magneticForceLinesDrawer ){
			return;
		}
		for( int i = 0; i < magneticForceLinesDrawer.FetchMagnetForceLineNum(); i ++ ){
			if( allArrowObjectList.Count <= i ){
				break;
			}
			Vector3[] posArray = magneticForceLinesDrawer.FetchMagnetForceLinePositionList(i);
			for( int j = 0; j < posArray.Length; j ++ ){
				if( 0 == (j % arrowDrawInterval) && allArrowObjectList[i].Count > (j / arrowDrawInterval) ){
					Quaternion arrowDirection = Quaternion.identity;

					arrowDirection = Quaternion.FromToRotation(posArray[Math.Min(j + (arrowDrawInterval / 2), posArray.Length - 1)] - posArray[j], Vector3.up);
//					arrowDirection = Quaternion.FromToRotation(posArray[Math.Min(j + (arrowDrawInterval / 2), posArray.Length - 1)] - posArray[Math.Max(j - (arrowDrawInterval / 2), 0)], Vector3.up);
					allArrowObjectList[i][(j / arrowDrawInterval)].transform.SetPositionAndRotation(posArray[j], arrowDirection);
				}
			}
		}

		return;
	}

	//描画する矢印のオブジェクトを管理する(GameObjectの生成と削除を管理).
	public void DrawArrow()
	{
		if( null == magneticForceLinesDrawer ){
			return;
		}
		for( int i = 0; i < magneticForceLinesDrawer.FetchMagnetForceLineNum(); i ++ ){
			List<GameObject> arrowObjectList;
			if( allArrowObjectList.Count > i ){
				arrowObjectList = allArrowObjectList[i];
			}
			else{
				arrowObjectList = new List<GameObject>( );
			}
			Vector3[] posArray = magneticForceLinesDrawer.FetchMagnetForceLinePositionList(i);
			//1つの線の各要素数に対して不足分があるか確認し、不足分がある場合は要素を増やす.
			if( arrowObjectList.Count == (posArray.Length / arrowDrawInterval) ){
				continue;
			}
			for( int j = 0; j < posArray.Length; j ++ ){
				if( 0 == (j % arrowDrawInterval) && arrowObjectList.Count <= (j / arrowDrawInterval) ){
					Quaternion arrowDirection = Quaternion.identity;

					arrowDirection = Quaternion.FromToRotation(posArray[Math.Min(j + (arrowDrawInterval / 2), posArray.Length - 1)] - posArray[j], Vector3.up);
					arrowObjectList.Add(Instantiate(arrowSrcObject, posArray[j], arrowDirection, transform));
				}
			}
			//要素が多い場合は削除する.
			if( arrowObjectList.Count > (posArray.Length / arrowDrawInterval) ){
				arrowObjectList.GetRange((posArray.Length / arrowDrawInterval), arrowObjectList.Count - (posArray.Length / arrowDrawInterval)).ForEach((GameObject obj) => {Destroy(obj);});
				arrowObjectList.RemoveRange((posArray.Length / arrowDrawInterval), arrowObjectList.Count - (posArray.Length / arrowDrawInterval));
			}
			if( allArrowObjectList.Count <= i ){
				allArrowObjectList.Add(arrowObjectList);
			}
		}
		if( allArrowObjectList.Count > magneticForceLinesDrawer.FetchMagnetForceLineNum() ){
			foreach( List<GameObject> objList in allArrowObjectList.GetRange(magneticForceLinesDrawer.FetchMagnetForceLineNum(), allArrowObjectList.Count - magneticForceLinesDrawer.FetchMagnetForceLineNum()) ){
				objList.ForEach((GameObject obj) => {Destroy(obj);});
			}
			allArrowObjectList.RemoveRange(magneticForceLinesDrawer.FetchMagnetForceLineNum(), allArrowObjectList.Count - magneticForceLinesDrawer.FetchMagnetForceLineNum());
		}

		return;
	}

	public void SwitchValidDraw(bool isDraw)
	{
		isValidDraw = isDraw;
		foreach( List<GameObject> objList in allArrowObjectList ){
			objList.ForEach((GameObject obj) => {obj.SetActive(isValidDraw);});
		}

		return;
	}

#if UNITY_EDITOR
	//インスペクター上で値を変えても確認できるように対応.
	private void OnValidate()
	{
		if( isValidDraw != isPrevValidDraw ){
			SwitchValidDraw(isValidDraw);
			isPrevValidDraw = isValidDraw;
		}

		return;
	}
#endif

}
