using System;
using System.Collections.Generic;
using System.Linq;
using Arianrhod.Model;
using Arianrhod.UseCase;
using Arianrhod.View.Game;
using UniRx;
using UniRx.Async;
using Zenject;

namespace Arianrhod.Presenter
{
    public class DiceRollPresenter : IInitializable ,IDisposable
    {
        private readonly IDiceFactory _diceFactory = default;
        private readonly IPhaseProvider _phaseProvider = default;
        private readonly IDicePhaseFinalizer _dicePhaseFinalizer = default;
        private readonly ISkillRollDetail _skillRollDetail = default;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public DiceRollPresenter(
            IDiceFactory diceFactory,
            IPhaseProvider phaseProvider,
            IDicePhaseFinalizer dicePhaseFinalizer,
            ISkillRollDetail skillRollDetail
        )
        {
            _diceFactory = diceFactory;
            _phaseProvider = phaseProvider;
            _dicePhaseFinalizer = dicePhaseFinalizer;
            _skillRollDetail = skillRollDetail;
        }

        public void Initialize()
        {
            _phaseProvider.OnPhaseChanged()
                .Where(phase => phase == Phase.Dice)
                .Subscribe(_ => DiceRoll())
                .AddTo(_disposable);
        }

        private async void DiceRoll()
        {
            var dices = new List<DiceRollView>();
            for (var i = 0; i < _skillRollDetail.RollNum(); i++)
            {
                dices.Add(_diceFactory.Create(_skillRollDetail.GetSkillDiceType(), i));
            }

            await UniTask.Delay(500);

            var tasks = new List<UniTask<int>>();
            foreach (var dice in dices)
            {
                tasks.Add(dice.OnRoll());
            }

            var damage = await UniTask.WhenAll(tasks.Select(_ => _).ToArray());
            
            _dicePhaseFinalizer.SetDamageNum(damage.Sum());
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}