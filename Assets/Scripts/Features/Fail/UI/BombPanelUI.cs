using Project.Features.Economy.Runtime;
using Project.Core;
using UnityEngine;
using UnityEngine.UI;
using VertigoCase.Shared.Events;
using TMPro;

namespace Project.Features.Fail.UI
{
    public class BombPanelUI : MonoBehaviour
    {
        [Header("Root")]
        [SerializeField] private GameObject root; // ui_bomb_group

        [Header("Buttons")]
        [SerializeField] private Button giveUpButton;
        [SerializeField] private Button reviveGoldButton;
        [SerializeField] private Button reviveCashButton;

        [Header("Cost Texts (UI)")]
        [SerializeField] private TMP_Text reviveGoldCostText;
        [SerializeField] private TMP_Text reviveCashCostText;

        [Header("Currency Keys")]
        [SerializeField] private string goldKey = "wheel_item_gold";
        [SerializeField] private string cashKey = "wheel_item_cash";

        private void Awake()
        {
            if (root == null) root = gameObject;
            root.SetActive(false);
        }

        private void OnEnable()
        {
            EventManager.Subscribe<WheelSpinFailedEvent>(OnSpinFailed);

            // buton click
            if (giveUpButton != null) giveUpButton.onClick.AddListener(OnGiveUp);
            if (reviveGoldButton != null) reviveGoldButton.onClick.AddListener(OnReviveGold);
            if (reviveCashButton != null) reviveCashButton.onClick.AddListener(OnReviveCash);

            EventManager.Subscribe<DataSavedEvent>(OnDataSaved);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<WheelSpinFailedEvent>(OnSpinFailed);
            EventManager.Unsubscribe<DataSavedEvent>(OnDataSaved);

            if (giveUpButton != null) giveUpButton.onClick.RemoveListener(OnGiveUp);
            if (reviveGoldButton != null) reviveGoldButton.onClick.RemoveListener(OnReviveGold);
            if (reviveCashButton != null) reviveCashButton.onClick.RemoveListener(OnReviveCash);
        }

        private void OnSpinFailed(WheelSpinFailedEvent _)
        {
            root.SetActive(true);
            RefreshInteractables();
        }

        private void OnDataSaved(DataSavedEvent _)
        {
            if (root != null && root.activeInHierarchy)
                RefreshInteractables();
        }

        private void RefreshInteractables()
        {

            int goldCost = GameRules.REVIVE_GOLD_COST;
            int cashCost = GameRules.REVIVE_CASH_COST;

            if (reviveGoldCostText) reviveGoldCostText.text = goldCost.ToString();
            if (reviveCashCostText) reviveCashCostText.text = cashCost.ToString();

            if (reviveGoldButton != null)
                reviveGoldButton.interactable = Wallet.CanSpend(goldKey, goldCost);
            if (reviveCashButton != null)
                reviveCashButton.interactable = Wallet.CanSpend(cashKey, cashCost);
        }

        private void OnGiveUp()
        {
            EventManager.Publish(new RunRestartRequestedEvent());
            root.SetActive(false);
        }

        private void OnReviveGold()
        {
            EventManager.Publish(new ReviveRequestedEvent(goldKey));
            root.SetActive(false);
        }

        private void OnReviveCash()
        {
            EventManager.Publish(new ReviveRequestedEvent(cashKey));
            root.SetActive(false);
        }
    }
}
