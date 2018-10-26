using Smod2;
using Smod2.Attributes;

namespace Speedrun
{
    [PluginDetails(
        author = "LewisTehMinerz",
        name = "Speedrun",
        description = "Speedrun gamemode, written for Centurion.",
        id = "centurion.gamemode.speedrun",
        version = "1.0",
        SmodMajor = 3,
        SmodMinor = 0,
        SmodRevision = 0
        )]
    public class SpeedrunPlugin : Plugin
    {
        public bool activatesNextRound = false;
        public bool activated = false;

        public override void OnDisable() {}

        public override void OnEnable()
        {
            Info("Hello world!");
        }

        public override void Register()
        {
            AddCommand("speedrun", new SpeedrunCommand(this));
            AddEventHandlers(new SpeedrunEventHandler(this));
        }
    }
}
