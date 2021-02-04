using System;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace Discord_Bot_Tutorial.Commands
{
    class FunCommands:BaseCommandModule
    {
        [Command("ping")]
        [Description("Returns pong")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }

        [Command("add")]
        [Description("Adds two numbers together")]
        [RequireRoles(RoleCheckMode.Any, "lamer", "elachi")]
        public async Task Add(CommandContext ctx,
            [Description("First Number")] string numberOne,
            [Description("Second Number")] string numberTwo)
        {
            var isNum1 = int.TryParse(numberOne, out int number1);
            var isNum2 = int.TryParse(numberTwo, out int number2);

            string message = String.Empty;

            if (isNum1 && isNum2)
                message = (number1 + number2).ToString();
            else
                message = "This is not a number!";

            await ctx.Channel.SendMessageAsync(message)
                .ConfigureAwait(false);
        }

        [Command("hi")]
        [Description("Say hi to user.")]
        public async Task Handshake(CommandContext ctx, [Description("Bot username.")] string botName)
        {
            string message = String.Empty;
            if (ctx.Client.CurrentUser.Username == botName)
                message = $"Hi, {ctx.Member.Username}!";
            else
                message = "That is not my name!";

            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }
    }
}
