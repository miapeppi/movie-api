using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment3.Migrations
{
    public partial class MovieDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Character",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Character", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Franchise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Franchise", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReleaseYear = table.Column<int>(type: "int", nullable: false),
                    Director = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Trailer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FranchiseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movie_Franchise_FranchiseId",
                        column: x => x.FranchiseId,
                        principalTable: "Franchise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterMovie",
                columns: table => new
                {
                    MoviesId = table.Column<int>(type: "int", nullable: false),
                    CharactersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterMovie", x => new { x.MoviesId, x.CharactersId });
                    table.ForeignKey(
                        name: "FK_CharacterMovie_Character_CharactersId",
                        column: x => x.CharactersId,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterMovie_Movie_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Character",
                columns: new[] { "Id", "Alias", "FullName", "Gender", "Picture" },
                values: new object[,]
                {
                    { 1, "", "Ellen Ripley", "female", "" },
                    { 2, "Ant-Man", "Scott Lang", "male", "" },
                    { 3, "", "Sam Hell", "male", "" },
                    { 4, "", "Gilbert Kane", "male", "" }
                });

            migrationBuilder.InsertData(
                table: "Franchise",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Marvel Cinematic Universe. Contains all Marvel superhero movies that share the same universe.", "MCU" },
                    { 2, "Donald G. Jackson's movies, set in a post-apocalyptic world where mutant frogs live alongside humans.", "Frogtown" },
                    { 3, "Movie series built upon Ridley Scott's movie, Alien.", "Alien" }
                });

            migrationBuilder.InsertData(
                table: "Movie",
                columns: new[] { "Id", "Director", "FranchiseId", "Genre", "Picture", "ReleaseYear", "Title", "Trailer" },
                values: new object[,]
                {
                    { 4, "", 1, "Superhero", "https://upload.wikimedia.org/wikipedia/fi/thumb/a/a8/Ant-Man.jpg/250px-Ant-Man.jpg", 2015, "Ant-Man", "https://youtu.be/UpPx7E27Bc8" },
                    { 1, "Donald G. Jackson", 2, "Sci-fi", "http://2.bp.blogspot.com/_5OGIrfjjcc0/TUSI68WDrUI/AAAAAAAACJc/P_eYYNUL3Po/w1200-h630-p-k-no-nu/hell_comes_to_frogtown_c.jpg", 1988, "Hell Comes To Frogtown", "https://www.youtube.com/watch?v=JUml88mJxyQ" },
                    { 2, "Ridley Scott", 3, "Sci-fi", "https://media-cache.cinematerial.com/p/500x/qbgrcxaf/alien-movie-cover.jpg?v=1456154180", 1979, "Alien", "https://youtu.be/LjLamj-b0I8" },
                    { 3, "James Cameron", 3, "Sci-fi", "https://m.media-amazon.com/images/I/51cAAgWdFfL._AC_.jpg", 1986, "Aliens", "https://youtu.be/CQMFckAuyEA" }
                });

            migrationBuilder.InsertData(
                table: "CharacterMovie",
                columns: new[] { "CharactersId", "MoviesId" },
                values: new object[,]
                {
                    { 2, 4 },
                    { 3, 1 },
                    { 1, 2 },
                    { 4, 2 },
                    { 1, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterMovie_CharactersId",
                table: "CharacterMovie",
                column: "CharactersId");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_FranchiseId",
                table: "Movie",
                column: "FranchiseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterMovie");

            migrationBuilder.DropTable(
                name: "Character");

            migrationBuilder.DropTable(
                name: "Movie");

            migrationBuilder.DropTable(
                name: "Franchise");
        }
    }
}
