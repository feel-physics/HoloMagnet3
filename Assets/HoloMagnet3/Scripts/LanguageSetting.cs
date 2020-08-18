using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

public class LanguageSetting : MonoBehaviour
{
	//言語タイプ.
	public enum LanguageType{
		//日本語.
		Japanese,
		//英語.
		English,
	}

	//アプリに設定された現在の言語設定.
	private static LanguageType currentLanguageType = LanguageType.Japanese;
	public static LanguageType CurrentLanguageType
	{
		get
		{
			return currentLanguageType;
		}
		set
		{
			currentLanguageType = value;
		}
	}

	//既存のテキストに対して、言語設定ごとに差し替えるテキストを保持する定数(数が少ないので、定数としてコード内に埋め込む).
	static readonly ReadOnlyDictionary<string, ReadOnlyDictionary<LanguageType, string>> localizeTextData = new ReadOnlyDictionary<string, ReadOnlyDictionary<LanguageType, string>>(new Dictionary<string, ReadOnlyDictionary<LanguageType, string>>(){
		{"磁力線表示", new ReadOnlyDictionary<LanguageType, string>(new Dictionary<LanguageType, string>(){
			{LanguageType.Japanese, "磁力線表示" },　{LanguageType.English, "Show Magnetic Force Lines"}
		})},
		{"次のシーン", new ReadOnlyDictionary<LanguageType, string>(new Dictionary<LanguageType, string>(){
			{LanguageType.Japanese, "次のシーン" },{LanguageType.English, "Proceed to next scene"}
		})},
	});

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	//言語設定を日本語にする.
	public void SwitchLanguageJapanese()
	{
		CurrentLanguageType = LanguageType.Japanese;
	}

	//言語設定を英語にする.
	public void SwitchLanguageEnglish()
	{
		CurrentLanguageType = LanguageType.English;
	}

}
