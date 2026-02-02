namespace VertigoCase.Shared.Events
{
    public readonly struct RewardGrantedEvent
    {
        public readonly string ItemKey;
        public readonly int Amount;
        public readonly bool IsBomb;

        public RewardGrantedEvent(string itemKey, int amount, bool isBomb)
        {
            ItemKey = itemKey;
            Amount = amount;
            IsBomb = isBomb;
        }
    }
}
