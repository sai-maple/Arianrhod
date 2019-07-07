using UniRx.Async;
using UnityEngine;

namespace Arianrhod.View.Ui
{
    public class ScreenBase : MonoBehaviour
    {
        [SerializeField] private Animator _animator = default;
        private static readonly int[] AnimatorHash =
        {
            Animator.StringToHash("open"),
            Animator.StringToHash("close")
        };

        public virtual async void Open()
        {
            _animator.SetTrigger(AnimatorHash[0]);
            await UniTask.Delay(1000);
        }

        public virtual async  void Close()
        {
            _animator.SetTrigger(AnimatorHash[1]);
            await UniTask.Delay(1000);
        }
    }
}