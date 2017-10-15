using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using minileadgen.core.Helpers;
using minileadgen.core.Models;

namespace minileadgen.core
{
    public partial class minileadgen_corePage : ContentPage
    {
        LeadManager manager;

        public minileadgen_corePage()
        {
            InitializeComponent();

            manager = LeadManager.DefaultManager;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Set syncItems to true in order to synchronize the data on startup when running in offline mode
            await RefreshItems(true);
        }

        // Event handlers
        public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#context
        public async void OnComplete(object sender, EventArgs e)
        {

        }

        // Data methods
        async Task AddItem(Lead item)
        {
            await manager.InsertLeadAsync(item);
            leadsList.ItemsSource = await manager.GetLeadsAsync();
        }

        public async void OnAdd(object sender, EventArgs e)
        {
            var l = new Lead
            {
                Country = newCountryName.Text,
                Source = newSourceName.Text
            };
            await AddItem(l);

            newCountryName.Text = string.Empty;
            newCountryName.Unfocus();
            newSourceName.Text = string.Empty;
            newSourceName.Unfocus();
        }

        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#pulltorefresh
        public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;
            try
            {
                await RefreshItems(false);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
            }
        }

        public async void OnSyncItems(object sender, EventArgs e)
        {
            await RefreshItems(true);
        }

        private async Task RefreshItems(bool showActivityIndicator)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                leadsList.ItemsSource = await manager.GetLeadsAsync();
            }
        }

        private class ActivityIndicatorScope : IDisposable
        {
            private bool showIndicator;
            private ActivityIndicator indicator;
            private Task indicatorDelay;

            public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
            {
                this.indicator = indicator;
                this.showIndicator = showIndicator;

                if (showIndicator)
                {
                    indicatorDelay = Task.Delay(2000);
                    SetIndicatorActivity(true);
                }
                else
                {
                    indicatorDelay = Task.FromResult(0);
                }
            }

            private void SetIndicatorActivity(bool isActive)
            {
                this.indicator.IsVisible = isActive;
                this.indicator.IsRunning = isActive;
            }

            public void Dispose()
            {
                if (showIndicator)
                {
                    indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }
    }
}
