using IncrementalMauiApp;

namespace IncrementalMauiAppTests
{
    public class GameEngineTests
    {
        [Fact]
        public void BuyMiner_WhenEnoughGold_IncreasesCountAndCost()
        {
            var engine = new GameEngine
            {
                Gold = 20
            };
            int oldCost = engine.MinerCost;

            bool result = engine.BuyMiner();

            Assert.True(result);
            Assert.Equal(2, engine.MinerCount);
            Assert.Equal(20 - oldCost, engine.Gold);
            Assert.Equal((int)Math.Ceiling(oldCost * 1.15), engine.MinerCost);
        }

        [Fact]
        public void BuyMiner_WhenNotEnoughGold_DoesNothing()
        {
            var engine = new GameEngine
            {
                Gold = 5
            };

            bool result = engine.BuyMiner();

            Assert.False(result);
        }

        [Fact]
        public void BuyDrill_WhenEnoughGold_IncreasesCountAndCost()
        {
            var engine = new GameEngine
            {
                Gold = 100
            };
            int oldCost = engine.DrillCost;

            bool result = engine.BuyDrill();

            Assert.True(result);
            Assert.Equal(1, engine.DrillCount);
            Assert.Equal(100 - oldCost, engine.Gold);
            Assert.Equal((int)Math.Ceiling(oldCost * 1.15), engine.DrillCost);
        }

        [Fact]
        public void BuyDrill_WhenNotEnoughGold_DoesNothing()
        {
            var engine = new GameEngine
            {
                Gold = 5
            };

            bool result = engine.BuyDrill();

            Assert.False(result);
        }

        [Fact]
        public void BuyExcavator_WhenEnoughGold_IncreasesCountAndCost()
        {
            var engine = new GameEngine
            {
                Gold = 400
            };
            int oldCost = engine.ExcavatorCost;

            bool result = engine.BuyExcavator();

            Assert.True(result);
            Assert.Equal(1, engine.ExcavatorCount);
            Assert.Equal(400 - oldCost, engine.Gold);
            Assert.Equal((int)Math.Ceiling(oldCost * 1.15), engine.ExcavatorCost);
        }

        [Fact]
        public void BuyExcavator_WhenNotEnoughGold_DoesNothing()
        {
            var engine = new GameEngine
            {
                Gold = 5
            };

            bool result = engine.BuyExcavator();

            Assert.False(result);
        }

        [Fact]
        public void Tick_AddsCorrectGold()
        {
            var engine = new GameEngine
            {
                MinerCount = 2,
                DrillCount = 2,
                ExcavatorCount = 2
            };
            int expected = (2 * 1) + (2 * 5) + (2 * 20);    // 2 miners = 2 gold, 2 drills = 10 gold, 2 excavators = 40 gold

            engine.Tick();

            Assert.Equal(expected, engine.Gold);
        }

        [Fact]
        public void GameWinEvent_FiresAtOneMillion()
        {
            var engine = new GameEngine
            {
                Gold = 999_999
            };

            bool fired = false;
            engine.GameWonEvent += () => fired = true;

            engine.Tick();

            Assert.True(fired);
            Assert.True(engine.GameWon);
            Assert.Equal(1_000_000, engine.Gold);
        }
    }
}
