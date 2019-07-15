using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringOffset : MonoBehaviour {

    [SerializeField]
    Transform target;


    [SerializeField]
    float lowPassFactor = 0.1f;
    [SerializeField]
    float lowPassFactorQ = 0.2f;

    private Vector3 intermediateValueBuf;
    private Quaternion intermediateValueBufVec4;

    // Use this for initialization
    void Start()
    {

        lowPassFilter(transform.position, ref intermediateValueBuf, lowPassFactor, true);
        lowPassFilter(transform.rotation, ref intermediateValueBufVec4, lowPassFactorQ, true);
    }

    // Update is called once per frame
    void Update () {
        if (transform.hasChanged && target != null)
        {
            //target.SetPositionAndRotation(transform.position, transform.rotation);
            ReceiveTransform(transform.position, transform.rotation);
        }
	}

    public void ReceiveTransform(Vector3 pos, Quaternion rot)
    {


        target.transform.SetPositionAndRotation(
            lowPassFilter(pos, ref intermediateValueBuf, lowPassFactor, false),
            lowPassFilter(rot, ref intermediateValueBufVec4, lowPassFactorQ, false)

            );

    }


    Vector3 lowPassFilter(Vector3 targetValue, ref Vector3 intermediateValueBuf, float factor, bool init)
    {

        Vector3 intermediateValue;

        //intermediateValue needs to be initialized at the first usage.
        if (init)
        {
            intermediateValueBuf = targetValue;
        }

        intermediateValue.x = (targetValue.x * factor) + (intermediateValueBuf.x * (1.0f - factor));
        intermediateValue.y = (targetValue.y * factor) + (intermediateValueBuf.y * (1.0f - factor));
        intermediateValue.z = (targetValue.z * factor) + (intermediateValueBuf.z * (1.0f - factor));

        intermediateValueBuf = intermediateValue;

        return intermediateValue;
    }



    Quaternion lowPassFilter(Quaternion targetValue, ref Quaternion intermediateValueBufVec4, float factor, bool init)
    {

        Quaternion intermediateValue;

        //intermediateValue needs to be initialized at the first usage.
        if (init)
        {
            intermediateValueBufVec4 = targetValue;
        }

        intermediateValue.x = (targetValue.x * factor) + (intermediateValueBufVec4.x * (1.0f - factor));
        intermediateValue.y = (targetValue.y * factor) + (intermediateValueBufVec4.y * (1.0f - factor));
        intermediateValue.z = (targetValue.z * factor) + (intermediateValueBufVec4.z * (1.0f - factor));
        intermediateValue.w = (targetValue.w * factor) + (intermediateValueBufVec4.w * (1.0f - factor));

        intermediateValueBufVec4 = intermediateValue;

        return intermediateValue;
    }

}
