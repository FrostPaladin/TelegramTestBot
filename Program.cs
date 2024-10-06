// See https://aka.ms/new-console-template for more information
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;

var token = Environment.GetEnvironmentVariable("TOKEN") ?? "7131927351:AAHqPA2Rj_cQGiBIMzBLd-wbD1rVz4L8buo";
Dictionary<int, string> nameChat = new Dictionary<int, string>();
int n = 1;
int i = 1;
int j = 1;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient(token, cancellationToken: cts.Token);
var me = await bot.GetMeAsync();
await bot.DropPendingUpdatesAsync();
bot.OnError += OnError;
bot.OnMessage += OnMessage;
bot.OnUpdate += OnUpdate;

Console.WriteLine($"@{me.Username} is running... Press Escape to terminate");
while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
cts.Cancel(); // stop the bot


async Task OnError(Exception exception, HandleErrorSource source)
{
    Console.WriteLine(exception);
    await Task.Delay(2000, cts.Token);
}


async Task OnMessage(Message msg, UpdateType type)
{
    if (n == 1)
    {
        await bot.SendTextMessageAsync(msg.Chat, "Дарова, братишка!");
        await bot.SendTextMessageAsync(msg.Chat, "Как тебя звать?");
        n++;
        j++;
    }
    else if (n == 2)
    {
        string userName = msg.Text;
        nameChat.Add(i, userName);
        string uzver = nameChat[1];
        var nameExists1 = nameChat.ContainsKey(1);
        await bot.SendTextMessageAsync(msg.Chat, "Хорошее имя, " + uzver + "!");
        await bot.SendTextMessageAsync(msg.Chat, "Велкам ту мастерская Шота у Ашота!\n" +
            "С чего начнем?");
        n++;
        {
            if (msg.Text is not { } text)
                Console.WriteLine($"Received a message of type {msg.Type}");
            else if (text.StartsWith('/'))
            {
                var space = text.IndexOf(' ');
                if (space < 0) space = text.Length;
                var command = text[..space].ToLower();
                if (command.LastIndexOf('@') is > 0 and int at)
                    if (command[(at + 1)..].Equals(me.Username, StringComparison.OrdinalIgnoreCase))
                        command = command[..at];
                    else
                        return;
                await OnCommand(command, text[space..].TrimStart(), msg);
            }
            else
                await OnTextMessage(msg);
        }
    }
    else
    {
        {
            if (msg.Text is not { } text)
                Console.WriteLine($"Received a message of type {msg.Type}");
            else if (text.StartsWith('/'))
            {
                var space = text.IndexOf(' ');
                if (space < 0) space = text.Length;
                var command = text[..space].ToLower();
                if (command.LastIndexOf('@') is > 0 and int at)
                    if (command[(at + 1)..].Equals(me.Username, StringComparison.OrdinalIgnoreCase))
                        command = command[..at];
                    else
                        return;
                await OnCommand(command, text[space..].TrimStart(), msg);
            }
            else
                await OnTextMessage(msg);
        }
    }
}
async Task OnUpdate(Update update)
{
    if (update is { CallbackQuery: { } query })
    {
        await bot.AnswerCallbackQueryAsync(query.Id, $"You picked {query.Data}");
        await bot.SendTextMessageAsync(query.Message!.Chat, $"User {nameChat[1]} clicked on {query.Data}");
    }
}
async Task OnTextMessage(Message msg)
{
    Console.WriteLine($"Received text '{msg.Text}' in {msg.Chat}");
    await OnCommand("/start", "", msg);
}

