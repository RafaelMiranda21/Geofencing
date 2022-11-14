using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using polygonalGeofencing.Views;
using Xamarin.Forms;
using polygonalGeofencing.Models;
using System.Collections.ObjectModel;

namespace polygonalGeofencing.ViewModels
{
    //recebimento do id da fazenda
    [QueryProperty(nameof(PegarId), "parametro_id")]
    class CadastroAreaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

 
        ObservableCollection<Area> ListaArea = new ObservableCollection<Area>();


        public ObservableCollection<Area> listaArea
        {
            get => ListaArea;
            set => ListaArea = value;
        }


        string txtFazenda, txtArea, txtDesc;
        int idfazenda;

        public int idFazenda
        {
            get => idfazenda;
            set
            {
                idfazenda = value;
                PropertyChanged(this, new PropertyChangedEventArgs("idFazenda"));
            }
        }


        public string nomeArea
        {
            get => txtArea;
            set
            {
                txtArea = value;
                PropertyChanged(this, new PropertyChangedEventArgs("nomeArea"));
            }
        }


        public string descArea
        {
            get => txtDesc;
            set
            {
                txtDesc = value;
                PropertyChanged(this, new PropertyChangedEventArgs("descArea"));
            }
        }


        public string nomeFazenda
        {
            get => txtFazenda;
            set
            {
                txtFazenda = value;
                PropertyChanged(this, new PropertyChangedEventArgs("nomeFazenda"));
            }
        }

        public string PegarId
        {
            set
            {
                int id_parametro = Convert.ToInt32(Uri.UnescapeDataString(value));
                //chama a função mostrar fazenda passando como parametro o id da fazenda
                mostrarFazenda.Execute(id_parametro); 
            }
        }


        public ICommand CadastrarPontos
        {
            get
            {
                return new Command<int>(async (int id) =>
                {
                    await Shell.Current.GoToAsync($"//CadastroPontos?parametro_id={id}");
                });
            }
        }


        public ICommand EditarArea
        {
            get
            {
                return new Command<Area>(async (area) =>
                {
                    //redireciona para a página para editar a descrição da área passando o id da área como parametro
                    await Shell.Current.GoToAsync($"//EditAreaModal?parametro_id={area.idArea}");
                });
            }
        }
        
        public ICommand AddGeo
        {
            get
            {
                return new Command<Area>(async (area) =>
                {
                    //redireciona para a página para selecionar a área passando o id da área como parametro
                    await Shell.Current.GoToAsync($"//CadastroPontos?parametro_id={area.idArea}");
                });
            }
        }

        public ICommand RemoverArea
        {
            get
            {
                return new Command<Area>(async (area) =>
                {
                    try
                    {
                        //envia um alerta para a confirmação da exclusão
                        bool conf = await Application.Current.MainPage.DisplayAlert("Tem Certeza?", "Excluir", "Sim", "Não");

                        if (conf)//se o usuário confirmar
                        {
                            //a área e deletada do banco de dados
                            await App.database.DeleteArea(area.idArea);
                            //todos os pontos adicionados a area e excluido
                            await App.database.DellPontosArea(area.idArea);
                            //atualiza a área
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

        public ICommand mostrarFazenda
        {
            get => new Command<int>(async (int id) =>
            {
                try
                {
                    //pega as informações da fazenda no banco de dados
                    Fazenda model = await App.database.GetByIdFazenda(id);
                    /*adiciona o nome da fazenda em uma label para exibir
                    para o usuário em qual fazenda ele esta adicionando a area*/
                    this.nomeFazenda = "Fazenda " + model.nome;
                    this.idFazenda = model.idFazenda;//coloca o id da fazenda em um campo oculto
                    AtualizarLista.Execute(null); //chama a função atualizar lista
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("ERRO", ex.Message, "OK");
                }

            });
        }


        

        public ICommand novaArea
        {
            get => new Command(async () =>
            {
                try
                {
                    //a classe recebe as informações digitadas pelo usuário
                    Area model = new Area()
                    {
                        nome = this.txtArea,
                        descricao = this.txtDesc,
                        idFazenda = this.idfazenda
                    };
                    //insere as informações no banco de dados
                    await App.database.InsertArea(model);
                    //emite um alerta para usuário informando do sucesso do cadastro
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Area Cadastrada", "ok");
                    //chama a função para atualizar a listaao 
                    AtualizarLista.Execute(null);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "ok");
                }
            });
        }





        public ICommand AtualizarLista
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        //Pega todas as áreas cadastradas para a fazenda em referencia
                        List<Area> tmp = await App.database.GetAllRowsArea(this.idFazenda);
                        //Limpa a lista
                        listaArea.Clear();
                        //Para cada dado retornado do banco é adicionado na lista
                        tmp.ForEach(i => listaArea.Add(i));

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
                return new Command (() =>
                {
                    try
                    {
                        Shell.Current.GoToAsync("//CadastroFazenda");

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
