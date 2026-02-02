namespace VertigoCase.Shared.Events
{
    public readonly struct WheelZoneChangedEvent
    {
        public readonly int Zone;
        public WheelZoneChangedEvent(int zone) => Zone = zone;
    }
}
