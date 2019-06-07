using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TicTacToeGameService.Models;

namespace TicTacToeGameService.Interfaces
{
    public interface ICallBackClient
    {
        [OperationContract(IsOneWay = true)]
        void PrintMove(Move move);
        [OperationContract(IsOneWay = true)]
        void DisableAllButtons();
        [OperationContract(IsOneWay = true)]
        void DisableButton(int x, int y);
        [OperationContract(IsOneWay = true)]
        void CreateButtons(int sideSize);
        [OperationContract(IsOneWay = true)]
        void PrintAllMove(List<Move> moves);
        [OperationContract(IsOneWay = true)]
        void PrintNameClients(List<string> users);
        [OperationContract(IsOneWay = true)]
        void SetStatus(string status);
        [OperationContract(IsOneWay = true)]
        void Victory();
        [OperationContract(IsOneWay = true)]
        void VictoryResults(string winnerStatus);
        [OperationContract(IsOneWay = true)]
        void SetMove(bool IsMove);
    }
}
