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
    public class CadastroUsuarioViewModel : INotifyPropertyChanged
    {

       public event PropertyChangedEventHandler PropertyChanged;


        public ICommand Voltar
        {
            get => new Command(() =>
            {
                try
                {
                    Shell.Current.GoToAsync("//Home");
                }
                catch (Exception ex)
                {
                    Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "ok");
                }

            });
        }

        string nome, sobrenome, senha, email, confsenha;

        public string Nome //nome da variavel que esta no Binding
        {
            get => nome;
            set
            {
                nome = value; //a variavel nome criada recebera o valor digitado pelo usuario
                PropertyChanged(this, new PropertyChangedEventArgs("nomeUsuario"));
            }
        }
        public string Sobrenome //sobrenome da variavel que esta no Binding
        {
            get => sobrenome;
            set
            {
                sobrenome = value; //a variavel nome criada recebera o valor digitado pelo usuario
                PropertyChanged(this, new PropertyChangedEventArgs("sobrenomeUsuario"));
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = value;  //a variavel email criada recebera o valor digitado pelo usuario
                PropertyChanged(this, new PropertyChangedEventArgs("emailUsuario"));
            }
        }

        public string Senha
        {
            get => senha;
            set
            {
                senha = value; //a variavel senha criada recebera o valor digitado pelo usuario
                PropertyChanged(this, new PropertyChangedEventArgs("senhaUsuario"));
            }
        }
        public string confSenha
        {
            get => confsenha;
            set
            {
                confsenha = value;
                PropertyChanged(this, new PropertyChangedEventArgs("confSenhaUsuario"));
            }
        }

        public ICommand NovoUsuario
        {
            get => new Command(async () => {

                try
                {

                    //A classe usuário recebe as informações do usuário
                    Usuario model = new Usuario()
                    {
                        nome = this.nome,
                        sobrenome = this.sobrenome,
                        senha = this.senha,
                        email = this.email,
                        ativo = 1
                    };

                    //verifica se os campos foram preenchidos
                    if (nome != null && sobrenome != null && senha != null && email != null && confSenha != null)
                    {
                        //verifica se as senhas são iguais
                        if (senha == confSenha)
                        {
                            //verifica se o e-mail informado ja esta cadastrado
                            List<Usuario> verEmail = await App.database.verificarEmail(email);
                            if (verEmail.Count == 0)
                            {
                                //insere as informações do usuario no banco
                                await App.database.CadastroUsuario(model);

                                //pega o id do dono para inserir na informação do usuário
                                List<Logado> logado = await App.database.GetLogado();
                                Usuario idDono = await App.database.GetByIdUsuario(logado.ToList()[0].idUsuario);
                                List<Usuario> idUsuario = await App.database.GetUsuario(email,senha);
                                //adiciona o id do proprietario no registro do usuário
                                await App.database.UpdateDono(idDono.idDono, idUsuario.ToList()[0].idUsuario);

                                await App.Current.MainPage.DisplayAlert("Sucesso", "Funcionário Cadastrado", "OK");
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Falha", "E-mail já cadastrado", "Ok");
                            }

                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Falha", "Senhas diferentes", "ok");
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Falha", "Preencha Todos os campos", "ok");
                    }


                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "ok");
                }


            });
        }


    }
}
