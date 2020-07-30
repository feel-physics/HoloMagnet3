using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticForceLineArrowDrawer : MonoBehaviour {

	[SerializeField]private BarMagnetMagneticForceLinesDrawer magneticForceLinesDrawer = null; 

	//磁力線の矢印用の元になるprefab.
	[SerializeField]private GameObject arrowSrcObject = null;
	//棒磁石に表示する矢印の元になるprefab.
	[SerializeField]private GameObject barMagnetArrowSrcObject = null;

	//磁力線内の要素何点につき、矢印を描画するかの間隔.
	[SerializeField]private int arrowDrawInterval = 20;
	//矢印を描画開始する磁力線内の要素番号のオフセット(磁力線の根本等に矢印が集中することの軽減を意識して設定できるように).
	[SerializeField]private int arrowDrawIntervalOffset = 0;

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
				AddArrowObject(posArray, arrowObjectList, i < (magneticForceLinesDrawer.FetchMagnetForceLineNum() / 2));
				allArrowObjectList.Add(arrowObjectList);
			}
		}

		//棒磁石内の矢印.
		if( barMagnetArrowSrcObject != null ){
			barMagnetArrowObject = Instantiate(barMagnetArrowSrcObject, transform);
			//棒磁石の中心部に表示するためのオフセット座標.
			barMagnetArrowObject.transform.localPosition = new Vector3(0.0115f, 0.0f, 0.0f);
			//デフォルトは非表示.
			barMagnetArrowObject.SetActive(false);
//			if( allArrowObjectList.Count == 0 ){
//				barMagnetArrowObject.SetActive(false);
//			}
		}

		return;
	}
	
	// Update is called once per frame
	void Update ()
	{
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
		Vector3 upVector = Vector3.up;
		//N極用とS極用で向きを反転する(ループ回数が多いので、処理負荷的にifで内部で分岐させるのではなく、それぞれの処理を書く).
		for( int i = 0; i < magneticForceLinesDrawer.FetchMagnetForceLineNum() / 2; i ++ ){
			if( i >= allArrowObjectList.Count ){
				break;
			}
			Vector3[] posArray = magneticForceLinesDrawer.FetchMagnetForceLinePositionList(i);
			for( int j = 0; j < posArray.Length; j ++ ){
				if( ((j + arrowDrawIntervalOffset) % arrowDrawInterval) == 0 && (j / arrowDrawInterval) < allArrowObjectList[i].Count ){
					Quaternion arrowDirection = Quaternion.identity;
					Vector3 lookVector = posArray[Math.Min(j + (arrowDrawInterval / 2), posArray.Length - 1)] - posArray[Math.Max(j - (arrowDrawInterval / 2), 0)];

					if( Vector3.zero != lookVector ){
						arrowDirection = Quaternion.LookRotation(lookVector, upVector);
					}
					allArrowObjectList[i][(j / arrowDrawInterval)].transform.SetPositionAndRotation(posArray[j], arrowDirection);
				}
			}
		}
		for( int i = magneticForceLinesDrawer.FetchMagnetForceLineNum() / 2; i < magneticForceLinesDrawer.FetchMagnetForceLineNum(); i ++ ){
			if( i >= allArrowObjectList.Count ){
				break;
			}
			Vector3[] posArray = magneticForceLinesDrawer.FetchMagnetForceLinePositionList(i);
			for( int j = 0; j < posArray.Length; j ++ ){
				if( ((j + arrowDrawIntervalOffset) % arrowDrawInterval) == 0 && (j / arrowDrawInterval) < allArrowObjectList[i].Count ){
					Quaternion arrowDirection = Quaternion.identity;
					Vector3 lookVector = posArray[Math.Max(j - (arrowDrawInterval / 2), 0)] - posArray[Math.Min(j + (arrowDrawInterval / 2), posArray.Length - 1)];

					if( Vector3.zero != lookVector ){
						arrowDirection = Quaternion.LookRotation(lookVector, upVector);
					}
					allArrowObjectList[i][(j / arrowDrawInterval)].transform.SetPositionAndRotation(posArray[j], arrowDirection);
				}
			}
		}

		return;
	}

	//描画する矢印のオブジェクトを管理する(GameObjectの生成と削除を管理).
	public void DrawArrow()
	{
		if( magneticForceLinesDrawer == null || isValidDraw == false ){
			return;
		}
		for( int i = 0; i < magneticForceLinesDrawer.FetchMagnetForceLineNum(); i ++ ){
			List<GameObject> arrowObjectList;
			if( i < allArrowObjectList.Count ){
				arrowObjectList = allArrowObjectList[i];
			}
			//磁力線の数に対して、矢印の方が少ない場合はリストを新たに生成する.
			else{
				arrowObjectList = new List<GameObject>( );
			}
			Vector3[] posArray = magneticForceLinesDrawer.FetchMagnetForceLinePositionList(i);
			//1つの線の各要素数に対して不足分があるか確認し、不足分がある場合は要素を増やす.
			if( (posArray.Length / arrowDrawInterval) == arrowObjectList.Count ){
				continue;
			}
			//磁力線1本内の要素が少ない場合は生成する.
			AddArrowObject(posArray, arrowObjectList, i < (magneticForceLinesDrawer.FetchMagnetForceLineNum() / 2));
			//磁力線1本内の要素が多い場合は削除する.
			if( (posArray.Length / arrowDrawInterval) < arrowObjectList.Count ){
				arrowObjectList.GetRange((posArray.Length / arrowDrawInterval), arrowObjectList.Count - (posArray.Length / arrowDrawInterval)).ForEach((GameObject obj) => {Destroy(obj);});
				arrowObjectList.RemoveRange((posArray.Length / arrowDrawInterval), arrowObjectList.Count - (posArray.Length / arrowDrawInterval));
			}
			//磁力線の数に対して、矢印の方が少ない場合は生成したものを追加する.
			if( i >= allArrowObjectList.Count ){
				allArrowObjectList.Add(arrowObjectList);
			}
		}
		//磁力線の数に対して、矢印の方が多い場合は削除する.
		if( magneticForceLinesDrawer.FetchMagnetForceLineNum() < allArrowObjectList.Count ){
			foreach( List<GameObject> objList in allArrowObjectList.GetRange(magneticForceLinesDrawer.FetchMagnetForceLineNum(), allArrowObjectList.Count - magneticForceLinesDrawer.FetchMagnetForceLineNum()) ){
				objList.ForEach((GameObject obj) => {Destroy(obj);});
			}
			allArrowObjectList.RemoveRange(magneticForceLinesDrawer.FetchMagnetForceLineNum(), allArrowObjectList.Count - magneticForceLinesDrawer.FetchMagnetForceLineNum());
		}
		//磁力線に連動して矢印が表示されている場合は、磁石内のオブジェクトも表示させる.
		barMagnetArrowObject.SetActive(allArrowObjectList.Count > 0);

		return;
	}

	//描画の有効/無効を切り替え(オブジェクトのactiveの切り替え).
	public void SwitchValidDraw(bool isDraw)
	{
		isValidDraw = isDraw;
		foreach( List<GameObject> objList in allArrowObjectList ){
			objList.ForEach((GameObject obj) => {obj.SetActive(isValidDraw);});
		}
		barMagnetArrowObject.SetActive(isValidDraw);

		return;
	}

	//矢印用オブジェクトの生成(初期の位置と向きも計算しておく).
	private void AddArrowObject(Vector3[] posArray, List<GameObject> arrowObjectList, bool isNorth)
	{
		for( int j = 0; j < posArray.Length; j ++ ){
			if( ((j + arrowDrawIntervalOffset) % arrowDrawInterval) == 0 ){
				Quaternion arrowDirection = Quaternion.identity;
				Vector3 lookVector;

				//N極用、S極用で向きを反転させる.
				if( isNorth == true ){
					lookVector = posArray[Math.Min(j + (arrowDrawInterval / 2), posArray.Length - 1)] - posArray[Math.Max(j - (arrowDrawInterval / 2), 0)];
					if( Vector3.zero != lookVector ){
						arrowDirection = Quaternion.LookRotation(lookVector, Vector3.up);
					}
				}
				else{
					lookVector = posArray[Math.Max(j - (arrowDrawInterval / 2), 0)] - posArray[Math.Min(j + (arrowDrawInterval / 2), posArray.Length - 1)];
					if( Vector3.zero != lookVector ){
						arrowDirection = Quaternion.LookRotation(lookVector, Vector3.up);
					}
				}
				arrowObjectList.Add(Instantiate(arrowSrcObject, posArray[j], arrowDirection, transform));
			}
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
