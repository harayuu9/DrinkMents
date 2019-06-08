using Grpc.Core;
using MagicOnion.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSendServer : MonoBehaviour, IChatHubReceiver
{
	private Channel _channel;
	private IChatHub _chatHub;

	private bool firstCheck = false;

	// Start is called before the first frame update
	void Awake()
	{
		if (FindObjectsOfType<DataSendServer>().Length > 1)
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		//Client側のHubの初期化       
		_channel = new Channel("localhost:12345", ChannelCredentials.Insecure);
		//_channel = new Channel("os3-364-15487.vs.sakura.ne.jp:12345", ChannelCredentials.Insecure);
		_chatHub = StreamingHubClient.Connect<IChatHub, IChatHubReceiver>(this._channel, this);
		firstCheck = true;
	}

	private async void OnDestroy()
	{
		if (firstCheck)
		{
			if (alive)
				await _chatHub.LeaveAsync();
			await this._chatHub.DisposeAsync();
			await this._channel.ShutdownAsync();
		}
	}

	private bool alive = false;

	public async void AddNewUserData(UserData data)
	{
		alive = true;
		await _chatHub.AddNewUserData(data);
	}

	public async void AddNewBoardData(BoardData data)
	{
		await _chatHub.AddNewBoardData(data);
	}

	public async void BoardAssign(int idx, UserData userData)
	{
		await _chatHub.BoardAssign(idx, userData);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void OnUpdateBoard(List<BoardData> boardDatas)
	{
		StateManager.AllBoardData = boardDatas;
	}

	public void OnUpdateUser(List<UserData> userDatas)
	{
		StateManager.AllUserData = userDatas;
	}
}
