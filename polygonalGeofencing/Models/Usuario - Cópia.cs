using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace polygonalGeofencing.Models
{
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int idUsuario { get; set; }

        public string nome { get; set; }

        public string email { get; set; }

        public string senha { get; set; }
        

    }
}
