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

namespace MealPlan.ViewModels
{
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
        //***Constructor****
        public NewMealPlanPageModel()
        {
            MealPlan = new ObservableRangeCollection<Meal>();
            AllMeals = new ObservableRangeCollection<Meal>();
            IsFavorite = true;
            SelectedCommand = new MvvmHelpers.Commands.Command(OnSelectedCommand);
            SaveCommand = new MvvmHelpers.Commands.Command(OnSaveCommand);
            CancelCommand = new MvvmHelpers.Commands.Command(OnCancelCommand);
            GetAllMeals();
        }

        private void OnSelectedCommand(object obj)
        {
            MealPlan.Add(obj as Meal);
        }

        private void OnCancelCommand(object obj)
        {
            ResetForm();
        }

        private async void OnSaveCommand(object obj)
        {
            await Shell.Current.DisplayAlert("Save", "Save Command Hit", "OK");
            /*
            if (!Validate())
            {
                var rout = $"{nameof(MealPlanPage)}";
                await Shell.Current.GoToAsync(rout);
                return;
            }
            Meal item = new Meal { Name = Name, Author = Author, Favortie = IsFavorite };
            DatabaseControl db = await DatabaseControl.Instance;
            await db.SaveItemAsync(item);
            ResetForm();
            var route = $"{nameof(MealPlanPage)}";
            await Shell.Current.GoToAsync(route);  */
        }
        void ResetForm()
        {
            Name = "";
            Author = "";
            IsFavorite = false;
        }
        bool Validate()
        {
            if (string.IsNullOrEmpty(Name))
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
                AllMeals.Add(m);
            }
        }
    }
}
