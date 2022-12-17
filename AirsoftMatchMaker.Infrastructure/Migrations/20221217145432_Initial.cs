using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirsoftMatchMaker.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Credits = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameModes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PointsToWin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    Wins = table.Column<int>(type: "int", nullable: false),
                    Losses = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matchmakers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matchmakers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matchmakers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleRequests_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mapsize = table.Column<int>(type: "int", nullable: false),
                    Terrain = table.Column<int>(type: "int", nullable: false),
                    AverageEngagementDistance = table.Column<int>(type: "int", nullable: false),
                    GameModeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maps_GameModes_GameModeId",
                        column: x => x.GameModeId,
                        principalTable: "GameModes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Ammo = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    SkillLevel = table.Column<int>(type: "int", nullable: false),
                    SkillPoints = table.Column<int>(type: "int", nullable: false),
                    PlayerClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Players_PlayerClasses_PlayerClassId",
                        column: x => x.PlayerClassId,
                        principalTable: "PlayerClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AmmoBoxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmmoBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmmoBoxes_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Result = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GameStatus = table.Column<int>(type: "int", nullable: false),
                    MapId = table.Column<int>(type: "int", nullable: false),
                    GameModeId = table.Column<int>(type: "int", nullable: false),
                    MatchmakerId = table.Column<int>(type: "int", nullable: false),
                    TeamRedId = table.Column<int>(type: "int", nullable: false),
                    TeamRedPoints = table.Column<int>(type: "int", nullable: false),
                    TeamRedOdds = table.Column<int>(type: "int", nullable: false),
                    TeamBlueId = table.Column<int>(type: "int", nullable: false),
                    TeamBluePoints = table.Column<int>(type: "int", nullable: false),
                    TeamBlueOdds = table.Column<int>(type: "int", nullable: false),
                    OddsAreUpdated = table.Column<bool>(type: "bit", nullable: false),
                    GameBetCreditsContainerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_GameModes_GameModeId",
                        column: x => x.GameModeId,
                        principalTable: "GameModes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Matchmakers_MatchmakerId",
                        column: x => x.MatchmakerId,
                        principalTable: "Matchmakers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Teams_TeamBlueId",
                        column: x => x.TeamBlueId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Teams_TeamRedId",
                        column: x => x.TeamRedId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clothes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClothingColor = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: true),
                    PlayerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clothes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clothes_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clothes_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeamRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    TeamRequestType = table.Column<int>(type: "int", nullable: false),
                    TeamRequestStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamRequests_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamRequests_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FeetPerSecond = table.Column<double>(type: "float", nullable: false),
                    FireRate = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AverageAmmoExpendedPerGame = table.Column<int>(type: "int", nullable: false),
                    WeaponType = table.Column<int>(type: "int", nullable: false),
                    PreferedEngagementDistance = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: true),
                    PlayerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weapons_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Weapons_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BetStatus = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    WinningTeamId = table.Column<int>(type: "int", nullable: false),
                    CreditsBet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Odds = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bets_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameBetCreditsContainers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    TeamRedCreditsBet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TeamBlueCreditsBet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BetsArePaidOut = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameBetCreditsContainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameBetCreditsContainers_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "52f73adc-3c27-40de-b00e-2e2b382da84c", "52f73adc-3c27-40de-b00e-2e2b382da84c", "Responsible for granting roles.", "Role", "Administrator", "ADMINISTRATOR" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "6b3c10a1-4a55-411a-8dca-49574cb55e74", "Participates in games. Cannot be a active vendor or matchmaker.", "Role", "Player", "PLAYER" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "b48af83e-7873-4ecd-82de-5d517e7b31f9", "User with no other roles. Can only bet on games.", "Role", "GuestUser", "GUESTUSER" },
                    { "d0bd950a-e2d5-46cf-a6c1-1f0efa4144ce", "d0bd950a-e2d5-46cf-a6c1-1f0efa4144ce", "Imports and sells items. Cannot be a active player or matchmaker.", "Role", "Vendor", "VENDOR" },
                    { "fc9628b0-fa92-4be1-9f1f-9095d66f1ff8", "fc9628b0-fa92-4be1-9f1f-9095d66f1ff8", "Creates games and collects entry fee. Cannot be a active vendor or player. Cannot bet on games!", "Role", "Matchmaker", "MATCHMAKER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Credits", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "0a9aab7f-739a-41d8-b18d-8b797c7a2dfe", 0, "5364d927-2414-458e-bbdc-d380d6433b15", 200m, "Liam@gmail.com", false, false, null, "LIAM@GMAIL.COM", "LIAM", "AQAAAAEAACcQAAAAENxbWAcl5keJV0Y/4Lr4sVbWDgLcKGm2HiBhhgRRGd18Nwp+aleDvp+EpJLf0s92kQ==", null, false, "af91b70c-57d0-42d5-85d9-e47748ab432c", false, "Liam" },
                    { "14677dd9-7de7-41c0-9418-e43ddcf64859", 0, "9aaaae80-df07-400f-b61e-6b083a55f3ac", 200m, "John@gmail.com", false, false, null, "JOHN@GMAIL.COM", "JOHN", "AQAAAAEAACcQAAAAEGSLg/FJ/lI9yb5tv1BTdgN+ELAfUthRhMHp1MJ4cVn+wP2Q6/i/4iHVZzm482XP0A==", null, false, "eb64b814-d4a4-44bd-bba8-4d48eb4dc079", false, "John" },
                    { "18a322e4-ade8-4f13-8981-4cac7be64b9c", 0, "95800c1e-dd94-409f-ba3d-3cdeaf2ee7ba", 200m, "Stoyan@gmail.com", false, false, null, "STOYAN@GMAIL.COM", "STOYAN", "AQAAAAEAACcQAAAAEBHSIxjxAqr4w4MrmHIsiqkemm3GG+zhPdDQmYT1SHbCCAkrOh09FODc+7G9pCKvxw==", null, false, "61e688e3-84ea-4579-9da6-ef4d9f10bf2c", false, "Stoyan" },
                    { "1f1087d3-a55a-4b7a-932e-1c3f9817fcf0", 0, "16d499ab-cf03-41fe-86e8-179225cc9d93", 200m, "Daniel@gmail.com", false, false, null, "DANIEL@GMAIL.COM", "DANIEL", "AQAAAAEAACcQAAAAELk6FDIVJ2/yNKJ1cDMC//+74urOHAHHmc6G1fiFT+ZOaFq+HaVGN0d0wS5McxadWg==", null, false, "73d0ba13-0925-46c8-95ae-e2cbc8467e41", false, "Daniel" },
                    { "1f5be09b-2910-4ac0-8ff5-5c525ddf1b61", 0, "12bfa966-6743-4bb4-8ab2-649778794742", 200m, "Paul@gmail.com", false, false, null, "PAUL@GMAIL.COM", "PAUL", "AQAAAAEAACcQAAAAEB2KV/3qTbNTbdVdBSq5y6JusuRY9ncX+WXZoM3ji2r9NqQ4asi9ozFnpHblLhOPgQ==", null, false, "d19f8b1d-a1b1-4aec-9f4c-3642bb64409a", false, "Paul" },
                    { "202efe8b-7748-49ca-834c-fd1c37978ab2", 0, "85b36ff8-4fac-4391-9b5b-4177c661b935", 200m, "Georgi@gmail.com", false, false, null, "GEORGI@GMAIL.COM", "GEORGI", "AQAAAAEAACcQAAAAEG5i1SSirktWBqiI4AWC8jHtuXM+7P3Ev6FzaJil7ZFe7QyutLaUUAamHWkXK3f7sA==", null, false, "dcff68d3-02a5-48b1-9fd3-aa706106ab22", false, "Georgi" },
                    { "2a1bd8b6-6d06-470b-9dda-fe88aa1bf5e8", 0, "4548dcaa-669c-40df-b045-35ba3d19bafb", 200m, "Ivan@gmail.com", false, false, null, "IVAN@GMAIL.COM", "IVAN", "AQAAAAEAACcQAAAAEJMlyq38DbJZkyC4xHTC9ynGCjV8y+1TXWxs33y8ZCxfS00vzfMkjHJdluQ5evCKhA==", null, false, "3b004132-2a60-48d2-aad0-ec5027406623", false, "Ivan" },
                    { "3bf3238b-ab04-4945-8bd0-1eabf8a208d5", 0, "36741c0c-5542-4130-9212-63808de21fa9", 200m, "Tihomir@gmail.com", false, false, null, "TIHOMIR@GMAIL.COM", "TIHOMIR", "AQAAAAEAACcQAAAAELSgy57IZj6E4eN5l4CPi8Gd/PR/fAeTtSK4Vu5KzW64Hp1MBYMRsRUljEFQ8WhsIA==", null, false, "bfa50aa8-2eb1-4241-873d-09823ba096db", false, "Tihomir" },
                    { "4d64daba-17d4-452c-af3e-5d731a250283", 0, "b1d8befd-54f7-4337-b36f-a9f08871b44f", 200m, "Michael@gmail.com", false, false, null, "MICHAEL@GMAIL.COM", "MICHAEL", "AQAAAAEAACcQAAAAENH/s6lLBBZFkIME0vtU1SjY4nYvP6Q1mUyRHoSGsJfpAtLpVD/ls3ZzjG06TZui5w==", null, false, "77e91de5-e8da-43c8-a4af-231ba3a2643d", false, "Michael" },
                    { "56d661fd-2339-498a-bd7e-c95f37908b28", 0, "ef050970-2fbc-4ae7-a254-7bf38f25c9c8", 200m, "Petar@gmail.com", false, false, null, "PETAR@GMAIL.COM", "PETAR", "AQAAAAEAACcQAAAAEK1zQA+ysR+7joxCVGLQjcjRW9fkVf6M9WADeP60L1uERlgPF7uOUz4BvW2QR4xG4A==", null, false, "78444e12-e704-4cad-88e2-ebc4ead236bc", false, "Petar" },
                    { "5f83ea0f-418b-463f-9a52-bf1b9eac8bc6", 0, "4a3e1c64-d888-478b-a946-e5a07010df0d", 200m, "Philip@gmail.com", false, false, null, "PHILIP@GMAIL.COM", "PHILIP", "AQAAAAEAACcQAAAAEKhR3ENPfcSdxSuiPFptaKJqTY6zRRgUXPcbxl2cTF37bKpNerJKO3ZJkSDHoOproA==", null, false, "f6dfab00-e56a-42b6-8625-522f63202364", false, "Philip" },
                    { "6f4bc586-751a-4a4b-8fec-4c7145b47a3e", 0, "33f1ac73-e7b8-4b8f-8b59-55231887ebe9", 200m, "Dimitrichko@gmail.com", false, false, null, "DIMITRICHKO@GMAIL.COM", "DIMITRICHKO", "AQAAAAEAACcQAAAAEGlTgU9riVFoclKkhFYHvMb19jc9sGjZ1NRSnCtga75hEFyhEst0lRsHnudqp8JEsg==", null, false, "49e6839d-a15e-417d-a366-a5b54536c898", false, "Dimitrichko" },
                    { "77388c0c-698c-4df9-9ad9-cef29116b666", 0, "482883e0-cfd4-4f6d-807d-30a91cd0f164", 200m, "Vasil@gmail.com", false, false, null, "VASIL@GMAIL.COM", "VASIL", "AQAAAAEAACcQAAAAEOqLjjTCFPpYE3AbhNvghIuHtTIObn5Meh6FeOkWz44sHefIdZDVnCk+dFzTO49+yA==", null, false, "003ca97a-8a04-4bfe-8c41-f0acf46575e4", false, "Vasil" },
                    { "799495ef-8794-491d-94d9-6bd37d51ba40", 0, "7f0692c0-dee2-490b-a118-d52085260f94", 200m, "Ivaylo@gmail.com", false, false, null, "IVAYLO@GMAIL.COM", "IVAYLO", "AQAAAAEAACcQAAAAEAVvgyvq9SJpVVtCij57AP2LeUjwSaoDfOHop4l3em9xXYSv4u+aBLosSxuNOzjGZw==", null, false, "ed02d38e-6c84-45bc-a737-d942924d59d9", false, "Ivaylo" },
                    { "b2451308-1197-4362-be78-f7ea7ca35fe9", 0, "7e5e12f1-b49d-40d4-bbf6-9e8f4a6f7bad", 200m, "Alexander@gmail.com", false, false, null, "ALEXANDER@GMAIL.COM", "ALEXANDER", "AQAAAAEAACcQAAAAECt8ODUWi7oS5Rd+dkdbzzKRm/f2Ysu9G9viWUbv+kB7hAI0u673Z2i79adiCrcFhw==", null, false, "82e937f6-54e2-42d7-9d2b-4232b3dab9cc", false, "Alexander" },
                    { "c5d9e543-7c2f-4345-a014-ebd860eef718", 0, "a5164fac-1687-4d7a-924e-60cbe9ae3ebf", 200m, "Krum@gmail.com", false, false, null, "KRUM@GMAIL.COM", "KRUM", "AQAAAAEAACcQAAAAECpJ46hL/M4TAh5zQDYeBPE1G+WTxWGeE9jK5FxH11Pn7kgaC4GRF8NDQGCCvDWvOA==", null, false, "3a06ca0b-fa60-425a-837b-e8ea00f73ac6", false, "Krum" },
                    { "c95011ef-d0e4-49c0-bbdd-1b9985bf7a74", 0, "07358601-4d2e-4ae9-bab9-12ec6dc616b7", 200m, "Walter@gmail.com", false, false, null, "WALTER@GMAIL.COM", "WALTER", "AQAAAAEAACcQAAAAEKVfd9eVKsiVw9JJdPhPv2owFfOxNg09sK+uFQMsRrwcNfQttqLrHKyxJn+4NJV8Vg==", null, false, "5a508bca-0f4a-4666-a90f-0546bcd22685", false, "Walter" },
                    { "cc1cb39b-c0cf-41ed-856c-d3943aec605a", 0, "ec5aa4ea-e525-4953-943a-4d1bdef72b5e", 200m, "Joe@gmail.com", false, false, null, "JOE@GMAIL.COM", "JOE", "AQAAAAEAACcQAAAAEO+8vAWmYpwoAEyr7HMljrxQkvDaXkFNiK3TrDP6pKH9p+Yrqpdi937yqomOIN2Dyg==", null, false, "c507b157-dbfb-4579-a5de-d8ec8cf20aa6", false, "Joe" },
                    { "f3534aed-259b-4ff7-b816-15e8207e084a", 0, "a4816e16-033f-4de9-b8d0-b63f775dc718", 200m, "Todor@gmail.com", false, false, null, "TODOR@GMAIL.COM", "TODOR", "AQAAAAEAACcQAAAAEB2KucUMAkwQsNgh6hxzrVdYlOn5xun/TnxE16S5SA8QlYGUoA5hd1EXNJqfUXBGTg==", null, false, "322851f3-6497-4598-b6e5-91f3f16676d0", false, "Todor" },
                    { "f580c1f9-d41f-455e-b4ec-705b834e4b19", 0, "20e0e59b-2740-4fd1-9cb7-071ce0f66c79", 200m, "Hank@gmail.com", false, false, null, "HANK@GMAIL.COM", "HANK", "AQAAAAEAACcQAAAAEHRrToIm14Mp3AiA9TFEOXedEkAGbgRuBydxe5lKPvwmtoxQlR6nrs2bhGYFdJ++dg==", null, false, "a66232ee-50e5-4e9f-9efa-b0951d047800", false, "Hank" }
                });

            migrationBuilder.InsertData(
                table: "GameModes",
                columns: new[] { "Id", "Description", "Name", "PointsToWin" },
                values: new object[,]
                {
                    { 1, "Whoever captures the flag first scores a point.", "Capture the flag", 3 },
                    { 2, "The team which controls the point in the middle for 5 minutes wins.", "Secure area", 2 }
                });

            migrationBuilder.InsertData(
                table: "PlayerClasses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "New to the game. Prone to make mistakes.", "New Player" },
                    { 2, "Provides good advice and coordinates teams well.", "Leader" },
                    { 3, "Always goes first. Good in both defence and offence.", "Frontline" },
                    { 4, "High accuracy over long range. Struggles in close range.", "Marksman" },
                    { 5, "Loves to sneak behind and surprise enemy teams from behind.", "Sneaky" },
                    { 6, "Excels in defending, lacks in attacking.", "Camper" },
                    { 7, "Excels in attacking, lacks in defending.", "Rusher" }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "Losses", "Name", "Wins" },
                values: new object[,]
                {
                    { 1, 0, "Alpha", 0 },
                    { 2, 0, "Bravo", 0 },
                    { 3, 0, "Charlie", 1 },
                    { 4, 1, "Delta", 0 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "0a9aab7f-739a-41d8-b18d-8b797c7a2dfe" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "0a9aab7f-739a-41d8-b18d-8b797c7a2dfe" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "14677dd9-7de7-41c0-9418-e43ddcf64859" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "14677dd9-7de7-41c0-9418-e43ddcf64859" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "18a322e4-ade8-4f13-8981-4cac7be64b9c" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "18a322e4-ade8-4f13-8981-4cac7be64b9c" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "1f1087d3-a55a-4b7a-932e-1c3f9817fcf0" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "1f1087d3-a55a-4b7a-932e-1c3f9817fcf0" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "1f5be09b-2910-4ac0-8ff5-5c525ddf1b61" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "202efe8b-7748-49ca-834c-fd1c37978ab2" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "202efe8b-7748-49ca-834c-fd1c37978ab2" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "2a1bd8b6-6d06-470b-9dda-fe88aa1bf5e8" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "2a1bd8b6-6d06-470b-9dda-fe88aa1bf5e8" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "3bf3238b-ab04-4945-8bd0-1eabf8a208d5" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "3bf3238b-ab04-4945-8bd0-1eabf8a208d5" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "4d64daba-17d4-452c-af3e-5d731a250283" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "4d64daba-17d4-452c-af3e-5d731a250283" },
                    { "52f73adc-3c27-40de-b00e-2e2b382da84c", "56d661fd-2339-498a-bd7e-c95f37908b28" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "56d661fd-2339-498a-bd7e-c95f37908b28" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "5f83ea0f-418b-463f-9a52-bf1b9eac8bc6" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "5f83ea0f-418b-463f-9a52-bf1b9eac8bc6" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "6f4bc586-751a-4a4b-8fec-4c7145b47a3e" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "6f4bc586-751a-4a4b-8fec-4c7145b47a3e" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "77388c0c-698c-4df9-9ad9-cef29116b666" },
                    { "d0bd950a-e2d5-46cf-a6c1-1f0efa4144ce", "77388c0c-698c-4df9-9ad9-cef29116b666" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "799495ef-8794-491d-94d9-6bd37d51ba40" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "799495ef-8794-491d-94d9-6bd37d51ba40" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "b2451308-1197-4362-be78-f7ea7ca35fe9" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "b2451308-1197-4362-be78-f7ea7ca35fe9" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "c5d9e543-7c2f-4345-a014-ebd860eef718" },
                    { "fc9628b0-fa92-4be1-9f1f-9095d66f1ff8", "c5d9e543-7c2f-4345-a014-ebd860eef718" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "c95011ef-d0e4-49c0-bbdd-1b9985bf7a74" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "c95011ef-d0e4-49c0-bbdd-1b9985bf7a74" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "cc1cb39b-c0cf-41ed-856c-d3943aec605a" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "f3534aed-259b-4ff7-b816-15e8207e084a" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "f3534aed-259b-4ff7-b816-15e8207e084a" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "f580c1f9-d41f-455e-b4ec-705b834e4b19" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "f580c1f9-d41f-455e-b4ec-705b834e4b19" }
                });

            migrationBuilder.InsertData(
                table: "Maps",
                columns: new[] { "Id", "AverageEngagementDistance", "Description", "GameModeId", "ImageUrl", "Mapsize", "Name", "Terrain" },
                values: new object[,]
                {
                    { 1, 1, "Large forest map in Norway", 1, "https://cdn.britannica.com/87/138787-050-33727493/Belovezhskaya-Forest-Poland.jpg", 2, "Forest", 1 },
                    { 2, 0, "Small Field in California", 2, "https://www.arboursabroad.com/westflanders_be_110318-56/", 0, "Clear Field", 2 },
                    { 3, 2, "Extra Large snowy map in Russia", 2, "https://www.rukavillas.com/wp-content/uploads/2020/01/snowpalace-outside-800.jpg", 3, "Snow Villa", 3 }
                });

            migrationBuilder.InsertData(
                table: "Matchmakers",
                columns: new[] { "Id", "IsActive", "UserId" },
                values: new object[] { 1, true, "c5d9e543-7c2f-4345-a014-ebd860eef718" });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Ammo", "IsActive", "PlayerClassId", "SkillLevel", "SkillPoints", "TeamId", "UserId" },
                values: new object[,]
                {
                    { 1, 200, true, 1, 0, 100, 1, "202efe8b-7748-49ca-834c-fd1c37978ab2" },
                    { 2, 200, true, 2, 0, 100, 1, "2a1bd8b6-6d06-470b-9dda-fe88aa1bf5e8" },
                    { 3, 600, true, 6, 0, 100, 2, "4d64daba-17d4-452c-af3e-5d731a250283" },
                    { 4, 200, true, 7, 0, 100, 2, "b2451308-1197-4362-be78-f7ea7ca35fe9" },
                    { 5, 450, true, 1, 0, 100, null, "f3534aed-259b-4ff7-b816-15e8207e084a" },
                    { 6, 450, true, 4, 0, 100, null, "f580c1f9-d41f-455e-b4ec-705b834e4b19" },
                    { 7, 450, true, 6, 0, 100, 3, "14677dd9-7de7-41c0-9418-e43ddcf64859" },
                    { 8, 450, true, 7, 0, 100, 3, "c95011ef-d0e4-49c0-bbdd-1b9985bf7a74" },
                    { 9, 450, true, 4, 0, 100, 3, "1f1087d3-a55a-4b7a-932e-1c3f9817fcf0" },
                    { 10, 450, true, 5, 0, 100, 3, "5f83ea0f-418b-463f-9a52-bf1b9eac8bc6" },
                    { 11, 450, true, 1, 0, 100, 3, "0a9aab7f-739a-41d8-b18d-8b797c7a2dfe" },
                    { 12, 450, true, 4, 0, 100, 4, "3bf3238b-ab04-4945-8bd0-1eabf8a208d5" },
                    { 13, 450, true, 6, 0, 100, 4, "18a322e4-ade8-4f13-8981-4cac7be64b9c" },
                    { 14, 450, true, 7, 0, 100, 4, "799495ef-8794-491d-94d9-6bd37d51ba40" },
                    { 15, 450, true, 1, 0, 100, 4, "6f4bc586-751a-4a4b-8fec-4c7145b47a3e" }
                });

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "Id", "IsActive", "UserId" },
                values: new object[] { 1, true, "77388c0c-698c-4df9-9ad9-cef29116b666" });

            migrationBuilder.InsertData(
                table: "AmmoBoxes",
                columns: new[] { "Id", "Amount", "Name", "Price", "Quantity", "VendorId" },
                values: new object[,]
                {
                    { 1, 50, "Small box", 10m, 900, 1 },
                    { 2, 150, "Large box", 20m, 700, 1 },
                    { 3, 300, "Extra Large box", 40m, 500, 1 },
                    { 4, 1000, "Needlessly Large box", 100m, 300, 1 }
                });

            migrationBuilder.InsertData(
                table: "Clothes",
                columns: new[] { "Id", "ClothingColor", "Description", "ImageUrl", "Name", "PlayerId", "Price", "VendorId" },
                values: new object[,]
                {
                    { 1, 0, "Hard to spot in forest.", null, "Green outfit", 3, 50m, null },
                    { 2, 0, "Hard to spot in forest.", null, "Green outfit", 13, 50m, null },
                    { 3, 0, "Hard to spot in forest.", null, "Green outfit", 5, 50m, null },
                    { 4, 0, "Hard to spot in forest.", null, "Green outfit", 8, 50m, null },
                    { 5, 0, "Hard to spot in forest.", null, "Green outfit", 14, 50m, null },
                    { 6, 0, "Hard to spot in forest.", null, "Green outfit", 12, 50m, null },
                    { 7, 0, "Hard to spot in forest.", null, "Green outfit", 1, 50m, null },
                    { 8, 0, "Hard to spot in forest.", null, "Green Ghillie Suit", 6, 80m, null },
                    { 9, 0, "Hard to spot in forest.", null, "Green Ghillie Suit", 9, 80m, null },
                    { 10, 0, "Hard to spot in forest.", null, "Green Army camouflage", 11, 60m, null },
                    { 11, 0, "Hard to spot in forest.", null, "Green Army camouflage", 7, 60m, null },
                    { 12, 2, "Hard to spot in urban enviroment.", null, "Urban outfit", 10, 40m, null },
                    { 13, 2, "Hard to spot in urban enviroment.", null, "Urban outfit", 4, 40m, null },
                    { 14, 2, "Hard to spot in urban enviroment.", null, "Urban outfit", 2, 40m, null },
                    { 15, 2, "Hard to spot in urban enviroment.", null, "Urban outfit", 15, 40m, null },
                    { 16, 2, "Hard to spot in urban enviroment.", null, "Gray tracksuit", null, 20m, 1 },
                    { 17, 3, "Hard to spot in field.", null, "Brown Army outfit ", null, 30m, 1 },
                    { 18, 3, "Hard to spot in field.", null, "Brown Army outfit ", null, 30m, 1 },
                    { 19, 3, "Hard to spot in field.", null, "Brown Army outfit ", null, 30m, 1 },
                    { 20, 3, "Hard to spot in field.", null, "Brown Ghillie suit", null, 45m, 1 },
                    { 21, 3, "Hard to spot in field.", null, "Brown Ghillie suit", null, 45m, 1 },
                    { 22, 3, "Hard to spot in field.", null, "Brown Ghillie suit", null, 45m, 1 },
                    { 23, 1, "Hard to spot in snow.", null, "White outfit", null, 40m, 1 },
                    { 24, 1, "Hard to spot in snow.", null, "White outfit", null, 40m, 1 },
                    { 25, 1, "Hard to spot in snow.", null, "White Ghillie suit", null, 55m, 1 },
                    { 26, 1, "Hard to spot in snow.", null, "White Ghillie suit", null, 55m, 1 },
                    { 27, 1, "Hard to spot in snow.", null, "White Ghillie suit", null, 55m, 1 },
                    { 28, 0, "Hard to spot in forest.", null, "Green Ghillie Suit", null, 80m, 1 }
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "Date", "EntryFee", "GameBetCreditsContainerId", "GameModeId", "GameStatus", "MapId", "MatchmakerId", "Name", "OddsAreUpdated", "Result", "TeamBlueId", "TeamBlueOdds", "TeamBluePoints", "TeamRedId", "TeamRedOdds", "TeamRedPoints" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 12, 16, 16, 54, 31, 495, DateTimeKind.Local).AddTicks(5748), 40m, 1, 1, 0, 1, 1, "Alpha vs Bravo", true, null, 2, 124, 0, 1, -126, 0 },
                    { 2, new DateTime(2022, 12, 18, 16, 54, 31, 495, DateTimeKind.Local).AddTicks(5785), 40m, 2, 1, 0, 2, 1, "Bravo vs Alpha", false, null, 1, -140, 0, 2, 140, 0 },
                    { 3, new DateTime(2022, 12, 18, 16, 54, 31, 495, DateTimeKind.Local).AddTicks(5789), 40m, 3, 1, 0, 2, 1, "Charlie vs Delta", false, null, 4, 150, 0, 3, -160, 0 }
                });

            migrationBuilder.InsertData(
                table: "TeamRequests",
                columns: new[] { "Id", "PlayerId", "TeamId", "TeamRequestStatus", "TeamRequestType" },
                values: new object[,]
                {
                    { 1, 5, 1, 1, 0 },
                    { 2, 6, 2, 1, 0 }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "AverageAmmoExpendedPerGame", "Description", "FeetPerSecond", "FireRate", "ImageUrl", "Name", "PlayerId", "PreferedEngagementDistance", "Price", "VendorId", "WeaponType" },
                values: new object[,]
                {
                    { 1, 40, "Small pistol", 120.0, 300.0, "https://arms-bg.com/wp-content/uploads/imported/2.6412_17_links_2000_1125_0-600x600.jpg", "Glock 17", 10, 0, 20m, null, 0 },
                    { 2, 15, "Shotgun", 150.0, 100.0, "https://www.airsoft.bg/products/1334236938_160704__031226700_1656_02022011.jpg", "Benelli M3", 7, 1, 50m, null, 1 },
                    { 3, 90, "Popular Assault Rifle", 300.0, 666.0, "https://arms-bg.com/wp-content/uploads/2021/11/cyma-cm002a1-600x600.jpg", "M4A1", 5, 1, 100m, null, 3 },
                    { 4, 15, "Sniper Rifle", 500.0, 20.0, "https://cqb.bg/wp-content/uploads/1152193374_1.jpg", "AWP", 1, 2, 130m, null, 4 },
                    { 5, 120, "Good Smg", 110.0, 700.0, "https://nelo-mill.com/wp-content/uploads/2019/07/2.6311_MP5A5_links_ret_613_400_0.jpg", "Mp5", 2, 0, 70m, null, 2 }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "AverageAmmoExpendedPerGame", "Description", "FeetPerSecond", "FireRate", "ImageUrl", "Name", "PlayerId", "PreferedEngagementDistance", "Price", "VendorId", "WeaponType" },
                values: new object[,]
                {
                    { 6, 100, "Very fast fire rate", 280.0, 1200.0, "https://shop.crgroup.bg/media/t44s4/2543.jpg", "FAMAS", 9, 1, 80m, null, 3 },
                    { 7, 180, "Machine gun with good fire rate and good accuracy", 260.0, 900.0, "https://cqb.bg/wp-content/uploads/1152226012_6.jpg", "M249", 6, 1, 150m, null, 5 },
                    { 8, 20, "Old fashioned sniper rifle for classy people", 400.0, 15.0, "https://cqb.bg/wp-content/uploads/1152190150_3.jpg", "Kar98k", 8, 2, 110m, null, 4 },
                    { 9, 60, "Versatile assault rifle with good accuracy", 240.0, 600.0, "https://www.airsoft.bg/products/1333793093_Kalashnikov-AKM-AEG_CG120914_airsoft_zm.jpg", "AKM", 4, 1, 90m, null, 3 },
                    { 10, 300, "Overkill", 290.0, 3000.0, "https://www.evike.com/images/large/34905.jpg", "Minigun", 12, 0, 250m, null, 5 },
                    { 11, 40, "Small pistol. Good in tight quarters.", 110.0, 300.0, "https://i.pinimg.com/736x/92/86/dc/9286dcbb94e7faf0d648e63dd199de2f--products-is-.jpg", "USP", 13, 0, 25m, null, 0 },
                    { 12, 70, "Assault rifle good in most ranges.", 200.0, 666.0, "https://iwi.net/wp-content/uploads/2021/08/ACE_22_IWI_3687.jpg", "Galil", 14, 1, 60m, null, 3 },
                    { 13, 10, "Old sniper rifle frow WW2.", 240.0, 15.0, "https://static3.gunfire.com/eng_pl_Mosin-Nagant-1891-30-rifle-replica-with-PU-scope-1152227065_1.webp", "Mosin Nagant", 3, 2, 50m, null, 4 },
                    { 14, 90, "British bullpup assault rifle", 400.0, 650.0, "https://static4.gunfire.com/eng_pl_L85A2-Assault-Rifle-Replica-1152213851_1.webp", "L85A2", 11, 1, 80m, null, 3 },
                    { 15, 120, "Small automatic pistol", 125.0, 900.0, "https://media.mwstatic.com/product-images/src/alt1/850/850613a1.jpg?imwidth=480", "Glock 18 auto", 15, 0, 30m, null, 0 },
                    { 16, 20, "Only for terminators", 140.0, 50.0, "https://taylorsfirearms.com/media/catalog/product/cache/a309b6cb2676967c1a0c3ab51e5fa3c7/1/8/1887bl-l_2641_2_.jpg", "Winchester model 1887", null, 0, 70m, 1, 1 },
                    { 17, 45, "Burst fire  assault rifle .", 250.0, 400.0, "https://cdn.shopify.com/s/files/1/1333/2651/products/Copy_of_M16-A3-01_grande.jpg?v=1571467240", "M16A3", null, 1, 60m, 1, 3 },
                    { 18, 75, "Popular Silenced Assault Rifle", 310.0, 600.0, "https://esportzbet.com/wp-content/uploads/2019/05/dw1-min.png", "M4A1-S", null, 1, 110m, 1, 3 },
                    { 19, 15, "Sniper Rifle good for long range.", 450.0, 20.0, "https://i0.wp.com/cms.sofrep.com/wp-content/uploads/2013/07/M40A5.jpg?fit=562%2C198&ssl=1", "M40A5", null, 2, 120m, 1, 4 },
                    { 20, 150, "Good Smg with insane fire rate", 100.0, 1200.0, "https://static4.gunfire.com/eng_pl_KRISS-Vector-Submachine-Gun-Replica-Half-Tan-1152223174_6.webp", "Kriss Vector", null, 0, 100m, 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "Bets",
                columns: new[] { "Id", "BetStatus", "CreditsBet", "GameId", "Odds", "UserId", "WinningTeamId" },
                values: new object[,]
                {
                    { 1, 0, 20m, 1, -122, "cc1cb39b-c0cf-41ed-856c-d3943aec605a", 1 },
                    { 2, 0, 20m, 1, 120, "1f5be09b-2910-4ac0-8ff5-5c525ddf1b61", 2 },
                    { 3, 0, 20m, 1, 122, "799495ef-8794-491d-94d9-6bd37d51ba40", 2 },
                    { 4, 0, 30m, 1, -122, "6f4bc586-751a-4a4b-8fec-4c7145b47a3e", 1 }
                });

            migrationBuilder.InsertData(
                table: "GameBetCreditsContainers",
                columns: new[] { "Id", "BetsArePaidOut", "GameId", "TeamBlueCreditsBet", "TeamRedCreditsBet" },
                values: new object[,]
                {
                    { 1, false, 1, 40m, 50m },
                    { 2, false, 2, 0m, 0m },
                    { 3, false, 3, 0m, 0m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AmmoBoxes_VendorId",
                table: "AmmoBoxes",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_GameId",
                table: "Bets",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_UserId",
                table: "Bets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Clothes_PlayerId",
                table: "Clothes",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Clothes_VendorId",
                table: "Clothes",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_GameBetCreditsContainers_GameId",
                table: "GameBetCreditsContainers",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameModeId",
                table: "Games",
                column: "GameModeId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_MapId",
                table: "Games",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_MatchmakerId",
                table: "Games",
                column: "MatchmakerId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_TeamBlueId",
                table: "Games",
                column: "TeamBlueId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_TeamRedId",
                table: "Games",
                column: "TeamRedId");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_GameModeId",
                table: "Maps",
                column: "GameModeId");

            migrationBuilder.CreateIndex(
                name: "IX_Matchmakers_UserId",
                table: "Matchmakers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_PlayerClassId",
                table: "Players",
                column: "PlayerClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_UserId",
                table: "Players",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleRequests_RoleId",
                table: "RoleRequests",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleRequests_UserId",
                table: "RoleRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRequests_PlayerId",
                table: "TeamRequests",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRequests_TeamId",
                table: "TeamRequests",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_UserId",
                table: "Vendors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_PlayerId",
                table: "Weapons",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_VendorId",
                table: "Weapons",
                column: "VendorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmmoBoxes");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Bets");

            migrationBuilder.DropTable(
                name: "Clothes");

            migrationBuilder.DropTable(
                name: "GameBetCreditsContainers");

            migrationBuilder.DropTable(
                name: "RoleRequests");

            migrationBuilder.DropTable(
                name: "TeamRequests");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "Matchmakers");

            migrationBuilder.DropTable(
                name: "PlayerClasses");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "GameModes");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
