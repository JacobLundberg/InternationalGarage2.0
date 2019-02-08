using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternationalGarage2_0.Migrations
{
    public partial class Add_VehicleType_AndChange_ParkedVehicle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MemberName",
                table: "Member",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "ParkedVehicle",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VehicleTypeId",
                table: "ParkedVehicle",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VehicleType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicle_MemberId",
                table: "ParkedVehicle",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicle_VehicleTypeId",
                table: "ParkedVehicle",
                column: "VehicleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicle_Member_MemberId",
                table: "ParkedVehicle",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicle_VehicleType_VehicleTypeId",
                table: "ParkedVehicle",
                column: "VehicleTypeId",
                principalTable: "VehicleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicle_Member_MemberId",
                table: "ParkedVehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicle_VehicleType_VehicleTypeId",
                table: "ParkedVehicle");

            migrationBuilder.DropTable(
                name: "VehicleType");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicle_MemberId",
                table: "ParkedVehicle");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicle_VehicleTypeId",
                table: "ParkedVehicle");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "ParkedVehicle");

            migrationBuilder.DropColumn(
                name: "VehicleTypeId",
                table: "ParkedVehicle");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Member",
                newName: "MemberName");
        }
    }
}
