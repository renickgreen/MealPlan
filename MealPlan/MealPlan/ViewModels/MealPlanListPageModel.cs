using MealPlan.Models;
using MvvmHelpers;
using MealPlan.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MvvmHelpers.Commands;
using MealPlan.Views;
using Xamarin.Forms;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MealPlan.ViewModels
{
    public class MealPlanListPageModel : ViewModelBase
    {
        private ObservableRangeCollection<MealPlanModel> _mealPlans;
        public ObservableRangeCollection<MealPlanModel> MealPlans
        {
            get => _mealPlans;
            set => SetProperty(ref _mealPlans, value);
        }
        private Meal _selectedPlan;
        public Meal SelectedPlan
        {
            get => _selectedPlan;
            set => SetProperty(ref _selectedPlan, value);
        }
        public AsyncCommand<object> SelectedCommand { get; }
        public MealPlanListPageModel()
        {
            Title = "Best Meal Plan Ever";
            MealPlans = new ObservableRangeCollection<MealPlanModel>();
            SelectedCommand = new AsyncCommand<object>(Selected);
            GetMealPlans();
        }
        async Task Selected(object args)
        {
            DatabaseControl db = await DatabaseControl.IPlan;
            var meal = args as MealPlanModel;
            if (meal == null)
                return;
            SelectedPlan = null;
            // await DeleteCommand(db);
            // return;
            //await Shell.Current.DisplayAlert("Selected", meal.Name, "ok");
            var route = $"{nameof(MealPlanDetailPage)}?{nameof(MealPlanDetailPageModel.MealPlanId)}={meal.Id}";
            await Shell.Current.GoToAsync(route);
        }
        async Task DeleteCommand(DatabaseControl db)
        {
            await db.DeleteAllAsync();
        }
        async void GetMealPlans()
        {
                DatabaseControl db = await DatabaseControl.IPlan;
                List<MealPlanModel> plan = await db.GetPlansAsync();
                foreach(MealPlanModel p in plan)
            {
                MealPlans.Add(p);
            }
        }
    }
}