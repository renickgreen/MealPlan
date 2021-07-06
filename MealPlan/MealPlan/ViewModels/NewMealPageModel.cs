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

namespace MealPlan.ViewModels
{
    class NewMealPageModel : ViewModelBase
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
        private ICommand _photoCommand;
        public ICommand PhotoCommand
        {
            get => _photoCommand;
            set => SetProperty(ref _photoCommand, value);
        }
        private string _photoPath;
        public string PhotoPath
        {
            get => _photoPath;
            set => SetProperty(ref _photoPath, value);
        }
        

        //***Constructor****
        public NewMealPageModel()
        {
            
            IsFavorite = true;
            SaveCommand = new MvvmHelpers.Commands.Command(OnSaveCommand);
            CancelCommand = new MvvmHelpers.Commands.Command(OnCancelCommand);
            PhotoCommand = new MvvmHelpers.Commands.Command(OnPhotoCommand);
        }

        private void OnPhotoCommand(object obj)
        {
            Task photoTask = TakePhotoAsync();
        }

        private void OnCancelCommand(object obj)
        {
            ResetForm();
        }

        private async void OnSaveCommand(object obj)
        {
            if (!Validate()) 
            {
                var rout = $"{nameof(MealPlanPage)}";
                await Shell.Current.GoToAsync(rout);
                return; 
            }
            Meal item = new Meal { Name = Name, Author = Author, Favortie = IsFavorite, RecipeImage = PhotoPath};
            DatabaseControl db = await DatabaseControl.Instance;
            await db.SaveItemAsync(item);
            ResetForm();
           // var route = $"{nameof(MealPlanPage)}";
            await Shell.Current.GoToAsync("..");
        }
        void ResetForm()
        {
            Name = "";
            Author = "";
            PhotoPath = "";
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
        public async Task TakePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is now supported on the device
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

       public async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            PhotoPath = newFile;
        }
    }
}
