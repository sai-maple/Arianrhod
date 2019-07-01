using DG.Tweening;

namespace HULCUS
{
    public static class MyDOTweenExtension
    {
        // TweenのAwaiter
        public struct TweenAwaiter : System.Runtime.CompilerServices.ICriticalNotifyCompletion
        {
            private Tween _tween;

            public TweenAwaiter(Tween tween) => this._tween = tween;

            public bool IsCompleted => _tween.IsComplete();

            public void GetResult() { }

            public void OnCompleted(System.Action continuation) => _tween.OnKill(() => continuation());

            public void UnsafeOnCompleted(System.Action continuation) => _tween.OnKill(() => continuation());
        }

        // Tweenに対する拡張メソッド
        public static TweenAwaiter GetAwaiter(this Tween self)
        {
            return new TweenAwaiter(self);
        }
    }
}