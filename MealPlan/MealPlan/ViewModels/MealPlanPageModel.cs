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
    public class MealPlanPageModel : ViewModelBase
    {
        private ICommand _appearingCommand;
        public ICommand AppearingCommand
        {
            get => _appearingCommand;
            set => SetProperty(ref _appearingCommand, value);
        }
        private ICommand _newPlanCommand;
        public ICommand NewPlanCommand
        {
            get => _newPlanCommand;
            set => SetProperty(ref _newPlanCommand, value);
        }
        private ICommand _newMealCommand;
        public ICommand NewMealCommand
        {
            get => _newMealCommand;
            set => SetProperty(ref _newMealCommand, value);
        }
        private ObservableRangeCollection<Meal> _meals;
        public ObservableRangeCollection<Meal> Meals
        {
            get => _meals;
            set => SetProperty(ref _meals, value);
        }
        private Meal _selectedMeal;
        public Meal SelectedMeal
        {
            get => _selectedMeal;
            set => SetProperty(ref _selectedMeal, value);
        }
        public AsyncCommand<object> SelectedCommand { get; }
        public MealPlanPageModel()
        {
            Title = "Best Meal Plan Ever";
            Meals = new ObservableRangeCollection<Meal>();
            SelectedCommand = new AsyncCommand<object>(Selected);
            AppearingCommand = new MvvmHelpers.Commands.Command(OnAppearingCommand);
            NewPlanCommand = new MvvmHelpers.Commands.Command(OnNewPlanCommand);
            NewMealCommand = new MvvmHelpers.Commands.Command(OnNewMealCommand);
           // GetMealPlan();
        }

        private void OnNewMealCommand(object obj)
        {
            Shell.Current.GoToAsync($"{nameof(NewMealPage)}");
        }

        private void OnNewPlanCommand(object obj)
        {
            Shell.Current.GoToAsync($"{nameof(NewMealPlanPage)}");
        }

        private void OnAppearingCommand(object obj)
        {
            GetMealPlan();
        }

        async Task Selected(object args)
        {
            DatabaseControl db = await DatabaseControl.Instance;
            var meal = args as Meal;
            if (meal == null)
                return;
            SelectedMeal = null;
           // await DeleteCommand(db);
           // return;
            var route = $"{nameof(MealDetailPage)}?{nameof(MealDetailPageModel.MealId)}={meal.Id}";
            await Shell.Current.GoToAsync(route);
        }
        async Task DeleteCommand(DatabaseControl db)
        {
            await db.DeleteAllAsync();
        }
        async void GetMealPlan()
        {
            AppShell.CurrentPlan = Preferences.Get("current_plan_key", 0);
            if (AppShell.CurrentPlan > 0)
            {
                DatabaseControl db = await DatabaseControl.IPlan;
                MealPlanModel plan = await db.GetPlanAsync(AppShell.CurrentPlan);
                ExtractMealsAsync(plan.Meals);
                Title = plan.Name;
            }
        }
        void ExtractMealsAsync(string meals)
        {
            var commaIndex = 0;
            var numIndex = 0;
            List<int> Ids = new List<int>();
            while(commaIndex < meals.Length-1)
            {
                commaIndex = meals.IndexOf(',');
                var num = int.Parse(meals.Substring(numIndex, commaIndex));
                Ids.Add(num);
                meals = meals.Substring(commaIndex + 1);
            }
            GetMeals(Ids);
        }
        async void GetMeals(List<int> list)
        {
            Meals.Clear();
            DatabaseControl db = await DatabaseControl.Instance;
            foreach(int item in list)
            {
                Meal meal = await db.GetItemAsync(item);
                if(meal != null)
                {
                    Meals.Add(meal);
                }
            }
        }
    }
}
