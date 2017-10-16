using System;
using System.Threading.Tasks;
using System.Collections.Generic;
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
            Name nl = new Name
            {
                firstname = newFirstName.Text,
                last = newLastName.Text

            };

            Address naddr = new Address
            {
                type = newAddrType.Text,
                streetNumber = newStreetNum.Text,
                streetName = newAddressName.Text
            };

            var l = new Lead
            {
                Country = newCountryName.Text,
                Source = newSourceName.Text,
                Name = nl,
                Address = naddr

            };
            await AddItem(l);

            newCountryName.Text = string.Empty;
            newCountryName.Unfocus();
            newSourceName.Text = string.Empty;
            newSourceName.Unfocus();
            newFirstName.Text = string.Empty;
            newFirstName.Unfocus();
            newLastName.Text = string.Empty;
            newLastName.Unfocus();
            newStreetNum.Text = string.Empty;
            newStreetNum.Unfocus();
            newAddressName.Text = string.Empty;
            newAddressName.Unfocus();
            newAddrType.Text = string.Empty;
            newAddrType.Unfocus();
        }

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
