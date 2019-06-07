using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TicTacToeGameWPFClient.TicTacToeServiceReference;

namespace TicTacToeGameWPFClient
{
    public class CallBackHandler : ITicTacToeGameContractCallback
    {
        public void CreateButtons(int sideSize)
        {
            ((MainWindow)Application.Current.MainWindow).sideSize = sideSize;
            ((MainWindow)Application.Current.MainWindow).SetComboBoxField();
            ((MainWindow)Application.Current.MainWindow).AddButtonsToWrapPanel();
            int widthFirstColumn = (int)(sideSize * ((MainWindow)Application.Current.MainWindow).ButtonSize + ((MainWindow)Application.Current.MainWindow).ButtonMargin * 2 * sideSize);
            ((MainWindow)Application.Current.MainWindow).Column1.MaxWidth = ((MainWindow)Application.Current.MainWindow).Column1.MinWidth = widthFirstColumn;
            ((MainWindow)Application.Current.MainWindow).Row1.MinHeight = widthFirstColumn;
            ((MainWindow)Application.Current.MainWindow).windowGame.MinHeight = widthFirstColumn + 154;
            ((MainWindow)Application.Current.MainWindow).windowGame.MinWidth = widthFirstColumn + ((MainWindow)Application.Current.MainWindow).Column2.Width.Value + 16;
            ((MainWindow)Application.Current.MainWindow).labelStatusNewGame.Content = null;
        }

        public void DisableAllButtons()
        {
            int x = ((MainWindow)Application.Current.MainWindow).buttons.GetLength(0);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    ((MainWindow)Application.Current.MainWindow).buttons[i, j].IsEnabled = false;
                }
            }
        }

        public void DisableButton(int x, int y)
        {
            ((MainWindow)Application.Current.MainWindow).buttons[x, y].IsEnabled = false;
        }

        public void EnableAndClearAllButtons()
        {
            int x = ((MainWindow)Application.Current.MainWindow).buttons.GetLength(0);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    ((MainWindow)Application.Current.MainWindow).buttons[i, j].IsEnabled = true;
                    ((MainWindow)Application.Current.MainWindow).buttons[i, j].Content = null;
                }
            }
        }

        public void PrintAllMove(List<Move> moves)
        {
            moves.ForEach(m => PrintMove(m));
        }

        public void PrintMove(Move move)
        {
            Image imgSymbol = new Image();
            if (move.PlayerStatus == "player №1")
                imgSymbol.Source = ((MainWindow)Application.Current.MainWindow).CrossSource;
            else if (move.PlayerStatus == "player №2")
                imgSymbol.Source = ((MainWindow)Application.Current.MainWindow).ZeroSource;
            ((MainWindow)Application.Current.MainWindow).buttons[move.X, move.Y].Content = imgSymbol;
        }

        public void PrintNameClients(List<string> users)
        {
            ((MainWindow)Application.Current.MainWindow).richTextBoxUsers.Document.Blocks.Clear();
            ((MainWindow)Application.Current.MainWindow).richTextBoxUsers.AddRangeText(users);
        }

        public void SetMove(bool IsMove)
        {
            ((MainWindow)Application.Current.MainWindow).IsMove = IsMove;
        }

        public void SetStatus(string status)
        {
            ((MainWindow)Application.Current.MainWindow).labelStatusName.Content = status;
            ((MainWindow)Application.Current.MainWindow).move.PlayerStatus = status;
            if (status == "player №1")
            {
                ((MainWindow)Application.Current.MainWindow).UserSource = ((MainWindow)Application.Current.MainWindow).CrossSource;
                ((MainWindow)Application.Current.MainWindow).IsMove = true;
            }
            else if (status == "player №2")
            {
                ((MainWindow)Application.Current.MainWindow).UserSource = ((MainWindow)Application.Current.MainWindow).ZeroSource;
            }
        }

        public void Victory()
        {
            MessageBox.Show("Congratulations!!! You WIN!!!");
            if (((MainWindow)Application.Current.MainWindow).labelStatusName.Content.ToString() == "player №1")
            {
                ((MainWindow)Application.Current.MainWindow).buttonStart.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).comboBoxField.IsEnabled = true;
            }
            else
                ((MainWindow)Application.Current.MainWindow).labelStatusNewGame.Content = "For new game please wait player №1";
        }

        public void VictoryResults(string winnerStatus)
        {
            if (((MainWindow)Application.Current.MainWindow).labelStatusName.Content.ToString() == "player №1")
            {
                ((MainWindow)Application.Current.MainWindow).buttonStart.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).comboBoxField.IsEnabled = true;
            }
            else
                ((MainWindow)Application.Current.MainWindow).labelStatusNewGame.Content = "For new game please wait player №1";
            if (winnerStatus!=null)
                MessageBox.Show($"The game was won by: {winnerStatus}");
            else
                MessageBox.Show($"There are no winners in the game.");
        }
    }
}