async Task OnCommand(string command, string args, Message msg)
{
    Console.WriteLine($"Received command: {command} {args}");
    switch (command)
    {
        case "/start":
            await bot.SendTextMessageAsync(msg.Chat, """
                <b><u>Меню</u></b>:
                /описание - расскажем, о чем вообще это дело
                /где      - а где собственно мы засели?
                /услуги   - что мы можем сделать
                /контакты - кому звонить в случае чего
                """, parseMode: ParseMode.Html, linkPreviewOptions: true);
            break;
        case "/описание":
            await bot.SendTextMessageAsync(msg.Chat, """
                Автомастерская <b><u>'Шота у Ашота'</u></b>
                Понтов мало, много дела в отличии от распальцованных сервисов
                Работаем над любыми тачками, забугорными и нашими
                Качество выше чем горы Кавказа!
                """, parseMode: ParseMode.Html, linkPreviewOptions: true);
            break;
        case "/где":
            await bot.SendTextMessageAsync(msg.Chat, "Наша точка находится на" +
                "\nШипиловская ул., 28, Москва, 115569 " +
                "\nПриезжай, " + nameChat[1] + ", мы тебя ждем!" +
                "\nРаботаем от зари до зари!");
            break;
        case "/контакты":
            await bot.SendTextMessageAsync(msg.Chat, "Как с нами созвониться:" +
                "\nАшот Танкян - 8 (495) XXX-XX-XX" +
                "\n- +7(901) 365-27-XX" +
                "\nД. Малакян - 8 (495) ZZZ-ZZ-ZZ" +
                "\nПочта: manilovecars@mail.ru");
            break;
        case "/то":
            await bot.SendTextMessageAsync(msg.Chat, "Техобслуживание::Замена масла - 800,00 ₽" +
                "\nТехобслуживание::Развал схождения - 1 500,00 ₽" +
                "\nТехобслуживание::Замена свечей - 800,00 ₽" +
                "\nТехобслуживание::Компьютерная диагностика - 1 200,00 ₽" +
                "\nТехобслуживание::Диагностика двигателя");
            break;
        case "/диагноз":
            await bot.SendTextMessageAsync(msg.Chat, "Диагностика::Компьютерная диагностика - 800,00 ₽" +
                "\nДиагностика::Диагностика подвески - 800,00 ₽" +
                "\nДиагностика::Система зажигания - 800,00 ₽" +
                "\nДиагностика::Система топливоподачи и система впрыска бензиновых двигателей -  1 500,00 ₽");
            break;
        case "/мойка":
            await bot.SendTextMessageAsync(msg.Chat, "Автомойка::Химчистка салона - 4 500,00 ₽" +
                "\nАвтомойка::Дезинфекция автомобиля - 300,00 ₽" +
                "\nАвтомойка::Комплексная автомойка премиум класса - 9 500, 00₽" +
                "\nАвтомойка::Полировка кузова и фар - 500,00 ₽");
            break;
        case "/двигатель":
            await bot.SendTextMessageAsync(msg.Chat, "Двигатель::Промывка инжектора и форсунок - 2 000,00 ₽" +
                "\nДвигатель::Замена масла - 800,00 ₽" +
                "\nДвигатель::Замена свечей - 600,00 ₽" +
                "\nДвигатель::Капитальный ремонт двигателей - по случаю");
            break;
        case "/шиномонтаж":
            await bot.SendTextMessageAsync(msg.Chat, "Шиномонтаж - 1 000,00 ₽" +
                "\nШиномонтаж::Балансировка - 1 000,00 ₽" +
                "\nШиномонтаж::Ремонт дисков - 1 000,00 ₽");
            break;
        case "/трансмиссия":
            await bot.SendTextMessageAsync(msg.Chat, "Трансмиссия::Замена масла в мостах - 1 000,00 ₽" +
                "\nТрансмиссия::Замена шрусов - 1 000,00 ₽" +
                "\nТрансмиссия::Замена сцепления - 3 000,00 ₽");
            break;
        case "/покраска":
            await bot.SendTextMessageAsync(msg.Chat, ":Покраска кузова - 5 900,00 ₽ ₽" +
                "\nПодбор краски - 1 000,00 ₽" +
                "\nУстранение царапин - 4 000,00 ₽");
            break;
        case "/услуги":
            await bot.SendTextMessageAsync(msg.Chat, """
                Мы выполняем кучу всего разного!
                Что интересует?
                <b><u>Меню</u></b>:
                /то          - техобслуживание
                /диагноз     - диагностика, посмотрим что с тачкой не так
                /мойка       - машинка будет блестеть как пик Эльбруса
                /двигатель   - заглянем под капот
                /трансмиссия - чтоб не улетел в кювет
                /шиномонтаж  - подготовим к зиме
                /покраска    - закрасим любые царапины
                """, parseMode: ParseMode.Html, linkPreviewOptions: true);
            break;
    }
}                                        
