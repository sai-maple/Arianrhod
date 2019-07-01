using Arianrhod.Entity;

namespace Arianrhod.Model
{
    public class PanelModel
    {
        private PanelEntity _entity = default;
        private Character _character = default;

        public Character GetCharacter()
        {
            return _entity.panelState == PanelState.Character ? _character : null;
        }

        public void Invaded(Character character)
        {
            if (_entity.panelState == PanelState.Dice)
            {
                character.AddDice(_entity.diceType);
            }

            _character = character;
            _entity.panelState = PanelState.Character;
        }

        public void Escaped()
        {
            _character = null;
            _entity.panelState = PanelState.Empty;
        }

        public bool Invasive()
        {
            return _entity.panelState != PanelState.Character;
        }
    }
}