using System.Collections;
using System.Collections.Generic;
using HoloLensCameraStream;
using OpenCVForUnity.CoreModule;
using UnityEngine;

public class FrameReadyParameterClass {
	public Mat graymat;
	public Matrix4x4 projectionMatrix;
	public Matrix4x4 cameraToWorldMatrix;
	public CameraIntrinsics cameraIntrinsics;
}
