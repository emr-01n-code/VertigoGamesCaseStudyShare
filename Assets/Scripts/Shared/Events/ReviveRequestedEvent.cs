namespace VertigoCase.Shared.Events
{
    public readonly struct ReviveRequestedEvent
    {
        public readonly string CurrencyKey;
        public ReviveRequestedEvent(string currencyKey) => CurrencyKey = currencyKey;
    }
}
