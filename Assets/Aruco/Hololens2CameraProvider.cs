using System;
using System.Collections;
using System.Collections.Generic;
using HoloLensCameraStream;
using HoloLensWithOpenCVForUnity.UnityUtils.Helper;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using UnityEngine;
using UnityEngine.Events;

public class Hololens2CameraProvider : CameraProvider {
	[SerializeField] private HololensCameraStreamToMatHelper _hololensCameraStreamToMatHelper;
	readonly static Queue<Action> ExecuteOnMainThread = new Queue<Action>();
	[SerializeField] private int cameraWidth = 896;
	[SerializeField] private int cameraHeight = 504;
	[SerializeField] private int cameraFPS = 30; 
	[Serializable] private class FrameReadyEvent : UnityEvent<FrameReadyParameterClass> { };
	[SerializeField] private FrameReadyEvent OnMatFrameReady = new FrameReadyEvent();
	
	override public void init(out int _width, out int _height) {
		_hololensCameraStreamToMatHelper.frameMatAcquired += OnFrameMatAcquired;
		Debug.Log("Hololens2CameraProvider Initialize");
		_hololensCameraStreamToMatHelper.Initialize();

		_width = _hololensCameraStreamToMatHelper.requestedWidth;
		_height = _hololensCameraStreamToMatHelper.requestedWidth;
	}

	public void OnHololens2TextureToMatHelperInitialized() {
		Debug.Log("Hololens2CameraProvider Initialize done.");
	}

	public void OnHololens2TextureToMatHelperDisposed() {
		Debug.Log("OnHololens2TextureToMatHelperDisposed");
		lock (ExecuteOnMainThread) {
			ExecuteOnMainThread.Clear();			
		}
	}

	public void OnHololens2TextureToMatHelperErorOccurred(WebCamTextureToMatHelper.ErrorCode errorCode) {
		Debug.Log("OnHololens2TextureToMatHelperErorOccurred  " + errorCode);
	}

	public void OnFrameMatAcquired(Mat grayMat, Matrix4x4 projectionMatrix, Matrix4x4 cameraToWorldMatrix,
		CameraIntrinsics cameraIntrinsics) {
		Debug.Log("OnFrameMatAcquired invoked");
		
		var param = new FrameReadyParameterClass {
			graymat = grayMat,
			projectionMatrix = projectionMatrix,
			cameraToWorldMatrix = cameraToWorldMatrix,
			cameraIntrinsics = cameraIntrinsics
		};

		Enqueue(() => {
			Debug.Log("QUEUED ACTION INVOKED.");
			if (!_hololensCameraStreamToMatHelper.IsPlaying()) {
				Debug.Log("HololensCameraStreamToMatHelper IS NOT RUNNING.");
				return;
			}
			OnMatFrameReady?.Invoke(param);
			grayMat.Dispose();
		});
	}

	private void Enqueue(Action action) {
		lock (ExecuteOnMainThread) {
			ExecuteOnMainThread.Enqueue(action);
		}
	}

	override public Color32[] getImage() {
		return null;
	}

	// Start is called before the first frame update
	void Start() { }


	// Update is called once per frame
	void Update() {
		lock (ExecuteOnMainThread) {
			while (ExecuteOnMainThread.Count > 0) {
				if (ExecuteOnMainThread.Count == 1) {
					ExecuteOnMainThread.Dequeue().Invoke();
				}
				else {
					ExecuteOnMainThread.Dequeue();
				}
			}
		}
	}
}