using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotEng
{
    public class WordStorage
    {
        private const string _path = "wordstorage.txt";
        public Dictionary<string, string> GetAllWords()
        {
            try
            {
                var dic = new Dictionary<string, string>();
                if (File.Exists(_path))
                {

                    foreach (var line in File.ReadAllLines(_path))
                    {
                        var words = line.Split('|');
                        if (words.Length == 2)
                        {
                            dic.Add(words[0], words[1]);
                        }
                    }
                }
                return dic;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось считать файл со словарем");
                return new Dictionary<string, string>();
            }

        }

        public void AddWord(string eng, string rus)
        {
            try
            {
                using (var writer = new StreamWriter(_path, true))
                {
                    writer.WriteLine($"{eng}|{rus}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось добавить слово {eng} в словарь");

            }


        }

        //public void ReadWords()
        //{

        //    //using (StreamReader read = new StreamReader(_path))
        //    //    while (!read.EndOfStream) 
        //    //      Console.WriteLine(read.ReadLine());

        //    using (FileStream fstream = File.OpenRead(_path))
        //    {

        //        byte[] buffer = new byte[fstream.Length];

        //        fstream.Read(buffer, 0, buffer.Length);

        //        string textFromFile = Encoding.Default.GetString(buffer);
                
        //    }
           
        //}

        public void DellWord1(string eng, string rus)
        {

            var re = File.ReadAllLines(_path, Encoding.Default).Where(s => !s.Contains(eng));
            File.WriteAllLines(_path, re, Encoding.Default);

        }


        public void DellWord()
        {
            
            {
                using (var newWriter = new StreamWriter(_path, false))
                {
                    newWriter.Write("");


                }
            }
           
        }
               
    }
}
