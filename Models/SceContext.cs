using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SistemaCE.Models;

public partial class SceContext : DbContext
{
    public SceContext()
    {
    }

    public SceContext(DbContextOptions<SceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrativo> Administrativos { get; set; }

    public virtual DbSet<Asistencium> Asistencia { get; set; }

    public virtual DbSet<CalificacionAlumno> CalificacionAlumnos { get; set; }

    public virtual DbSet<Carrera> Carreras { get; set; }

    public virtual DbSet<Certificado> Certificados { get; set; }

    public virtual DbSet<ContactoPersona> ContactoPersonas { get; set; }

    public virtual DbSet<Division> Divisions { get; set; }

    public virtual DbSet<Docente> Docentes { get; set; }

    public virtual DbSet<DocenteMateriaGrupo> DocenteMateriaGrupos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Especialidad> Especialidads { get; set; }

    public virtual DbSet<EstadoAsistencium> EstadoAsistencia { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Grupo> Grupos { get; set; }

    public virtual DbSet<GrupoBase> GrupoBases { get; set; }

    public virtual DbSet<Inscripcion> Inscripcions { get; set; }

    public virtual DbSet<Materia> Materias { get; set; }

    public virtual DbSet<Nivel> Nivels { get; set; }

    public virtual DbSet<Periodo> Periodos { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<PersonaUsuario> PersonaUsuarios { get; set; }

    public virtual DbSet<SesionClase> SesionClases { get; set; }

    public virtual DbSet<TipoComunicacion> TipoComunicacions { get; set; }

    public virtual DbSet<Titulacion> Titulacions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=sce;Username=postgres;Password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrativo>(entity =>
        {
            entity.HasKey(e => e.IdAdministrativo).HasName("administrativo_pkey");

            entity.ToTable("administrativo");

            entity.HasIndex(e => e.NumeroEmpleado, "administrativo_numero_empleado_key").IsUnique();

            entity.Property(e => e.IdAdministrativo)
                .ValueGeneratedNever()
                .HasColumnName("id_administrativo");
            entity.Property(e => e.Departamento).HasColumnName("departamento");
            entity.Property(e => e.NumeroEmpleado).HasColumnName("numero_empleado");
            entity.Property(e => e.Puesto).HasColumnName("puesto");
            entity.Property(e => e.Rfc)
                .HasMaxLength(15)
                .HasColumnName("rfc");
            entity.Property(e => e.Sueldo)
                .HasPrecision(10, 2)
                .HasColumnName("sueldo");

            entity.HasOne(d => d.IdAdministrativoNavigation).WithOne(p => p.Administrativo)
                .HasForeignKey<Administrativo>(d => d.IdAdministrativo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("administrativo_id_administrativo_fkey");

            entity.HasOne(d => d.NumeroEmpleadoNavigation).WithOne(p => p.Administrativo)
                .HasForeignKey<Administrativo>(d => d.NumeroEmpleado)
                .HasConstraintName("administrativo_numero_empleado_fkey");
        });

        modelBuilder.Entity<Asistencium>(entity =>
        {
            entity.HasKey(e => e.IdAsistencia).HasName("asistencia_pkey");

            entity.ToTable("asistencia");

            entity.HasIndex(e => new { e.IdSesion, e.IdEstudiante }, "asistencia_id_sesion_id_estudiante_key").IsUnique();

            entity.Property(e => e.IdAsistencia).HasColumnName("id_asistencia");
            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.IdSesion).HasColumnName("id_sesion");
            entity.Property(e => e.Observacion).HasColumnName("observacion");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Asistencia)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("asistencia_id_estado_fkey");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.Asistencia)
                .HasForeignKey(d => d.IdEstudiante)
                .HasConstraintName("asistencia_id_estudiante_fkey");

            entity.HasOne(d => d.IdSesionNavigation).WithMany(p => p.Asistencia)
                .HasForeignKey(d => d.IdSesion)
                .HasConstraintName("asistencia_id_sesion_fkey");
        });

        modelBuilder.Entity<CalificacionAlumno>(entity =>
        {
            entity.HasKey(e => e.IdCalificacion).HasName("calificacion_alumno_pkey");

            entity.ToTable("calificacion_alumno");

            entity.HasIndex(e => new { e.IdEstudiante, e.IdMateria, e.IdGrupo }, "uq_estudiante_materia_grupo").IsUnique();

            entity.Property(e => e.IdCalificacion).HasColumnName("id_calificacion");
            entity.Property(e => e.IdDocente).HasColumnName("id_docente");
            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.IdGrupo).HasColumnName("id_grupo");
            entity.Property(e => e.IdMateria).HasColumnName("id_materia");
            entity.Property(e => e.Parcial1)
                .HasPrecision(5, 2)
                .HasColumnName("parcial_1");
            entity.Property(e => e.Parcial2)
                .HasPrecision(5, 2)
                .HasColumnName("parcial_2");
            entity.Property(e => e.Parcial3)
                .HasPrecision(5, 2)
                .HasColumnName("parcial_3");
            entity.Property(e => e.PromedioFinal)
                .HasPrecision(5, 2)
                .HasColumnName("promedio_final");

            entity.HasOne(d => d.IdDocenteNavigation).WithMany(p => p.CalificacionAlumnos)
                .HasForeignKey(d => d.IdDocente)
                .HasConstraintName("calificacion_alumno_id_docente_fkey");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.CalificacionAlumnos)
                .HasForeignKey(d => d.IdEstudiante)
                .HasConstraintName("calificacion_alumno_id_estudiante_fkey");

            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.CalificacionAlumnos)
                .HasForeignKey(d => d.IdGrupo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("calificacion_alumno_id_grupo_fkey");

            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.CalificacionAlumnos)
                .HasForeignKey(d => d.IdMateria)
                .HasConstraintName("calificacion_alumno_id_materia_fkey");
        });

        modelBuilder.Entity<Carrera>(entity =>
        {
            entity.HasKey(e => e.IdCarrera).HasName("carrera_pkey");

            entity.ToTable("carrera");

            entity.Property(e => e.IdCarrera).HasColumnName("id_carrera");
            entity.Property(e => e.IdDivision).HasColumnName("id_division");
            entity.Property(e => e.IdNivel).HasColumnName("id_nivel");
            entity.Property(e => e.Nombre).HasColumnName("nombre");

            entity.HasOne(d => d.IdDivisionNavigation).WithMany(p => p.Carreras)
                .HasForeignKey(d => d.IdDivision)
                .HasConstraintName("carrera_id_division_fkey");

            entity.HasOne(d => d.IdNivelNavigation).WithMany(p => p.Carreras)
                .HasForeignKey(d => d.IdNivel)
                .HasConstraintName("carrera_id_nivel_fkey");
        });

        modelBuilder.Entity<Certificado>(entity =>
        {
            entity.HasKey(e => e.IdCertificado).HasName("certificado_pkey");

            entity.ToTable("certificado");

            entity.Property(e => e.IdCertificado).HasColumnName("id_certificado");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.FechaEmision).HasColumnName("fecha_emision");
            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.IdGrupo).HasColumnName("id_grupo");
            entity.Property(e => e.TipoCertificado).HasColumnName("tipo_certificado");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.Certificados)
                .HasForeignKey(d => d.IdEstudiante)
                .HasConstraintName("certificado_id_estudiante_fkey");

            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.Certificados)
                .HasForeignKey(d => d.IdGrupo)
                .HasConstraintName("certificado_id_grupo_fkey");
        });

        modelBuilder.Entity<ContactoPersona>(entity =>
        {
            entity.HasKey(e => e.IdContacto).HasName("contacto_persona_pkey");

            entity.ToTable("contacto_persona");

            entity.Property(e => e.IdContacto).HasColumnName("id_contacto");
            entity.Property(e => e.Dato)
                .HasMaxLength(50)
                .HasColumnName("dato");
            entity.Property(e => e.IdPersona).HasColumnName("id_persona");
            entity.Property(e => e.TipoComunicacion).HasColumnName("tipo_comunicacion");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.ContactoPersonas)
                .HasForeignKey(d => d.IdPersona)
                .HasConstraintName("contacto_persona_id_persona_fkey");

            entity.HasOne(d => d.TipoComunicacionNavigation).WithMany(p => p.ContactoPersonas)
                .HasForeignKey(d => d.TipoComunicacion)
                .HasConstraintName("contacto_persona_tipo_comunicacion_fkey");
        });

        modelBuilder.Entity<Division>(entity =>
        {
            entity.HasKey(e => e.IdDivision).HasName("division_pkey");

            entity.ToTable("division");

            entity.Property(e => e.IdDivision).HasColumnName("id_division");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.Nomenclatura)
                .HasMaxLength(10)
                .HasColumnName("nomenclatura");
        });

        modelBuilder.Entity<Docente>(entity =>
        {
            entity.HasKey(e => e.IdDocente).HasName("docente_pkey");

            entity.ToTable("docente");

            entity.HasIndex(e => e.NumeroEmpleado, "docente_numero_empleado_key").IsUnique();

            entity.Property(e => e.IdDocente)
                .ValueGeneratedNever()
                .HasColumnName("id_docente");
            entity.Property(e => e.CedulaProfesional)
                .HasMaxLength(20)
                .HasColumnName("cedula_profesional");
            entity.Property(e => e.NumeroEmpleado).HasColumnName("numero_empleado");
            entity.Property(e => e.Rfc)
                .HasMaxLength(15)
                .HasColumnName("rfc");
            entity.Property(e => e.Sueldo)
                .HasPrecision(10, 2)
                .HasColumnName("sueldo");

            entity.HasOne(d => d.IdDocenteNavigation).WithOne(p => p.Docente)
                .HasForeignKey<Docente>(d => d.IdDocente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("docente_id_docente_fkey");

            entity.HasOne(d => d.NumeroEmpleadoNavigation).WithOne(p => p.Docente)
                .HasForeignKey<Docente>(d => d.NumeroEmpleado)
                .HasConstraintName("docente_numero_empleado_fkey");

            entity.HasMany(d => d.IdEspecialidads).WithMany(p => p.IdDocentes)
                .UsingEntity<Dictionary<string, object>>(
                    "DocenteEspecialidad",
                    r => r.HasOne<Especialidad>().WithMany()
                        .HasForeignKey("IdEspecialidad")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("docente_especialidad_id_especialidad_fkey"),
                    l => l.HasOne<Docente>().WithMany()
                        .HasForeignKey("IdDocente")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("docente_especialidad_id_docente_fkey"),
                    j =>
                    {
                        j.HasKey("IdDocente", "IdEspecialidad").HasName("docente_especialidad_pkey");
                        j.ToTable("docente_especialidad");
                        j.IndexerProperty<int>("IdDocente").HasColumnName("id_docente");
                        j.IndexerProperty<int>("IdEspecialidad").HasColumnName("id_especialidad");
                    });
        });

        modelBuilder.Entity<DocenteMateriaGrupo>(entity =>
        {
            entity.HasKey(e => e.IdDocenteMateriaGrupo).HasName("docente_materia_grupo_pkey");

            entity.ToTable("docente_materia_grupo");

            entity.HasIndex(e => new { e.IdDocente, e.IdMateria, e.IdGrupo }, "docente_materia_grupo_id_docente_id_materia_id_grupo_key").IsUnique();

            entity.Property(e => e.IdDocenteMateriaGrupo).HasColumnName("id_docente_materia_grupo");
            entity.Property(e => e.IdDocente).HasColumnName("id_docente");
            entity.Property(e => e.IdGrupo).HasColumnName("id_grupo");
            entity.Property(e => e.IdMateria).HasColumnName("id_materia");

            entity.HasOne(d => d.IdDocenteNavigation).WithMany(p => p.DocenteMateriaGrupos)
                .HasForeignKey(d => d.IdDocente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("docente_materia_grupo_id_docente_fkey");

            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.DocenteMateriaGrupos)
                .HasForeignKey(d => d.IdGrupo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("docente_materia_grupo_id_grupo_fkey");

            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.DocenteMateriaGrupos)
                .HasForeignKey(d => d.IdMateria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("docente_materia_grupo_id_materia_fkey");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("empleado_pkey");

            entity.ToTable("empleado");

            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.TipoEmpleado)
                .HasMaxLength(10)
                .HasColumnName("tipo_empleado");
        });

        modelBuilder.Entity<Especialidad>(entity =>
        {
            entity.HasKey(e => e.IdEspecialidad).HasName("especialidad_pkey");

            entity.ToTable("especialidad");

            entity.Property(e => e.IdEspecialidad).HasColumnName("id_especialidad");
            entity.Property(e => e.Especialidad1).HasColumnName("especialidad");
        });

        modelBuilder.Entity<EstadoAsistencium>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("estado_asistencia_pkey");

            entity.ToTable("estado_asistencia");

            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.IdEstudiante).HasName("estudiante_pkey");

            entity.ToTable("estudiante");

            entity.Property(e => e.IdEstudiante)
                .ValueGeneratedNever()
                .HasColumnName("id_estudiante");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.FechaIngreso).HasColumnName("fecha_ingreso");
            entity.Property(e => e.IdGrupo).HasColumnName("id_grupo");
            entity.Property(e => e.Matricula).HasColumnName("matricula");

            entity.HasOne(d => d.IdEstudianteNavigation).WithOne(p => p.Estudiante)
                .HasForeignKey<Estudiante>(d => d.IdEstudiante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("estudiante_id_estudiante_fkey");

            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.Estudiantes)
                .HasForeignKey(d => d.IdGrupo)
                .HasConstraintName("estudiante_id_grupo_fkey");
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.IdGrupo).HasName("grupo_pkey");

            entity.ToTable("grupo");

            entity.HasIndex(e => new { e.IdGrupoBase, e.IdPeriodo }, "uq_grupo_base_periodo").IsUnique();

            entity.Property(e => e.IdGrupo).HasColumnName("id_grupo");
            entity.Property(e => e.IdCarrera).HasColumnName("id_carrera");
            entity.Property(e => e.IdGrupoBase).HasColumnName("id_grupo_base");
            entity.Property(e => e.IdPeriodo).HasColumnName("id_periodo");
            entity.Property(e => e.Turno)
                .HasMaxLength(10)
                .HasColumnName("turno");

            entity.HasOne(d => d.IdCarreraNavigation).WithMany(p => p.Grupos)
                .HasForeignKey(d => d.IdCarrera)
                .HasConstraintName("grupo_id_carrera_fkey");

            entity.HasOne(d => d.IdGrupoBaseNavigation).WithMany(p => p.Grupos)
                .HasForeignKey(d => d.IdGrupoBase)
                .HasConstraintName("grupo_id_grupo_base_fkey");

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.Grupos)
                .HasForeignKey(d => d.IdPeriodo)
                .HasConstraintName("grupo_id_periodo_fkey");
        });

        modelBuilder.Entity<GrupoBase>(entity =>
        {
            entity.HasKey(e => e.IdGrupoBase).HasName("grupo_base_pkey");

            entity.ToTable("grupo_base");

            entity.Property(e => e.IdGrupoBase).HasColumnName("id_grupo_base");
            entity.Property(e => e.Nombre)
                .HasMaxLength(10)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Inscripcion>(entity =>
        {
            entity.HasKey(e => e.IdInscripcion).HasName("inscripcion_pkey");

            entity.ToTable("inscripcion");

            entity.Property(e => e.IdInscripcion).HasColumnName("id_inscripcion");
            entity.Property(e => e.FechaInscripcion).HasColumnName("fecha_inscripcion");
            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.IdGrupo).HasColumnName("id_grupo");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.Inscripcions)
                .HasForeignKey(d => d.IdEstudiante)
                .HasConstraintName("inscripcion_id_estudiante_fkey");

            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.Inscripcions)
                .HasForeignKey(d => d.IdGrupo)
                .HasConstraintName("inscripcion_id_grupo_fkey");
        });

        modelBuilder.Entity<Materia>(entity =>
        {
            entity.HasKey(e => e.IdMateria).HasName("materias_pkey");

            entity.ToTable("materias");

            entity.Property(e => e.IdMateria).HasColumnName("id_materia");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Nivel>(entity =>
        {
            entity.HasKey(e => e.IdNivel).HasName("nivel_pkey");

            entity.ToTable("nivel");

            entity.Property(e => e.IdNivel).HasColumnName("id_nivel");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .HasColumnName("nombre");
            entity.Property(e => e.Nomenclatura)
                .HasMaxLength(20)
                .HasColumnName("nomenclatura");
        });

        modelBuilder.Entity<Periodo>(entity =>
        {
            entity.HasKey(e => e.IdPeriodo).HasName("periodo_pkey");

            entity.ToTable("periodo");

            entity.Property(e => e.IdPeriodo).HasColumnName("id_periodo");
            entity.Property(e => e.Anio).HasColumnName("anio");
            entity.Property(e => e.Periodo1)
                .HasMaxLength(50)
                .HasColumnName("periodo");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("persona_pkey");

            entity.ToTable("persona");

            entity.Property(e => e.IdPersona).HasColumnName("id_persona");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(20)
                .HasColumnName("apellido_materno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(20)
                .HasColumnName("apellido_paterno");
            entity.Property(e => e.Curp)
                .HasMaxLength(20)
                .HasColumnName("curp");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<PersonaUsuario>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("persona_usuario_pkey");

            entity.ToTable("persona_usuario");

            entity.Property(e => e.IdPersona)
                .ValueGeneratedNever()
                .HasColumnName("id_persona");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Usuario)
                .HasMaxLength(20)
                .HasColumnName("usuario");

            entity.HasOne(d => d.IdPersonaNavigation).WithOne(p => p.PersonaUsuario)
                .HasForeignKey<PersonaUsuario>(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("persona_usuario_id_persona_fkey");
        });

        modelBuilder.Entity<SesionClase>(entity =>
        {
            entity.HasKey(e => e.IdSesion).HasName("sesion_clase_pkey");

            entity.ToTable("sesion_clase");

            entity.Property(e => e.IdSesion).HasColumnName("id_sesion");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.HoraFin).HasColumnName("hora_fin");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.IdDocente).HasColumnName("id_docente");
            entity.Property(e => e.IdGrupo).HasColumnName("id_grupo");
            entity.Property(e => e.IdMateria).HasColumnName("id_materia");

            entity.HasOne(d => d.IdDocenteNavigation).WithMany(p => p.SesionClases)
                .HasForeignKey(d => d.IdDocente)
                .HasConstraintName("sesion_clase_id_docente_fkey");

            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.SesionClases)
                .HasForeignKey(d => d.IdGrupo)
                .HasConstraintName("sesion_clase_id_grupo_fkey");

            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.SesionClases)
                .HasForeignKey(d => d.IdMateria)
                .HasConstraintName("sesion_clase_id_materia_fkey");
        });

        modelBuilder.Entity<TipoComunicacion>(entity =>
        {
            entity.HasKey(e => e.IdTipoComunicacion).HasName("tipo_comunicacion_pkey");

            entity.ToTable("tipo_comunicacion");

            entity.Property(e => e.IdTipoComunicacion).HasColumnName("id_tipo_comunicacion");
            entity.Property(e => e.Comunicacion)
                .HasMaxLength(20)
                .HasColumnName("comunicacion");
        });

        modelBuilder.Entity<Titulacion>(entity =>
        {
            entity.HasKey(e => e.IdTitulacion).HasName("titulacion_pkey");

            entity.ToTable("titulacion");

            entity.Property(e => e.IdTitulacion).HasColumnName("id_titulacion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaAprobacion).HasColumnName("fecha_aprobacion");
            entity.Property(e => e.FechaExpedicion).HasColumnName("fecha_expedicion");
            entity.Property(e => e.FechaSolicitud).HasColumnName("fecha_solicitud");
            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.IdNivel).HasColumnName("id_nivel");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.Titulacions)
                .HasForeignKey(d => d.IdEstudiante)
                .HasConstraintName("titulacion_id_estudiante_fkey");

            entity.HasOne(d => d.IdNivelNavigation).WithMany(p => p.Titulacions)
                .HasForeignKey(d => d.IdNivel)
                .HasConstraintName("titulacion_id_nivel_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
