using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class BoardList : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		GameObject obj = transform.GetChild(1).gameObject;
		for (int i = 0; i < StateManager.AllBoardData.Count; i++)
		{
			var newObj = Instantiate(obj, transform);
			newObj.transform.position -= new Vector3(0, i * 50, 0);
			var texts = newObj.GetComponentsInChildren<Text>();
			texts[0].text = StateManager.AllBoardData[i].ToString();
			int nowPeople = StateManager.AllBoardData[i].Users.Count;
			int maxPeople = StateManager.AllBoardData[i].People;
			texts[1].text = nowPeople.ToString() + "/" + maxPeople.ToString();
			var tmpi = i;
			newObj.GetComponentInChildren<Button>().onClick.AddListener(() => { FindObjectOfType<DataSendServer>().BoardAssign(tmpi, StateManager.MyUserData); });
			if (nowPeople == maxPeople)
				newObj.SetActive(false);
		}
		obj.SetActive(false);
	}

	public void NewBoardCreate()
	{
		SceneManager.LoadScene("SampleScene");
	}

	public void UserIDView()
	{
		SceneManager.LoadScene("UserIDView");
	}
}
