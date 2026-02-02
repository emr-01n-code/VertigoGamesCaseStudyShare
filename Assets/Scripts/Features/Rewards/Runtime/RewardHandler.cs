using System.Collections.Generic;
using Project.Features.Rewards.Persistence;
using UnityEngine;
using VertigoCase.Shared.Events;

namespace Project.Features.Rewards.Runtime
{
    public class RewardHandler : MonoBehaviour
    {
        private readonly RewardBank _bank = new RewardBank();

        private void OnEnable()
        {
            EventManager.Subscribe<RewardGrantedEvent>(OnRewardGranted);

            EventManager.Subscribe<RunExitRequestedEvent>(OnExitSpin);
            EventManager.Subscribe<RunRestartRequestedEvent>(OnRestartSpin);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<RewardGrantedEvent>(OnRewardGranted);

            EventManager.Unsubscribe<RunExitRequestedEvent>(OnExitSpin);
            EventManager.Unsubscribe<RunRestartRequestedEvent>(OnRestartSpin);

        }

        private void OnRewardGranted(RewardGrantedEvent e)
        {

            if (e.IsBomb)
            {
                return;
            }

            _bank.Add(e.ItemKey, e.Amount);

            EventManager.Publish(new RewardsUpdatedEvent(_bank.Totals));

        }

        private void OnExitSpin(RunExitRequestedEvent _)
        {

            var snapshot = new Dictionary<string, int>(_bank.Totals);

            RewardSaver.SaveFromSessionTotals(snapshot);

            _bank.Clear();
            EventManager.Publish(new RewardsClearedEvent());
            EventManager.Publish(new RewardsUpdatedEvent(_bank.Totals));

            EventManager.Publish(new RunSummaryRequestedEvent(snapshot));
        }

        private void OnRestartSpin(RunRestartRequestedEvent _)
        {
            _bank.Clear();
            EventManager.Publish(new RewardsClearedEvent());
            EventManager.Publish(new RewardsUpdatedEvent(_bank.Totals));
        }

    }
}
