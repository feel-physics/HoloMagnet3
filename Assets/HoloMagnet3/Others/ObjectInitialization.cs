using UnityEngine;

public class ObjectInitialization : MonoBehaviour {

    private void Awake()
    {
        string logMessage = " was initialized";

        if (gameObject.name == "PointMagnet01")
        {
            /*  Todo: どのオブジェクトかを調べる 
            // 磁石を横向きに生成する
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

            transform.localPosition = new Vector3(0, 0, 0.7f);
            Debug.Log(gameObject.name + logMessage);
            */
        }
        else if(gameObject.name == "Axises")
        {
            transform.localPosition = new Vector3(1.0f, 0f, 0f);
            Debug.Log(gameObject.name + logMessage);
        }
        else if(gameObject.name == "BarMagnet01")
        {
            transform.localPosition = new Vector3(0, 0f, 2f);
            //transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            Debug.Log(gameObject.name + logMessage);
        }
    }
}
