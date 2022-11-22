using System;
using System.Collections.Generic;

namespace WebAPIChamba.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Postulacions = new HashSet<Postulacion>();
        }

        public int IdUsuario { get; set; }
        public string ApodoUsuario { get; set; } = null!;
        public string NombresUsuario { get; set; } = null!;
        public string ApellidosUsuario { get; set; } = null!;
        public DateOnly FNacUsuario { get; set; }
        public string SexoUsuario { get; set; } = null!;
        public string EstudiosUsuario { get; set; } = null!;
        public string CorreoUsuario { get; set; } = null!;
        public string BiografiaUsuario { get; set; } = null!;
        public string FotoPerfilUsuario { get; set; } = null!;

        public virtual ICollection<Postulacion> Postulacions { get; set; }
    }
}
