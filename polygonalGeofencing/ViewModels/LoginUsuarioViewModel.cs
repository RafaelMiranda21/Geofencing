using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using polygonalGeofencing.Views;
using System.ComponentModel;
using polygonalGeofencing.Models;
using System.Linq;
using System.Threading.Tasks;

namespace polygonalGeofencing.ViewModels
{
    public class LoginUsuarioViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
  
        string nome ,sobrenome, senha, email,confsenha;

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
        
       


        public ICommand Entrar //recebe o comando do botão entrar
        {
            get => new Command(async () => {
                try
                {
                    List<Usuario> model = await App.database.GetUsuario(this.email, this.senha); //consulta no banco as credenciais informadas pelo usuário
                    if (model.ToList().Count > 0) //caso a lista recebida seja maior que 0, ou seja conseguiu achar informação no banco
                    {
                        await App.database.DelLogado(); //deleta o registro de usuário logado
                        Logado Logado = new Logado() //classe Logado, para registrar qual usuário esta logado,
                                                     //para que na proxima vez que entrar no aplicativo logue direto
                        {
                            idUsuario = model.ToList()[0].idUsuario, //insere o id do usuario que esta logando
                            data = DateTime.Now //adiciona a data atual para que no proximo login seja verificado se a sessão dele expirou
                        };
                        await App.database.InserirLogado(Logado); //insere as informações no banco
                        await Shell.Current.GoToAsync("//Home");

                        //App.Current.MainPage = new AppShell(); //redireciona o usuário para a homePage
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Erro", "Email ou senha invalida", "OK"); //credenciais não encontradas no banco
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "ok"); //caso alguma instrução de erro
                }




            });
        }


        public ICommand NovoUsuario
            {
                get => new Command(async () => {

                    try
                    {

                        Usuario model = new Usuario() //instancia um novo objeto atribuindo os valores digitados
                            {
                                nome = this.nome,
                                sobrenome = this.sobrenome,
                                senha = this.senha,
                                email = this.email,
                                ativo = 1
                            };

                        //se os campos não forem nulos
                        if (nome != null && sobrenome != null && senha != null && email != null && confSenha != null)
                        {
                            //verifica se as senhas são compativeis
                            if(senha == confSenha)
                            {
                                //procura no banco o e-mail digitado para ver se ja existe cadastro com o e-mail
                                List<Usuario> verEmail =  await App.database.verificarEmail(email);                          
                                if (verEmail.Count == 0)//caso o e-mail não esteja cadastrado
                                {
                                    await App.database.CadastroUsuario(model);//insere as informações do usuário no banco
                                    List<Usuario> usuario = await App.database.GetUsuario(email, senha); //pega os dados do usuário cadastrado
                                    int idDono = usuario.ToList()[0].idUsuario; //pega o id do Usuário cadastrado
                                    await App.database.UpdateDono(idDono, idDono); //atualiza os dados do usuário colocando o id dele como dono

                                    //com o sucesso do cadastro do usuário e perguntado a ele se ele ja deseja fazer o login
                                    bool answer = await Application.Current.MainPage.DisplayAlert("Usuário Cadastrado", "Deseja fazer o login", "Sim", "Não");
                                    if (answer) //caso a resposta seja sim
                                    {
                                        await App.database.DelLogado(); //limpa a tabela logado
                                        Logado Logado = new Logado()
                                        {
                                            idUsuario = usuario.ToList()[0].idUsuario, //insere na tabela logado o id do usuário
                                            data = DateTime.Now //coloca o data e a hora do login
                                        };

                                        await App.database.InserirLogado(Logado);

                                        await Shell.Current.GoToAsync("//Home");//usuário e redirecionado para a home page
                                    }
                                }
                                else
                                {
                                    //caso o e-mail já for cadastrado e disparado um alerta para o usuário
                                    await Application.Current.MainPage.DisplayAlert("Falha", "E-mail já cadastrado", "Ok");
                                }
                                
                            }
                            else
                            {
                                //caso as senhas digitadas não forem iguais e disparado um alerta para o usuário
                                await Application.Current.MainPage.DisplayAlert("Falha", "Senhas diferentes", "ok");
                            }                  
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Falha", "Preencha Todos os campos", "ok");
                        }


                    }
                    catch(Exception ex)
                    {
                        //se ocorrer algum erro durante o processo de cadastro e disparado um alerta com o erro
                       await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "ok");
                    }




                });
            }
        
       
    }
}
