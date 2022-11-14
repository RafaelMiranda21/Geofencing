using polygonalGeofencing.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace polygonalGeofencing.ViewModels
{

    public class ListaFuncionarioViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        ObservableCollection<Usuario> ListaFuncionario = new ObservableCollection<Usuario>();


        public ObservableCollection<Usuario> listaFuncionario
        {
            get => ListaFuncionario;
            set => ListaFuncionario = value;
        }




        public ICommand EditarFuncionario
        {
            get
            {
                return new Command<Usuario>(async (funcionario) =>
                {
                    List<Logado> logado = await App.database.GetLogado();
                    Usuario usuario = await App.database.GetByIdUsuario(logado.ToList()[0].idUsuario);


                    List<Fazenda> fazendas = await App.database.GetFazendaUsuario(usuario.idDono);

                if (fazendas.Count > 0)
                    {
                       await Shell.Current.GoToAsync($"//ListaFuncionario/AcessoFuncModal?parametro_id={funcionario.idUsuario}");
                    }
                    else
                    {
                       await App.Current.MainPage.DisplayAlert("Ops", "Nenhuma Fazenda Criada", "OK");
                    }

                    
                });
            }
        }


        public ICommand AtualizarLista
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        //pega o id do usuário logado
                        List<Logado> logado = await App.database.GetLogado();
                        //pega as informações do usuário
                        Usuario usuario = await App.database.GetByIdUsuario(logado.ToList()[0].idUsuario);
                        //pega todos os funcionarios do proprietario das fazendas em que o usuário logado tem permissão 
                        List<Fazenda> fazendas = await App.database.GetFazendaFuncionario(usuario.idUsuario,usuario.idDono);

                        string str = "";


                        //colocar na variavel str todas as fazendas em que o usuário tem acesso
                        foreach (Fazenda fazenda in fazendas.ToList())
                        {
                            str = fazenda.idFazenda + ",";
                        }

                        List<Usuario> funcionarios = await App.database.GetFuncionariosFazenda(str.Substring(0, str.Length - 1), usuario.idUsuario, usuario.idDono);
                        //limpa a lista
                        listaFuncionario.Clear();
                        //para cada registro de funcionario no banco e adicionado na lista
                        funcionarios.ForEach(i => listaFuncionario.Add(i));

                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "OK");

                    }

                });
            }
        }
        

        public ICommand RemoverFuncionario
        {
            get
            {
                return new Command<Usuario>(async (usuario) =>
                {
                    try
                    {
                        //Alerta de confirmação para exclusão
                        bool conf = await Application.Current.MainPage.DisplayAlert("Tem Certeza?", "Excluir", "Sim", "Não");

                        if (conf) //se form confirmado
                        {
                            //o usuário e setado como ativo = 0
                            await App.database.ExcluirUsuario(usuario.idUsuario);
                            //atualiza a lista 
                            AtualizarLista.Execute(null);
                        }

                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "OK");

                    }

                });
            }
        }

        public ICommand Voltar
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                         Shell.Current.GoToAsync("//Home");

                    }
                    catch (Exception ex)
                    {
                         Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "OK");

                    }

                });
            }
        }

  

    }
}
