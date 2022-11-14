using polygonalGeofencing.Models;
using polygonalGeofencing.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace polygonalGeofencing.ViewModels
{
    [QueryProperty(nameof(PegarIdNavegacao), "parametro_id")]
     class CadastroPontosViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string txtArea;
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


        public string nomeArea
        {
            get => txtArea;
            set
            {
                txtArea = value;
                PropertyChanged(this, new PropertyChangedEventArgs("nomeArea"));
            }
        }


        public string PegarIdNavegacao
        {
            set
            {
                int id_parametro = Convert.ToInt32(Uri.UnescapeDataString(value));
                //chama a função mostrar área passando o id da área como parametro
                mostrarArea.Execute(id_parametro);
            }
        }





        public ICommand mostrarArea
        {
            get => new Command<int>(async (int id) =>
            {
                try
                {
                    //pega as informações da área no banco de dados
                    Area model = await App.database.GetByIdArea(id);
                    //mostra na tela o nome da área 
                    this.nomeArea = "Geofencing " + model.nome;
                    this.idArea = model.idArea;
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "ok");
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
                    Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "ok");
                }

            });
        }

        public ICommand cadastrarPonto
        {
            get => new Command<string>(async (string posicao) => {
                try
                {
                    //pega a string recebida por parametro e separa a latitude e longitude
                    string latitude = posicao.Split(',')[0];
                    string longitude = posicao.Split(',')[1];

                    //insere as informações na classe
                    Pontos model = new Pontos()
                    {
                        idArea = this.idArea,
                        // Utiliza-se o CultureInfo para ponto seja considerar o . como separador decimal
                        latitude = Convert.ToDouble(latitude, new CultureInfo("en-US")), 
                        longitude = Convert.ToDouble(longitude, new CultureInfo("en-US"))
                    };
                    //insere a informação no banco
                    await App.database.InsertPonto(model);

                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "ok");
                }
            });
        }
        
       
    }
}
