using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ETZ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    MaterialUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    MaterialType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatorUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleterUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifierUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Speakers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Surname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SpeakerPhotoUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatorUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleterUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifierUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Speakers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SponsorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SponsorCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatorUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleterUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifierUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SponsorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatorUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleterUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifierUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatorUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleterUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifierUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialContents_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialPlacements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlacementCode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatorUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleterUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifierUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialPlacements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialPlacements_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpeakerContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    SpeakerId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Description = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatorUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleterUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifierUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeakerContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpeakerContents_Speakers_SpeakerId",
                        column: x => x.SpeakerId,
                        principalTable: "Speakers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sponsors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SponsorLogoUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    SponsorTypeId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatorUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleterUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifierUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sponsors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sponsors_SponsorTypes_SponsorTypeId",
                        column: x => x.SponsorTypeId,
                        principalTable: "SponsorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SponsorTypeContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    LanguageCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    SponsorTypeId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatorUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleterUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifierUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SponsorTypeContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SponsorTypeContents_SponsorTypes_SponsorTypeId",
                        column: x => x.SponsorTypeId,
                        principalTable: "SponsorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopicContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatorUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleterUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifierUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicContents_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialContents_MaterialId_LanguageCode",
                table: "MaterialContents",
                columns: new[] { "MaterialId", "LanguageCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaterialPlacements_MaterialId",
                table: "MaterialPlacements",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_DisplayOrder",
                table: "Materials",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_SpeakerContents_SpeakerId",
                table: "SpeakerContents",
                column: "SpeakerId");

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_DisplayOrder",
                table: "Speakers",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsors_DisplayOrder",
                table: "Sponsors",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsors_SponsorTypeId",
                table: "Sponsors",
                column: "SponsorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorTypeContents_SponsorTypeId",
                table: "SponsorTypeContents",
                column: "SponsorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorTypes_SponsorCode",
                table: "SponsorTypes",
                column: "SponsorCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopicContents_TopicId",
                table: "TopicContents",
                column: "TopicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialContents");

            migrationBuilder.DropTable(
                name: "MaterialPlacements");

            migrationBuilder.DropTable(
                name: "SpeakerContents");

            migrationBuilder.DropTable(
                name: "Sponsors");

            migrationBuilder.DropTable(
                name: "SponsorTypeContents");

            migrationBuilder.DropTable(
                name: "TopicContents");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Speakers");

            migrationBuilder.DropTable(
                name: "SponsorTypes");

            migrationBuilder.DropTable(
                name: "Topics");
        }
    }
}
