using System.Linq;
using UnityEngine;

public class CompassManagedlyUpdater : MonoBehaviour
{
    //GameObject[] southPoles;
    //GameObject[] northPoles;

    Transform[] southPolesTransform;
    Transform[] northPolesTransform;

    void Start()
    {
        //northPoles = GameObject.FindGameObjectsWithTag("North Pole");
        //southPoles = GameObject.FindGameObjectsWithTag("South Pole");

        southPolesTransform = GameObject.FindGameObjectsWithTag("North Pole").
            Select(go => go.transform).
            ToArray();
        northPolesTransform = GameObject.FindGameObjectsWithTag("South Pole").
            Select(go => go.transform).
            ToArray();

    }

    public void ManagedlyUpdate()
    {
        // コンパスを回転させ、明るさを変える
        Rotate();
    }
    
    void Rotate()
    {
        // 合力ベクトル
        Vector3 forceResultant = 
            MagneticForceCalculator.Instance.ForceResultant(
                MyHelper.ToPoleArray(southPolesTransform),
                MyHelper.ToPoleArray(northPolesTransform),
                transform.position);

        // コンパスの向きを設定する
        transform.LookAt(transform.position + forceResultant);
        transform.Rotate(-90f, 0f, 0f);
    }
}