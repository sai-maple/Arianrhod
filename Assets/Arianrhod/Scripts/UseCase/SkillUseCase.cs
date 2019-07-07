using Arianrhod.Model;

namespace Arianrhod.UseCase
{
    public interface ISkillSelector
    {
        void SelectSkillIndex(int index);
        void OnSubmit();
    }

    public interface ISkillRollDetail
    {
        DiceType GetSkillDiceType();
        int RollNum();
    }
    
    public class SkillUseCase : ISkillSelector,ISkillRollDetail
    {
        private readonly ICurrentSkillModel _currentSkill = default;
        private readonly ITurnCharacterProvider _turnCharacter = default;

        public SkillUseCase(ICurrentSkillModel currentSkill, ITurnCharacterProvider turnCharacter)
        {
            _currentSkill = currentSkill;
            _turnCharacter = turnCharacter;
        }

        public void SelectSkillIndex(int index)
        {
            _currentSkill.OnSkillSet(_turnCharacter.OnTurnCharacterChanged().Value.Skill(index));
        }

        public void OnSubmit()
        {
            _currentSkill.OnSubmit();
        }

        public DiceType GetSkillDiceType()
        {
            return _currentSkill.OnSkillChanged().Value.DiceType;
        }

        public int RollNum()
        {
            return _turnCharacter.OnTurnCharacterChanged().Value.DiceNumCount(_currentSkill.OnSkillChanged().Value);
        }
    }
}