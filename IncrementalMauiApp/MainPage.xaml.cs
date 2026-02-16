namespace IncrementalMauiApp
{
    public partial class MainPage : ContentPage
    {
        private readonly GameEngine _engine = new();

        private PeriodicTimer? _timer;
        private CancellationTokenSource? _cts;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _engine.GameWonEvent += OnGameWon;

            _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
            _cts = new CancellationTokenSource();
            RunGameLoop(_cts.Token);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            _engine.GameWonEvent -= OnGameWon;

            _cts?.Cancel();
            _cts?.Dispose();
            _timer?.Dispose();
        }

        private async void RunGameLoop(CancellationToken token)
        {
            try
            {
                if (_timer == null) return;

                while (await _timer.WaitForNextTickAsync(token))
                {
                    _engine.Tick();
                    await MainThread.InvokeOnMainThreadAsync(UpdateUI);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async void OnGameWon()
        {
            _cts?.Cancel();

            await MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("You Win!", "You reached 1,000,000 gold!", "OK"));
        }

        private void OnBuyMinerClicked(object sender, EventArgs e)
        {
            _engine.BuyMiner();
            UpdateUI();
        }

        private void OnBuyDrillClicked(object sender, EventArgs e)
        {
            _engine.BuyDrill();
            UpdateUI();
        }

        private void OnBuyExcavatorClicked(object sender, EventArgs e)
        {
            _engine.BuyExcavator();
            UpdateUI();
        }

        private void UpdateUI()
        {
            GoldLabel.Text = $"Gold: {_engine.Gold}";

            MinerCountLabel.Text = $"Owned: {_engine.MinerCount}";
            DrillCountLabel.Text = $"Owned: {_engine.DrillCount}";
            ExcavatorCountLabel.Text = $"Owned: {_engine.ExcavatorCount}";

            MinerCostLabel.Text = $"Cost: {_engine.MinerCost}";
            DrillCostLabel.Text = $"Cost: {_engine.DrillCost}";
            ExcavatorCostLabel.Text = $"Cost: {_engine.ExcavatorCost}";
        }
    }
}