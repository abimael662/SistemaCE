namespace SistemaCE.Models
{
    public class DocenteMateria
    {

        public int IdDocenteMateria { get; set; }
        public int IdDocente { get; set; }
        public int IdMateria { get; set; }
        public int IdGrupo { get; set; }

        public virtual Docente? IdDocenteNavigation { get; set; }

        public virtual Materia? IdMateriaNavigation { get; set; }

        public virtual Grupo? IdGrupoNavigation { get; set; }
    }
}
