using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class mvpv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubstructureCostProfile_Substructures_Substructure.Id",
                table: "SubstructureCostProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_TopsideCostProfile_Topsides_Topside.Id",
                table: "TopsideCostProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopsideCostProfile",
                table: "TopsideCostProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubstructureCostProfile",
                table: "SubstructureCostProfile");

            migrationBuilder.RenameTable(
                name: "TopsideCostProfile",
                newName: "TopsideCostProfiles");

            migrationBuilder.RenameTable(
                name: "SubstructureCostProfile",
                newName: "SubstructureCostProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_TopsideCostProfile_Topside.Id",
                table: "TopsideCostProfiles",
                newName: "IX_TopsideCostProfiles_Topside.Id");

            migrationBuilder.RenameIndex(
                name: "IX_SubstructureCostProfile_Substructure.Id",
                table: "SubstructureCostProfiles",
                newName: "IX_SubstructureCostProfiles_Substructure.Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopsideCostProfiles",
                table: "TopsideCostProfiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubstructureCostProfiles",
                table: "SubstructureCostProfiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubstructureCostProfiles_Substructures_Substructure.Id",
                table: "SubstructureCostProfiles",
                column: "Substructure.Id",
                principalTable: "Substructures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopsideCostProfiles_Topsides_Topside.Id",
                table: "TopsideCostProfiles",
                column: "Topside.Id",
                principalTable: "Topsides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubstructureCostProfiles_Substructures_Substructure.Id",
                table: "SubstructureCostProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_TopsideCostProfiles_Topsides_Topside.Id",
                table: "TopsideCostProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopsideCostProfiles",
                table: "TopsideCostProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubstructureCostProfiles",
                table: "SubstructureCostProfiles");

            migrationBuilder.RenameTable(
                name: "TopsideCostProfiles",
                newName: "TopsideCostProfile");

            migrationBuilder.RenameTable(
                name: "SubstructureCostProfiles",
                newName: "SubstructureCostProfile");

            migrationBuilder.RenameIndex(
                name: "IX_TopsideCostProfiles_Topside.Id",
                table: "TopsideCostProfile",
                newName: "IX_TopsideCostProfile_Topside.Id");

            migrationBuilder.RenameIndex(
                name: "IX_SubstructureCostProfiles_Substructure.Id",
                table: "SubstructureCostProfile",
                newName: "IX_SubstructureCostProfile_Substructure.Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopsideCostProfile",
                table: "TopsideCostProfile",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubstructureCostProfile",
                table: "SubstructureCostProfile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubstructureCostProfile_Substructures_Substructure.Id",
                table: "SubstructureCostProfile",
                column: "Substructure.Id",
                principalTable: "Substructures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopsideCostProfile_Topsides_Topside.Id",
                table: "TopsideCostProfile",
                column: "Topside.Id",
                principalTable: "Topsides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
