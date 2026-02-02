using Project.Features.Economy.Runtime;
using Project.Core;
using UnityEngine;
using VertigoCase.Shared.Events;

namespace Project.Features.Fail.Runtime
{
    public class ReviveHandler : MonoBehaviour
    {
        private const string GOLD_KEY = "wheel_item_gold";
        private const string CASH_KEY = "wheel_item_cash";

        private void OnEnable()
        {
            EventManager.Subscribe<ReviveRequestedEvent>(OnReviveRequested);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<ReviveRequestedEvent>(OnReviveRequested);
        }

        private void OnReviveRequested(ReviveRequestedEvent e)
        {
            var key = e.CurrencyKey;

            int cost = key == GOLD_KEY ? GameRules.REVIVE_GOLD_COST : GameRules.REVIVE_CASH_COST;

            if (!Wallet.CanSpend(key, cost))
            {
                return;
            }

            Wallet.TrySpend(key, cost);
        }
    }
}

