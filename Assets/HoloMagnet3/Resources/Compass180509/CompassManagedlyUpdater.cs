using System.Collections.Generic;
using UnityEngine;

public class CompassManagedlyUpdater : MonoBehaviour
{
    public enum State { Compass2D, Compass3D };
    public State state = State.Compass3D;

    GameObject[] southPoles;
    GameObject[] northPoles;
    MeshRenderer meshRenderer;
    Material materialNorth;
    Material materialSouth;

    // Todo: Listがわからない
    //List<GameObject> northPolesList;
    //List<GameObject> southPolesList;

    float brightness = 0.001f;  // 明るさの係数

    // カラーオブジェクトをプリロード（あらかじめ作っておく）して入れ替える
    static Color originalCompassNorthColor = new Color(1f, 0.384f, 0.196f);
    static Color originalCompassSouthColor = new Color(0.341f, 0.525f, 1f);

    void Start()
    {
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        materialNorth = meshRenderer.materials[0];
        materialSouth = meshRenderer.materials[1];
    }

    public void ManagedlyUpdate()
    {
        // Todo: Listがわからない
        northPoles = GameObject.FindGameObjectsWithTag("North Pole");
        southPoles = GameObject.FindGameObjectsWithTag("South Pole");

        // Todo: Listがわからない
        //northPolesList.Add(BarMagnetModel.Instance.NorthPoleReference);
        //southPolesList.Add(BarMagnetModel.Instance.SouthPoleReference);

        // コンパスを回転させる
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
        float brightnessOfForce = forceResultant.sqrMagnitude * brightness;

        // Emissioonを変える
        // Unity5のStandardシェーダのパラメタをスクリプトからいじろうとして丸一日潰れた話 - D.N.A.のおぼえがき
        // http://dnasoftwares.hatenablog.com/entry/2015/03/19/100108
        var materialNorthEmission = materialNorth.GetColor("_Emission");
        var materialSouthEmission = materialSouth.GetColor("_Emission");  //  Todo: northとsouthを分ける意味はあるのか？
        materialNorth.SetColor("_Emission", ColorWithBrightness(true, materialNorthEmission, brightnessOfForce));
        materialSouth.SetColor("_Emission", ColorWithBrightness(false, materialSouthEmission, brightnessOfForce));
    }

    Color ColorWithBrightness(bool isNorth, Color color, float brightness)
    {
        Color originalColor;
        if (isNorth)  // if文  Todo: if文を排除してパフォーマンスを上げたい
        {
            originalColor = originalCompassNorthColor;
        }
        else
        {
            originalColor = originalCompassSouthColor;
        }

        //brightness *= 0.01f;

        if (1.0f < brightness)  // 明るすぎるとき
        {
            brightness = 1.0f + (brightness - 1.0f) * 0.00050f;  // 最後の値のさじ加減が大切
        }
        else if (brightness <= 0.5f)  // 暗すぎるとき
        {
            brightness = 0.0f + brightness; // 明るさの最低値を決める。プロジェクターに出すときは0.5くらいで。
        }
        // Todo: 1行で書けないか？
        float colorR = originalColor.r * brightness;
        float colorG = originalColor.g * brightness;
        float colorB = originalColor.b * brightness;
        return new Color(colorR, colorG, colorB);
    }
}