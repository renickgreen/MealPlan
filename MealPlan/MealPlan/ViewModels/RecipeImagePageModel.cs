using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MealPlan.ViewModels;
using MealPlan.Views;
using Xamarin.Forms;

namespace MealPlan.ViewModels
{
    [QueryProperty(nameof(PhotoPath), nameof(PhotoPath))]
    public class RecipeImagePageModel : ViewModelBase
    {
        private string _photoPath;
        public string PhotoPath 
        { 
            get => _photoPath; 
            set => SetProperty(ref _photoPath, value);
        }
        private ICommand _goBackCommand;
        public ICommand GoBackCommand
        {
            get => _goBackCommand;
            set => SetProperty(ref _goBackCommand, value);
        }
        public RecipeImagePageModel()
        {
            GoBackCommand = new MvvmHelpers.Commands.AsyncCommand(OnGoBackCommand);
        }

        private async Task OnGoBackCommand()
        {
            await Shell.Current.GoToAsync($"///{ nameof(MealPlanPage)}");
        }
    }
}
