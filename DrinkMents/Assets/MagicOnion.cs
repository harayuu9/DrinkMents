using MagicOnion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Server -> Client API
/// </summary>
public interface IChatHubReceiver
{
	///// <summary>
	///// 参加したことをクライアントに伝える
	///// </summary>
	///// <param name="name">参加した人の名前</param>
	//void OnJoin(string name);
	void OnUpdateBoard(List<BoardData> boardDatas);
}

public interface IChatHub : IStreamingHub<IChatHub, IChatHubReceiver>
{
	Task AddNewUserData(UserData userData);
	Task LeaveAsync();
	Task AddNewBoardData(BoardData boardData);
}