using polygonalGeofencing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace polygonalGeofencing.ViewModels
{

    [QueryProperty(nameof(PegarId), "parametro_id")]
    public class EditarAreaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;



        string txtDesc, txtmostrarNome;
        int idarea;

        public int idArea
        {
            get => idarea;
            set
            {
                idarea = value;
                PropertyChanged(this, new PropertyChangedEventArgs("idArea"));
            }
        }

        
        public string mostrarNome
        {
            get => txtmostrarNome;
            set
            {
                txtmostrarNome = value;
                PropertyChanged(this, new PropertyChangedEventArgs("mostrarNome"));
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

        
        public string PegarId
        {
            set
            {
                int id_parametro = Convert.ToInt32(Uri.UnescapeDataString(value));
                //chama a função para mostrar as informações da área
                mostrarArea.Execute(id_parametro);
            }
        }
        public ICommand mostrarArea
        {
            get => new Command<int>(async (int id) =>
            {
                try
                {
                    //pega no banco as informações da área
                    Area model = await App.database.GetByIdArea(id);
                    this.mostrarNome =  model.nome; //mostra na tela o nome da área
                    this.idArea = model.idArea; //guarda o id da área
                    //formata a descrição para a melhor visualização
                    this.descArea = String.Format(model.descricao.Replace(",", "\n\n"));
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("ERRO", ex.Message, "OK");
                }

            });
        }


        public ICommand editarArea
        {
            get => new Command (async () =>
            {
                try
                {
                    //recebe o campo da descrição trocando as quebras de linha por ','
                    string descricao = String.Format(this.descArea.Replace("\n\n", ","));
                    //atualiza a informação no banco
                    await App.database.UpdateArea(this.idArea, descricao);
                    //dispara um alerta informando que a alteração foi realizada
                    await App.Current.MainPage.DisplayAlert("Sucesso", "Área Atualizada", "OK");
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("ERRO", ex.Message, "OK");
                }

            });
        }

        public ICommand Voltar
        {
            get => new Command(() =>
            {
                try
                {
                    Shell.Current.GoToAsync("//CadastroArea");
                }
                catch (Exception ex)
                {
                    App.Current.MainPage.DisplayAlert("ERRO", ex.Message, "OK");
                }

            });
        }



    }
}
