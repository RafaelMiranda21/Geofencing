using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using polygonalGeofencing.Models;
using polygonalGeofencing.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace polygonalGeofencing.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastroFazenda : ContentPage
    {
        public CadastroFazenda()
        {
            InitializeComponent();
            BindingContext = new CadastroFazendaViewModel();
        }


        protected async override void OnAppearing() //ao inicializar a pagina, quando ela for exibida
        {

            List<Logado> logado = await App.database.GetLogado();
            Usuario usuario = await App.database.GetByIdUsuario(logado[0].idUsuario);

            //se o usuário logado não for o dono ele não pode cadastrar fazenda
            if(usuario.idUsuario != usuario.idDono)
            {
                cadastroNome.IsEnabled = false;
                NovaFazenda.IsEnabled = false;

            }
            else
            {
                cadastroNome.IsEnabled = true;
                NovaFazenda.IsEnabled = true;
            }

            var vm = (CadastroFazendaViewModel)BindingContext;
            //chama a função atualizar lista da viewmodel para carregar as fazendas cadastradas
            vm.AtualizarLista.Execute(null); 
        }



        private void btnExcluir_Clicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            var fazenda = button?.BindingContext as Fazenda;
            var vm = BindingContext as CadastroFazendaViewModel;

            vm?.RemoverFazenda.Execute(fazenda);
        }

        private void btnAddArea_Clicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            //pega o id da fazenda
            var fazenda = button?.BindingContext as Fazenda;
            var vm = BindingContext as CadastroFazendaViewModel;
            //chama a função cadastrarArea passando como parametro o id da fazenda
            vm?.CadastrarArea.Execute(fazenda);
        }


        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //botão voltar para Home
            await Shell.Current.GoToAsync("//Home");
        }
    }
}