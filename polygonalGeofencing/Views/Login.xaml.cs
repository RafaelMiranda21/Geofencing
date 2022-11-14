using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using polygonalGeofencing.Models;
using polygonalGeofencing.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace polygonalGeofencing.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
           
        }

        protected override async void OnAppearing() //ao inicializar a pagina, quando ela for exibida
        {



            //await App.database.DelAllUsuarios();
            //await App.database.DelLogado();
            //await App.database.DelArea();
            //await App.database.DelFazenda();
            //await App.database.DelPontos();
            //await App.database.DelAllAcessos();


            List<Logado> logado = await App.database.GetLogado(); //pega as informações da tabela logado

            if (logado.Count > 0) //verifica se há registro
            {
                //verifica a diferença entre a data atual e a data do banco
                int diasLogado = (int)DateTime.Today.Subtract(logado.ToList()[0].data).TotalDays; 
                /*Verifica no banco se este usuário ja esta logado a mais de 2 dias, se estiver remover o registro dele,
                para que ele possa logar de novo*/
                if (diasLogado >= 2) 
                {
                    await App.database.DelLogado(); //deletrar as informações da tabela logado
                }
                else
                {
                    //se a diferença for de menos de 2 dias redirecionar para a home page
                    await Shell.Current.GoToAsync("//Home");
                }

            }

        }

        private async void btnEntrar_Clicked(object sender, EventArgs e)
        {
            //redireciona para a tela entrarModal, chando-o dessa forma ira abrir como modal
            await Shell.Current.GoToAsync("//Login/entrarModal"); 
        }

        private async  void btnCadastrar_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Login/cadastroModal");
        }
    }
}