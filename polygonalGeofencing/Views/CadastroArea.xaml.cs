using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using polygonalGeofencing.ViewModels;
using polygonalGeofencing.Models;

namespace polygonalGeofencing.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastroArea : ContentPage
    {

        public CadastroArea()
        {
            InitializeComponent();
            BindingContext = new CadastroAreaViewModel();
        }
        protected override void OnAppearing() //ao inicializar a pagina, quando ela for exibida
        {
            var vm = (CadastroAreaViewModel)BindingContext;
        }

        private void btnExcluir_Clicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            var area = button?.BindingContext as Area;
            var vm = BindingContext as CadastroAreaViewModel;

            vm?.RemoverArea.Execute(area);
        }


        private void btnEditArea_Clicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            var area = button?.BindingContext as Area;
            var vm = BindingContext as CadastroAreaViewModel;

            vm?.EditarArea.Execute(area);
        }

      
        private void btnAddGeo_Clicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            var area = button?.BindingContext as Area;
            var vm = BindingContext as CadastroAreaViewModel;

            vm?.AddGeo.Execute(area);
        }


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
}