using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviseWord
{
    public class Datas
    {
        public Datas()
        {
            Words = new();
            Translations = new();

            ResetSettings = false;
        }

        // 文件I/O
        public FileStream WordsFile;

        // 词库长度
        public int WordsCount { get; set; }

        // 词库
        public List<string> Words { get; set; }
        public List<string> Translations { get; set; }

        // 运行配置 清空用户配置
        public bool ResetSettings { get; set; }

        public void ImportWords(string fileName)
        {
            // 打开词库文件
            WordsFile = File.Open(fileName, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new(WordsFile, Encoding.UTF8);

            try
            {
                // 读入词库文件
                string FileLine;
                string[] FileLineSplit;
                while ((FileLine = streamReader.ReadLine()) is not null)
                {
                    FileLineSplit = FileLine.Split('\t');
                    Words.Add(FileLineSplit[0]);
                    Translations.Add(FileLineSplit[1]);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            streamReader.Close();
            WordsFile.Close();

            WordsCount = Words.Count;
        }

        public void ClearWords()
        {
            Words.Clear();
            Translations.Clear();
        }
    }
}
