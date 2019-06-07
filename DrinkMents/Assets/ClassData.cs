using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassData : MonoBehaviour
{
	Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
		dropdown = GetComponent<Dropdown>();
		List<string> classNames = new List<string>()
		{
			"ゲーム制作",
			"ゲーム企画",
			"ゲームデザイン",
			"CG映像",
			"グラフィックデザイン",
			"アニメーター",
			"イラストレーター",
			"カーデザイン",
			"カーモデラー",
			"ロボット開発",
			"高度情報処理",
			"WEB開発",
			"サウンドエンジニア",
			"サウンドクリエイター"
		};

		dropdown.AddOptions(classNames);
    }
}
