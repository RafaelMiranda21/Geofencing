using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using polygonalGeofencing.Models;
using polygonalGeofencing.Views;
using System.Collections.ObjectModel;
using System.Linq;

namespace polygonalGeofencing.ViewModels
{

    class CadastroFazendaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        ObservableCollection<Fazenda> ListaFazendas = new ObservableCollection<Fazenda>();
        

        

        public ObservableCollection<Fazenda> listaFazendas
        {
            get => ListaFazendas;
            set => ListaFazendas = value;
        }
        
  
        
 
      

        string nomeFazenda;
        int id;

        
        public string Nome
        {
            get => nomeFazenda;
            set
            {
                nomeFazenda = value;
                PropertyChanged(this, new PropertyChangedEventArgs("nomeFazenda"));
            }
        }

        public int Id
        {
            get => id;
            set
            {
                id = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Id"));
            }
        }


        public ICommand NovaFazenda
        {
            get => new Command(async () => {

                try
                {
                    List<Logado> logado = await App.database.GetLogado();
                   
                         Fazenda model = new Fazenda()
                        {
                            nome = this.nomeFazenda,
                            idUsuario = logado.ToList()[0].idUsuario
                        };

                        await App.database.InsertFazenda(model);

                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Fazenda Cadastrada", "ok");
                        AtualizarLista.Execute(null);

                }
                catch(Exception ex)
                {
                   await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "ok");
                }




            });
        }



        public ICommand CadastrarArea
        {
            get
            {
                return new Command<Fazenda>(async (fazenda) =>
                {
                    //redirecionamento para a página de cadastro de area passando como parametro o id da fazenda
                    await Shell.Current.GoToAsync($"//CadastroArea?parametro_id={fazenda.idFazenda}");
                });
            }
        }
        
        public ICommand Voltar
        {
            get
            {
                return new Command (async () =>
                {
                    await Shell.Current.GoToAsync("//Home");
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

                        List<Fazenda> fazendas = null;
                        //pega o usuário logado
                        List<Logado> logado = await App.database.GetLogado();
                        //pega as informações do usuário
                        Usuario usuario = await App.database.GetByIdUsuario(logado.ToList()[0].idUsuario);

                        
                        fazendas = await App.database.GetFazendaFuncionario(logado.ToList()[0].idUsuario, usuario.idDono);



                        //limpa a lista
                        listaFazendas.Clear();
                        if(fazendas != null)
                        {
                            //cria a lista de fazendas cadastradas
                            fazendas.ForEach(i => listaFazendas.Add(i));
                            
                        }
                        

                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "OK");

                    }
                    
                });
            }
        }


        public ICommand RemoverFazenda
        {
            get
            {
                return new Command<Fazenda>(async (fazenda) =>
                {
                    try
                    {
                        List<Logado> logado = await App.database.GetLogado();
                        Usuario usuario = await App.database.GetByIdUsuario(logado[0].idUsuario);

                        //se o usuário logado não for o dono ele não pode remover fazenda
                        if (usuario.idUsuario == usuario.idDono)
                        {                         
                            //Alerta para confirmação da exclusão
                            bool conf = await Application.Current.MainPage.DisplayAlert("Tem Certeza?", "Excluir", "Sim", "Não");

                            //se o usuário confirmar a exclusão
                            if (conf)
                            {
                                //exclui a fazenda do banco de dados
                                await App.database.DeleteFazenda(fazenda.idFazenda);
                                //chama a função para recarregar a lista
                                AtualizarLista.Execute(null);
                            }
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Ops","Somente o proprietario pode remover a fazenda", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "OK");
                    }
                });
            }
        }




    }
}
