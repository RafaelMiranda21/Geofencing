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

    [QueryProperty(nameof(PegarId), "parametro_id")]
    public class ControleAcesso : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public string PegarId
        {
            set
            {
                int id_parametro = Convert.ToInt32(Uri.UnescapeDataString(value));
                mostrarFuncionario.Execute(id_parametro);
            }
        }



        string funcionario, nomeFazenda;
        int idfuncionario;
        public string Funcionario
        {
            get => funcionario;
            set
            {
                funcionario = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Funcionario"));
            }
        }

        public int idFuncionario
        {
            get => idfuncionario;
            set
            {
                idfuncionario = value;
                PropertyChanged(this, new PropertyChangedEventArgs("idFuncionario"));
            }
        }


        public ICommand mostrarFuncionario
        {
            get
            {
                return new Command<int>(async (usuarioid) =>
                {
                    try
                    {

                        Usuario funcionario = await App.database.GetByIdUsuario(usuarioid);

                        this.Funcionario = funcionario.nome;
                        this.idFuncionario = funcionario.idUsuario;
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
                        Shell.Current.GoToAsync("//ListaFuncionario");

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
