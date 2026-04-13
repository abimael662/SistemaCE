using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SistemaCE.Models
{
    public class DocenteMateria
    {

        public int IdDocenteMateria { get; set; }
        public int IdDocente { get; set; }
        public int IdMateria { get; set; }
        public int IdGrupo { get; set; }

        [ValidateNever]
        public virtual Docente? IdDocenteNavigation { get; set; }

        [ValidateNever]
        public virtual Materia? IdMateriaNavigation { get; set; }

        [ValidateNever]
        public virtual Grupo? IdGrupoNavigation { get; set; }
    }
}
