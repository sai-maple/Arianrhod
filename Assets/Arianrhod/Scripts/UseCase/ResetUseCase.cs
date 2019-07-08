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

        public ResetUseCase(
            IStageReset stageReset,
            ILoadCharacterReset loadCharacter
        )
        {
            _stageReset = stageReset;
            _loadCharacter = loadCharacter;
        }

        public void OnReset()
        {
            _stageReset.Reset();
            _loadCharacter.Reset();
        }
    }
}