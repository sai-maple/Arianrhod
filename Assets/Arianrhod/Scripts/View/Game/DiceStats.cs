using UnityEngine;

//Code for determining the upward facing side of the die. 
namespace Arianrhod.View.Game
{
    public class DiceStats : MonoBehaviour
    {
        public Transform[] diceSides;

        public int side = 1;

        // Start is called before the first frame update
        void Start()
        {
        }

        void Update()
        {
            CheckDiceSide();
        }

        void CheckDiceSide()
        {
            for (int i = 0; i < diceSides.Length; i++)
            {
                if (diceSides[i].position.y > diceSides[side - 1].position.y)
                    side = i + 1;
            }
        }
    }
}