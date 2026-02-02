using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VertigoCase.Shared.Events;
using Project.Features.Economy.Runtime;
using Project.Features.Wheel.Config;

namespace Project.Features.Economy.UI
{
    public class CurrencyDisplayUI : MonoBehaviour
    {
        [SerializeField] private string currencyKey; 
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text amountText;

        private void Start()
        {
            Refresh();
        }

        private void OnEnable()
        {
            EventManager.Subscribe<DataSavedEvent>(OnDataSaved);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<DataSavedEvent>(OnDataSaved);
        }

        private void OnDataSaved(DataSavedEvent _)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (WheelItemDatabase.Instance != null &&
                WheelItemDatabase.Instance.TryGet(currencyKey, out var so) &&
                so.Icon != null && iconImage != null)
            {
                iconImage.sprite = so.Icon;
                iconImage.enabled = true;
            }

            if (amountText != null)
                amountText.text = Wallet.Get(currencyKey).ToString();
        }
    }
}
