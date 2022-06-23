using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace BotEng
{
    class Program
    {
       
        static TelegramBotClient Bot = new TelegramBotClient("5573970950:AAGZqvIKMygWDRHq-FqT1nHYRfjAFHc9z04");    
        static CancellationTokenSource cts = new CancellationTokenSource();
        static void Main(string[] args)
        {
            ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
            Bot.StartReceiving(Telega.TelegaUpdateAsync,
                               Telega.TelegaErrorAsync,
                               receiverOptions,
                               cts.Token);

            Console.WriteLine($"Бот запущен и ждет сообщения...");
            Console.ReadLine();

            // Отправить запрос на отмену, чтобы остановить бота
            cts.Cancel();
        }
    }
}
