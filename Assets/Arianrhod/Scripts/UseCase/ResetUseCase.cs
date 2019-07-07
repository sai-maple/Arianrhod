using Arianrhod.Model;

namespace Arianrhod.UseCase
{
    public interface IResetUseCase
    {
        void OnReset();
    }
    
    public class ResetUseCase : IResetUseCase
    {
        private readonly IStageReset _stageReset = default;
        private readonly ILoadCharacterReset _loadCharacter = default;
        private readonly IResidueCharacters _residueCharacters = default;

        public ResetUseCase(
            IStageReset stageReset,
            ILoadCharacterReset loadCharacter,
            IResidueCharacters residueCharacters
        )
        {
            _stageReset = stageReset;
            _loadCharacter = loadCharacter;
            _residueCharacters = residueCharacters;
        }

        public void OnReset()
        {
            _stageReset.Reset();
            _loadCharacter.Reset();
            _residueCharacters.Reset();
        }
    }
}