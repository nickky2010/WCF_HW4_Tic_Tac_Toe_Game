using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TicTacToeGameWPFClient.TicTacToeServiceReference;

namespace TicTacToeGameWPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int sideSize = 0;
        int x = 0;
        int y = 0;
        char[] splitters = new char[] { 'x', 'y' };
        public Move move = new Move();
        InstanceContext instanceContext = new InstanceContext(new CallBackHandler());
        TicTacToeGameContractClient proxy;
        public double ButtonSize { get; set; } = 60d;
        public double ButtonMargin { get; set; } = 1d;
        public Button[,] buttons;
        public ImageSource CrossSource { get; set; }
        public ImageSource ZeroSource { get; set; }
        public ImageSource UserSource { get; set; }
        public bool IsMove { get; set; }

        private void Click_c(object sender, EventArgs e)
        {
            if (IsMove)
            {
                string[] pos = (sender as Button).Name.Split(splitters, StringSplitOptions.RemoveEmptyEntries);
                int.TryParse(pos[0], out x);
                int.TryParse(pos[1], out y);
                buttons[x, y].IsEnabled = false;
                Image userSymbol = new Image();
                userSymbol.Source = UserSource;
                buttons[x, y].Content = userSymbol;
                move.X = x;
                move.Y = y;
                proxy.Move(move);
                IsMove = false;
            }
            else
            {
                MessageBox.Show("Wait! It's not your turn now!");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            proxy = new TicTacToeGameContractClient(instanceContext);
            LoadResourses();
            FillToComboBoxField();
        }
        private void LoadResourses()
        {
            ResourceDictionary myResourceDictionary = new ResourceDictionary();
            myResourceDictionary.Source = new Uri("ButtonStyle.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary);
            CrossSource = BitmapFrame.Create(Application.GetResourceStream(new Uri("../../Images/cross.png", UriKind.Relative)).Stream);
            ZeroSource = BitmapFrame.Create(Application.GetResourceStream(new Uri("../../Images/zero.png", UriKind.Relative)).Stream);
        }

        private void FillToComboBoxField()
        {
            for (int i = 3; i < 11; i++)
            {
                comboBoxField.Items.Add($"{i}x{i}");
            }
            comboBoxField.SelectedIndex = 0;
        }
        
        private void ComboBoxField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string size = (sender as ComboBox).SelectedItem as string;
            int.TryParse(size.Remove(size.IndexOf('x')), out sideSize);            
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            buttonStart.IsEnabled = false;
            textBoxPlayerName.IsEnabled = false;
            comboBoxField.IsEnabled = false;
            if (labelStatusName.Content == null)
                proxy.JoinAsync(textBoxPlayerName.Text, sideSize);
            else if(labelStatusName.Content.ToString() == "player №1")
            {
                proxy.ResetGame(sideSize);
                IsMove = true;
            }
            else if (labelStatusName.Content.ToString() == "player №2")
            {
                IsMove = false;
            }
        }
        public void SetComboBoxField()
        {
            for (int i = 0; i < comboBoxField.Items.Count; i++)
            {
                string sizeCombo = comboBoxField.Items[i] as string;
                int.TryParse(sizeCombo.Remove(sizeCombo.IndexOf('x')), out int sideSizeCombo);
                if (sideSizeCombo == sideSize)
                {
                    comboBoxField.SelectedIndex = i;
                    return;
                }
            }
        }

        public void CreateButtons()
        {
            buttons = new Button[sideSize, sideSize];
            for (int i = 0; i < sideSize; i++)
            {
                for (int j = 0; j < sideSize; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].MaxWidth = buttons[i, j].MinWidth = ButtonSize;
                    buttons[i, j].MaxHeight = buttons[i, j].MinHeight = ButtonSize;
                    buttons[i, j].Margin = new Thickness(ButtonMargin);
                    buttons[i, j].Name = $"x{i}y{j}";
                    buttons[i, j].Click += Click_c;
                    buttons[i, j].Style = (Style)Application.Current.Resources["ButtonStyles"];
                }
            }
        }
        public void AddButtonsToWrapPanel()
        {
            CreateButtons();
            if (wrapPanel.Children.Count > 0)
                wrapPanel.Children.Clear();
            for (int i = 0; i < sideSize; i++)
            {
                for (int j = 0; j < sideSize; j++)
                {
                    wrapPanel.Children.Add(buttons[i, j]);
                }
            }
        }

        private void TextBoxPlayerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxPlayerName.Text == "")
                buttonStart.IsEnabled = false;
            else
                buttonStart.IsEnabled = true;
        }
        private async void WindowGame_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await proxy.LeaveAsync();
        }
    }
}
