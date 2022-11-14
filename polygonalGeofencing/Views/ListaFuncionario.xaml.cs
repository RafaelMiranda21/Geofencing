using polygonalGeofencing.Models;
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
    public partial class ListaFuncionario : ContentPage
    {
        public ListaFuncionario()
        {
            InitializeComponent();
            BindingContext = new ListaFuncionarioViewModel();
        }

        protected override void OnAppearing() //ao inicializar a pagina, quando ela for exibida
        {
            var vm = (ListaFuncionarioViewModel)BindingContext;
            //chama a função atualizar lista na viewmodel
            vm.AtualizarLista.Execute(null);

        }

        private void btnAddPerm_Clicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            var area = button?.BindingContext as Usuario;
            var vm = BindingContext as ListaFuncionarioViewModel;

            vm?.EditarFuncionario.Execute(area);
        }

        private void btnExcluir_Clicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            var area = button?.BindingContext as Usuario;
            var vm = BindingContext as ListaFuncionarioViewModel;

            vm?.RemoverFuncionario.Execute(area);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
}