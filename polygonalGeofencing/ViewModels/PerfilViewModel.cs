using polygonalGeofencing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace polygonalGeofencing.ViewModels
{
    public class PerfilViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string nome, sobrenome, email;

        public string Nome //nome da variavel que esta no Binding
        {
            get => nome;
            set
            {
                nome = value; //a variavel nome criada recebera o valor digitado pelo usuario
                PropertyChanged(this, new PropertyChangedEventArgs("Nome"));
            }
        }
        public string Sobrenome //sobrenome da variavel que esta no Binding
        {
            get => sobrenome;
            set
            {
                sobrenome = value; //a variavel nome criada recebera o valor digitado pelo usuario
                PropertyChanged(this, new PropertyChangedEventArgs("Sobrenome"));
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = value;  //a variavel email criada recebera o valor digitado pelo usuario
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }

        public ICommand mostrarPerfil
        {
            get => new Command(async () => {

                try
                {

                    List<Logado> logado = await App.database.GetLogado(); //pega o id do usuário logado
                    //pega as informações do usuário
                    Usuario usuario = await App.database.GetByIdUsuario(logado.ToList()[0].idUsuario);

                    this.Nome = usuario.nome;
                    this.Sobrenome = usuario.sobrenome;
                    this.Email = usuario.email; 
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "ok");
                }


            });
        }

    }
}
