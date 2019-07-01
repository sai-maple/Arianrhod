using System.Collections.Generic;
using System.Linq;
using Arianrhod.Model;

namespace Arianrhod.UseCase
{
    public class StageUseCase
    {
        private List<List<PanelModel>> _stage = new List<List<PanelModel>>();

        public StageUseCase()
        {

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