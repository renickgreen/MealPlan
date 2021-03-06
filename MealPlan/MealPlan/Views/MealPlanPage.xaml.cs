using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace MealPlan.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MealPlanPage : ContentPage
    {
        public MealPlanPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            AppShell.CurrentPlan = Preferences.Get("current_plan_key", 0);
        }
    }
}