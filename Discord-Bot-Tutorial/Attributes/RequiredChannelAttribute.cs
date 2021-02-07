using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Discord_Bot_Tutorial.Attributes
{
    class RequiredChannelAttribute:CheckBaseAttribute
    {
        public IReadOnlyList<string> ChannelNames { get; }
        public ChannelCheckMode CheckMode { get; set; }

        public RequiredChannelAttribute(ChannelCheckMode checkMode, params string[] channelNames)
        {
            ChannelNames = new ReadOnlyCollection<string>(channelNames);
            CheckMode = checkMode;
        }
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            if (ctx.Guild == null || ctx.Member == null)
            {
                return Task.FromResult(false);
            }

            var contains = ChannelNames.Contains(ctx.Channel.Name, StringComparer.OrdinalIgnoreCase);

            return CheckMode switch
            {
                ChannelCheckMode.Any => Task.FromResult(contains),
                ChannelCheckMode.None => Task.FromResult(!contains),

                _ => Task.FromResult(false)
            };
        }
    }
}
