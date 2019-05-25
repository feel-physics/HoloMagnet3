using UnityEngine;

public class CompassManagedlyUpdater : MonoBehaviour
{
    GameObject[] southPoles;
    GameObject[] northPoles;

    void Start()
    {
        northPoles = GameObject.FindGameObjectsWithTag("North Pole");
        southPoles = GameObject.FindGameObjectsWithTag("South Pole");
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
            MagneticForceCalculator.Instance.ForceResultant(northPoles, southPoles, transform.position);

        // コンパスの向きを設定する
        transform.LookAt(transform.position + forceResultant);
        transform.Rotate(-90f, 0f, 0f);
    }
}