using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserList : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		GameObject obj = transform.GetChild(1).gameObject;
		for (int i = 0; i < StateManager.AllUserData.Count; i++)
		{
			var newObj = Instantiate(obj, transform);
			newObj.transform.position -= new Vector3(0, i * 50, 0);
			newObj.GetComponentInChildren<Text>().text = StateManager.AllUserData[i].ToString();
		}
		obj.SetActive(false);
	}
}
