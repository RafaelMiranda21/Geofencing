using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using polygonalGeofencing.Models;
using System.Threading.Tasks;

namespace polygonalGeofencing.Helpers
{
    public class SQLiteDataBaseHelper
    {
        readonly SQLiteAsyncConnection _db;


        public SQLiteDataBaseHelper(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<Acesso>().Wait();
            _db.CreateTableAsync<Area>().Wait();
            _db.CreateTableAsync<Fazenda>().Wait();
            _db.CreateTableAsync<Logado>().Wait();
            _db.CreateTableAsync<Pontos>().Wait();
            _db.CreateTableAsync<Usuario>().Wait();
            
            
 
        }



        /************* Comandos Acesso ***************/

        //Pega todos acessos do usuário do usuário em uma fazenda especifica ou em qualquer uma
        public Task<List<Acesso>> GetAcessoUsuario(int idUsuario,int idFazenda)
        {
            string sql = "";
            //se for passado um id valido e selecionado os acessos daquela fazenda
            if (idFazenda >= 0)
            {
                sql = "SELECT * from Acesso where idUsuario = " + idUsuario + " AND idFazenda = " + idFazenda;
            }
            else // se não seleciona todos acessos
            {
                sql = "SELECT * from Acesso where idUsuario = " + idUsuario;
            }
            
            return _db.QueryAsync<Acesso>(sql);
        }

        //Insere o acesso do usuário
        public Task<List<Acesso>> InsertAcesso(int idUsuario, int idTipo, int idFazenda)
        {
            string sql = "INSERT INTO Acesso (idUsuario, idTipo, idFazenda) VALUES ('" + idUsuario + "', '" + idTipo + "', '" + idFazenda + "')";
            return _db.QueryAsync<Acesso>(sql);
        }

        //Remove o acesso do usuário
        public Task<List<Acesso>> DeleteAcessoUsuario(int idUsuario,int idTipo,int idFazenda)
        {
            string sql = "DELETE from Acesso where idUsuario = '" + idUsuario + "' AND idTipo = '" + idTipo + "' AND idFazenda = '" + idFazenda + "'";
            return _db.QueryAsync<Acesso>(sql);
        }
        
        //Remove todos acessos do banco de dados local
        public Task<List<Acesso>> DelAllAcessos()
        {
            string sql = "DELETE from Acesso";
            return _db.QueryAsync<Acesso>(sql);
        }

        /*********************************************/




        /************* Comandos Usuários *************/
        
        //Seleciona o usuário a partir do id
        public Task<Usuario> GetByIdUsuario(int id)
        {
            return _db.Table<Usuario>().FirstAsync(i => i.idUsuario == id);
        }

        //Seleciona o Usuário a partir do e-mail e da senha
        public Task<List<Usuario>> GetUsuario(string email, string senha)
        {
            string sql = "SELECT * from Usuario where email = '" + email + "' and senha = '" + senha + "' AND ativo = 1";
            return _db.QueryAsync<Usuario>(sql);
        }

        //Seleciona o usuário a partir do e-mail
        public Task<List<Usuario>> verificarEmail(string email)
        {
            string sql = "SELECT * from Usuario where email = '" + email + "' AND ativo = 1";
            return _db.QueryAsync<Usuario>(sql);
        }

