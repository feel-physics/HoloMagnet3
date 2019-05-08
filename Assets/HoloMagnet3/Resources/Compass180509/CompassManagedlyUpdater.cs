using System.Collections.Generic;
using UnityEngine;

public class CompassManagedlyUpdater : MonoBehaviour
{
    // 明るさの係数
    // 3次元の場合は方位磁針を暗めに
    public float BrightnessCoefficient = 0.001f;
    // それ以外の場合は方位磁針を明るめに
    public float BrightnessCoefficient3D = 0.00015f;

    GameObject[] southPoles;
    GameObject[] northPoles;
    MeshRenderer meshRenderer;
    Material materialNorth;
    Material materialSouth;

    float brightnessCoefficient;

    // カラーオブジェクトをプリロード（あらかじめ作っておく）して入れ替える
    static Color originalCompassNorthColor = new Color(1f, 0.384f, 0.196f);
    static Color originalCompassSouthColor = new Color(0.341f, 0.525f, 1f);

    void Start()
    {
        if (MySceneManager.Instance.MyScene == MySceneManager.MySceneEnum.Compasses_3D)
        {
            brightnessCoefficient = BrightnessCoefficient3D;
        }
        else
        {
            brightnessCoefficient = BrightnessCoefficient;
        }

        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        materialNorth = meshRenderer.materials[0];
        materialSouth = meshRenderer.materials[1];
    }

    public void ManagedlyUpdate()
    {
        northPoles = GameObject.FindGameObjectsWithTag("North Pole");
        southPoles = GameObject.FindGameObjectsWithTag("South Pole");

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

        // 合力の大きさでコンパスの明るさを決める
        // Todo:  2次元の色の減衰が強すぎて、磁石に隣接する方位磁針にしか色がつかない。仕上げの段階で調整する。
        float brightnessOfForce = forceResultant.sqrMagnitude * brightnessCoefficient;

        // Emissioonを変える
        /*
         * 参考にさせて頂きました：
         * Unity5のStandardシェーダのパラメタをスクリプトからいじろうとして丸一日潰れた話 - D.N.A.のおぼえがき
         * http://dnasoftwares.hatenablog.com/entry/2015/03/19/100108
         */
        var materialNorthEmission = materialNorth.GetColor("_Emission");
        var materialSouthEmission = materialSouth.GetColor("_Emission");  //  Todo: northとsouthを分ける意味はあるのか？
        materialNorth.SetColor("_Emission", ColorWithBrightness(true, materialNorthEmission, brightnessOfForce));
        materialSouth.SetColor("_Emission", ColorWithBrightness(false, materialSouthEmission, brightnessOfForce));
    }

    Color ColorWithBrightness(bool isNorth, Color color, float brightness)
    {
        Color originalColor;

        // 準備
        if (isNorth)  // if文  Todo: if文を排除してパフォーマンスを上げたい
        {
            originalColor = originalCompassNorthColor;
        }
        else
        {
            originalColor = originalCompassSouthColor;
        }

        // 明るさの微調整
        if (1.5f < brightness)  // 明るすぎるとき
        {
            brightness = 1.5f + (brightness - 1.5f) * 0.00050f;  // 最後の値のさじ加減が大切
        }
        else if (brightness <= 0.5f)  // 暗すぎるとき
        {
            brightness = 0.0f + brightness; // 明るさの最低値を決める。プロジェクターに出すときは0.5くらいで。
        }

        // 最終的に適用するColor
        // Todo: 1行で書けないか？
        float colorR = originalColor.r * brightness;
        float colorG = originalColor.g * brightness;
        float colorB = originalColor.b * brightness;
        return new Color(colorR, colorG, colorB);
    }
}