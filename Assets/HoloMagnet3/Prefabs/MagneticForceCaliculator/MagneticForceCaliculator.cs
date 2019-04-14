using HoloToolkit.Unity;
using System.Linq;
using UnityEngine;

public class MagneticForceCaliculator : Singleton<MagneticForceCaliculator> {

#if false
    private Vector3 ForceResultant(List<GameObject> northPoles, List<GameObject> southPoles, Vector3 positionCurrentPoint)
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

    private Vector3 ForceResultantOfOnePoles(List<GameObject> poles, Vector3 positionCurrentPoint)
    {
        var sumOfForceFromOnePoleToCurrentPoint =
            poles.Select(pole =>
            {
                Vector3 positionBarMagnetNorthPole = pole.transform.position;

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

                return forceFromOnePoleToCurrentPoint;
            }).
            Aggregate((n0, n1) => n0 + n1);
        return sumOfForceFromOnePoleToCurrentPoint;
    }
#else
    public Vector3 ForceResultant(GameObject[] northPoles, GameObject[] southPoles, Vector3 positionCurrentPoint)
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

    private Vector3 ForceResultantOfOnePoles(GameObject[] poles, Vector3 positionCurrentPoint)
    {
        var sumOfForceFromOnePoleToCurrentPoint =
            poles.Select(pole =>
            {
                Vector3 positionBarMagnetNorthPole = pole.transform.position;

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

                return forceFromOnePoleToCurrentPoint;
            }).
            Aggregate((n0, n1) => n0 + n1);
        return sumOfForceFromOnePoleToCurrentPoint;
    }
#endif
}
