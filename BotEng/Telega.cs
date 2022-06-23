using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotEng
{
    public class Telega
    {
       
        static Totur Tutor = new Totur();
        static Dictionary<long, string> LastWord = new Dictionary<long, string>();


        public static Task TelegaErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(exception.Message);
            return Task.CompletedTask;
        }

        public static async Task TelegaUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            

            switch (update.Type)
            {
                case UpdateType.Message:
                    await BotOnMessageReceived(botClient, update.Message);
                    break;
                                   
                default:
                    await UnknownUpdateHandlerAsync(botClient, update);
                    break;
            }
        }

        
        static async Task<Message> Started(ITelegramBotClient botClient, Message message)
        {
                 string started = $"Приветствую, {message.From.FirstName} {message.From.LastName}!\n" 
                                                                ;

            return await botClient.SendTextMessageAsync(chatId: message.From.Id,
                                                       text: started);
        }


        static async Task<Message> Helped(ITelegramBotClient botClient, Message message)
        {
            string helped = "Использование:\n" +
                            "/add  <eng>  <rus>  —  Добавить английское слово и его перевод \n" +
                            "/check  <eng>  <rus>  —  Проверка правильности перевода слова \n"+
                            "/dellW <eng>  <rus>  —  Удаление слова"
                           ;

            return await botClient.SendTextMessageAsync(chatId: message.From.Id,
                                                       text: helped);
        }


        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message) 
        {
                       
            Console.WriteLine($"Тип полученного сообщения: {message.Type}");
            if ( message.Type != MessageType.Text)
                return;

           
               
     

            if (message.Text.ToLower() == "/start")
            {

                await Started(botClient, message);
                return;
            }

            if (message.Text.ToLower() == "/help")
            {

                await Helped(botClient, message);
                return;
            }


            var userId = message.From.Id;
           
            var msArgs = message.Text.Split(' ');
            string text="";
            switch (msArgs[0])
            {
                    case "/add":
                    text = AddWords(msArgs);
                    break;

                case "/check":
                    text = CheckWord(msArgs);
                 
                   
                    break;

                case "/get":
                    var newWord = GetRandomEngWord(userId);
                    text = Tutor.GetRandomEngWord(userId);
                    text = $"{text}\r\nСледущие слово : {newWord}";
                    break;

                case "/dell":
                    text = DellWords(msArgs);
                    break;

                //case "/myword":
                //    text = ReadWords(msArgs);
                //    break;

                case "/dellW":
                    text = DellWords1(msArgs);
                    break;


                default:
                    if (LastWord.ContainsKey(userId))
                    {
                        text = CheckWord(LastWord[userId], msArgs[0]);
                        newWord = GetRandomEngWord(userId);
                        text = $"{text}\r\nСледущие слово : {newWord}";
                    }
                    

                    break;
                         
            }
           
            await botClient.SendTextMessageAsync(chatId: message.From.Id, text:text);
            


        }




        private static string GetRandomEngWord(long userId) 
        {
            var text = Tutor.GetRandomEngWord(userId);
            if (LastWord.ContainsKey(userId))
                LastWord[userId] = text;
            else
                LastWord.Add(userId, text);
            return text;
        }

        private static string CheckWord(string[] msArr)
        {
            if (msArr.Length != 3)
                return "Неверное количество слов. Их должно быть 2 <eng>  <rus>.";
            else
            {

                return CheckWord(msArr[1], msArr[2]);
            }

        }
        private static string CheckWord(string eng, string rus)
        {
            if (Tutor.CheckWord(eng, rus))
                return "Правильно!";
            else
            {
                var correctAsnwer = Totur.Translate(eng);
                return $"Неверно. Правильный ответ: \"{correctAsnwer}\".";
            }

        }

        private static string AddWords(String[] msArr)
        {
            if (msArr.Length != 3)
                return "Неверное количество слов. Их должно быть 2 <eng>  <rus>";
            else
            {
                Tutor.AddWord(msArr[1], msArr[2]);
                return "Новое слово добавлено в словарь";
            }

        }

        //private static string ReadWords(String[] msArr)
        //{
        //    if (msArr.Length >= 0)
        //        Tutor.ReadWords();
        //    return "Ваш список слов";


        //}

        private static string DellWords(String[] msArr)
        {
            if (msArr.Length >= 0)
                Tutor.DellWord();
            return "Удален весь список слов";


        }

        private static string  DellWords1(String[] msArr)
        {
            if (msArr.Length != 3)
                return "Неверное количество слов. Их должно быть 2 <eng>  <rus>";
            Tutor.DellWord1(msArr[1], msArr[2]);
            return "Слово удалено";


        }


        private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            Console.WriteLine($"Неизвестный тип обновления: {update.Type}");
            return Task.CompletedTask;
        }




    } 
}
