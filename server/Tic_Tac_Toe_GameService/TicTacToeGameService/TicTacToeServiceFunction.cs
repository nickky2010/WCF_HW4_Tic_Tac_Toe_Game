using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using TicTacToeGameService.Interfaces;
using TicTacToeGameService.Models;

namespace TicTacToeGameService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class TicTacToeServiceFunction : ITicTacToeGameContract
    {
        static ConnectedUser connectedUser;
        static List<ConnectedUser> connectedUsers = new List<ConnectedUser>();
        static List<Move> moves = new List<Move>();
        static int sideSize = 0;
        static string winnerStatus = null;
        static int countMove = 0;

        public List<string> GetUsersList()
        {
            return (connectedUsers.Select(u => u.Login+ " - " +u.Status).ToList());
        }
        private void CreateArrayMoves()
        {
            moves.Clear();
            for (int i = 0; i < sideSize; i++)
            {
                for (int j = 0; j < sideSize; j++)
                {
                    moves.Add(new Move());
                }
            }
        }
        public void Join(string login, int sideSizeNeed)
        {
            ICallBackClient callBackClient = OperationContext.Current.GetCallbackChannel<ICallBackClient>();
            connectedUser = new ConnectedUser { CallBack = callBackClient, Login = login };
            if (connectedUsers.Count == 0)
            {
                connectedUser.Status = "player №1";
                sideSize = sideSizeNeed;
                CreateArrayMoves();
                callBackClient.CreateButtons(sideSize);
            }
            else if (connectedUsers.Count == 1)
            {
                connectedUser.Status = "player №2";
                callBackClient.CreateButtons(sideSize);
                callBackClient.PrintAllMove(moves);
            }
            else
            {
                connectedUser.Status = $"observer №{connectedUsers.Count - 1}";
                callBackClient.CreateButtons(sideSize);
                callBackClient.DisableAllButtons();
                callBackClient.PrintAllMove(moves);
            }
            connectedUsers.Add(connectedUser);
            callBackClient.SetStatus(connectedUser.Status);
            foreach (ConnectedUser u in connectedUsers)
            {
                u.CallBack.PrintNameClients(GetUsersList());
            }
        }
        private bool CheckResultWinAndClearTempResult(ref int resultP1, ref int resultP2)
        {
            if (resultP1 == sideSize)
            {
                winnerStatus = "player №1";
                return true;
            }
            else if (resultP2 == sideSize)
            {
                winnerStatus = "player №2";
                return true;
            }
            resultP1 = 0;
            resultP2 = 0;
            return false;
        }
        private bool IsGameOver()
        {
            // main diagonal
            int resultP1 = 0;
            int resultP2 = 0;
            for (int i = 0; i < sideSize; i++)
            {
                if (moves.ElementAt((i*sideSize)+i).PlayerStatus == "player №1")
                    resultP1++;
                else if (moves.ElementAt((i * sideSize) + i).PlayerStatus == "player №2")
                    resultP2++;
                else
                    break;
                if (resultP1 > 0 && resultP2 > 0)
                    break;
            }
            if(CheckResultWinAndClearTempResult(ref resultP1, ref resultP2))
                return true;
            // other diagonal
            for (int i = 0; i < sideSize; i++)
            {
                if (moves.ElementAt((i*sideSize)+(sideSize - 1 - i)).PlayerStatus == "player №1")
                    resultP1++;
                else if (moves.ElementAt((i * sideSize) + (sideSize - 1 - i)).PlayerStatus == "player №2")
                    resultP2++;
                else
                    break;
                if (resultP1 > 0 && resultP2 > 0)
                    break;
            }
            if (CheckResultWinAndClearTempResult(ref resultP1, ref resultP2))
                return true;
            // x diagonal
            for (int x = 0; x < sideSize; x++)
            {
                for (int y = 0; y < sideSize; y++)
                {
                    if (moves.ElementAt((x * sideSize) + y).PlayerStatus == "player №1")
                        resultP1++;
                    else if (moves.ElementAt((x * sideSize) + y).PlayerStatus == "player №2")
                        resultP2++;
                    else
                        break;
                    if (resultP1 > 0 && resultP2 > 0)
                        break;
                }
                if (CheckResultWinAndClearTempResult(ref resultP1, ref resultP2))
                    return true;
            }
            // y diagonal
            for (int y = 0; y < sideSize; y++)
            {
                for (int x = 0; x < sideSize; x++)
                {
                    if (moves.ElementAt((x * sideSize) + y).PlayerStatus == "player №1")
                        resultP1++;
                    else if (moves.ElementAt((x * sideSize) + y).PlayerStatus == "player №2")
                        resultP2++;
                    else
                        break;
                    if (resultP1 > 0 && resultP2 > 0)
                        break;
                }
                if (CheckResultWinAndClearTempResult(ref resultP1, ref resultP2))
                    return true;
            }
            if(countMove == sideSize * sideSize)
                return true;
            return false;
        }

        public void Leave()
        {
            connectedUsers.Remove(connectedUser);
            foreach (ConnectedUser u in connectedUsers)
            {
                u.CallBack.PrintNameClients(GetUsersList());
            }
        }

        public void Move(Move move)
        {
            moves.ElementAt((move.X * sideSize) + move.Y).PlayerStatus = move.PlayerStatus;
            countMove++;
            foreach (ConnectedUser u in connectedUsers)
            {
                if(u.Status != move.PlayerStatus)
                    u.CallBack.PrintMove(move);
            }
            if (connectedUsers.Count > 1 && connectedUsers[0].Status == move.PlayerStatus)
            {
                connectedUsers[0].CallBack.SetMove(false);
                connectedUsers[1].CallBack.DisableButton(move.X, move.Y);
                connectedUsers[1].CallBack.SetMove(true);
            }
            else if (connectedUsers.Count > 1 && connectedUsers[1].Status == move.PlayerStatus)
            {
                connectedUsers[1].CallBack.SetMove(false);
                connectedUsers[0].CallBack.DisableButton(move.X, move.Y);
                connectedUsers[0].CallBack.SetMove(true);
            }
            if (IsGameOver())
            {
                foreach (ConnectedUser u in connectedUsers)
                {
                    if (u.Status != winnerStatus)
                        u.CallBack.VictoryResults(winnerStatus);
                    else
                        u.CallBack.Victory();
                }
            }
        }

        public void ResetGame(int sideSizeNeed)
        {
            sideSize = sideSizeNeed;
            CreateArrayMoves();
            winnerStatus = null;
            countMove = 0;
            foreach (ConnectedUser u in connectedUsers)
            {
                if (connectedUsers.Count > 0 && connectedUsers[0].Status == u.Status)
                {
                    u.CallBack.CreateButtons(sideSize);
                }
                else if (connectedUsers.Count > 1 && connectedUsers[1].Status == u.Status)
                {
                    u.CallBack.CreateButtons(sideSize);
                    u.CallBack.PrintAllMove(moves);
                }
                else
                {
                    u.CallBack.CreateButtons(sideSize);
                    u.CallBack.DisableAllButtons();
                    u.CallBack.PrintAllMove(moves);
                }
            }
        }
    }
}
