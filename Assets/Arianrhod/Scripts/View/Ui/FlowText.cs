using System;
using TMPro;
using UniRx;

namespace Arianrhod.View.Ui
{
    public interface IFlowText
    {
        void SetTxt(string message);
    }

    public class FlowText : TextMeshProUGUI, IFlowText
    {
        private IDisposable _subscription = Disposable.Empty;

        public void SetTxt(string message)
        {
            text = "";
            if (Equals(_subscription, Disposable.Empty)) _subscription.Dispose();
            var index = 0;
            _subscription = Observable.Interval(TimeSpan.FromMilliseconds(10))
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
                {
                    index++;
                    if (index >= message.Length)
                    {
                        _subscription.Dispose();
                        text = message;
                    }
                    else
                    {
                        var former = message.Substring(0, index);
                        var latter = message.Substring(index, message.Length - index);

                        text = former + "<alpha=#01>" + latter;
                    }
                });
        }
    }
}