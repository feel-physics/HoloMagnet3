using HoloToolkit.Unity;
using System.Linq;
using UnityEngine;

public struct Pole
{
    public Vector3 position;
}

// sealed：このクラスから継承できなくする
public sealed class MagneticForceCalculator
{
    // 最初に new されたときにインスタンス化され、以後は使い回す
    public static readonly MagneticForceCalculator Instance = new MagneticForceCalculator();

    // コンストラクタをprivateにすることで、外部からnewできなくする
    private MagneticForceCalculator()
    {
    }

    public Vector3 ForceResultant(Pole[] northPoles, Pole[] southPoles, Vector3 positionCurrentPoint)
    {
        // --- N極 ---
        Vector3 sumOfForceFromNorthPoleToCurrentPoint =
            ForceResultantOfOnePoles(northPoles, positionCurrentPoint);

        // --- S極 ---
        Vector3 sumOfForceFromSouthPoleToCurrentPoint =
            ForceResultantOfOnePoles(southPoles, positionCurrentPoint);

        var forceResultant = sumOfForceFromNorthPoleToCurrentPoint - sumOfForceFromSouthPoleToCurrentPoint;
        return forceResultant;
    }

    private Vector3 ForceResultantOfOnePoles(Pole[] poles, Vector3 positionCurrentPoint)
    {
        var sumOfForceFromOnePoleToCurrentPoint = Vector3.zero;
        foreach(var pole in poles)
        {
            var positionBarMagnetNorthPole = pole.position;

            // N極からの現在の頂点への変位ベクトル(ベクトルn)
            var displacementFromOnePoleToCurrentPoint = positionCurrentPoint - positionBarMagnetNorthPole;

            // ベクトルnの長さの2乗（これで単位ベクトルを割る）
            var lengthSquareFromOnePoleToCurrentPoint =
            displacementFromOnePoleToCurrentPoint.sqrMagnitude;

            // ベクトルnの単位ベクトル
            var normalizedDisplacementFromOnePoleToCurrentPoint =
                displacementFromOnePoleToCurrentPoint.normalized;

            // ベクトルn
            var forceFromOnePoleToCurrentPoint =
                normalizedDisplacementFromOnePoleToCurrentPoint / (float)lengthSquareFromOnePoleToCurrentPoint;

            sumOfForceFromOnePoleToCurrentPoint += forceFromOnePoleToCurrentPoint;
        }
        return sumOfForceFromOnePoleToCurrentPoint;
    }
}
