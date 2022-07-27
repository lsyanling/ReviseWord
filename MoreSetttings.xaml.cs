using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ReviseWord
{
    /// <summary>
    /// MoreSetttings.xaml 的交互逻辑
    /// </summary>
    public partial class MoreSetttings : Window
    {
        public MoreSetttings(Datas datas)
        {
            InitializeComponent();

            Datas = datas;
        }

        // 词库和配置数据
        public Datas Datas { get; set; }

        private void Button_ResetConfigure_Click(object sender, RoutedEventArgs e)
        {
            Datas.ResetSettings = true;
            ((Button)sender).Opacity = 0.3;
        }

        private void Button_LoadWords_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*"
            };
            openFileDialog.ShowDialog();

            try
            {
                if (openFileDialog.FileNames.Length > 0)
                {
                    foreach (string fileName in openFileDialog.FileNames)
                    {
                        Datas.ImportWords(fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Button_ClearWords_Click(object sender, RoutedEventArgs e)
        {
            Datas.ClearWords();
            ((Button)sender).Opacity = 0.3;
        }
    }
}
