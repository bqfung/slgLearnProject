using SLGLearn.Data;

namespace SLGLearn.Battle
{
    public readonly struct BattleReport
    {
        public BattleReport(
            StrongholdConfig stronghold,
            int marchPower,
            int enemyPower,
            bool victory,
            int losses,
            bool firstClearReward)
        {
            Stronghold = stronghold;
            MarchPower = marchPower;
            EnemyPower = enemyPower;
            Victory = victory;
            Losses = losses;
            FirstClearReward = firstClearReward;
        }

        public StrongholdConfig Stronghold { get; }
        public int MarchPower { get; }
        public int EnemyPower { get; }
        public bool Victory { get; }
        public int Losses { get; }
        public bool FirstClearReward { get; }
    }
}
