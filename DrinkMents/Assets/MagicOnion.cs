using MagicOnion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IChatHubReceiver
{
	void OnUpdateBoard(List<BoardData> boardDatas);
	void OnUpdateUser(List<UserData> userDatas);
}

public interface IChatHub : IStreamingHub<IChatHub, IChatHubReceiver>
{
	Task AddNewUserData(UserData userData);
	Task LeaveAsync();
	Task AddNewBoardData(BoardData boardData);
	Task BoardAssign(int idx, UserData userData);
}