        //Seleciona os funcionarios da fazenda
        public Task<List<Usuario>> GetFuncionariosFazenda(string idFazenda,int idLogado, int idDono)
        {
            string sql = "";
            if (idDono == idLogado)
            {
                sql = "SELECT * from Usuario where idDono = " + idDono + " AND idDono <> idUsuario AND ativo = 1";
            }
            else
            {
                if (idFazenda.Contains(","))
                {
                    idFazenda.Replace(",", "','"); //caso seja 10,20,30 -> '10','20','30'
                    sql = " select distinct u.idUsuario,u.nome,u.sobrenome from usuario u " +
                          " left outer join acesso a on a.idUsuario = u.idUsuario " +
                          " where(a.idFazenda in '" + idFazenda + "' or a.idfazenda is null) and u.ativo = 1 and and u.idDono = " + idDono + " u.idUsuario not in (" + idLogado + "," + idDono + ") ";
                }
                else
                {

                    sql = " select distinct u.idUsuario,u.nome,u.sobrenome from usuario u " +
                          " left outer join acesso a on a.idUsuario = u.idUsuario " +
                          " where(a.idFazenda = " + idFazenda + " or a.idfazenda is null) and u.ativo = 1 and u.idDono = " + idDono + " and u.idUsuario not in (" + idLogado  + "," + idDono + ") ";
                }
            }
                
         
            return _db.QueryAsync<Usuario>(sql);

        }

        //Deleta todos usuários
        public Task<List<Usuario>> DelAllUsuarios()
        {
            string sql = "DELETE FROM Usuario";
            return _db.QueryAsync<Usuario>(sql);
        }

        //Insere o usuário no banco de dados local
        public Task<int> CadastroUsuario(Usuario model)
        {
            return _db.InsertAsync(model);
        }

        //Atualiza informações do usuário
        public Task<List<Usuario>> UpdateDono(int idDono,int idUsuario)
        {
            string sql = "UPDATE Usuario SET idDono = " + idDono + " WHERE idUsuario = " + idUsuario;
            return _db.QueryAsync<Usuario>(sql);
        }
        
        //Atualiza o status de ativo do usuário
        public Task<List<Usuario>> ExcluirUsuario(int idUsuario)
        {
            string sql = "UPDATE Usuario SET ativo = 0 WHERE idUsuario = " + idUsuario;
            return _db.QueryAsync<Usuario>(sql);
        }

        /********************************************/



        /************* Comandos Usuário Logado *************/

        //Seleciona o usuário logado
        public Task<List<Logado>> GetLogado()
        {
            string sql = "SELECT * from Logado";
            return _db.QueryAsync<Logado>(sql);
        }

        //Deleta o usuário logado
        public Task<List<Logado>> DelLogado()
        {
            string sql = "DELETE FROM Logado";
            return _db.QueryAsync<Logado>(sql);
        }

        //Insere o usuário logado
        public Task<int> InserirLogado(Logado model)
        {
            return _db.InsertAsync(model);
        }

        /********************************************/



        /************* Comandos Fazenda *************/

        //Seleciona a fazenda especificada
        public Task<Fazenda> GetByIdFazenda(int id)
        {
            return _db.Table<Fazenda>().FirstAsync(i => i.idFazenda == id);
        }
        
        //Seleciona a fazenda a partir do nome
        public Task<List<Fazenda>> GetByNomeFazenda(string nome,int idDono)
        {
            string sql = "SELECT * FROM Fazenda WHERE nome like '%"+ nome + "%' and idUsuario = " + idDono;
            return _db.QueryAsync<Fazenda>(sql);
        }

        //Deleta todas fazendas do banco de dados
        public Task<List<Fazenda>> DelFazenda()
        {
            string sql = "DELETE FROM Fazenda";
            return _db.QueryAsync<Fazenda>(sql);
        }

        public Task<List<Fazenda>> GetFazendaUsuario(int idDono)
        {
            string sql = "SELECT * from Fazenda where idUsuario = " + idDono;
            return _db.QueryAsync<Fazenda>(sql);
        }
        
        //Pegar as fazenda que o Funcionario tem acesso
        public Task<List<Fazenda>> GetFazendaFuncionario(int idFuncionario,int idDono)
        {
            string sql = "";
            if (idFuncionario == idDono)
            {
                sql = " SELECT distinct(f.idFazenda), f.nome from Fazenda f " +
                      " WHERE f.idUsuario = " + idDono;
            }
            else{
                 sql = " SELECT distinct(f.idFazenda), f.nome from Fazenda f " +
                    " INNER JOIN Acesso a ON a.idFazenda = f.idFazenda " +
                    " WHERE a.idUsuario = " + idFuncionario;
            }
            
            return _db.QueryAsync<Fazenda>(sql);
        }

