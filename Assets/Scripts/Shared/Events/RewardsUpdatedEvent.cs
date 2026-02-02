using System.Collections.Generic;

namespace VertigoCase.Shared.Events
{
    public readonly struct RewardsUpdatedEvent
    {
        public readonly IReadOnlyDictionary<string, int> Totals;

        public RewardsUpdatedEvent(IReadOnlyDictionary<string, int> totals)
        {
            Totals = totals;
        }
    }
}
