using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace polygonalGeofencing.Models
{
    public class Usuario
    {
        [PrimaryKey, AutoIncrement] //determina que a variável a baixo será AutoIncrement
        public int idUsuario { get; set; } //identificador do Usuário

        public int idDono { get; set; } //identificador do Dono da Fazenda

        public int ativo { get; set; } //informa se o usuário esta ativo ou não no sistema

        public string nome { get; set; } //Nome do usuário

        public string sobrenome { get; set; } //Sobrenome do usuário 

        public string email { get; set; } //email do usuário

        public string senha { get; set; } //senha do usuário
      
    }
}
