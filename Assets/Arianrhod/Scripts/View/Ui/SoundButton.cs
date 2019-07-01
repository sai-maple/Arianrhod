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
        [SerializeField] protected SoundEffect _soundEffect = default;
        protected SignalBus signalBus = default;

        [Inject]
        public void Construct(SignalBus signal)
        {
            this.signalBus = signal;
        }

        public virtual IObservable<Unit> OnClickObservable()
        {
            return this.OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromMilliseconds(100))
                .Do(_ => signalBus.Fire(new SoundSignal(_soundEffect)));
        }
    }
}