using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticForceLineArrowDrawer : MonoBehaviour {

	[SerializeField]private BarMagnetMagneticForceLinesDrawer magneticForceLinesDrawer = null; 

	//磁力線の矢印用の元になるprefab.
	[SerializeField]private GameObject arrowSrcObject = null;
	//某磁石に表示する矢印の元になるprefab.
	[SerializeField]private GameObject barMagnetArrowSrcObject = null;

	[SerializeField]private int arrowDrawInterval = 20;

	private List<List<GameObject>> allArrowObjectList = new List<List<GameObject>>();
	private GameObject barMagnetArrowObject = null;

	[SerializeField]private bool isValidDraw = true;
#if UNITY_EDITOR
	private bool isPrevValidDraw = true;
#endif

	// Use this for initialization
	void Start()
	{
		if( magneticForceLinesDrawer == null ){
			magneticForceLinesDrawer = GetComponent<BarMagnetMagneticForceLinesDrawer>();
		}
		//磁力線の矢印.
		if( null != magneticForceLinesDrawer ){
			for( int i = 0; i < magneticForceLinesDrawer.FetchMagnetForceLineNum(); i ++ ){
				List<GameObject> arrowObjectList = new List<GameObject>();
				Vector3[] posArray = magneticForceLinesDrawer.FetchMagnetForceLinePositionList(i);
				for( int j = 0; j < posArray.Length; j ++ ){
					if( (j % arrowDrawInterval) == 0 ){
						Quaternion arrowDirection = Quaternion.identity;

						arrowDirection = Quaternion.FromToRotation(posArray[Math.Min(j + (arrowDrawInterval / 2), posArray.Length - 1)] - posArray[j], Vector3.up);
//						arrowDirection = Quaternion.FromToRotation(posArray[Math.Min(j + (arrowDrawInterval / 2), posArray.Length - 1)] - posArray[Math.Max(j - (arrowDrawInterval / 2), 0)], Vector3.up);
						arrowObjectList.Add(Instantiate(arrowSrcObject, posArray[j], arrowDirection, transform));
					}
				}
				allArrowObjectList.Add(arrowObjectList);
			}
		}

		//某磁石内の矢印.
		if( barMagnetArrowSrcObject != null ){
			barMagnetArrowObject = Instantiate(barMagnetArrowSrcObject, transform);
			barMagnetArrowObject.transform.localPosition = new Vector3(0.0115f, 0.0f, 0.0f);
//			if( allArrowObjectList.Count == 0 ){
//				barMagnetArrowObject.SetActive(false);
//			}
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
		if( magneticForceLinesDrawer == null ){
			return;
		}
		for( int i = 0; i < magneticForceLinesDrawer.FetchMagnetForceLineNum(); i ++ ){
			if( i >= allArrowObjectList.Count ){
				break;
			}
			Vector3[] posArray = magneticForceLinesDrawer.FetchMagnetForceLinePositionList(i);
			for( int j = 0; j < posArray.Length; j ++ ){
				if( (j % arrowDrawInterval) == 0 && (j / arrowDrawInterval) < allArrowObjectList[i].Count ){
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
		if( magneticForceLinesDrawer == null ){
			return;
		}
		for( int i = 0; i < magneticForceLinesDrawer.FetchMagnetForceLineNum(); i ++ ){
			List<GameObject> arrowObjectList;
			if( i < allArrowObjectList.Count ){
				arrowObjectList = allArrowObjectList[i];
			}
			else{
				arrowObjectList = new List<GameObject>( );
			}
			Vector3[] posArray = magneticForceLinesDrawer.FetchMagnetForceLinePositionList(i);
			//1つの線の各要素数に対して不足分があるか確認し、不足分がある場合は要素を増やす.
			if( (posArray.Length / arrowDrawInterval) == arrowObjectList.Count ){
				continue;
			}
			for( int j = 0; j < posArray.Length; j ++ ){
				if( (j % arrowDrawInterval) == 0 && (j / arrowDrawInterval) >= arrowObjectList.Count ){
					Quaternion arrowDirection = Quaternion.identity;

					arrowDirection = Quaternion.FromToRotation(posArray[Math.Min(j + (arrowDrawInterval / 2), posArray.Length - 1)] - posArray[j], Vector3.up);
					arrowObjectList.Add(Instantiate(arrowSrcObject, posArray[j], arrowDirection, transform));
				}
			}
			//要素が多い場合は削除する.
			if( (posArray.Length / arrowDrawInterval) < arrowObjectList.Count ){
				arrowObjectList.GetRange((posArray.Length / arrowDrawInterval), arrowObjectList.Count - (posArray.Length / arrowDrawInterval)).ForEach((GameObject obj) => {Destroy(obj);});
				arrowObjectList.RemoveRange((posArray.Length / arrowDrawInterval), arrowObjectList.Count - (posArray.Length / arrowDrawInterval));
			}
			if( i >= allArrowObjectList.Count ){
				allArrowObjectList.Add(arrowObjectList);
			}
		}
		if( magneticForceLinesDrawer.FetchMagnetForceLineNum() < allArrowObjectList.Count ){
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
		barMagnetArrowSrcObject.SetActive(isValidDraw);

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
