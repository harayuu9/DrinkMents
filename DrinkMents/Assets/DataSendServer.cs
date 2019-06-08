using Grpc.Core;
using MagicOnion.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSendServer : MonoBehaviour, IChatHubReceiver
{
	private Channel _channel;
	private IChatHub _chatHub;

	// Start is called before the first frame update
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		//Client側のHubの初期化       
		//_channel = new Channel("localhost:12345", ChannelCredentials.Insecure);
		_channel = new Channel("os3-364-15487.vs.sakura.ne.jp:12345", ChannelCredentials.Insecure);
		_chatHub = StreamingHubClient.Connect<IChatHub, IChatHubReceiver>(this._channel, this);
	}

	private async void OnDestroy()
	{
		if (alive)
			await _chatHub.LeaveAsync();
		await this._chatHub.DisposeAsync();
		await this._channel.ShutdownAsync();
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

	public void OnUpdateBoard(List<BoardData> boardDatas)
	{
		StateManager.AllBoardData = boardDatas;
	}

	public void OnUpdateUser(List<UserData> userDatas)
	{
		StateManager.AllUserData = userDatas;
	}
}
