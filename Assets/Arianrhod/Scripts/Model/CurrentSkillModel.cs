using System;
using Arianrhod.Entity;
using UniRx;

namespace Arianrhod.Model
{
    public interface ICurrentSkillModel
    {
        IReadOnlyReactiveProperty<SkillEntity> OnSkillChanged();
        IObservable<SkillEntity> OnSkillSubmit();
        void OnSkillSet(SkillEntity skillEntity);
        void OnSubmit();
    }

    public class CurrentSkillModel : ICurrentSkillModel, IDisposable
    {
        private readonly ReactiveProperty<SkillEntity> _skill = default;
        public IReadOnlyReactiveProperty<SkillEntity> OnSkillChanged() => _skill;
        private readonly Subject<SkillEntity> _submit = default;
        public IObservable<SkillEntity> OnSkillSubmit() => _submit.Publish().RefCount();

        public CurrentSkillModel()
        {
            _skill = new ReactiveProperty<SkillEntity>();
        }

        public void OnSkillSet(SkillEntity skillEntity)
        {
            _skill.Value = skillEntity;
        }

        public void OnSubmit()
        {
            _submit.OnNext(_skill.Value);
        }

        public void Dispose()
        {
            _skill.Dispose();
            _submit.OnCompleted();
            _skill.Dispose();
        }
    }
}