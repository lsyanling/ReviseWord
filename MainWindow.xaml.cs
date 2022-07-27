using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ReviseWord
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeProperties();
        }

        // 词库数据
        public Datas Datas { get; set; }

        // 当前词号
        public int WordIndex { get; set; }

        // 定时器
        public DispatcherTimer Timer { get; set; }

        // 运行配置 置顶
        public bool WindowTopMost { get; set; }
        // 运行配置 随机开关
        public bool RandomMode { get; set; }

        // 设置窗口
        public MoreSetttings MoreSetttings { get; set; }

        // 初始化程序属性
        private void InitializeProperties()
        {
            // 初始化词库、定时器
            Datas = new();
            Timer = new();

            // 初始化运行配置
            WindowTopMost = false;
            RandomMode = false;

            try
            {
                // 导入词库
                Datas.ImportWords("Words.txt");
            }
            catch (Exception ex)
            {

            }

            // 初始化随机器
            Random random = new();
            WordIndex = random.Next(0, Datas.Words.Count);

            // 启动定时器
            Timer.Tick += SetTimer;
            Timer.Interval = new TimeSpan(0, 0, 0, 5);
            Timer.Start();
        }

        // 窗口置顶
        private void Window_Deactivated(object sender, EventArgs e)
        {
            ((Window)sender).Topmost = WindowTopMost;
        }

        private void Button_TopMost_Click(object sender, RoutedEventArgs e)
        {
            if (WindowTopMost)
            {
                WindowTopMost = false;
                ((Button)sender).Content = "置顶";
                ((Button)sender).Opacity = 0.5;
            }
            else
            {
                WindowTopMost = true;
                ((Button)sender).Content = "取消置顶";
                ((Button)sender).Opacity = 0.3;
            }
        }

        // 定时器任务
        public void SetTimer(object? sender, EventArgs e)
        {
            Button_Next_Click(null, null);
        }

        // 是否显示中文
        private void Button_Display_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_Translation.Visibility == Visibility.Visible)
            {
                TextBox_Translation.Visibility = Visibility.Hidden;
                Button_Display.Content = "显示中文";
            }
            else
            {
                TextBox_Translation.Visibility = Visibility.Visible;
                Button_Display.Content = "隐藏中文";
            }
        }

        // 上一个单词
        private void Button_Previous_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WordIndex--;
                TextBox_Word.Text = Datas.Words[(WordIndex >= 0 ? WordIndex : -WordIndex) % Datas.WordsCount];
                TextBox_Translation.Text = Datas.Translations[(WordIndex >= 0 ? WordIndex : -WordIndex) % Datas.WordsCount];
            }
            catch (Exception ex)
            {

            }
        }

        // 下一个单词
        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (RandomMode)
                {
                    Random random = new();
                    WordIndex = random.Next(0, Datas.WordsCount);
                }
                else
                {
                    WordIndex++;
                }
                TextBox_Word.Text = Datas.Words[(WordIndex >= 0 ? WordIndex : -WordIndex) % Datas.WordsCount];
                TextBox_Translation.Text = Datas.Translations[(WordIndex >= 0 ? WordIndex : -WordIndex) % Datas.WordsCount];
            }
            catch (Exception ex)
            {

            }
        }

        private void Button_Random_Click(object sender, RoutedEventArgs e)
        {
            if (RandomMode)
            {
                RandomMode = false;
                ((Button)sender).Content = "随机关 [点击开启]";
                ((Button)sender).Opacity = 0.5;

                Button_Previous.IsEnabled = true;
            }
            else
            {
                RandomMode = true;
                ((Button)sender).Content = "随机开 [点击关闭]";
                ((Button)sender).Opacity = 0.3;

                Button_Previous.IsEnabled = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.FirstTimeRun)
            {
                Settings.Default.Reset();
                Settings.Default.FirstTimeRun = false;
                Settings.Default.Save();
            }

            // 加载窗口上次关闭时的位置和宽高
            Left = Settings.Default.WindowLocationX;
            Top = Settings.Default.WindowLocationY;
            Width = Settings.Default.WindowWidth;
            Height = Settings.Default.WindowHeight;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (Datas.ResetSettings)
            {
                Settings.Default.Reset();
                Settings.Default.Save();
            }
            else
            {
                // 保存当前窗口的位置和宽高到配置文件
                Settings.Default.WindowLocationX = Left;
                Settings.Default.WindowLocationY = Top;
                Settings.Default.WindowWidth = Width;
                Settings.Default.WindowHeight = Height;
                Settings.Default.Save();
            }
        }

        private void Button_MoreSettings_Click(object sender, RoutedEventArgs e)
        {
            MoreSetttings = new(Datas);
            MoreSetttings.ShowDialog();
        }
    }
}
