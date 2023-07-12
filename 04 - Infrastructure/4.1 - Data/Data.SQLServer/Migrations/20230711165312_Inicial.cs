using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(254)", unicode: false, maxLength: 254, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.Sql(@"
                                    CREATE PROCEDURE GenerateUserInserts
                                    AS
                                    BEGIN
                                        DECLARE @i INT = 1;
                                        WHILE (@i <= 15000)
                                        BEGIN
                                            DECLARE @FullName NVARCHAR(MAX) = 'User' + CAST(@i AS NVARCHAR);
                                            DECLARE @Email NVARCHAR(MAX) = 'user' + CAST(@i AS NVARCHAR) + '@example.com';
                                            DECLARE @Phone NVARCHAR(MAX) = '123-456-789' + RIGHT('000' + CAST(@i AS NVARCHAR), 3);
                                            DECLARE @BirthDate DATETIME2 = DATEADD(YEAR, -20, GETDATE());
                                            DECLARE @Active BIT = 1;
                                            DECLARE @Password NVARCHAR(MAX) = 'Password' + CAST(@i AS NVARCHAR);
                                            DECLARE @Role NVARCHAR(MAX) = 'Role' + CAST((@i % 5) AS NVARCHAR);
                                            DECLARE @Modified DATETIME2 = GETDATE();

                                            INSERT INTO [dbo].[User] ([FullName], [Email], [Phone], [BirthDate], [Active], [Password], [Role], [Modified])
                                            VALUES (@FullName, @Email, @Phone, @BirthDate, @Active, @Password, @Role, @Modified);

                                            SET @i = @i + 1;
                                        END
                                    END
                                    GO ");

            migrationBuilder.Sql(@"EXEC GenerateUserInserts");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
