using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserList : MonoBehaviour
{
	public GameObject userData;
	// Start is called before the first frame update
	void Start()
	{
		GameObject obj = transform.GetChild(1).gameObject;
		for (int i = 0; i < StateManager.AllUserData.Count; i++)
		{
			var newObj = Instantiate(obj, transform);
			newObj.transform.position -= new Vector3(0, i * 50, 0);
			newObj.GetComponentInChildren<Text>().text = StateManager.AllUserData[i].Class + "   " + StateManager.AllUserData[i].Name;
			int tmpi = i;
			newObj.GetComponentInChildren<Button>().onClick.AddListener(() => 
			{
				userData.SetActive(true);
				userData.transform.GetChild(0).GetComponent<Text>().text = StateManager.AllUserData[tmpi].Name;
				userData.transform.GetChild(1).GetComponent<Text>().text = StateManager.AllUserData[tmpi].Class;
				userData.transform.GetChild(2).GetComponent<Text>().text = StateManager.AllUserData[tmpi].Sex;
			});
		}
		obj.SetActive(false);
	}

	public void BoardScene()
	{
		SceneManager.LoadScene("BoardView");
	}

	public void CloseUserData()
	{
		userData.SetActive(false);
	}
}
