using Smod2.API;
using Smod2.Events;
using Smod2.EventHandlers;
using System.Timers;

namespace Speedrun
{
    partial class SpeedrunEventHandler : IEventHandlerSetRole
    {
        private bool alreadyAssignedLeader = false;

        public void OnSetRole(PlayerSetRoleEvent ev)
        {
            if (plugin.activated)
            {
                var itemAssignmentTimer = new Timer
                {
                    AutoReset = false,
                    Enabled = true,
                    Interval = 100 // .1 seconds
                };

                itemAssignmentTimer.Elapsed += delegate
                {
                    if (ev.Player.TeamRole.Role == Role.CLASSD)
                    {
                        if (!alreadyAssignedLeader)
                        {
                            ev.Player.GiveItem(ItemType.JANITOR_KEYCARD);
                            ev.Player.GiveItem(ItemType.E11_STANDARD_RIFLE);
                            alreadyAssignedLeader = true;
                            plugin.Debug(ev.Player.Name + " is Class-D leader.");
                        }
                        else
                        {
                            ev.Player.GiveItem(ItemType.COM15);
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
                        ev.Player.GiveItem(ItemType.MTF_COMMANDER_KEYCARD);
                        plugin.Debug("Assigned items for " + ev.Player.Name + ".");
                    }
                    else
                    {
                        plugin.Debug("Nothing to assign for " + ev.Player.Name + ".");
                    }
                };
            }
        }
    }
}
