using System;
using UniRx;
using UnityEngine;

namespace Arianrhod.View.Ui
{
    public class TitleScreen : ScreenBase
    {
        [SerializeField] private SoundButton _startButton = default;
        public IObservable<Unit> OnStart() => _startButton.OnClickObservable();
    }
}