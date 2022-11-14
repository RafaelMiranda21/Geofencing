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
    public partial class Home : ContentPage
    {

        public Home()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel();
        }

        protected override void OnAppearing() //ao inicializar a pagina, quando ela for exibida
        { 
            ControleAcesso();
        }



        private async void AdicionarFazenda(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//CadastroFazenda");
        }

        private async void AdicionarFuncionarios(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//CadastroFuncionario");
        }
  
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {

        }



        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ListaFuncionario");
        }


        public async void ControleAcesso()
        {

            //************Inicializa com os cards desabilitados***************
            frameFazenda.IsEnabled = false;
            frameFazenda.HasShadow = false;
            frameFazenda.BackgroundColor = Color.FromHex("e7e7e7");
            frameAddFunc.IsEnabled = false;
            frameAddFunc.HasShadow = false;
            frameAddFunc.BackgroundColor = Color.FromHex("e7e7e7");
            frameListaFunc.IsEnabled = false;
            frameListaFunc.HasShadow = false;
            frameListaFunc.BackgroundColor = Color.FromHex("e7e7e7");

            
            //******************************************************************



            List<Logado> logado = await App.database.GetLogado(); //pega o id do usuário logado
            //pega os acessos que o usuário tem
            List<Acesso> Acessos = await App.database.GetAcessoUsuario(logado.ToList()[0].idUsuario,-1); 
            //pega as informações do usuário
            Usuario usuario = await App.database.GetByIdUsuario(logado.ToList()[0].idUsuario);

            



           

            //verifica se o usuário logado e o dono
            if (usuario.idDono != usuario.idUsuario)
            {
                //coloca o nome completo do usuário na página
                Usuario.Text = usuario.nome + " " + usuario.sobrenome;
                //pega as fazendas que o usuário tem acesso
                List<Fazenda> fazendas =  await App.database.GetFazendaFuncionario(usuario.idUsuario, usuario.idDono);
                
                //coloca a quantidade de fazendas no quadro de visão geral
                qtdFazenda.Text = fazendas.Count().ToString();


                string str = "";
                List<Area> areas = null;
                List<Usuario> funcionarios = null;

               
                //colocar na variavel str todas as fazendas em que o usuário tem acesso
                foreach (Fazenda fazenda in fazendas.ToList())
                {
                    str = fazenda.idFazenda + ",";
                }


                if (str != "")
                {
                   areas = await App.database.GetAreasFazenda(str.Substring(0, str.Length - 1)); //Substring para retirar a ultima virgula 
                   funcionarios = await App.database.GetFuncionariosFazenda(str.Substring(0, str.Length - 1), usuario.idUsuario, usuario.idDono); //Substring para retirar a ultima virgula 
                }
                

                qtdArea.Text = "-";  //se o usuário não tiver acesso a nenhuma área
                qtdFuncionario.Text = "-";//se o usuário não tiver acesso a ver funcionario

                foreach (Acesso acesso in Acessos.ToList())
                {
                //Controle dos cards, habilitando os que o usuário tem acesso.
                    switch (acesso.idTipo)
                    {
                        case 1:
                            frameFazenda.IsEnabled = true;
                            frameFazenda.HasShadow = true;
                            frameFazenda.BackgroundColor = Color.FromHex("fff");

                            if(areas != null)
                            {
                                qtdArea.Text = areas.Count().ToString();
                            }
                            
                            break;
                        case 2:
                            frameAddFunc.IsEnabled = true;
                            frameAddFunc.HasShadow = true;
                            frameAddFunc.BackgroundColor = Color.FromHex("fff");
                            if (funcionarios != null)
                            {
                                qtdFuncionario.Text = funcionarios.Count().ToString();
                            }
                            break;
                        case 3:
                            frameListaFunc.IsEnabled = true;
                            frameListaFunc.HasShadow = true;
                            frameListaFunc.BackgroundColor = Color.FromHex("fff");
                            if (funcionarios != null)
                            {
                                qtdFuncionario.Text = funcionarios.Count().ToString();
                            }
                            break;
 
                    }
                }
            }
            else
            {
                //se o usuário for o dono habilitar to
                List<Fazenda> fazendas = await App.database.GetFazendaUsuario(usuario.idDono);
                List<Area> areas = await App.database.GetAreasUsuario(usuario.idDono);
                List<Usuario> funcionarios = await App.database.GetFuncionariosFazenda("-1",usuario.idUsuario, usuario.idDono);

                Usuario.Text = usuario.nome + " " + usuario.sobrenome;
                qtdFazenda.Text = fazendas.Count().ToString();
                qtdArea.Text = areas.Count().ToString();
                qtdFuncionario.Text = funcionarios.Count().ToString();

                frameFazenda.IsEnabled = true;
                frameFazenda.HasShadow = true;
                frameFazenda.BackgroundColor = Color.FromHex("fff");
                frameAddFunc.IsEnabled = true;
                frameAddFunc.HasShadow = true;
                frameAddFunc.BackgroundColor = Color.FromHex("fff");
                frameListaFunc.IsEnabled = true;
                frameListaFunc.HasShadow = true;
                frameListaFunc.BackgroundColor = Color.FromHex("fff");


            }

        }

  
    }
}