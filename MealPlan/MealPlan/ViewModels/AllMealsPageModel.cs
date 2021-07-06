using MealPlan.Models;
using MealPlan.Services;
using MealPlan.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlan.ViewModels
{
    class AllMealsPageModel : ViewModelBase
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

        public AllMealsPageModel()
        {
            Meals = new ObservableRangeCollection<Meal>();
            SelectedCommand = new AsyncCommand<object>(Selected);
            AppearingCommand = new MvvmHelpers.Commands.Command(OnAppearingCommand);
            NewPlanCommand = new MvvmHelpers.Commands.Command(OnNewPlanCommand);
            NewMealCommand = new MvvmHelpers.Commands.Command(OnNewMealCommand);
            // GetMealPlan();
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
            GetMeals();
        }
        async void GetMeals()
        {
            Meals.Clear();
            DatabaseControl db = await DatabaseControl.Instance;
            List<Meal> meal = await db.GetItemsAsync();
            foreach (Meal m in meal)
            {
                if (meal != null)
                {
                    Meals.Add(m);
                }
            }
        }
    }
}
