using System.Collections.Generic;
using System.Linq;
using Arianrhod.Entity;
using Arianrhod.Model;
using Unity.Mathematics;

namespace Arianrhod.UseCase
{
    public class StageUseCase
    {

        private CharacterResidue _characterResidue = default;
        private EnemyResidue _enemyResidue = default;
        private List<List<int>> _stageHash = new List<List<int>>();
        private Dictionary<int, PanelModel> _stage = new Dictionary<int, PanelModel>();
        
        public StageUseCase()
        {

        }

        public IEnumerable<PanelModel> MakeStage()
        {
            _stageHash.Clear();
            _stage.Clear();
            return _stage.Values;
        }

        private static void Invaded(Character character, PanelModel target)
        {
            target.Invaded(character);
        }

        private static void Escaped(PanelModel target)
        {
            target.Escaped();
        }

        public bool Invasive(PanelModel target)
        {
            return target.Invasive();
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

        public void MoveCharacter(Character character, IEnumerable<PanelModel> movePath)
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