using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Arianrhod.View.Ui
{
    public interface ISoundButton
    {
        IObservable<Unit> OnClickObservable();
    }
    
    public class SoundButton : Button
    {
        [SerializeField] private SoundEffect _soundEffect = default;
        private SignalBus _signalBus = default;

        [Inject]
        public void Construct(SignalBus signal)
        {
            _signalBus = signal;
        }

        public virtual IObservable<Unit> OnClickObservable()
        {
            return this.OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromMilliseconds(100))
                .Do(_ => _signalBus.Fire(new SoundSignal(_soundEffect)));
        }
    }
}