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
    public class CpuCharacterPresenter : IInitializable,IDisposable
    {
        private readonly ICharacterView _characterView = default;
        private readonly IPhaseProvider _phaseProvider = default;
        private readonly IPhaseRegister _phaseRegister = default;
        private readonly ICurrentSkillModel _currentSkill = default;
        private readonly ITurnCharacterProvider _turnCharacter = default;
        private readonly IDamagePhaseFinalizer _damagePhaseFinalizer = default;
        private readonly IResidueEnemyRegister _enemyRegister = default;
        private readonly IMoveLoadProvider _moveLoadProvider = default;
        private readonly ICharacterMove _characterMove = default;
        private readonly ICpuUseCase _cpuUseCase = default;
        
        private readonly Character _character = default;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public CpuCharacterPresenter(
            ICharacterView characterView,
            IPhaseProvider phaseProvider,
            IPhaseRegister phaseRegister,
            ICurrentSkillModel currentSkill,
            ITurnCharacterProvider turnCharacter,
            IDamagePhaseFinalizer damagePhaseFinalizer,
            IResidueCharacterRegister residueCharacter,
            IResidueEnemyRegister enemyRegister ,
            IMoveLoadProvider moveLoadProvider,
            ICharacterMove characterMove ,
            ICpuUseCase cpuUseCase,
            Character character 
        )
        {
            _characterView = characterView;
            _phaseProvider = phaseProvider;
            _phaseRegister = phaseRegister;
            _currentSkill = currentSkill;
            _turnCharacter = turnCharacter;
            _damagePhaseFinalizer = damagePhaseFinalizer;
            _enemyRegister = enemyRegister;
            _moveLoadProvider = moveLoadProvider;
            _characterMove = characterMove;
            _cpuUseCase = cpuUseCase;
            _character = character;
        }
        
        public void Initialize()
        {
            var entity = _characterView.GetEntity();
            _character.Initialize(entity.Id, entity, entity.SkillEntities, entity.Owner);
            _enemyRegister.AddEnemy(_character);
            
            // 移動アニメーション
            _moveLoadProvider.OnLoadSubmit()
                .Where(_ => _characterView.GetEntity() == _turnCharacter.OnTurnCharacterChanged().Value.CharacterEntity)
                .Subscribe(OnMove).AddTo(_disposable);

            _phaseProvider.OnPhaseChanged()
                .Where(phase => phase == GamePhase.Damage)
                .Where(_ => _character == _turnCharacter.OnTurnCharacterChanged().Value)
                .Subscribe(_ =>
                {
                    _characterView.OnAnimation(_currentSkill.OnSkillChanged().Value.AnimationState);
                    _characterView.OnAnimationEnded()
                        .Take(1)
                        .Subscribe(animation =>
                        {
                            _damagePhaseFinalizer.EmitDamage();
                        });
                }).AddTo(_disposable);
            
            _character.OnDirectionChanged()
                .Subscribe(_characterView.SetRotation).AddTo(_disposable);
            _character.Position()
                .Subscribe(_characterView.SetPosition)
                .AddTo(_disposable);
            _character.OnHpChanged()
                .Where(hp => hp <= 0)
                .Subscribe(_ => _characterView.OnDead())
                .AddTo(_disposable);

            _phaseProvider.OnPhaseChanged()
                .Where(phase => phase == GamePhase.Move)
                .Where(_ => _character == _turnCharacter.OnTurnCharacterChanged().Value)
                .Subscribe(_ =>
                {
                    var load = _cpuUseCase.CpuMove();
                    var panelEntities = load.ToList();
                    if (panelEntities.Any())
                    {
                        _cpuUseCase.MoveCharacter(panelEntities);
                    }
                    else
                    {
                        _phaseRegister.SkipAttack();
                    }
                })
                .AddTo(_disposable);
                
            _phaseProvider.OnPhaseChanged()
                .Where(phase => phase == GamePhase.Attack)
                .Where(_ => _character == _turnCharacter.OnTurnCharacterChanged().Value)
                .Subscribe(_ =>
                {
                    if (!_cpuUseCase.CpuAttack())
                    {
                        _phaseRegister.SkipAttack();
                    }
                })
                .AddTo(_disposable);
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
            throw new NotImplementedException();
        }
    }
}