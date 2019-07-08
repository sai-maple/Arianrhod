using System;
using System.Collections.Generic;
using System.Linq;
using Arianrhod.Entity;
using JetBrains.Annotations;
using UniRx;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

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
        IEnumerable<int> CpuAttackTargets(Character attacker);
        IEnumerable<int> CpuMoveTarget();
        IEnumerable<PanelEntity> AStartLoad(Character mover, Character target);
        void ModeEnd(PanelEntity panelEntity);
    }

    public interface IStageInitializer
    {
        IEnumerable<PanelModel> NextStage(List<List<int>> stage, List<Character> characters, List<Character> enemies);
    }

    public interface IMoveHandler
    {
        IObservable<PanelEntity> OnMoveEvent();
    }
    
    public class StageModel : IStageModel,IStageInitializer, IMoveHandler,IDisposable
    {
        private readonly List<List<int>> _stageHash = default;
        private readonly Dictionary<int, PanelModel> _stage = default;
        private readonly Subject<PanelEntity> _moveEndPanel = default;
        public IObservable<PanelEntity> OnMoveEvent() => _moveEndPanel.Publish().RefCount();

        public StageModel()
        {
            _stageHash = new List<List<int>>();
            _stage = new Dictionary<int, PanelModel>();
            _moveEndPanel = new Subject<PanelEntity>();
        }

        public IEnumerable<PanelModel> NextStage(List<List<int>> stage, List<Character> characters, List<Character> enemies)
        {
            _stageHash.Clear();
            _stage.Clear();
            var index = 0;
            var charaIndex = 0;
            var enemyIndex = 0;
            for (var line = 0; line < stage.Count; line++)
            {
                _stageHash.Add(new List<int>());
                for (var column = 0; column < stage[line].Count; column++)
                {
                    _stageHash[line].Add(index);
                    var id = -1;
                    if (stage[line][column] == 3)
                    {
                        if (characters.Count < charaIndex)
                        {
                            id = characters[charaIndex].Id;
                            characters[charaIndex].SetPosition(line, column);
                            charaIndex++;
                        }
                    }
                    if (stage[line][column] == 4)
                    {
                        if (enemies.Count < enemyIndex)
                        {
                            id = enemies[enemyIndex].Id;
                            enemies[enemyIndex].SetPosition(line, column);
                            enemyIndex++;
                        }
                    }
                    _stage.Add(index, new PanelModel(line, column, stage[line][column], id));
                    index++;
                }
            }

            return _stage.Values;
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
            
            var position = attacker.Position().Value;
            var x = position.x;
            var y = position.z;

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

        public IEnumerable<int> CpuAttackTargets(Character attacker)
        {
            var list = new List<int>();
            for (var skillIndex = 0; skillIndex < 3; skillIndex++)
            {
                var range = attacker.Skill(skillIndex).Range;
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {

                    range = RotateClockwise.Rotate(range);
                    attacker.SetDirection(direction);

                    var position = attacker.Position().Value;
                    var x = position.x;
                    var y = position.z;
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

                    if (list.Count != 0)
                    {
                        return list;
                    }
                }
            }

            return list;
        }

        public IEnumerable<int> CpuMoveTarget()
        {
            var list = new List<int>();
            foreach (var panel in _stage.Values)
            {
                var id = panel.GetCharacterId();
                if (id == -1) continue;
                if(panel.GetEntity().PanelState != PanelState.Character) continue;
                list.Add(id);
            }
            return list;
        }

        public IEnumerable<PanelEntity> AStartLoad(Character mover, Character target)
        {
            var nodes = new Node[ _stage.Count ];
            int start_index = _stageHash[mover.Position().Value.x][mover.Position().Value.z];
            int goal_index = _stageHash[target.Position().Value.x][target.Position().Value.z];
            nodes[ start_index ].status = STATUS_OPEN;
            nodes[ goal_index ].parentIndex = -1;
            
            int tries = 0;
            for ( ; tries < 100; tries++ )
            {
                // 最小スコアのノードを選択
                int node_index = -1;
                int min_score = int.MaxValue;
                for ( int i = 0; i < nodes.Length; i++ )
                {
                    // 開いていないならスキップ
                    if ( nodes[ i ].status != STATUS_OPEN )
                    {
                        continue;
                    }
                    // よりスコアが低いノードを選択
                    if (nodes[i].score >= min_score) continue;
                    node_index = i;
                    min_score = nodes[ i ].heuristic;
                }
                // 開いたノードがなかった
                if ( node_index == -1 )
                {
                    break;
                }

                var node = OpenNode(nodes, node_index,_stage[_stageHash[mover.Position().Value.x][mover.Position().Value.z]], _stage[_stageHash[target.Position().Value.x][target.Position().Value.z]]);
                if (nodes[goal_index] == node || node.length >= mover.CharacterEntity.Range)
                {
                    break;
                }
            }
            
            var buffer_path = new List<PanelModel>();
            for ( int index = goal_index; index != start_index; index = nodes[ index ].parentIndex )
            {
                if (index == goal_index || index == start_index)
                {
                    continue;
                }
                buffer_path.Add(_stage[index]);
            }

            return buffer_path.Select(panel => panel.GetEntity()).Reverse().Take(mover.CharacterEntity.Range);
        }
        
        const int STATUS_FREE = 0;      //!< 未探索
        const int STATUS_OPEN = 1;      //!< オープン
        const int STATUS_CLOSE = 2; //!< クローズ
        
        [ReadOnly]  // 共有する配列には、[ReadOnly]が必要
        public NativeArray<int> costs;
        
        private class Node
        {
            public int status;          //!< 状態
            public int parentIndex;     //!< 親の座標インデックス

            public int length;

            public int cost;                //!< コスト
            public int heuristic;           //!< ヒューリスティックコスト

            //! スコア計算プロパティ
            public int score
            {
                get { return cost + heuristic; }
            }
        }

        private Node OpenNode(Node[] nodes, int node_index, PanelModel start, PanelModel goal)
        {
            // 添字から座標に
            var center = _stage[node_index];

            int center_cost = nodes[node_index].cost;

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var open_position = OpenPosition(center, direction);

                if (open_position == goal)
                {
                    nodes[_stageHash[open_position.GetEntity().X][open_position.GetEntity().Y]].parentIndex = node_index;
                    return nodes[_stageHash[goal.GetEntity().X][goal.GetEntity().Y]];
                }
                
                if (open_position == null || !open_position.Invasive())
                {
                    continue;
                }

                // コスト計算
                int cost = costs[_stageHash[open_position.GetEntity().X][open_position.GetEntity().Y]] + center_cost +
                           1;
                int heuristic = math.abs(goal.GetEntity().X - open_position.GetEntity().X) +
                                math.abs(goal.GetEntity().Y - open_position.GetEntity().Y);
                int score = cost + heuristic + 1;

                int next_index = _stageHash[open_position.GetEntity().X][open_position.GetEntity().Y];
                if (nodes[next_index].status == STATUS_FREE || nodes[next_index].score > score)
                {
                    nodes[next_index].status = STATUS_OPEN;
                    nodes[next_index].cost = cost;
                    nodes[next_index].length = math.abs(start.GetEntity().X - open_position.GetEntity().X) +
                                               math.abs(start.GetEntity().Y - open_position.GetEntity().Y);
                    nodes[next_index].heuristic = heuristic;
                    nodes[next_index].parentIndex = node_index;
                }
            }

            nodes[node_index].status = STATUS_CLOSE;

            return nodes[node_index];
        }

        [CanBeNull]
        private PanelModel OpenPosition(PanelModel center ,Direction direction)
        {
            var x = direction == Direction.Right ? 1 : 0;
            x = direction == Direction.Left ? -1 : 0;
            var y = direction == Direction.Up ? 1 : 0;
            y = direction == Direction.Down ? -1 : 0;

            if (_stageHash.Count <= center.GetEntity().X + x || _stageHash[0].Count <= center.GetEntity().Y + y)
            {
                return null;
            }
            
            var index = _stageHash[center.GetEntity().X + x][center.GetEntity().Y + y];
            return _stage[index];
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