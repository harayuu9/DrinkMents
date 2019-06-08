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
		//Client側のHubの初期化       
		_channel = new Channel("os3-364-15487.vs.sakura.ne.jp:12345", ChannelCredentials.Insecure);
		_chatHub = StreamingHubClient.Connect<IChatHub, IChatHubReceiver>(this._channel, this);
	}

	private async void OnDestroy()
	{
		await _chatHub.LeaveAsync();
		await this._chatHub.DisposeAsync();
		await this._channel.ShutdownAsync();
	}


	public async void AddNewUserData(UserData data)
	{
		await _chatHub.AddNewUserData(data);
	}

	public async void AddNewBoardData(BoardData data)
	{
		await _chatHub.AddNewBoardData(data);
	}

	public void OnUpdateBoard(List<BoardData> boardDatas)
	{

	}
}
