using MealPlan.Models;
using MealPlan.Services;
using System;
using System.Collections.Generic;
using System.Text;
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
        public MealDetailPageModel()
        {
           //   GetMeal();
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
    }
}
