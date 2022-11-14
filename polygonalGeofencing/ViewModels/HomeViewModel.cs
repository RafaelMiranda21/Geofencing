using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using polygonalGeofencing.Views;
using Xamarin.Forms;
using polygonalGeofencing.Models;
using System.Collections.ObjectModel;

namespace polygonalGeofencing.ViewModels
{
   
    class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand Sair
        {
            get => new Command(async () => {

                try
                {
                    await App.database.DelLogado(); //limpa a tabela logado
                    await Shell.Current.GoToAsync("//Login");//redireciona para a tela de login
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "ok");
                }

            });
        }


    }
}
