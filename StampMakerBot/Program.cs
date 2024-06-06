using Microsoft.VisualBasic;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Types;
using static System.Net.Mime.MediaTypeNames;

namespace PatternMaker_bot
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new TelegramBotClient("PASTE YOUR BOT TOKEN HERE");

            client.StartReceiving(Update, Error);
            Console.ReadLine();
        }

        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;

            if (message.Text != null)
            {
                Console.WriteLine($"{message.Chat.FirstName}  ||  {message.Text}");
                if (message.Text.ToLower().Contains("/start"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Hello🥰 I`m Stamp Maker bot. Just send me picture and I will send you a stamp with it");
                    return;
                }

                if (message.Text.ToLower().Contains("/help"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Step 1:\n    Choose picture\nStep 2:\n   Click 3 dots and choose 'Send as file'\nStep 3:\n   Wait a few seconds and enjoy your new picture💓");
                    return;
                }

                if (message.Text.ToLower().Contains("/about"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "This bot was made by Ozerova Alisa❤️");
                    return;
                }

                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "I don't understand you😅 Write /help or send me picture in a file format");
                    return;
                }
            }

            if (message.Photo != null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Nice photo🤩 Let's try another variant: send me picture in a file format🖇");
                return;
            }
            
            if (message.Document != null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Wait a minute...");

                var field = update.Message.Document.FileId;
                var fileInfo = await botClient.GetFileAsync(field);
                var filePath = fileInfo.FilePath;

                string destinationFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{message.Document.FileName}";
                await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
                await botClient.DownloadFileAsync(filePath, fileStream);
                fileStream.Close();

                String param = $@"""{destinationFilePath}"" /effect=(5, -100) /convert={destinationFilePath.Replace(".jpg", " (edited).jpg")}";
                Console.WriteLine(param);
                Process.Start(@"PASTE YOUR WAY TO THE PROGRAM HERE", param);

                await Task.Delay(1500);

                await using Stream stream = System.IO.File.OpenRead(destinationFilePath.Replace(".jpg", " (edited).jpg"));
                await botClient.SendDocumentAsync(message.Chat.Id, InputFile.FromStream(stream, message.Document.FileName.Replace(".jpg", " (edited).jpg")));

                return;
            }

            if (message.Text != null)
            {
                Console.WriteLine($"{message.Chat.FirstName}  ||  {message.Text}");
                if (message.Text.ToLower().Contains("/exit"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "See you soon)");
                    return;

                }
            }
        }

        private static Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}