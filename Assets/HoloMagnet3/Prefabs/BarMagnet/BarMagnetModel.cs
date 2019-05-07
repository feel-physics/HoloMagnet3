using HoloToolkit.Unity;
using UnityEngine;

public class BarMagnetModel : Singleton<BarMagnetModel> {
    public GameObject NorthPoleReference;
    public GameObject SouthPoleReference;
    public GameObject MagneticForceLinePrefab;
    public bool IsDrawing;
}
