using System.Collections.Generic;
using Arianrhod.Entity;
using UnityEngine;

namespace Arianrhod.Model
{
    public interface ILoadCharacter
    {
        IEnumerable<CharacterEntity> LoadCharacters();
        IEnumerable<CharacterEntity> LoadEnemies();
    }

    public interface ILoadCharacterReset
    {
        void Reset();
    }
    
    public class LoadCharacterModel : ILoadCharacter
    {
        private int _stageIndex = 0;

        private readonly string[] _stageHush =
        {
            "","","","","",""
        };

        public void Reset()
        {
            _stageIndex = 0;
        }

        public IEnumerable<CharacterEntity> LoadCharacters()
        {
            var stage = Resources.Load(_stageHush[0]) as TextAsset;
            Debug.Assert(stage != null, nameof(stage) + " != null");
            return JsonUtility.FromJson<List<CharacterEntity>>(stage.text);
        }
        
        public IEnumerable<CharacterEntity> LoadEnemies()
        {
            _stageIndex++;
            var stage = Resources.Load(_stageHush[_stageIndex]) as TextAsset;
            Debug.Assert(stage != null, nameof(stage) + " != null");
            return JsonUtility.FromJson<List<CharacterEntity>>(stage.text);
        }
    }
}