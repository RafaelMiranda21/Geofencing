using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace polygonalGeofencing.Models
{
    public class Logado
    {
        [PrimaryKey,AutoIncrement] //determina que a variável a baixo será AutoIncrement

        public int idLogado { get; set; } //identificador Logado

        public int idUsuario { get; set; } //identificador do Usuário

        public DateTime data { get; set; } //variável data

    }
}
