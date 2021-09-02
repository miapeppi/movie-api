using Assignment3.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3.Models
{
    public class MovieDbContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Franchise> Franchises { get; set; }

        public MovieDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Franchise)
                .WithMany(f => f.Movies)
                .HasForeignKey(m => m.FranchiseId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Franchise>().HasData(new Franchise { Id = 1, Name = "MCU", Description = "Marvel Cinematic Universe. Contains all Marvel superhero movies that share the same universe." });
            modelBuilder.Entity<Franchise>().HasData(new Franchise { Id = 2, Name = "Frogtown", Description = "Donald G. Jackson's movies, set in a post-apocalyptic world where mutant frogs live alongside humans." });
            modelBuilder.Entity<Franchise>().HasData(new Franchise { Id = 3, Name = "Alien", Description = "Movie series built upon Ridley Scott's movie, Alien." });

            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = 1,
                    Title = "Hell Comes To Frogtown",
                    Genre = "Sci-fi",
                    ReleaseYear = 1988,
                    Director = "Donald G. Jackson",
                    Picture = "http://2.bp.blogspot.com/_5OGIrfjjcc0/TUSI68WDrUI/AAAAAAAACJc/P_eYYNUL3Po/w1200-h630-p-k-no-nu/hell_comes_to_frogtown_c.jpg",
                    Trailer = "https://www.youtube.com/watch?v=JUml88mJxyQ",
                    FranchiseId = 2
                });
            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = 2,
                    Title = "Alien",
                    Genre = "Sci-fi",
                    ReleaseYear = 1979,
                    Director = "Ridley Scott",
                    Picture = "https://media-cache.cinematerial.com/p/500x/qbgrcxaf/alien-movie-cover.jpg?v=1456154180",
                    Trailer = "https://youtu.be/LjLamj-b0I8",
                    FranchiseId = 3
                });
            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = 3,
                    Title = "Aliens",
                    Genre = "Sci-fi",
                    ReleaseYear = 1986,
                    Director = "James Cameron",
                    Picture = "https://m.media-amazon.com/images/I/51cAAgWdFfL._AC_.jpg",
                    Trailer = "https://youtu.be/CQMFckAuyEA",
                    FranchiseId = 3
                });
            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = 4,
                    Title = "Ant-Man",
                    Genre = "Superhero",
                    ReleaseYear = 2015,
                    Director = "",
                    Picture = "https://upload.wikimedia.org/wikipedia/fi/thumb/a/a8/Ant-Man.jpg/250px-Ant-Man.jpg",
                    Trailer = "https://youtu.be/UpPx7E27Bc8",
                    FranchiseId = 1
                });

            modelBuilder.Entity<Character>().HasData(
                new Character { Id = 1, FullName = "Ellen Ripley", Alias = "", Gender = "female", Picture = "" },
                new Character { Id = 2, FullName = "Scott Lang", Alias = "Ant-Man", Gender = "male", Picture = "" },
                new Character { Id = 3, FullName = "Sam Hell", Alias = "", Gender = "male", Picture = "" },
                new Character { Id = 4, FullName = "Gilbert Kane", Alias = "", Gender = "male", Picture = "" }
                );

            modelBuilder.Entity<Movie>()
                .HasMany(left => left.Characters)
                .WithMany(right => right.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "CharacterMovie",
                    right => right.HasOne<Character>().WithMany().HasForeignKey("CharactersId"),
                    left => left.HasOne<Movie>().WithMany().HasForeignKey("MoviesId"),
                    je =>
                    {
                        je.HasKey("MoviesId", "CharactersId");
                        je.HasData(
                            new { CharactersId = 1, MoviesId = 2 },
                            new { CharactersId = 4, MoviesId = 2 },
                            new { CharactersId = 3, MoviesId = 1 },
                            new { CharactersId = 2, MoviesId = 4 },
                            new { CharactersId = 1, MoviesId = 3 }
                            );
                    });
        }

    }
}
