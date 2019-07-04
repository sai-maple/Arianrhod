using System;
using System.Collections.Generic;
using Arianrhod.Entity;
using UniRx;
using Unity.Mathematics;

namespace Arianrhod.Model
{
    public interface IStageModel
    {
        void Invaded(Character character, PanelEntity target);
        void Escaped(PanelEntity target);
        bool Invasive(PanelEntity target);
        PanelModel GetPanel(PanelEntity target);

        void TargetReset();
        IEnumerable<int> TargetCharacterIds(Character attacker, int skillIndex);
        void ModeEnd(PanelEntity panelEntity);
    }

    public interface IMoveHandler
    {
        IObservable<PanelEntity> OnMoveEvent();
    }
    
    public class StageModel : IStageModel, IMoveHandler,IDisposable
    {
        private readonly List<List<int>> _stageHash = new List<List<int>>();
        private readonly Dictionary<int, PanelModel> _stage = new Dictionary<int, PanelModel>();
        private readonly Subject<PanelEntity> _moveEndPanel = default;
        public IObservable<PanelEntity> OnMoveEvent() => _moveEndPanel.Publish().RefCount();
        
        public void MakeStage()
        {
            _stageHash.Clear();
            _stage.Clear();
        }
        
        // キャラクターの進入
        public void Invaded(Character character, PanelEntity target)
        {
            _stage[_stageHash[target.X][target.Y]].Invaded(character);
        }

        // キャラクターの離脱
        public void Escaped(PanelEntity target)
        {
            _stage[_stageHash[target.X][target.Y]].Escaped();
        }
        
        // 移動可能判定
        public bool Invasive(PanelEntity target)
        {
            return _stage[_stageHash[target.X][target.Y]].Invasive();
        }

        public PanelModel GetPanel(PanelEntity target)
        {
            return _stage[_stageHash[target.X][target.Y]];
        }

        // 攻撃範囲内のキャラクターID取得
        public IEnumerable<int> TargetCharacterIds(Character attacker, int skillIndex)
        {
            var list = new List<int>();
            var range = attacker.Skill(skillIndex).Range;
            for (var i = 0; i < (int) attacker.OnDirectionChanged().Value; i++)
            {
                range = RotateClockwise.Rotate(range);
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
                    var id = panel.GetCharacterId();
                    if (id == -1) continue;
                    list.Add(id);
                }
            }

            return list;
        }

        // 選択表示リセット
        public void TargetReset()
        {
            foreach (var panel in _stage.Values)
            {
                panel.Target(false);
            }
        }

        // キャラクター移動完了　最終地点更新
        public void ModeEnd(PanelEntity panelEntity)
        {
            _moveEndPanel.OnNext(panelEntity);
        }

        public void Dispose()
        {
            _moveEndPanel.Dispose();
        }
    }
}