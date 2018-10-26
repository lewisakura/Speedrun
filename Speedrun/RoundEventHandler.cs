using Smod2.API;
using Smod2.Events;
using Smod2.EventHandlers;
using System.Timers;

namespace Speedrun
{
    partial class SpeedrunEventHandler : IEventHandlerRoundRestart, IEventHandlerRoundStart, IEventHandlerCheckRoundEnd
    {
        private bool warheadDetonated = false;

        public void OnRoundRestart(RoundRestartEvent ev)
        {
            if (!plugin.activated && plugin.activatesNextRound)
            {
                plugin.Info("Speedrun gamemode is running this round!");
                plugin.activated = true;
            }
            else if (plugin.activated && !plugin.activatesNextRound)
            {
                plugin.activated = false;
            }
            // reset
            plugin.activatesNextRound = false;
            alreadyAssignedLeader = false;
            alreadyDClass = false;
            warheadDetonated = false;
            ignoreOne = false;
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            if (plugin.activated)
            {
                // in this version of the plugin there is no lcz decontamination timer, because it wouldn't make sense for people to die right away,
                // so you have 10 minutes to get out of the facility before the "nuke" goes off.
                var nukeTimer = new Timer
                {
                    AutoReset = false,
                    Enabled = true,
                    Interval = 10 * 60 * 1000 // 10 minutes
                };

                nukeTimer.Elapsed += delegate
                {
                    if (!warheadDetonated && plugin.activated)
                    {
                        plugin.Info("Nuke detonation.");
                        // waiting for smod2 update that brings a forced nuke detonation or a way to change the auto nuke timer, or both.
                        // for now, simulate it
                        foreach (var door in ev.Server.Map.GetDoors())
                        {
                            door.Open = true;
                        }

                        var secondTimer = new Timer
                        {
                            AutoReset = false,
                            Enabled = true,
                            Interval = 5000 // 5 seconds
                        };

                        secondTimer.Elapsed += delegate
                        {
                            ev.Server.Map.Shake();
                            foreach (var player in ev.Server.GetPlayers())
                            {
                                if (player.GetPosition().y < 900)
                                {
                                    player.Kill(); // automatically says that they died via nuke
                                }
                            }
                            warheadDetonated = true;
                        };
                    }
                };
            }
        }

        public void OnCheckRoundEnd(CheckRoundEndEvent ev)
        {
            ev.Status = ROUND_END_STATUS.ON_GOING;
            bool classDAlive = false;
            bool mtfAlive = false;
            foreach (var player in ev.Server.GetPlayers())
            {
                if (player.TeamRole.Team == Team.SPECTATOR) continue;

                if (player.TeamRole.Team == Team.NINETAILFOX) mtfAlive = true;

                if (player.TeamRole.Team == Team.CLASSD || player.TeamRole.Team == Team.CHAOS_INSURGENCY) classDAlive = true;
            }

            if (!classDAlive && !mtfAlive)
            {
                ev.Status = ROUND_END_STATUS.NO_VICTORY;
            } else if (classDAlive && !mtfAlive)
            {
                ev.Status = ROUND_END_STATUS.CI_VICTORY;
            } else if (!classDAlive && mtfAlive)
            {
                ev.Status = ROUND_END_STATUS.MTF_VICTORY;
            }
        }
    }
}
