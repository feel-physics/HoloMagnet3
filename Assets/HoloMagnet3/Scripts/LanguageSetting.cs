using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
