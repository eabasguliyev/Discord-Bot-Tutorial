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
    [AttributeUsage((AttributeTargets.Method | AttributeTargets.Class))]
    public class RequiredCategoriesAttribute: CheckBaseAttribute
    {
        public IReadOnlyList<string> CategoryNames { get; }
        public ChannelCheckMode CheckMode { get; set; }
        public RequiredCategoriesAttribute(ChannelCheckMode checkMode, params string[] channelNames)
        {
            CheckMode = checkMode;
            CategoryNames = new ReadOnlyCollection<string>(channelNames);
        }
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            if (ctx.Guild == null || ctx.Member == null)
            {
                return Task.FromResult(false);
            }

            var contains = CategoryNames.Contains(ctx.Channel.Parent.Name, StringComparer.OrdinalIgnoreCase);

            return CheckMode switch
            {
                ChannelCheckMode.Any => Task.FromResult(contains),
                ChannelCheckMode.None =>Task.FromResult(!contains),

                _ => Task.FromResult(false),
            };


        }
    }
}
