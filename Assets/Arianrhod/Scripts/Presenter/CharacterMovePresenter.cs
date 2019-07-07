using System;
using System.Collections.Generic;
using System.Linq;
using Arianrhod.Entity;
using Arianrhod.Model;
using Arianrhod.UseCase;
using Arianrhod.View.Game;
using UniRx;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace Arianrhod.Presenter
{
    public class CharacterMovePresenter : IInitializable, IDisposable
    {
        private readonly ICharacterView _characterView = default;
        private readonly IMoveLoadProvider _moveLoadProvider = default;
        private readonly ITurnCharacterProvider _turnCharacter = default;
        private readonly ICharacterMove _characterMove = default;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public CharacterMovePresenter(
            ICharacterView characterView,
            IMoveLoadProvider moveLoadProvider,
            ITurnCharacterProvider turnCharacter,
            ICharacterMove characterMove 
        )
        {
            _characterView = characterView;
            _moveLoadProvider = moveLoadProvider;
            _turnCharacter = turnCharacter;
            _characterMove = characterMove;
        }

        public void Initialize()
        {
            _moveLoadProvider.OnLoadSubmit()
                .Where(_ => _characterView.GetEntity() == _turnCharacter.OnTurnCharacterChanged().Value.CharacterEntity)
                .Subscribe(OnMove).AddTo(_disposable);
        }

        private async void OnMove(IEnumerable<PanelEntity> loadEntities)
        {
            var panelEntities = loadEntities.ToList();
            var load = panelEntities.Select(value => new Vector3(value.X, 1, value.Y));
            await UniTask.Run(() => _characterView.DoMove(load));
            _characterMove.MoveCharacter(panelEntities);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}