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
    public partial class entrarModal : ContentPage
    {
        public entrarModal()
        {
            InitializeComponent();
            BindingContext = new LoginUsuarioViewModel();
        }
        private async void btnCadastrar_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Login/cadastroModal");
        }
    }
}