using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotEng
{
    public class Totur
    {
        private WordStorage _storage = new WordStorage();
        private static Dictionary<string, string> _dic;



        private Random _rand = new Random();

        public Totur() => _dic = _storage.GetAllWords();

        public void AddWord(string eng, string rus) 
        {
            if (!_dic.ContainsKey(eng))
            {
                _dic.Add(eng, rus);
                _storage.AddWord(eng, rus);
            }
           
        }

        //public void ReadWords()
        //{
        //    _storage.ReadWords();
        //}

        public void DellWord()
        {


            _storage.DellWord();



        }

        public void DellWord1(string eng, string rus)
        {

            _storage.DellWord1(eng, rus);
           
        }

        public bool CheckWord(string eng, string rus) 
        {
            var answer = _dic[eng];
            return answer.ToLower() == rus.ToLower();
                  
        }
        public static string Translate (string eng) 
        {
            if (_dic.ContainsKey(eng))
                return _dic[eng];
            else
                return null;
        }
        public string GetRandomEngWord(long userId) 
        {
            var r = _rand.Next(0, _dic.Count);
            var keys = new List<string>(_dic.Keys);
            return keys[r];
        }
    }
}
