#define nekomimi
// @nekomimi suggested a way to tune up

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

    // 合力を返す
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

#if nekomimi
            // N極からの現在の頂点への変位ベクトル(ベクトルn)
            var vec_d = positionCurrentPoint - positionBarMagnetNorthPole;

            // ベクトルnの長さの2乗（これで単位ベクトルを割る）
            var len_d = 
                Mathf.Sqrt(vec_d.x * vec_d.x + vec_d.y * vec_d.y + vec_d.z * vec_d.z);

            // vec_f = (vec_d / |vec_d|) / (|vec_d|^2)
            //       =  vec_d / |vec_d|^3
            //       =  vec_d / len_d^3
            var forceFromOnePoleToCurrentPoint =
                vec_d / (len_d * len_d * len_d);
#else
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
#endif

            sumOfForceFromOnePoleToCurrentPoint += forceFromOnePoleToCurrentPoint;
        }
        return sumOfForceFromOnePoleToCurrentPoint;
    }
}
