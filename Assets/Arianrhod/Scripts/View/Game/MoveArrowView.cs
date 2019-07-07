using System.Collections.Generic;
using System.Linq;
using Arianrhod.Entity;
using UnityEngine;

namespace Arianrhod.View.Game
{
    public interface IMoveArrow
    {
        void SetArrow(IEnumerable<PanelEntity> panelEntities);
    }

    public class MoveArrowView : MonoBehaviour, IMoveArrow
    {
        [SerializeField] private LineRenderer _lineRenderer = default;

        public void SetArrow(IEnumerable<PanelEntity> panelEntities)
        {
            var positions = panelEntities.Select(entity => new Vector3(entity.X, 1, entity.Y)).ToArray();
            _lineRenderer.SetPositions(positions);
        }
    }
}