using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class HandTrackingController : MonoBehaviour, IMixedRealitySourceStateHandler, IMixedRealityHandJointHandler {
	[SerializeField] private TrackedHandJoint trackingJointRight;
	[SerializeField] private TrackedHandJoint trackingJointLeft;
	[SerializeField] private GameObject rightMagnetObject;
	[SerializeField] private GameObject leftMagnetObject;
	[SerializeField] private Vector3 leftRotationOffset;
	[SerializeField] private Vector3 rightRotationOffset;
	
	private void OnEnable() {
		CoreServices.InputSystem?.RegisterHandler<IMixedRealitySourceStateHandler>(this);
		CoreServices.InputSystem?.RegisterHandler<IMixedRealityHandJointHandler>(this);
	}

	private void OnDisable() {
		CoreServices.InputSystem?.UnregisterHandler<IMixedRealitySourceStateHandler>(this);
		CoreServices.InputSystem?.UnregisterHandler<IMixedRealityHandJointHandler>(this);
	}


	void Start() { }

	// Update is called once per frame
	void Update() { }

	public void OnSourceDetected(SourceStateEventData eventData) {
		var hand = eventData.Controller as IMixedRealityHand;
		if (hand != null) {
			Debug.Log("Source detected: " + hand.ControllerHandedness);
		}
	}

	public void OnSourceLost(SourceStateEventData eventData) {
		var hand = eventData.Controller as IMixedRealityHand;
		if (hand != null) {
			Debug.Log("Source lost: " + hand.ControllerHandedness);
		}
	}

	public void OnHandJointsUpdated(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData) {
		MixedRealityPose palmPose;
		if (eventData.Handedness == Handedness.None) return;
		if (eventData.InputData.TryGetValue(TrackedHandJoint.Palm, out palmPose)) {
			Debug.Log("Hand Joint Palm Updated: " + palmPose.Position);
			var handness = eventData.Handedness;
			var joint = eventData.InputData;
			if (handness == Handedness.Right) {
				var rot = joint[trackingJointRight].Rotation.eulerAngles + rightRotationOffset;
				rightMagnetObject.transform.position = joint[trackingJointRight].Position;
				rightMagnetObject.transform.eulerAngles = rot;
			}
			else {
				var rot = joint[trackingJointLeft].Rotation.eulerAngles + leftRotationOffset;
				leftMagnetObject.transform.position = joint[trackingJointRight].Position;
				leftMagnetObject.transform.eulerAngles = rot;
			}
		}
	}
}