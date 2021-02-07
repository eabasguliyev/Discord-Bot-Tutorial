using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using Discord_Bot_Tutorial.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace Discord_Bot_Tutorial.Commands
{
    class FunCommands:BaseCommandModule
    {
        [Command("ping")]
        [Description("Returns pong")]
        [RequiredCategories(ChannelCheckMode.Any, "Text Channels", "Example")]
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
        [RequiredChannel(ChannelCheckMode.Any, "general")]
        public async Task Handshake(CommandContext ctx, [Description("Bot username.")] string botName)
        {
            string message = String.Empty;
            if (ctx.Client.CurrentUser.Username == botName)
                message = $"Hi, {ctx.Member.Username}!";
            else
                message = "That is not my name!";

            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }

        [Command("respondMessage")]
        public async Task RespondMessage(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message =
                await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User)
                    .ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Content).ConfigureAwait(false);
        }

        [Command("respondReaction")]
        public async Task RespondReaction(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message =
                await interactivity.WaitForReactionAsync(x => x.Channel == ctx.Channel && x.User == ctx.User)
                    .ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Emoji).ConfigureAwait(false);
        }

        [Command("poll")]

        public async Task Poll(CommandContext ctx, TimeSpan duration, params DiscordEmoji[] emojiOptions)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var options = emojiOptions.Select(x => x.ToString());

            var pollEmbed = new DiscordEmbedBuilder()
            {
                Title = "Poll",
                Description = string.Join(" ", options)
            };

            var pollMessage = await ctx.Channel.SendMessageAsync(pollEmbed).ConfigureAwait(false);

            foreach (var option in emojiOptions)
            {
                await pollMessage.CreateReactionAsync(option).ConfigureAwait(false);
            }

            var result = await interactivity.
                CollectReactionsAsync(pollMessage, duration).ConfigureAwait(false);

            var distinctResult = result.Distinct(); // duplicateleri silir.

            var results = distinctResult.Select(x => $"{x.Emoji}: {x.Total}");
            await ctx.Channel.SendMessageAsync(string.Join("\n", results)).ConfigureAwait(false);
        }
        
    }
}
