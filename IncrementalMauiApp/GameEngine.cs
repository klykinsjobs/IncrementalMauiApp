namespace IncrementalMauiApp
{
    public class GameEngine
    {
        public int Gold { get; set; } = 0;

        public int MinerCount { get; set; } = 1;
        public int DrillCount { get; set; } = 0;
        public int ExcavatorCount { get; set; } = 0;

        public int MinerCost { get; set; } = 10;
        public int DrillCost { get; set; } = 50;
        public int ExcavatorCost { get; set; } = 200;

        private const int MinerRate = 1;
        private const int DrillRate = 5;
        private const int ExcavatorRate = 20;

        public bool GameWon { get; set; } = false;

        public event Action? GameWonEvent;

        public void Tick()
        {
            if (GameWon)
                return;

            Gold += MinerCount * MinerRate;
            Gold += DrillCount * DrillRate;
            Gold += ExcavatorCount * ExcavatorRate;

            if (Gold >= 1_000_000 && !GameWon)
            {
                Gold = 1_000_000;   // clamp to exact victory value
                GameWon = true;
                GameWonEvent?.Invoke();
            }
        }

        public bool BuyMiner()
        {
            if (Gold < MinerCost)
                return false;

            Gold -= MinerCost;
            MinerCount++;
            MinerCost = (int)Math.Ceiling(MinerCost * 1.15);
            return true;
        }

        public bool BuyDrill()
        {
            if (Gold < DrillCost)
                return false;

            Gold -= DrillCost;
            DrillCount++;
            DrillCost = (int)Math.Ceiling(DrillCost * 1.15);
            return true;
        }

        public bool BuyExcavator()
        {
            if (Gold < ExcavatorCost)
                return false;

            Gold -= ExcavatorCost;
            ExcavatorCount++;
            ExcavatorCost = (int)Math.Ceiling(ExcavatorCost * 1.15);
            return true;
        }
    }
}