using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity.CoreModule;
using UnityEngine;

public class MarkerDetectedParameterClass {
	public List<Mat> corners;
	public Mat ids;
	public float markerLength;
	public Mat cameraMatrix;
	public List<Mat> rejectedCorners;
	public MatOfDouble distCoeffs;
	public Mat rvecs;
	public Mat tvecs;
	public Matrix4x4 arCameraWorldMatrix;
	public MarkerDetectedParameterClass() {
		corners = new List<Mat>();
		ids = new Mat();
		markerLength = 0f;
		cameraMatrix = new Mat();
		rejectedCorners = new List<Mat>();
		distCoeffs = new MatOfDouble();
		rvecs = new Mat();
		tvecs = new Mat();
		arCameraWorldMatrix = new Matrix4x4();
	}

	public void Dispose() {
		foreach (var mat in corners) {
			mat.Dispose();
		}
		foreach (var mat in rejectedCorners) {
			mat.Dispose();
		}
		ids.Dispose();
		cameraMatrix.Dispose();
		distCoeffs.Dispose();
		rvecs.Dispose();
		tvecs.Dispose();
	}
}