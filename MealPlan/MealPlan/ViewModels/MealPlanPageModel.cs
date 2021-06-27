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

namespace MealPlan.ViewModels
{
    public class MealPlanPageModel : ViewModelBase
    {
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
            DatabaseControl db = await DatabaseControl.Instance;
            List<Meal> list = await db.GetItemsAsync();

            foreach(Meal m in list)
            {
                Meals.Add(m);
            }
        }
    }
}
