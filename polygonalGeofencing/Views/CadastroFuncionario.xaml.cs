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
    public partial class CadastroFuncionario : ContentPage
    {
        public CadastroFuncionario()
        {
            InitializeComponent();
            BindingContext = new CadastroUsuarioViewModel();
        }



        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
}