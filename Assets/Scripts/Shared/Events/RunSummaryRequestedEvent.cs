using System.Collections.Generic;

namespace VertigoCase.Shared.Events
{
    // Summary panel’e gösterilecek session snapshot
    public readonly struct RunSummaryRequestedEvent
    {
        public readonly IReadOnlyDictionary<string, int> SessionTotals;

        public RunSummaryRequestedEvent(IReadOnlyDictionary<string, int> sessionTotals)
        {
            SessionTotals = sessionTotals;
        }
    }
}
