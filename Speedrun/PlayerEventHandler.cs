using Smod2.API;
using Smod2.Events;
using Smod2.EventHandlers;
using System.Timers;

namespace Speedrun
{
    partial class SpeedrunEventHandler : IEventHandlerSetRole
    {
        private bool alreadyAssignedLeader = false;
        private bool ignoreOne = false; // used as a fix for the double setrole event

        public void OnSetRole(PlayerSetRoleEvent ev)
        {
            if (plugin.activated)
            {
                if (ignoreOne)
                {
                    if (ev.Player.TeamRole.Role == Role.CLASSD)
                    {
                        if (!alreadyAssignedLeader)
                        {
                            ev.Items.Add(ItemType.JANITOR_KEYCARD);
                            ev.Items.Add(ItemType.E11_STANDARD_RIFLE);
                            alreadyAssignedLeader = true;
                            plugin.Debug(ev.Player.Name + " is Class-D leader.");
                        }
                        else
                        {
                            ev.Items.Add(ItemType.COM15);
                        }
                        plugin.Debug("Assigned items for " + ev.Player.Name + ".");
                    }
                    else if (ev.Player.TeamRole.Role == Role.FACILITY_GUARD)
                    {
                        foreach (var item in ev.Player.GetInventory())
                        {
                            if (item.ItemType == ItemType.GUARD_KEYCARD)
                            {
                                item.Remove();
                            }
                        }
                        ev.Items.Add(ItemType.MTF_COMMANDER_KEYCARD);
                        plugin.Debug("Assigned items for " + ev.Player.Name + ".");
                    }
                    else
                    {
                        plugin.Debug("Nothing to assign for " + ev.Player.Name + ".");
                    }
                } else
                {
                    ignoreOne = true;
                }
            }
        }
    }
}
