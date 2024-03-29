﻿using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class XmlUtil
{
	// シリアライズ
	public static T Seialize<T>(string filename, T data)
	{
		using (var stream = new FileStream(filename, FileMode.Create))
		{

			var serializer = new XmlSerializer(typeof(T));
			var streamWriter = new StreamWriter(stream, System.Text.Encoding.UTF8);//追加
			serializer.Serialize(streamWriter, data);//変更
		}

		return data;
	}

	// デシリアライズ
	public static T Deserialize<T>(string filename)
	{
		using (var stream = new FileStream(filename, FileMode.Open))
		{
			var serializer = new XmlSerializer(typeof(T));
			return (T)serializer.Deserialize(stream);
		}
	}
}

[Serializable]
[MessagePackObject]
public struct UserData
{
	[Key(0)]
	public string Class;
	[Key(1)]
	public string Name;
	[Key(2)]
	public string No;
	[Key(3)]
	public string Sex;

	public override string ToString()
	{
		string str = "Class:" + Class;
		str += " Name:" + Name;
		str += " No:" + No;
		str += " Sex:" + Sex;
		return str;
	}
}

[Serializable]
[MessagePackObject]
public struct BoardData
{
	[Serializable]
	public enum State
	{
		YES,
		NO,
		NONE
	}

	[Key(0)] public UserData Sender;
	[Key(1)] public string Date;
	[Key(2)] public string Time;
	[Key(3)] public string Place;
	[Key(4)] public int Money;
	[Key(5)] public int People;
	[Key(6)] public int Grade;
	[Key(7)] public List<string> Classs;
	[Key(8)] public State Sex;
	[Key(9)] public List<UserData> Users;

	public override string ToString()
	{
		string str = "作成者:" + Sender.Name;
		str += "日付:" + Date + ":" + Time + "場所" + Place;
		return str;
	}
}

public class StateManager : MonoBehaviour
{
	//InitObject BoardCreateObject
	[SerializeField] private GameObject InitObject, BoardCreateObject;

	[SerializeField] private Text InitClass = null;
	[SerializeField] private Text InitName = null;
	[SerializeField] private Text InitNo = null;
	[SerializeField] private Text InitSex = null;

	[SerializeField] private Text BoardDate = null;
	[SerializeField] private Text BoardTime = null;
	[SerializeField] private Text BoardPlace = null;
	[SerializeField] private Text BoardMoney = null;
	[SerializeField] private Text BoardPeople = null;
	[SerializeField] private Text BoardGrade = null;
	[SerializeField] private Text[] BoardClasss = null;
	[SerializeField] private Text BoardSex = null;

	private string SavePath;
	public static UserData MyUserData;
	public static List<UserData> AllUserData = new List<UserData>();
	public static List<BoardData> AllBoardData = new List<BoardData>();
	private static bool init = false;
	// Start is called before the first frame update
	void Start()
	{
		SavePath = Application.streamingAssetsPath + "/save";
		// iOSでは下記設定を行わないとエラーになる
#if UNITY_IPHONE
        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
#endif
		if (AllUserData.Count == 0)
			Load();
		if (AllUserData.Count > 0 && init == false)
		{
			MyUserData = AllUserData[0];
			FindObjectOfType<DataSendServer>().AddNewUserData(AllUserData[0]);
			init = true;
			StartCoroutine(DelayLoad(0.5f, "BoardView"));
		}
		BoardCreateObject.SetActive(init);
		InitObject.SetActive(!init);
		//StartCoroutine(StateView());
	}

	IEnumerator DelayLoad(float second, string name)
	{
		yield return new WaitForSeconds(second);
		SceneManager.LoadScene(name);
	}

	public void InitSubmit()
	{
		MyUserData.Class = InitClass.text;
		MyUserData.Name = InitName.text;
		MyUserData.No = InitNo.text;
		MyUserData.Sex = InitSex.text;
		FindObjectOfType<DataSendServer>().AddNewUserData(MyUserData);
		init = true;
		AllUserData.Add(MyUserData);
		Debug.Log(MyUserData.ToString());
		Save();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void BoardSubmit()
	{
		BoardData data;
		data.Sender = MyUserData;
		data.Date = BoardDate.text;
		data.Time = BoardTime.text;
		data.Place = BoardPlace.text;
		if (int.TryParse(BoardMoney.text, out int result))
			data.Money = result;
		else
		{
			Debug.LogError("金を数字で入れろ");
			return;
		}
		if (int.TryParse(BoardPeople.text, out int result2))
			data.People = result2;
		else
		{
			Debug.LogError("人に数字で入れろ");
			return;
		}
		if (int.TryParse(BoardGrade.text, out int result3))
			data.Grade = result3;
		else
		{
			Debug.LogError("学年に数字で入れろ");
			return;
		}
		data.Classs = new List<string>();
		data.Classs.Add(BoardClasss[0].text);
		data.Classs.Add(BoardClasss[1].text);
		data.Classs.Add(BoardClasss[2].text);
		if (BoardSex.text == "男")
			data.Sex = BoardData.State.YES;
		else if (BoardSex.text == "女")
			data.Sex = BoardData.State.NO;
		else
			data.Sex = BoardData.State.NONE;

		data.Users = new List<UserData>();
		data.Users.Add(MyUserData);
		FindObjectOfType<DataSendServer>().AddNewBoardData(data);
		XmlUtil.Seialize(SavePath + "Boad.xml", data);
		StartCoroutine(DelayLoad(0.5f, "BoardView"));
	}

	public void Save()
	{
		XmlUtil.Seialize(SavePath + "AllData.xml", AllUserData);
	}

	public void Load()
	{
		if (!File.Exists(SavePath + "AllData.xml"))
			return;
		AllUserData = XmlUtil.Deserialize<List<UserData>>(SavePath + "AllData.xml");
	}
}
