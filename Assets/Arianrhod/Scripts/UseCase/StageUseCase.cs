using System.Collections.Generic;
using System.Linq;
using Arianrhod.Entity;
using Arianrhod.Model;
using JetBrains.Annotations;
using Unity.Mathematics;

namespace Arianrhod.UseCase
{

    public interface IPanelSelector
    {
        bool Invasive(PanelEntity target);
        Character GetCharacter(PanelEntity target);
    }
    
    public class StageUseCase : IPanelSelector
    {
        private readonly CharacterResidue _characterResidue = default;
        private readonly EnemyResidue _enemyResidue = default;
        private readonly List<List<int>> _stageHash = new List<List<int>>();
        private readonly Dictionary<int, PanelModel> _stage = new Dictionary<int, PanelModel>();
        
        public StageUseCase()
        {

        }

        public IEnumerable<PanelModel> MakeStage()
        {
            _stageHash.Clear();
            _stage.Clear();
            return _stage.Values;
        }

        private void Invaded(Character character, PanelEntity target)
        {
            _stage[_stageHash[target.X][target.Y]].Invaded(character);
        }

        private void Escaped(PanelEntity target)
        {
            _stage[_stageHash[target.X][target.Y]].Escaped();
        }
        
        // 移動可能判定
        public bool Invasive(PanelEntity target)
        {
            return _stage[_stageHash[target.X][target.Y]].Invasive();
        }

        // 選択パネルのキャラ獲得　or null
        [CanBeNull]
        public Character GetCharacter(PanelEntity target)
        {
            var panel = _stage[_stageHash[target.X][target.Y]];
            if (panel.GetCharacterId() == -1)
            {
                return null;
            }
            var character = _characterResidue.GetCharacter(panel.GetCharacterId());
            return character ?? _enemyResidue.GetCharacter(panel.GetCharacterId());
        }

        public IEnumerable<Character> Target(Character attacker, int skillIndex)
        {
            var list = new List<Character>();
            var range = attacker.Skill(skillIndex).Range;
            for (var i = 0; i < (int) attacker.OnDirectionChanged().Value; i++)
            {
                range = RotateClockwise.Rotate(range);
            }

            foreach (var panel in _stage.Values)
            {
                panel.Target(false);
            }

            var (x, y) = attacker.Position;
            for (var column = math.max(0, x - (range.Length - 1) / 2);
                column < math.max(_stageHash.Count, x + (range.Length - 1) / 2);
                column++)
            {
                for (var line = math.max(0, y - (range.GetLength(0) - 1) / 2);
                    line < math.max(_stageHash.Count, y + (range.GetLength(0) - 1) / 2);
                    line++)
                {
                    if (range[column, line] != 1) continue;
                    var panel = _stage[_stageHash[column][line]];
                    panel.Target(true);
                    var character = attacker.Owner == Owner.CPU
                        ? _characterResidue.GetCharacter(panel.GetCharacterId())
                        : _enemyResidue.GetCharacter(panel.GetCharacterId());
                    if (character == null) continue;
                    list.Add(character);
                }
            }
            return list;
        }

        public void MoveCharacter(Character character, IEnumerable<PanelEntity> movePath)
        {
            var panelEntities = movePath.ToList();
            foreach (var panel in panelEntities)
            {
                Invaded(character, panel);
                Escaped(panel);
            }

            Invaded(character, panelEntities.Last());
        }
    }
}