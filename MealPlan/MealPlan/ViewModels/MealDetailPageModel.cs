using MealPlan.Models;
using MealPlan.Services;
using MealPlan.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MealPlan.ViewModels
{
    [QueryProperty(nameof(MealId), nameof(MealId))]
    public class MealDetailPageModel : ViewModelBase
    {
        private string _mealId;
        public string MealId
        {
            get => _mealId;
            set
            { 
                SetProperty(ref _mealId, value);

                GetMeal();
            }
        }
        private Image _image;
        public Image Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }
        private string _photoPath;
        public string PhotoPath
        {
            get => _photoPath;
            set => SetProperty(ref _photoPath, value);
        }
        private Meal _theMeal;
        public Meal TheMeal
        {
            get => _theMeal;
            set => SetProperty(ref _theMeal, value);
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
        public MealDetailPageModel()
        {
            TapCommand = new MvvmHelpers.Commands.AsyncCommand(Selected);
            EditCommand = new MvvmHelpers.Commands.AsyncCommand(OnEditCommand);
            DeleteCommand = new MvvmHelpers.Commands.AsyncCommand(OnDeleteCommand);
        }

        private async void GetMeal()
        {
            TheMeal = new Meal();
            int.TryParse(MealId, out int result);
            DatabaseControl db = await DatabaseControl.Instance;
            TheMeal = await db.GetItemAsync(result);

            Name = TheMeal.Name;
            Author = TheMeal.Author;
            Description = TheMeal.Description;
            Favortie = TheMeal.Favortie;
            PhotoPath = TheMeal.RecipeImage;
        }
        async Task Selected()
        {
            var route = $"{nameof(RecipeImagePage)}?{nameof(RecipeImagePageModel.PhotoPath)}={PhotoPath}";
            await Shell.Current.GoToAsync(route);
        }
        async Task OnEditCommand()
        {
            ///TODO: Edit Button for Meal
            await Shell.Current.DisplayAlert(Title, "Go to Edit", "TODO");
        }
        async Task OnDeleteCommand()
        {
            if (await Shell.Current.DisplayAlert("Warning", $"Do you want to PERMANENTLY DELETE {Name}?", "Yes Delete", "No"))
            {
                DatabaseControl db = await DatabaseControl.Instance;
                await db.DeleteItemAsync(TheMeal);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
