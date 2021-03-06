using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using MealPlan.Models;
using MealPlan.Services;
using MvvmHelpers.Commands;
using Syncfusion.ListView;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;
using MealPlan.Views;
using MvvmHelpers;
using Syncfusion.ListView.XForms;

namespace MealPlan.ViewModels
{
    [QueryProperty(nameof(PlanId), nameof(PlanId))]
    public class NewMealPlanPageModel : ViewModelBase
    {
        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set => SetProperty(ref _isFavorite, value);
        }
        private bool _isEnable;
        public bool IsEnable
        {
            get => _isEnable;
            set => SetProperty(ref _isEnable, value);
        }
        private int _planId;
        public int PlanId
        {
            get => _planId;
            set
            {
                SetProperty(ref _planId, value);
                LoadPlanAsync();
            }
        }
        #region Commands
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get => _saveCommand;
            set => SetProperty(ref _saveCommand, value);
        }
        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get => _cancelCommand;
            set => SetProperty(ref _cancelCommand, value);
        }
        private ICommand _selectedCommand;
        public ICommand SelectedCommand
        {
            get => _selectedCommand;
            set => SetProperty(ref _selectedCommand, value);
        }
        private ICommand _removeCommand;
        public ICommand RemoveCommand
        {
            get => _removeCommand;
            set => SetProperty(ref _removeCommand, value);
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
        #endregion
        private ObservableRangeCollection<Meal> _allMeals;
        public ObservableRangeCollection<Meal> AllMeals
        {
            get => _allMeals;
            set => SetProperty(ref _allMeals, value);
        }
        private ObservableRangeCollection<Meal> _mealPlan;
        public ObservableRangeCollection<Meal> MealPlan
        {
            get => _mealPlan;
            set => SetProperty(ref _mealPlan, value);
        }
        public string MealsPlan { get; set; }
        //***Constructor****
        public NewMealPlanPageModel()
        {
            MealsPlan = "";
            MealPlan = new ObservableRangeCollection<Meal>();
            AllMeals = new ObservableRangeCollection<Meal>();
            IsFavorite = true;
            AddCommand = new MvvmHelpers.Commands.Command(OnAddCommand);
            DeleteCommand = new MvvmHelpers.Commands.Command(OnDeleteCommand);
            RemoveCommand = new MvvmHelpers.Commands.Command(OnRemoveCommand);
            SelectedCommand = new MvvmHelpers.Commands.Command(OnSelectedCommand);
            SaveCommand = new MvvmHelpers.Commands.Command(OnSaveCommand);
            CancelCommand = new MvvmHelpers.Commands.Command(OnCancelCommand);
            GetAllMeals();
        }

        private async void OnAddCommand(object obj)
        {
            await Shell.Current.DisplayAlert("ADD", "These Meal to Current Plan", "ok");
        }

        private async void OnDeleteCommand(object obj)
        {
            await Shell.Current.DisplayAlert("HELP", "Delete This Plan", "OK");
        }

        private void OnRemoveCommand(object obj)
        {

            Meal meal = new Meal();
            meal = (obj as Syncfusion.ListView.XForms.ItemTappedEventArgs).ItemData as Meal;
            MealPlan.Remove(meal);

        }

        private void OnSelectedCommand(object obj)
        {
            Meal meal = obj as Meal;
            meal.Order = MealPlan.Count + 1;
            MealPlan.Add(meal);
            
        }

        private void OnCancelCommand(object obj)
        {
            ResetForm();
        }

        private async void OnSaveCommand(object obj)
        {
            
            
            if (!Validate())
            {
                await Shell.Current.DisplayAlert("Missing Info", 
                    "Make sure the Name, Author are filled out and you added items to your Meal Plan list.", "OK");
                return;
            }
            var count = 0;
            MealsPlan = "";
            foreach(Meal m in MealPlan)
            {
                m.Order = count++;
                MealsPlan += $"{m.Id},";
            }

            MealPlanModel item = new MealPlanModel {Name = Name, Author = Author, Favortie = IsFavorite, Meals = MealsPlan };
            if (PlanId > 0) item.Id = PlanId; 
            DatabaseControl db = await DatabaseControl.IPlan;
            var planId = await db.SavePlanAsync(item);
            if(AppShell.CurrentPlan == 0)
            {
                Preferences.Set("current_plan_key", AppShell.CurrentPlan = planId);
            }
            ResetForm();
            await Shell.Current.GoToAsync(".."); /* */
        } 
        void ResetForm()
        {
            Name = "";
            Author = "";
            IsFavorite = false;
            MealPlan.Clear();
        }
        bool Validate()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Author) || MealPlan.Count == 0)
            {
                return false;
            }
            return true;
        }
        async void GetAllMeals()
        {
            DatabaseControl db = await DatabaseControl.Instance;
            List<Meal> list = await db.GetItemsAsync();

            foreach (Meal m in list)
            {
                if(m != null)
                {
                    AllMeals.Add(m);
                }
                
            }
        }
        async void LoadPlanAsync()
        {
            DatabaseControl db = await DatabaseControl.IPlan;
            MealPlanModel plan = await db.GetPlanAsync(PlanId);
            Name = plan.Name;
            Author = plan.Author;
            Description = plan.Description;
            Favortie = plan.Favortie;
            ExtractMealsAsync(plan.Meals);
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
                if(meal != null)
                {
                    MealPlan.Add(meal);
                }
                
            }
        }
    }
}
