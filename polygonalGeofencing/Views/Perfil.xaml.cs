using polygonalGeofencing.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace polygonalGeofencing.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Perfil : ContentPage
    {
        public Perfil()
        {
            InitializeComponent();
            BindingContext = new PerfilViewModel();
        }

        protected override void OnAppearing() //ao inicializar a pagina, quando ela for exibida
        {
            var vm = (PerfilViewModel)BindingContext;
            //chama a função mostrarPerfil da viewmodel
            vm.mostrarPerfil.Execute(null);
        }


        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Home");
        }
    }
}