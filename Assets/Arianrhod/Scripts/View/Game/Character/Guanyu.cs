using UniRx.Async;
using UnityEngine;

namespace Arianrhod.View.Game.Character
{
    public class Guanyu : MonoBehaviour
    {
        [SerializeField] private Transform _target = default;

        [SerializeField] private CurvelBullet _attackBullet = default;
        [SerializeField] private NormalBullet _magicBullet = default;
        [SerializeField] private NormalBullet _magic2Bullet = default;
        [SerializeField] private PosBullet _ultimateBullet = default;
        [SerializeField] private ParticlesEffect _damageEffect1 = default;
        [SerializeField] private ParticlesEffect _damageEffect2 = default;
        [SerializeField] private ParticlesEffect _damageEffect3 = default;

        private async void DelayBullet()
        {
            const int count = 20;
            const float angle = -0 / 2f * 18f;
            for (var i = 0; i < count; i++)
            {
                var bullet = Instantiate(_ultimateBullet);
                var transform1 = transform;
                bullet.player = transform1;
                bullet.startPos = transform1.position + new Vector3(0f, 0.01f, 0f);
                bullet.tarPos = MathUtil.calcTargetPosByRotation(transform1, angle + i * 18f, 10f);
                bullet.effectObj = _damageEffect1;
                bullet.bulleting();
                await UniTask.Delay(1);
                if (i % 6 != 0) continue;
                if (_damageEffect2 == null) continue;
                var effect = Instantiate(_damageEffect2);
                effect.transform.position = MathUtil.findChild(_target, "attackedPivot").position;
                effect.play();
            }
        }

        private void preAction(string actionName)
        {
            var arr = actionName.Split('|');
            var name = arr[0];
            switch (name)
            {
                case AnimationName.Attack:
                    if (_damageEffect1 != null)
                    {
                        var effect = Instantiate(_damageEffect1);
                        effect.transform.position = MathUtil.findChild(_target, "attackedPivot").position;
                        effect.play();
                    }
                    break;
                case AnimationName.Magic:
                    if (_damageEffect2 != null)
                    {
                        var effect = Instantiate(_damageEffect2);
                        effect.transform.position = MathUtil.findChild(_target, "attackedPivot").position;
                        effect.play();
                    }

                    if (_magicBullet != null)
                    {
                        DelayBullet();
                    }
                    break;
                case AnimationName.Magic2:
                    if (_magic2Bullet != null)
                    {
                        var bullet = Instantiate(_magic2Bullet);
                        bullet.player = transform;
                        bullet.target = _target;
                        bullet.effectObj = _damageEffect2;
                        bullet.bulleting();
                    }

                    if (_damageEffect2 != null)
                    {
                        var effect = Instantiate(_damageEffect2);
                        effect.transform.position = MathUtil.findChild(_target, "attackedPivot").position;
                        effect.play();
                    }
                    break;
                case AnimationName.Ultimate:
                    if (_damageEffect3 != null)
                    {
                        var effect = Instantiate(_damageEffect3);
                        effect.transform.position = MathUtil.findChild(_target, "attackedPivot").position;
                        effect.play();
                    }
                    break;
            }
        }
    }
}
