using Smod2.Events;
using Smod2.EventHandlers;

namespace Speedrun
{
    partial class SpeedrunEventHandler : IEventHandlerWarheadDetonate
    {
        public void OnDetonate()
        {
            warheadDetonated = true;
        }
    }
}
