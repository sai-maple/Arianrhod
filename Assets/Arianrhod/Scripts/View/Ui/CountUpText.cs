using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Arianrhod.View.Ui
{
    public class CountUpText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text = default;
        private int num;
        private int Number{
            set {
                num = value;
                _text.text = num.ToString();
            }
            get => num;
        }

        public async void Count(int start, int end, float duration = 1.0f)
        {
            Number = start;
            await DOTween.To(() => Number, (x) => Number = x, end, duration);
        }
    }
}