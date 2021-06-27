using MealPlan.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MealPlan
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NewMealPage), typeof(NewMealPage));
            Routing.RegisterRoute(nameof(MealPlanPage), typeof(MealPlanPage));
            Routing.RegisterRoute(nameof(MealDetailPage), typeof(MealDetailPage));
        }

    }
}
