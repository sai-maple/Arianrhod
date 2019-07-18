using System;
using Arianrhod.Entity;
using Arianrhod.View.Game;

namespace Arianrhod
{
    public interface ICharacterFactory
    {
        CharacterView Create(int id, CharacterEntity entity, Owner owner);
    }

    public class CharacterFactory : ICharacterFactory
    {
        private readonly CharacterView.Guanyu _guanyu = default;
        private readonly CharacterView.Huanggai _huanggai = default;
        private readonly CharacterView.Huanyueing _huanyueing = default;
        private readonly CharacterView.Lusu _lusu = default;
        private readonly CharacterView.Simayi _simayi = default;
        private readonly CharacterView.Zhugeliang _zhugeliang = default;

        public CharacterFactory(
            CharacterView.Guanyu guanyu,
            CharacterView.Huanggai huanggai,
            CharacterView.Huanyueing huanyueing,
            CharacterView.Lusu lusu,
            CharacterView.Simayi simayi,
            CharacterView.Zhugeliang zhugeliang
        )
        {
            _guanyu = guanyu;
            _huanggai = huanggai;
            _huanyueing = huanyueing;
            _lusu = lusu;
            _simayi = simayi;
            _zhugeliang = zhugeliang;

        }

        public CharacterView Create(int id, CharacterEntity entity, Owner owner)
        {
            switch (id)
            {
                case 0:
                    return _guanyu.Create(id, owner, entity);
                case 1:
                    return _huanggai.Create(id, owner, entity);
                case 2:
                    return _huanyueing.Create(id, owner, entity);
                case 3:
                    return _lusu.Create(id, owner, entity);
                case 4:
                    return _simayi.Create(id, owner, entity);
                case 5:
                    return _zhugeliang.Create(id, owner, entity);
                default:
                    throw new NullReferenceException();
            }
        }
    }
}