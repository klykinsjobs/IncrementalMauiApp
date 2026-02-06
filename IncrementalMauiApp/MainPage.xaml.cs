namespace IncrementalMauiApp
{
    public partial class MainPage : ContentPage
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

        private PeriodicTimer? _timer;
        private CancellationTokenSource? _cts;

        private bool _gameWon = false;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
            _cts = new CancellationTokenSource();

            RunGameLoop(_cts.Token);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _cts?.Cancel();
            _cts?.Dispose();
            _timer = null;
        }

        private async void RunGameLoop(CancellationToken token)
        {
            try
            {
                if (_timer == null) return;

                while (await _timer.WaitForNextTickAsync(token))
                {
                    if (_gameWon)
                        break;

                    Gold += MinerCount * MinerRate;
                    Gold += DrillCount * DrillRate;
                    Gold += ExcavatorCount * ExcavatorRate;

                    if (Gold >= 1_000_000 && !_gameWon)
                    {
                        _gameWon = true;
                        _cts?.Cancel();
                        await MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("You Win!", "You reached 1,000,000 gold!", "OK"));
                    }

                    await MainThread.InvokeOnMainThreadAsync(UpdateUI);
                }
            }
            catch (OperationCanceledException)
            {
                // Normal shutdown
            }
        }

        private void OnBuyMinerClicked(object sender, EventArgs e)
        {
            if (Gold >= MinerCost)
            {
                Gold -= MinerCost;
                MinerCount++;
                MinerCost = (int)(MinerCost * 1.15);
                UpdateUI();
            }
        }

        private void OnBuyDrillClicked(object sender, EventArgs e)
        {
            if (Gold >= DrillCost)
            {
                Gold -= DrillCost;
                DrillCount++;
                DrillCost = (int)(DrillCost * 1.15);
                UpdateUI();
            }
        }

        private void OnBuyExcavatorClicked(object sender, EventArgs e)
        {
            if (Gold >= ExcavatorCost)
            {
                Gold -= ExcavatorCost;
                ExcavatorCount++;
                ExcavatorCost = (int)(ExcavatorCost * 1.15);
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            GoldLabel.Text = $"Gold: {Gold}";

            MinerCountLabel.Text = $"Owned: {MinerCount}";
            DrillCountLabel.Text = $"Owned: {DrillCount}";
            ExcavatorCountLabel.Text = $"Owned: {ExcavatorCount}";

            MinerCostLabel.Text = $"Cost: {MinerCost}";
            DrillCostLabel.Text = $"Cost: {DrillCost}";
            ExcavatorCostLabel.Text = $"Cost: {ExcavatorCost}";
        }
    }
}