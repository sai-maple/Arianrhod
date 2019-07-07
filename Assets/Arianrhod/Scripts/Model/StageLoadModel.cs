using System.Collections.Generic;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Arianrhod.Model
{
    public interface ILoadStage
    {
        List<List<int>> LoadStage();
    }
    
    public class StageLoadModel : ILoadStage
    {
        private int _stageIndex = 0;

        private readonly string[] _stageHush =
        {
            "","","","","",""
        };

        public List<List<int>> LoadStage()
        {
            _stageIndex++;
            var stage = Resources.Load(_stageHush[_stageIndex]) as TextAsset;
            Debug.Assert(stage != null, nameof(stage) + " != null");
            return JsonUtility.FromJson<List<List<int>>>(stage.text);
        }
    }
}