﻿using System.Linq;
using UnityEngine;

public class CompassManagedlyUpdater : MonoBehaviour
{
    //GameObject[] southPoles;
    //GameObject[] northPoles;

    Transform[] southPolesTransforms;
    Transform[] northPolesTransforms;
    Pole[] southPoles;
    Pole[] northPoles;

    void Start()
    {
        //northPoles = GameObject.FindGameObjectsWithTag("North Pole");
        //southPoles = GameObject.FindGameObjectsWithTag("South Pole");

        southPolesTransforms = GameObject.FindGameObjectsWithTag("North Pole").
            Select(go => go.transform).
            ToArray();
        southPoles = new Pole[southPolesTransforms.Length];
        northPolesTransforms = GameObject.FindGameObjectsWithTag("South Pole").
            Select(go => go.transform).
            ToArray();
        northPoles = new Pole[northPolesTransforms.Length];

    }

    public void ManagedlyUpdate()
    {
        // コンパスを回転させ、明るさを変える
        Rotate();
    }
    
    void Rotate()
    {
        // 合力ベクトル
/*        Vector3 forceResultant = 
            MagneticForceCalculator.Instance.ForceResultant(
                MyHelper.ToPoleArray(southPolesTransforms),
                MyHelper.ToPoleArray(northPolesTransforms),
                transform.position);*/
        MyHelper.CopyPoleArray(northPoles, northPolesTransforms);
        MyHelper.CopyPoleArray(southPoles, southPolesTransforms);
        Vector3 forceResultant = MagneticForceCalculator.Instance.ForceResultant(
            southPoles, northPoles, transform.position);

        // コンパスの向きを設定する
        transform.LookAt(transform.position + forceResultant);
        transform.Rotate(-90f, 0f, 0f);
    }
}