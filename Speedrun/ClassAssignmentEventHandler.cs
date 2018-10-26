using System;
using System.Linq;

using Smod2.API;
using Smod2.Events;
using Smod2.EventHandlers;
using Smod2.EventSystem.Events;

namespace Speedrun
{
    partial class SpeedrunEventHandler : IEventHandlerInitialAssignTeam, IEventHandlerTeamRespawn
    {
        Team[] invalidTeams =
        {
            Team.SCP,
            Team.CHAOS_INSURGENCY,
            Team.SCIENTISTS,
            Team.TUTORIAL
        };

        
        private bool alreadyDClass = false;

        public void OnAssignTeam(PlayerInitialAssignTeamEvent ev)
        {
            if (plugin.activated)
            {
                if (plugin.Server.NumPlayers > 3)
                {
                    plugin.Debug("Enough players for randomization.");
                    if (invalidTeams.Contains(ev.Team))
                    {
                        plugin.Debug(ev.Player.Name + " is in an invalid team. Reassigning.");
                        var random = new Random().Next(0, 1);
                        if (random == 0)
                        {
                            ev.Team = Team.CLASSD;
                            plugin.Debug(ev.Player.Name + " is Class-D.");
                        }
                        else if (random == 1)
                        {
                            ev.Team = Team.NINETAILFOX;
                            plugin.Debug(ev.Player.Name + " is MTF.");
                        }
                        else
                        {
                            plugin.Error("!! RANDOM WAS NOT 0 OR 1! THE UNIVERSE IS BROKEN! !!");
                        }
                    } else
                    {
                        plugin.Debug(ev.Player.Name + " is not in an invalid team. Not reassigning.");
                    }
                } else
                {
                    plugin.Debug("Not enough players for randomization. Brute-force it.");
                    if (!alreadyDClass)
                    {
                        ev.Team = Team.CLASSD;
                        alreadyDClass = true;
                        plugin.Debug(ev.Player.Name + " is Class-D.");
                    } else
                    {
                        ev.Team = Team.NINETAILFOX;
                        plugin.Debug(ev.Player.Name + " is MTF.");
                    }
                }
            }
        }

        public void OnTeamRespawn(TeamRespawnEvent ev)
        {
            if (plugin.activated)
            {
                ev.SpawnChaos = false; // force ntf
                plugin.Debug("Forced NTF on team respawn.");
            }
        }
    }
}
