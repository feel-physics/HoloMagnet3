using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HoloLensCameraStream;
using OpenCVForUnity.ArucoModule;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ArucoMakerDetection : MonoBehaviour {
	public enum ArucoDictID {
		DICT_4X4_50,
		DICT_4X4_100,
		DICT_4X4_250,
		DICT_4X4_1000,
		DICT_5X5_50,
		DICT_5X5_100,
		DICT_5X5_250,
		DICT_5X5_1000,
		DICT_6X6_50,
		DICT_6X6_100,
		DICT_6X6_250,
		DICT_6X6_1000,
		DICT_7X7_50,
		DICT_7X7_100,
		DICT_7X7_250,
		DICT_7X7_1000,
		DICT_ARUCO_ORIGINAL,
		DICT_APRILTAG_16h5,
		DICT_APRILTAG_25h9,
		DICT_APRILTAG_36h10,
		DICT_APRILTAG_36h11
	}

	[SerializeField] private CameraProvider cameraProvider;
	[SerializeField] private float markerLength;
	[SerializeField] private ArucoDictID dictionaryId;

	[Serializable]
	private class OnMarkerDetectedEvent : UnityEvent<MarkerDetectedParameterClass> { };

	[SerializeField] private OnMarkerDetectedEvent OnMarkerDetectedFunction = new OnMarkerDetectedEvent();
	[SerializeField] private bool preview;
	[SerializeField] private GameObject previewObject;

	private Dictionary _dictionary;
	private List<Mat> _corners;
	private List<Mat> _rejectedCorners;
	private Mat _cameraMatrix;
	private MatOfDouble _distCoeffs;
	private Mat _ids;
	private Mat _rvecs;
	private Mat _tvecs;
	DetectorParameters _detectorParams;
	private Texture2D _previewTexture;
	private Mat _rgbMat4preview;
	bool _isThreadRunning = false;

	//for debug
	[SerializeField] private GameObject DebugLabel;

	private void Awake() {
		int cameraWidth = 0;
		int cameraHeight = 0;
		Debug.Log("ArucoMakerDetection Awake");

		_detectorParams = DetectorParameters.create();
		_dictionary = Aruco.getPredefinedDictionary((int) dictionaryId);
		_corners = new List<Mat>();
		_rejectedCorners = new List<Mat>();
		_ids = new Mat();
		_rvecs = new Mat();
		_tvecs = new Mat();

		if (cameraProvider != null) cameraProvider.init(out cameraWidth, out cameraHeight);
	}

	public void MarkerDetectionWithAruco(FrameReadyParameterClass frameParam) {
		_cameraMatrix = CreateCameraMatrix(frameParam.cameraIntrinsics.FocalLengthX,
			frameParam.cameraIntrinsics.FocalLengthY, frameParam.cameraIntrinsics.PrincipalPointX,
			frameParam.cameraIntrinsics.PrincipalPointY);

		_distCoeffs = new MatOfDouble(frameParam.cameraIntrinsics.RadialDistK1,
			frameParam.cameraIntrinsics.RadialDistK2, frameParam.cameraIntrinsics.RadialDistK3,
			frameParam.cameraIntrinsics.TangentialDistP1, frameParam.cameraIntrinsics.TangentialDistP2);

		Aruco.detectMarkers(frameParam.graymat, _dictionary, _corners, _ids, _detectorParams, _rejectedCorners,
			_cameraMatrix,
			_distCoeffs);
		
		if (_ids.total() > 0) {
			Aruco.estimatePoseSingleMarkers(_corners, markerLength, _cameraMatrix, _distCoeffs, _rvecs, _tvecs);
			MarkerDetectedParameterClass mdp = new MarkerDetectedParameterClass();
			mdp.corners = _corners;
			mdp.ids = _ids;
			mdp.rvecs = _rvecs;
			mdp.tvecs = _tvecs;
			mdp.arCameraWorldMatrix = frameParam.cameraToWorldMatrix;
//			var tmp = DebugLabel.GetComponent<TextMeshProUGUI>();
//			tmp.SetText("");
			OnMarkerDetectedFunction.Invoke(mdp);
		}

		if (preview && previewObject != null) {
			if (_previewTexture == null) {
				_rgbMat4preview = new Mat();
				_previewTexture = new Texture2D(frameParam.graymat.width(), frameParam.graymat.height(),
					TextureFormat.RGB24, false);
				previewObject.GetComponent<MeshRenderer>().material.mainTexture = _previewTexture;
				Debug.Log("build preview texture.");
			}

			Imgproc.cvtColor(frameParam.graymat, _rgbMat4preview, Imgproc.COLOR_GRAY2RGB);
			Utils.fastMatToTexture2D(_rgbMat4preview, _previewTexture);
			Debug.Log("Preview texture update.");
		}

		frameParam.graymat.Dispose();
	}

	private Mat CreateCameraMatrix(double fx, double fy, double cx, double cy) {
		Mat camMatrix = new Mat(3, 3, CvType.CV_64FC1);
		camMatrix.put(0, 0, fx);
		camMatrix.put(0, 1, 0);
		camMatrix.put(0, 2, cx);
		camMatrix.put(1, 0, 0);
		camMatrix.put(1, 1, fy);
		camMatrix.put(1, 2, cy);
		camMatrix.put(2, 0, 0);
		camMatrix.put(2, 1, 0);
		camMatrix.put(2, 2, 1.0f);
		return camMatrix;
	}

	private void OnDestroy() {
		foreach (var item  in _corners) item.Dispose();
		foreach (var item in _rejectedCorners) item.Dispose();
		if (_cameraMatrix != null) _cameraMatrix.Dispose();
		if (_ids != null) _ids.Dispose();
		if (_rvecs != null) _rvecs.Dispose();
		if (_tvecs != null) _tvecs.Dispose();
		if (_rgbMat4preview != null) _rgbMat4preview.Dispose();
	}

	// Start is called before the first frame update
	void Start() { }

	// Update is called once per frame
	void Update() { }
}