using System;
using System.Collections;
using System.Collections.Generic;
using HoloLensWithOpenCVForUnityExample;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using TMPro;
using UnityEngine;

public class MagnetMover : MonoBehaviour {
	[SerializeField] private int markerId;
	[SerializeField] private GameObject debugLabel;
	bool _idFound = false;
	private TextMeshProUGUI tmp = null;
	private Matrix4x4 invertYM;
	private Matrix4x4 invertZM;
	private Matrix4x4 ARM;
	System.Object sync = new System.Object();
	private string debugString;
	private ARGameObject arGameObj;
	[SerializeField] private GameObject target;
	[SerializeField] private bool useLerpFilter = true;
	[SerializeField] private bool showDebugInfo = false;
	void Start() {
		invertYM = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, -1, 1));
		invertZM = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, -1));
		arGameObj = target.GetComponent<ARGameObject>();
		if (debugLabel != null) {
			tmp = debugLabel.GetComponent<TextMeshProUGUI>();
		}
	}

	// Update is called once per frame
	void Update() {
		lock (sync) {
			if (useLerpFilter) {
				//move with lerp
				arGameObj.SetMatrix4x4(ARM);
			}
			else {
				//direct move
				ARUtils.SetTransformFromMatrix(gameObject.transform, ref ARM);
			}
		}

		if (tmp != null && showDebugInfo) {
			tmp.SetText($"{arGameObj.pendingPositionList.Count}");
		}
	}

	public void setPlace(MarkerDetectedParameterClass mdp) {
		int[] idArray = new int[mdp.ids.total()];
		mdp.ids.get(0, 0, idArray);

		bool idFound = false;
		for (int i = 0; i < idArray.Length; i++) {
			if (markerId == idArray[i]) {
				if (debugLabel != null) {
					idFound = true;
					double[] rvecArray = new double[3];
					double[] tvecArray = new double[3];
					mdp.rvecs.get(i, 0, rvecArray);
					mdp.tvecs.get(i, 0, tvecArray);
					PoseData poseDate = ARUtils.ConvertRvecTvecToPoseData(rvecArray, tvecArray);
					Matrix4x4 trs = Matrix4x4.TRS(poseDate.pos, poseDate.rot, Vector3.one);
					debugString = $"{poseDate.rot.x}:{poseDate.rot.y}:{poseDate.rot.z}";
					lock (sync) {
						ARM = invertYM * trs;
						ARM = ARM * invertZM;
						ARM = mdp.arCameraWorldMatrix * invertZM * ARM;
					}
				}
				break;
			}
		}
		_idFound = idFound;
	}
}