        //Inserir fazenda
        public Task<int> InsertFazenda(Fazenda model)
        {
            return _db.InsertAsync(model);
        }

        //Deletar fazenda 
        public Task<int> DeleteFazenda(int idFazenda)
        {
            return _db.Table<Fazenda>().DeleteAsync(i => i.idFazenda == idFazenda);
        }


        /********************************************/




        /************** Comandos Pontos *************/

        //Deleta todos os pontos
        public Task<List<Pontos>> DelPontos()
        {
            string sql = "DELETE FROM Pontos";
            return _db.QueryAsync<Pontos>(sql);
        }
        
        //Deleta todos pontos da área
        public Task<List<Pontos>> DellPontosArea(int idArea)
        {
            string sql = "DELETE FROM Pontos WHERE idArea = " + idArea;
            return _db.QueryAsync<Pontos>(sql);
        }

        //Seleciona todos os pontos da área
        public Task<List<Pontos>> GetPontosArea(int idArea)
        {
            string sql = "SELECT * from Pontos WHERE idArea = " + idArea;
            return _db.QueryAsync<Pontos>(sql);

        }

        //Insere os pontos
        public Task<int> InsertPonto(Pontos model)
        {
            return _db.InsertAsync(model);
        }

        /********************************************/


        /************** Comandos Área **************/
        
        //Pega todas as áreas da fazenda
        public Task<List<Area>> GetAllRowsArea(int idFazenda)
        {
            string sql = "SELECT * from Area where idFazenda = " + idFazenda;
            return _db.QueryAsync<Area>(sql);
        }

        //Atualiza as informações da área
        public Task<List<Area>> UpdateArea(int idArea,string desc)
        {
            string sql = "UPDATE Area SET descricao = '" + desc + "' WHERE idArea = " + idArea;
            return _db.QueryAsync<Area>(sql);
        }

        //Remove todas as áreas do banco de dados
        public Task<List<Area>> DelArea()
        {
            string sql = "DELETE FROM Area";
            return _db.QueryAsync<Area>(sql);
        }

        //Pega todas as áreas de um usuário
        public Task<List<Area>> GetAreasUsuario(int id)
        {
            string sql = "SELECT a.* from Area a " +
                " Inner Join Fazenda f on f.idFazenda = a.idFazenda  where f.idUsuario = " + id + "  ";
            return _db.QueryAsync<Area>(sql);
        }

        //Pega todas as áreas das fazemdas
        public Task<List<Area>> GetAreasFazenda(string idFazenda)
        {
            string sql = "";
            if (idFazenda.Contains(","))
            {

                idFazenda.Replace(",", "','"); //caso seja 10,20,30 -> '10','20','30'
                sql = " SELECT * from Area a " +
                      " Inner Join Fazenda f on f.idFazenda = a.idFazenda  " +
                      " where a.idFazenda IN ('" + idFazenda + "')";
            }
            else
            {
                sql = " SELECT * from Area a " +
                      " Inner Join Fazenda f on f.idFazenda = a.idFazenda  " +
                      " where a.idFazenda = " +  Int32.Parse(idFazenda);
            }
            
            return _db.QueryAsync<Area>(sql);
        }


        //Seleciona a área a partir do id
        public Task<Area> GetByIdArea(int id)
        {

            return _db.Table<Area>().FirstAsync(i => i.idArea == id);
        }


        // Insere a área
        public Task<int> InsertArea(Area model)
        {
            return _db.InsertAsync(model);
        }

        //Remove a área
        public Task<int> DeleteArea(int idArea)
        {
            return _db.Table<Area>().DeleteAsync(i => i.idArea == idArea);
        }

        /********************************************/
    }
}
