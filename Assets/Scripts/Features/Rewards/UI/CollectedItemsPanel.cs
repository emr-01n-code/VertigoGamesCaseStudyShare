using System.Collections.Generic;
using UnityEngine;
using VertigoCase.Shared.Events;
using Project.Shared.Pooling;

namespace Project.Features.Rewards.UI
{
    public class CollectedItemsPanel : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Transform contentRoot;
        [SerializeField] private CollectedItemRowUI rowPrefab;

        private readonly Dictionary<string, CollectedItemRowUI> _rows = new();

        private ComponentPool<CollectedItemRowUI> _rowPool;

        private void Awake()
        {
            _rowPool = new ComponentPool<CollectedItemRowUI>(rowPrefab, preloadCount: 8, parent: contentRoot);
        }

        private void OnEnable()
        {
            EventManager.Subscribe<RewardsUpdatedEvent>(OnRewardsUpdated);
            EventManager.Subscribe<RewardsClearedEvent>(OnRewardsCleared);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<RewardsUpdatedEvent>(OnRewardsUpdated);
            EventManager.Unsubscribe<RewardsClearedEvent>(OnRewardsCleared);
        }

        private void OnRewardsUpdated(RewardsUpdatedEvent e)
        {
            IReadOnlyDictionary<string, int> totals = e.Totals;

            foreach (var kv in totals)
            {
                var key = kv.Key;
                var amountTotal = kv.Value;

                if (!_rows.TryGetValue(key, out var row))
                {
                    row = _rowPool.Rent();
                    _rows.Add(key, row);
                }

                row.Set(key, amountTotal);
            }
        }

        private void OnRewardsCleared(RewardsClearedEvent _)
        {
            foreach (var kv in _rows)
            {
                if (kv.Value != null)
                    _rowPool.Release(kv.Value);
            }
            _rows.Clear();
        }
    }
}
