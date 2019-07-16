using System;
using UniRx;
using UnityEngine;

namespace Arianrhod.View.Ui
{
    public class ClearScreen : ScreenBase
    {
        [SerializeField] private SoundButton _returnButton = default;
        public IObservable<Unit> OnReturn() => _returnButton.OnClickObservable();

    }
}