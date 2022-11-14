using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace polygonalGeofencing.Models
{
    public class Acesso
    {
        [PrimaryKey, AutoIncrement] //determina que a variavel a baixo será auto increment
        public int idAcesso { get; set; }  //identificador unico do Acesso

        public int idUsuario { get; set; } // identificador do usuario

        // 1 - Criar Area 2 - Add Funcionario 3 - Add Permissão 
        public int idTipo { get; set; } // identificador do tipo de permisão

        public int idFazenda { get; set; } // identificador da Fazenda em que tem acesso

    }
}
