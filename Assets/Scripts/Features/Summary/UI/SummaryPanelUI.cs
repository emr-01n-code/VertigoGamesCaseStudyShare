using System.Collections.Generic;
using System.Linq;
using Project.Features.Wheel.Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VertigoCase.Features.Wheel.Data;
using VertigoCase.Shared.Events;

namespace Project.Features.Summary.UI
{
    public class SummaryPanelUI : MonoBehaviour
    {
        [Header("Root")]
        [SerializeField] private GameObject root;

        [Header("Texts")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text subtitleText;

        [Header("Cards")]
        [SerializeField] private Transform contentRoot;
        [SerializeField] private SummaryRewardCardUI cardPrefab;

        [Header("Buttons")]
        [SerializeField] private Button closeButton;
        [SerializeField] private Button restartButton;

        private readonly List<SummaryRewardCardUI> _spawned = new();

        private void Awake()
        {
            if (root == null) root = gameObject;
            root.SetActive(false);
        }

        private void OnEnable()
        {
            EventManager.Subscribe<RunSummaryRequestedEvent>(OnSummaryRequested);

            if (closeButton != null) closeButton.onClick.AddListener(OnClose);
            if (restartButton != null) restartButton.onClick.AddListener(OnRestart);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<RunSummaryRequestedEvent>(OnSummaryRequested);

            if (closeButton != null) closeButton.onClick.RemoveListener(OnClose);
            if (restartButton != null) restartButton.onClick.RemoveListener(OnRestart);
        }

        private void OnSummaryRequested(RunSummaryRequestedEvent e)
        {
            root.SetActive(true);

            if (titleText != null) titleText.text = "CASHED OUT!";
            if (subtitleText != null) subtitleText.text = "Your rewards were added to your wallet";

            RebuildCards(e.SessionTotals);
        }

        private void RebuildCards(IReadOnlyDictionary<string, int> totals)
        {
            ClearCards();

            if (totals == null || totals.Count == 0) return;

            var ordered = totals
                .OrderBy(kv =>
                {
                    if (TryGetTier(kv.Key, out var tier))
                        return TierOrder(tier);
                    return 99;
                })
                .ThenByDescending(kv => kv.Value)
                .ToList();

            foreach (var kv in ordered)
            {
                var card = Instantiate(cardPrefab, contentRoot);
                card.Set(kv.Key, kv.Value);
                _spawned.Add(card);
            }
        }

        private static int TierOrder(WheelItemTier tier)
        {
            return tier switch
            {
                WheelItemTier.Gold => 0,
                WheelItemTier.Silver => 1,
                WheelItemTier.Bronze => 2,
                _ => 99
            };
        }

        private bool TryGetTier(string itemKey, out WheelItemTier tier)
        {
            tier = WheelItemTier.Bronze;

            if (WheelItemDatabase.Instance == null) return false;
            if (!WheelItemDatabase.Instance.TryGet(itemKey, out var so) || so == null) return false;

            tier = so.Tier;
            return true;
        }

        private void ClearCards()
        {
            for (int i = 0; i < _spawned.Count; i++)
            {
                if (_spawned[i] != null)
                    Destroy(_spawned[i].gameObject);
            }
            _spawned.Clear();
        }

        private void OnClose()
        {
            root.SetActive(false);
        }

        private void OnRestart()
        {
            root.SetActive(false);
            EventManager.Publish(new RunRestartRequestedEvent());
        }
    }
}
