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
    public partial class AcessoFuncModal : ContentPage
    {
        bool flag = false;
        public AcessoFuncModal()
        {
            InitializeComponent();
            BindingContext = new ControleAcesso();
            
        }

        protected override async void OnAppearing() 
        {
            //flag para controle de permissões 
            flag = false;
            List<Logado> logado = await App.database.GetLogado();
            Usuario usuario = await App.database.GetByIdUsuario(logado.ToList()[0].idUsuario);

            //pega as fazendas que o usuário possui permissão
            List<Fazenda> fazendas = await App.database.GetFazendaFuncionario(usuario.idUsuario, usuario.idDono);

            //se o resultado da busca for maior que 0
            if(fazendas.Count > 0)
            {
                //adiciona no picker as fazendas
                foreach (Fazenda fazenda in fazendas.ToList())
                {
                    listaFazenda.Items.Add(fazenda.nome);
                }

            }
            else
            {
                //se não tiver registro oculta o campo da lista de fazenda
                listaFazenda.IsVisible = false;
            }

            //chama a função de controle de acesso
            ControleAcesso();
        }
 
        private void Voltar_Tapped(object sender, EventArgs e)
        {

        }


        //evento chamado ao trocar o estado do switch da área
        private void AcessoArea_Toggled(object sender, ToggledEventArgs e)
        {               
            var troca = AcessoArea.IsToggled;
            AtualizaAcesso(troca, 1);
        }

        //evento chamado ao trocar o estado do switch de adicionar funcionário
        private void AcessoAddFunc_Toggled(object sender, ToggledEventArgs e)
        {
            var troca = AcessoAddFunc.IsToggled;
            AtualizaAcesso(troca, 2);
        }

        //evento chamado ao trocar o estado do switch de adicionar permissões para o funcionário
        private void AcessoAddPerm_Toggled(object sender, ToggledEventArgs e)
        {
            var troca = AcessoAddPerm.IsToggled;

            AtualizaAcesso(troca, 3);
        }

        
        //evento ao trocar de item no dropdown das fazendas
        private void listaFazenda_SelectedIndexChanged(object sender, EventArgs e)
        {
            ControleAcesso();
        }

        //função para atualizar acessos, recebe como parametro o estado do switch e o tipo de permissão
        public async void AtualizaAcesso(bool Acesso, int Tipoid)
        {
            //recebe a fazenda selecionada no dropdown
            var fazenda = listaFazenda.SelectedItem;
            List<Logado> logado = await App.database.GetLogado();
            Usuario usuario = await App.database.GetByIdUsuario(logado.ToList()[0].idUsuario);

            if (!flag)
            {
                if (listaFazenda.IsVisible)
                {
                    //se tiver fazenda selecionada
                    if (fazenda != null)
                    {
                        //pega os dados da fazenda
                        List<Fazenda> modelFazenda = await App.database.GetByNomeFazenda(fazenda.ToString(), usuario.idDono);
                        int idFazenda = modelFazenda[0].idFazenda;

                        //se o estado do switch for ativo
                        if (Acesso) //Concedeu Acesso
                        {
                            //adiciona o acesso para o funcionário
                            await App.database.InsertAcesso(Int32.Parse(idFuncionario.Text), Tipoid, idFazenda);
                            await App.Current.MainPage.DisplayAlert("Sucesso", "Acesso Liberado", "OK");
                        }
                        else //retirou acesso
                        {
                            //retira o acesso do funcionário
                            await App.database.DeleteAcessoUsuario(Int32.Parse(idFuncionario.Text), Tipoid, idFazenda);
                            await App.Current.MainPage.DisplayAlert("Sucesso", "Acesso Revogado", "OK");
                        }
                    }
                    else
                    {
                        //se não selecionou nenhuma fazenda e disparado um alerta para o usuário selecionar a fazenda
                        await App.Current.MainPage.DisplayAlert("Ops", "Selecione a Fazenda", "OK");
                        flag = true;
                        //reverte a ação do usuário
                        switch (Tipoid)
                        {
                            case 1:
                                AcessoArea.IsToggled = !AcessoArea.IsToggled;
                                break;
                            case 2:
                                AcessoAddFunc.IsToggled = !AcessoAddFunc.IsToggled;
                                break;
                            case 3:
                                AcessoAddPerm.IsToggled = !AcessoAddPerm.IsToggled;
                                break;
   
                        }
                        flag = false;
                    }
                }
                else
                {
                    await Shell.Current.GoToAsync("//ListaFuncionario");
                }
            }
            else{
                flag = false;
            }
        }

        //função de controle de acesso
        public async void ControleAcesso()
        {
            //recebe a fazenda selecionada no dropdown
            var fazenda = listaFazenda.SelectedItem;
            List<Logado> logado = await App.database.GetLogado();
            Usuario usuario = await App.database.GetByIdUsuario(logado.ToList()[0].idUsuario);

            //se tiver fazenda selecionada
            if (fazenda != null)
            {
                //pega os dados da fazenda no banco
                List<Fazenda> modelFazenda = await App.database.GetByNomeFazenda(fazenda.ToString(),usuario.idDono);
                int idFazenda = modelFazenda[0].idFazenda;
                
                //pega os acessos do funcionário
                List<Acesso> Acessos = await App.database.GetAcessoUsuario(Int32.Parse(idFuncionario.Text), idFazenda);

                //troca o estado dos switchs de acordo com acessos do funcionário
                if(Acessos.Count > 0)
                {
                    foreach (Acesso acesso in Acessos.ToList())
                    {
                        
                        switch (acesso.idTipo)
                        {
                            case 1:
                                flag = true;
                                AcessoArea.IsToggled = true;
                                break;
                            case 2:
                                flag = true;
                                AcessoAddFunc.IsToggled = true;
                                break;
                            case 3:
                                flag = true;
                                AcessoAddPerm.IsToggled = true;
                                break;


                        }
                            flag = false;
                    }
                }
                else
                {
                    flag = true;
                    AcessoArea.IsToggled = false;
                    flag = true;
                    AcessoAddFunc.IsToggled = false;
                    flag = true;
                    AcessoAddPerm.IsToggled = false;
                    flag = false;

                }
            }
            else
            {
                flag = true;
                AcessoArea.IsToggled = false;
                flag = true;
                AcessoAddFunc.IsToggled = false;
                flag = true;
                AcessoAddPerm.IsToggled = false;
                flag = false;

            }
         

        }

      
    }
}