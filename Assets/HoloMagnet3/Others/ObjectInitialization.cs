using UnityEngine;

/// <summary>
/// Unityでビルドしなくても、実機で確かめながら位置などを微調整できる
/// </summary>
public class ObjectInitialization : MonoBehaviour {

    private void Awake()
    {
        string logMessage = " was initialized";

        if (gameObject.name == "PointMagnet01")
        {
            /*
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
            if (MySceneManager.Instance.MyScene == MySceneManager.MySceneEnum.Compasses_3D)
            {
                transform.localPosition = new Vector3(0, -0.7f, 2f);
            }
            else
            {
                transform.localPosition = new Vector3(0, 0f, 2f);
            }
            Debug.Log(gameObject.name + logMessage);
        }
    }
}
