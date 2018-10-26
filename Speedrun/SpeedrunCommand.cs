using Smod2.Commands;

namespace Speedrun
{
    class SpeedrunCommand : ICommandHandler
    {
        private SpeedrunPlugin plugin;

        public SpeedrunCommand(SpeedrunPlugin plugin)
        {
            this.plugin = plugin;
        }

        public string GetCommandDescription()
        {
            return "Activates the Speedrun gamemode.";
        }

        public string GetUsage()
        {
            return "SPEEDRUN";
        }

        public string[] OnCall(ICommandSender sender, string[] args)
        {
            plugin.activatesNextRound = true;
            return new string[] { "Speedrun gamemode will activate on next round." };
        }
    }
}
