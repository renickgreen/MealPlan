using MealPlan.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealPlan
{
    public partial class App : Application
    {
        public static double ScreenWidth;
    public static double ScreenHeight;

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDYzMjAyQDMxMzkyZTMxMmUzMGlYS1k0Sk1WY0dHOW5jWitDREttVUhUaDhpaG1neUViNHU3NkRxNHY5eWc9");
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
