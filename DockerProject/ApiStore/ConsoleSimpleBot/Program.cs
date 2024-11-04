// See https://aka.ms/new-console-template for more information
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Start telegram bot {0}!", "t.me/BestMiniPigBot");

var token = "7229049738:AAF-8zRyFvJ8aI3naCLpXqY1xez87aKWU-4";

var botPig = new TelegramBotClient(token);


botPig.StartReceiving(Update, Error);

async Task Update(ITelegramBotClient client, Update update, CancellationToken token)
{
    var message = update.Message;
    var callbackQuery = update.CallbackQuery;

    if(callbackQuery != null)
    {
        string callbackData = callbackQuery.Data;
        if (callbackData == "btn1_callback")
        {
            await client.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: "You pressed Button 1"
            );
        }
        else if (callbackData == "btn2_callback")
        {
            await client.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: "You pressed Button 2"
            );
        }
    }

    if(message == null || message.Type != MessageType.Text)
        return;

    Console.WriteLine($"{message.Chat.FirstName} {message.Chat.LastName}" +
        $"\t|\t{message.Text}");

    if (message.Text.ToLower().Contains("/start"))
    {

        var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Button 1", "btn1_callback"),
                    InlineKeyboardButton.WithCallbackData("Button 2", "btn2_callback")
                }
            });

        // Send a message with InlineKeyboard
        await client.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Оберіть опцію:",
            replyMarkup: inlineKeyboard
        );


        //ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        //    {
        //        new KeyboardButton[] { "Білий  🥰"},
        //        new KeyboardButton[] { "Чорний 😍" },
        //        new KeyboardButton[] { "Рижий ❤️" },
        //    })
        //{
        //    ResizeKeyboard = true
        //};

        //Message sentMessage = await client.SendTextMessageAsync(
        //    chatId: message.Chat.Id,
        //    text: "Оберіть колір волося",
        //    replyMarkup: replyKeyboardMarkup,
        //    cancellationToken: token);
    }
}

async Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
{
    throw new NotImplementedException();
}


Console.ReadLine();
