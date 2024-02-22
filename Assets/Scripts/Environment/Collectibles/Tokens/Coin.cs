using System;
using Utility.Save_System;
using Utility.Save_System.Data;

namespace Environment.Collectibles.Tokens
{
    public class Coin : Token, IPersistentDataHandler
    {
        public static event Action CountChanged;
        
        public static int Count { get; private set; }

        public void Save(ref GameData gameData)
        {
            gameData.Coins = Count;
        }

        public void Load(GameData gameData)
        {
            Count = gameData.Coins;
        }

        public override void Add(int val)
        {
            Count += val;
            CountChanged?.Invoke();
        }
    }
}