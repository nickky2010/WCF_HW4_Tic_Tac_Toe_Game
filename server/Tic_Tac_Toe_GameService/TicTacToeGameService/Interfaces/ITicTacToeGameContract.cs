using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TicTacToeGameService.Models;

namespace TicTacToeGameService.Interfaces
{
    [ServiceContract(CallbackContract = typeof(ICallBackClient))]
    public interface ITicTacToeGameContract
    {
        [OperationContract(IsOneWay = true)]
        void Join(string login, int sideSize);
        [OperationContract(IsOneWay = true)]
        void Leave();
        [OperationContract(IsOneWay = true)]
        void Move(Move move);
        [OperationContract(IsOneWay = true)]
        void ResetGame(int sideSize);
    }
}
