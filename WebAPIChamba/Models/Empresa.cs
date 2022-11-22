using System;
using System.Collections.Generic;

namespace WebAPIChamba.Models
{
    public partial class Empresa
    {
        public Empresa()
        {
            Puestos = new HashSet<Puesto>();
        }

        public int IdEmpresa { get; set; }
        public string ApodoEmpresa { get; set; } = null!;
        public string NombreEmpresa { get; set; } = null!;
        public string RubroEmpresa { get; set; } = null!;
        public DateOnly FFundacion { get; set; }
        public string DireccionEmpresa { get; set; } = null!;
        public string CorreoEmpresa { get; set; } = null!;
        public string BiografiaEmpresa { get; set; } = null!;
        public string FotoPerfilEmpresa { get; set; } = null!;

        public virtual ICollection<Puesto> Puestos { get; set; }
    }
}
