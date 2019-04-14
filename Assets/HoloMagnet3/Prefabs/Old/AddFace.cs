using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if !UNITY_EDITOR
using System;
using Windows.Storage;
using Windows.Storage.Search;
#endif

public class AddFace : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //PrimitiveTypeを使って生成するオブジェクトの形を指定
        GameObject planeNorth = GameObject.CreatePrimitive(PrimitiveType.Plane);
        GameObject planeSouth = GameObject.CreatePrimitive(PrimitiveType.Plane);

        planeNorth.transform.parent = gameObject.transform;
        planeSouth.transform.parent = gameObject.transform;

        //位置をてきとうに設定
        planeNorth.transform.localPosition = new Vector3(0.0125f,  0.0375f, -0.0251f);
        planeSouth.transform.localPosition = new Vector3(0.0125f, -0.0375f, -0.0255f);

        planeNorth.transform.localScale = new Vector3(0.0025f, 1f, 0.0025f);
        planeSouth.transform.localScale = new Vector3(0.0025f, 1f, 0.0025f);

        planeNorth.transform.Rotate(90, 180, 0);
        planeSouth.transform.Rotate(90, 180, 0);

        /*
        SetTextureToPlane("FaceNorth", planeNorth);
        SetTextureToPlane("FaceSouth", planeSouth);
        */
#if !UNITY_EDITOR
        SetTextureFromCameraRollToPlane(planeNorth, planeSouth);
#endif
    }

    // Update is called once per frame
    void Update () {
		
	}

#if !UNITY_EDITOR
    async void SetTextureFromCameraRollToPlane(GameObject planeNorth, GameObject planeSouth)
    {
        // カメラロールフォルダは以下のように取得する
        StorageFolder CameraFolder = Windows.Storage.KnownFolders.CameraRoll;
        // GetFilesAsysnc()でファイルのリストを取得する
        var filesInFolder = await CameraFolder.GetFilesAsync();
        
        // 以下await後の処理ここから
        int count = filesInFolder.Count;
        // 最後から2番目の写真をN極に貼り付けるために取得する
        string filePathNorth = filesInFolder[count - 2].Path;
        // 最後の写真をS極に貼り付けるために取得する
        string filePathSouth = filesInFolder[count - 1].Path;

        // 2枚の写真をそれぞれ貼り付ける
        SetTextureToPlane(filePathNorth, planeNorth);
        SetTextureToPlane(filePathSouth, planeSouth);
        // 以下await後の処理ここまで
    }
#endif

    void SetTextureToPlane(string filePath, GameObject plane)
    {
        Texture2D texture = LoadJPGorPNG(filePath);  // ファイルパスからテクスチャを取得する

        texture = GetSquareCroppedTexture(texture);  // 正方形にする

        Material material = new Material(Shader.Find("HoloToolkit/StandardFast"));
        material.EnableKeyword("_EMISSION");
        material.SetTexture("_EmissionMap", texture);
        material.SetColor("_EmissionColor", Color.gray);
        plane.GetComponent<Renderer>().material = material;
    }

    public static Texture2D LoadJPGorPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions. byte 配列からテクスチャへ変換し PNG/JPG 画像を読み込みます
        }
        return tex;
    }

    Texture2D GetSquareCroppedTexture(Texture2D textureOriginal)
    {
        int length = textureOriginal.height;
        int margin = (textureOriginal.width - length) / 2;
        Color[] c = textureOriginal.GetPixels(margin, 0, length, length);
        Texture2D textureCropped = new Texture2D(length, length);
        textureCropped.SetPixels(c);
        textureCropped.Apply();
        return textureCropped;
    }
}
