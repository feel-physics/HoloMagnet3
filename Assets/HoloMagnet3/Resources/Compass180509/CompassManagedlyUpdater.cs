using UnityEngine;

public class CompassManagedlyUpdater : MonoBehaviour
{
    //// 明るさの係数
    //// 通常は方位磁針を明るめに
    //public float BrightnessCoefficient = 0.001f;
    //// 3次元の場合は方位磁針を暗めに
    //public float BrightnessCoefficient3D = 0.0001f;

    GameObject[] southPoles;
    GameObject[] northPoles;

    //float brightnessCoefficient;

    // カラーオブジェクトをプリロード（あらかじめ作っておく）して入れ替える
    //static Color originalCompassNorthColor = new Color(1f, 0.384f, 0.196f);
    //static Color originalCompassSouthColor = new Color(0.341f, 0.525f, 1f);

    void Start()
    {
        //if (MySceneManager.Instance.MyScene == MySceneManager.MySceneEnum.Compasses_3D)
        //{
        //    brightnessCoefficient = BrightnessCoefficient3D;
        //}
        //else
        //{
        //    brightnessCoefficient = BrightnessCoefficient;
        //}

        northPoles = GameObject.FindGameObjectsWithTag("North Pole");
        southPoles = GameObject.FindGameObjectsWithTag("South Pole");

    }

    public void ManagedlyUpdate()
    {
        // コンパスを回転させ、明るさを変える
        CompassRotateAndChangeEmission();
    }
    
    void CompassRotateAndChangeEmission()
    {
        // 合力ベクトル
        Vector3 forceResultant = 
            MagneticForceCaliculator.Instance.ForceResultant(northPoles, southPoles, transform.position);

        // コンパスの向きを設定する
        transform.LookAt(transform.position + forceResultant);
        transform.Rotate(-90f, 0f, 0f);
        
    }

    //Color ColorWithBrightness(bool isNorth, Color color, float brightness)
    //{
    //    Color originalColor;

    //    // 準備
    //    if (isNorth)  // if文  Todo: if文を排除してパフォーマンスを上げたい
    //    {
    //        originalColor = originalCompassNorthColor;
    //    }
    //    else
    //    {
    //        originalColor = originalCompassSouthColor;
    //    }

    //    // 明るさの微調整
    //    if (1.5f < brightness)  // 明るすぎるとき
    //    {
    //        brightness = 1.5f + (brightness - 1.5f) * 0.00050f;  // 最後の値のさじ加減が大切
    //    }
    //    else if (brightness <= 0.5f)  // 暗すぎるとき
    //    {
    //        brightness = 0.0f + brightness; // 明るさの最低値を決める。プロジェクターに出すときは0.5くらいで。
    //    }

    //    // 最終的に適用するColor
    //    // Todo: 1行で書けないか？
    //    float colorR = originalColor.r * brightness;
    //    float colorG = originalColor.g * brightness;
    //    float colorB = originalColor.b * brightness;
    //    return new Color(colorR, colorG, colorB);
    //}
}