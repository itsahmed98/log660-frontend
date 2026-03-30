using log660_lab2_serveur.Models;
using Microsoft.EntityFrameworkCore;

namespace log660_lab2_serveur.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Utilisateurs
        public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Employe> Employes => Set<Employe>();
        public DbSet<Adresse> Adresses => Set<Adresse>();

        // Paiement / Forfait
        public DbSet<Forfait> Forfaits => Set<Forfait>();
        public DbSet<TypeCarte> TypesCarte => Set<TypeCarte>();
        public DbSet<CarteCredit> CartesCredit => Set<CarteCredit>();

        public DbSet<Statut> Statuts => Set<Statut>();

        // Personnes
        public DbSet<Personne> Personnes => Set<Personne>();
        public DbSet<Acteur> Acteurs => Set<Acteur>();
        public DbSet<Realisateur> Realisateurs => Set<Realisateur>();
        public DbSet<Scenariste> Scenaristes => Set<Scenariste>();

        // Films
        public DbSet<Film> Films => Set<Film>();
        public DbSet<CopieFilm> CopiesFilm => Set<CopieFilm>();
        public DbSet<Pays> Pays => Set<Pays>();
        public DbSet<Genre> Genres => Set<Genre>();

        // Tables d’association (N–N)
        public DbSet<FilmPays> FilmsPays => Set<FilmPays>();
        public DbSet<FilmGenre> FilmsGenres => Set<FilmGenre>();
        public DbSet<FilmScenariste> FilmsScenaristes => Set<FilmScenariste>();
        public DbSet<Role> Roles => Set<Role>();

        public DbSet<Location> Locations => Set<Location>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Utilisateur>()
                .HasOne(u => u.Adresse)
                .WithOne(a => a.Utilisateur)
                .HasForeignKey<Adresse>(a => a.IdUtilisateur)
                .OnDelete(DeleteBehavior.Cascade);

            // Client - Utilisateur (1:1)
            modelBuilder.Entity<Client>()
                .HasOne(c => c.Utilisateur)
                .WithOne(u => u.Client)
                .HasForeignKey<Client>(c => c.IdUtilisateur)
                .OnDelete(DeleteBehavior.Cascade);

            // Client - CarteCredit (1:1)
            modelBuilder.Entity<Client>()
                .HasOne(c => c.CarteCredit)
                .WithOne(cc => cc.Client)
                .HasForeignKey<Client>(c => c.NoCarte)
                .OnDelete(DeleteBehavior.Restrict);

            // Client - Forfait (N:1)
            modelBuilder.Entity<Client>()
                .HasOne(c => c.Forfait)
                .WithMany(f => f.Clients)
                .HasForeignKey(c => c.CodeForfait)
                .OnDelete(DeleteBehavior.Restrict);

            // Employe - Utilisateur (1:1)
            modelBuilder.Entity<Employe>()
                .HasOne(e => e.Utilisateur)
                .WithOne(u => u.Employe)
                .HasForeignKey<Employe>(e => e.IdUtilisateur)
                .OnDelete(DeleteBehavior.Cascade);

            // CarteCredit - TypeCarte (N:1)
            modelBuilder.Entity<CarteCredit>()
                .HasOne(cc => cc.TypeCarte)
                .WithMany(tc => tc.CartesCredit)
                .HasForeignKey(cc => cc.Type)
                .OnDelete(DeleteBehavior.Restrict);

            // Acteur - Personne (1:1)
            modelBuilder.Entity<Acteur>()
                .HasOne(a => a.Personne)
                .WithOne(p => p.Acteur)
                .HasForeignKey<Acteur>(a => a.IdPersonne)
                .OnDelete(DeleteBehavior.Cascade);

            // Realisateur - Personne (1:1)
            modelBuilder.Entity<Realisateur>()
                .HasOne(r => r.Personne)
                .WithOne(p => p.Realisateur)
                .HasForeignKey<Realisateur>(r => r.IdPersonne)
                .OnDelete(DeleteBehavior.Cascade);

            // Scenariste - Personne (1:1)
            modelBuilder.Entity<Scenariste>()
                .HasOne(s => s.Personne)
                .WithOne(p => p.Scenariste)
                .HasForeignKey<Scenariste>(s => s.IdPersonne)
                .OnDelete(DeleteBehavior.Cascade);

            // Film - Realisateur (N:1)
            modelBuilder.Entity<Film>()
                .HasOne(f => f.Realisateur)
                .WithMany(r => r.Films)
                .HasForeignKey(f => f.IdRealisateur)
                .OnDelete(DeleteBehavior.Restrict);

            // CopieFilm - Film (N:1)
            modelBuilder.Entity<CopieFilm>()
                .HasOne(c => c.Film)
                .WithMany(f => f.CopiesFilm)
                .HasForeignKey(c => c.IdFilm)
                .OnDelete(DeleteBehavior.Cascade);

            // CopieFilm - Statut (N:1)
            modelBuilder.Entity<CopieFilm>()
                .HasOne(c => c.StatutCopie)
                .WithMany(s => s.CopiesFilm)
                .HasForeignKey(c => c.Statut)
                .OnDelete(DeleteBehavior.Restrict);

            // FilmPays (N:N)
            modelBuilder.Entity<FilmPays>()
                .HasKey(fp => new { fp.IdFilm, fp.IdPays });

            modelBuilder.Entity<FilmPays>()
                .HasOne(fp => fp.Film)
                .WithMany(f => f.FilmsPays)
                .HasForeignKey(fp => fp.IdFilm)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FilmPays>()
                .HasOne(fp => fp.Pays)
                .WithMany(p => p.FilmsPays)
                .HasForeignKey(fp => fp.IdPays)
                .OnDelete(DeleteBehavior.Cascade);

            // FilmGenre (N:N)
            modelBuilder.Entity<FilmGenre>()
                .HasKey(fg => new { fg.IdFilm, fg.IdGenre });

            modelBuilder.Entity<FilmGenre>()
                .HasOne(fg => fg.Film)
                .WithMany(f => f.FilmsGenres)
                .HasForeignKey(fg => fg.IdFilm)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FilmGenre>()
                .HasOne(fg => fg.Genre)
                .WithMany(g => g.FilmsGenres)
                .HasForeignKey(fg => fg.IdGenre)
                .OnDelete(DeleteBehavior.Cascade);

            // FilmScenariste (N:N)
            modelBuilder.Entity<FilmScenariste>()
                .HasKey(fs => new { fs.IdFilm, fs.IdScenariste });

            modelBuilder.Entity<FilmScenariste>()
                .HasOne(fs => fs.Film)
                .WithMany(f => f.FilmsScenaristes)
                .HasForeignKey(fs => fs.IdFilm)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FilmScenariste>()
                .HasOne(fs => fs.Scenariste)
                .WithMany(s => s.FilmsEcrits)
                .HasForeignKey(fs => fs.IdScenariste)
                .OnDelete(DeleteBehavior.Cascade);

            // Role (N:N)
            modelBuilder.Entity<Role>()
                .HasKey(r => new { r.IdFilm, r.IdActeur });

            modelBuilder.Entity<Role>()
                .HasOne(r => r.Film)
                .WithMany(f => f.Roles)
                .HasForeignKey(r => r.IdFilm)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Role>()
                .HasOne(r => r.Acteur)
                .WithMany(a => a.Roles)
                .HasForeignKey(r => r.IdActeur)
                .OnDelete(DeleteBehavior.Cascade);

            // Location (N:N avec attributs)
            modelBuilder.Entity<Location>()
                .HasKey(l => new { l.IdUtilisateur, l.IdCopie, l.DateDebut });

            modelBuilder.Entity<Location>()
                .HasOne(l => l.Client)
                .WithMany(c => c.Locations)
                .HasForeignKey(l => l.IdUtilisateur)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                .HasOne(l => l.CopieFilm)
                .WithMany(c => c.Locations)
                .HasForeignKey(l => l.IdCopie)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
