using System;
using Arianrhod.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace Arianrhod.Presenter
{
    public class CameraPresenter : IInitializable, IDisposable
    {
        private readonly Camera _camera = default;
        private readonly ITurnCharacterProvider _turnCharacter = default;

        private static readonly Vector3 Offset = new Vector3(1, 1, 1);

        private IDisposable _currentDisposable = default;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public CameraPresenter(Camera camera, ITurnCharacterProvider turnCharacter)
        {
            _camera = camera;
            _turnCharacter = turnCharacter;
        }

        public void Initialize()
        {
            _turnCharacter.OnTurnCharacterChanged()
                .Subscribe(character =>
                {
                    _currentDisposable.Dispose();
                    _currentDisposable = character.Position()
                        .Subscribe(pos => _camera.transform.position = pos + Offset);
                })
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}