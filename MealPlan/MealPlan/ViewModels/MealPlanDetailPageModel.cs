using MealPlan.Models;
using MealPlan.Services;
using MealPlan.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MealPlan.ViewModels
{
    [QueryProperty(nameof(MealPlanId), nameof(MealPlanId))]
    public class MealPlanDetailPageModel : ViewModelBase
    {
        private string _mealPlanId;
        public string MealPlanId
        {
            get => _mealPlanId;
            set
            {
                SetProperty(ref _mealPlanId, value);

                GetMealPlan();
            }
        }
        private ObservableRangeCollection<Meal> _meals;
        public ObservableRangeCollection<Meal> Meals
        {
            get => _meals;
            set => SetProperty(ref _meals, value);
        }
        private MealPlanModel _thePlan;
        public MealPlanModel ThePlan
        {
            get => _thePlan;
            set => SetProperty(ref _thePlan, value);
        }
        private ICommand _tapCommand;
        public ICommand TapCommand
        {
            get => _tapCommand;
            set => SetProperty(ref _tapCommand, value);
        }
        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get => _editCommand;
            set => SetProperty(ref _editCommand, value);
        }
        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get => _deleteCommand;
            set => SetProperty(ref _deleteCommand, value);
        }
        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get => _addCommand;
            set => SetProperty(ref _addCommand, value);
        }
        public MealPlanDetailPageModel()
        {
            Meals = new ObservableRangeCollection<Meal>();
            AddCommand = new Command(OnAddCommand);
            TapCommand = new MvvmHelpers.Commands.AsyncCommand(Selected);
            EditCommand = new MvvmHelpers.Commands.AsyncCommand(OnEditCommand);
            DeleteCommand = new MvvmHelpers.Commands.AsyncCommand(OnDeleteCommand);
           // GetMealPlan();
        }

        private async void OnAddCommand(object obj)
        {
            AppShell.CurrentPlan = ThePlan.Id;
            Preferences.Set("current_plan_key", ThePlan.Id);
            await Shell.Current.DisplayAlert(ThePlan.Name, "Set as Meal Plan.  Great choice!", "Let's Eat");
            await Shell.Current.GoToAsync("..");
        }

        private async void GetMealPlan()
        {
            ThePlan = new MealPlanModel();
            int.TryParse(MealPlanId, out int result);
            DatabaseControl db = await DatabaseControl.IPlan;
            ThePlan = await db.GetPlanAsync(result);

            Name = ThePlan.Name;
            Author = ThePlan.Author;
            Description = ThePlan.Description;
            Favortie = ThePlan.Favortie;
            ExtractMealsAsync(ThePlan.Meals);
        }
        async Task Selected()
        {
            await Shell.Current.DisplayAlert("I was", "Selected", "OK");
          //  var route = $"{nameof(RecipeImagePage)}?{nameof(RecipeImagePageModel.PhotoPath)}={PhotoPath}";
          //  await Shell.Current.GoToAsync(route);
        }
        async Task OnEditCommand()
        {
            //await Shell.Current.DisplayAlert(Title, ThePlan.Id.ToString(), "TODO");
            await Shell.Current.GoToAsync($"{nameof(NewMealPlanPage)}?{nameof(NewMealPlanPageModel.PlanId)}={ThePlan.Id}");
        }
        async Task OnDeleteCommand()
        {
            if(await Shell.Current.DisplayAlert("Warning", $"Do you want to PERMANENTLY DELETE {Name}?", "Yes Delete", "No"))
            {
                DatabaseControl db = await DatabaseControl.IPlan;
                await db.DeleteItemAsync(ThePlan);
                await Shell.Current.GoToAsync("..");
            }
        }
        void ExtractMealsAsync(string meals)
        {
            var commaIndex = 0;
            var numIndex = 0;
            List<int> Ids = new List<int>();
            while (commaIndex < meals.Length - 1)
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
            DatabaseControl db = await DatabaseControl.Instance;
            foreach (int item in list)
            {
                Meal meal = await db.GetItemAsync(item);
                if (meal != null)
                {
                    Meals.Add(meal);
                }
            }
        }
    }
}
