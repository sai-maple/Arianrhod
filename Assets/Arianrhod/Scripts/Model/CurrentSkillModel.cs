using System;
using Arianrhod.Entity;
using UniRx;

namespace Arianrhod.Model
{
    public interface ICurrentSkillModel
    {
        IReadOnlyReactiveProperty<SkillEntity> OnSkillChanged();
        void OnSkillSet(SkillEntity skillEntity);
    }

    public class CurrentSkillModel : ICurrentSkillModel, IDisposable
    {
        private readonly ReactiveProperty<SkillEntity> _skill = default;
        public IReadOnlyReactiveProperty<SkillEntity> OnSkillChanged() => _skill;

        public CurrentSkillModel()
        {
            _skill = new ReactiveProperty<SkillEntity>();
        }

        public void OnSkillSet(SkillEntity skillEntity)
        {
            _skill.Value = skillEntity;
        }

        public void Dispose()
        {
            _skill.Dispose();
        }
    }
}