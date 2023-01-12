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
                    { "0a9aab7f-739a-41d8-b18d-8b797c7a2dfe", 0, "305d9f45-6a63-4a86-893d-26345b649c26", 200m, "Liam@gmail.com", false, false, null, "LIAM@GMAIL.COM", "LIAM", "AQAAAAEAACcQAAAAEEVl6fnjBKLWAzAC14F/aSkipd2WI2wfMlgsdhyVONq9VWPUDuJGaV1Vr+MqaaAL3A==", null, false, "57cb6261-f7d5-43e3-8938-61ebdb7b10f3", false, "Liam" },
                    { "14677dd9-7de7-41c0-9418-e43ddcf64859", 0, "11e13195-e7da-4451-ad54-d2601de7c4e1", 200m, "John@gmail.com", false, false, null, "JOHN@GMAIL.COM", "JOHN", "AQAAAAEAACcQAAAAEDO50VOWULnJry387CHeEOc8IB3kJRlojoZotpo6ZCKVa4V0XDMV4y2rpZ2o8UUSow==", null, false, "a8aba786-acd3-4da1-be83-054b525c6fbc", false, "John" },
                    { "18a322e4-ade8-4f13-8981-4cac7be64b9c", 0, "c8cce559-134e-47a9-8d6c-afc0b3fc3841", 200m, "Stoyan@gmail.com", false, false, null, "STOYAN@GMAIL.COM", "STOYAN", "AQAAAAEAACcQAAAAENeRxFPcTgtbULw70aytFUGtIP50T9uSAgPtivl3q3gCCPkbtpx0z80aTdtvj7+QEg==", null, false, "e6170186-9bae-4daa-8ed2-56ee2c541be9", false, "Stoyan" },
                    { "1f1087d3-a55a-4b7a-932e-1c3f9817fcf0", 0, "745372a1-5ce5-4978-a356-435762b47613", 200m, "Daniel@gmail.com", false, false, null, "DANIEL@GMAIL.COM", "DANIEL", "AQAAAAEAACcQAAAAEHVoZvC05bFzYJUpzAtOfIH7Un7ANeIhqjbJqG3i4tww5qC5w3TcX/Dm1xWT+n4umw==", null, false, "32bfc75c-1054-4663-970d-94cfba008972", false, "Daniel" },
                    { "1f5be09b-2910-4ac0-8ff5-5c525ddf1b61", 0, "73e97acb-9d9f-4e6a-a9e2-d01dc4982985", 200m, "Paul@gmail.com", false, false, null, "PAUL@GMAIL.COM", "PAUL", "AQAAAAEAACcQAAAAEH1M95IFJASdt7x093sbBDmHL2wMWfPJnFLABInN8S/pSDR1v4CQL+z6XYZgnMMsUw==", null, false, "fade49f4-c452-497b-9fb0-d847090dd83a", false, "Paul" },
                    { "202efe8b-7748-49ca-834c-fd1c37978ab2", 0, "89fdfeff-f46a-4411-95ee-b54b6a3219bc", 200m, "Georgi@gmail.com", false, false, null, "GEORGI@GMAIL.COM", "GEORGI", "AQAAAAEAACcQAAAAEEiWDC6WI/61n029UqHyJkS8fvpXF9P79/nzUf5UIPZbVrKl8itN4CKmkU1yqDVB2w==", null, false, "cdf999e7-40f5-4625-a80e-d0c42c61df8b", false, "Georgi" },
                    { "2a1bd8b6-6d06-470b-9dda-fe88aa1bf5e8", 0, "52c42abb-2e1e-4578-abaf-7ab3c160890a", 200m, "Ivan@gmail.com", false, false, null, "IVAN@GMAIL.COM", "IVAN", "AQAAAAEAACcQAAAAENgSNQAMfUER+orF8L8mAT4IeAwLQENWWiiEtXtYfMyij40NHFx10bTQVq70qJZ3yg==", null, false, "5cb2a0f5-1b8d-409f-b81f-5fc6d7e94be0", false, "Ivan" },
                    { "3bf3238b-ab04-4945-8bd0-1eabf8a208d5", 0, "8f01a575-04a1-4dee-afa5-bec59b26e338", 200m, "Tihomir@gmail.com", false, false, null, "TIHOMIR@GMAIL.COM", "TIHOMIR", "AQAAAAEAACcQAAAAED6Y7BVQ+eVazoDC5SbVr0p08BKHFalaQw7npoWxc3Wtb+G+0+DT7C+pVkjkY0zkbg==", null, false, "50530557-27a5-4dc1-b6a1-df8b90a08314", false, "Tihomir" },
                    { "4d64daba-17d4-452c-af3e-5d731a250283", 0, "3aa643d9-bdee-44ab-aedd-dd2e7389dde2", 200m, "Michael@gmail.com", false, false, null, "MICHAEL@GMAIL.COM", "MICHAEL", "AQAAAAEAACcQAAAAEFzk9D/K3G+/qs0dQcGuGuyaZLY2cPNrCJSE+ehYUFtqYMxwwvH5V1y/eS9oPBofeg==", null, false, "72819c6f-baef-456e-8be1-2e19e06f8515", false, "Michael" },
                    { "56d661fd-2339-498a-bd7e-c95f37908b28", 0, "85c8e7ae-05fb-4f96-95a3-e90a443012d0", 200m, "Admin@gmail.com", false, false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEIAOPfqr2jMGxJhGB69UA4NrcKvehiHndPqz6EjMFqzr0LwUzPoA1BnSDkbkz+HN3w==", null, false, "d534ba10-cdfb-451e-9071-5f433d05b684", false, "Admin" },
                    { "5f83ea0f-418b-463f-9a52-bf1b9eac8bc6", 0, "09d988e3-4548-4d47-aedf-7fbc514da447", 200m, "Philip@gmail.com", false, false, null, "PHILIP@GMAIL.COM", "PHILIP", "AQAAAAEAACcQAAAAECqRVT+h8ypVLCgcr2h/ZcwYXtOVHXpxDy8UM+pme2xTa2Nc0bxuzhgIRD32xy4SZg==", null, false, "112678c6-6469-4f51-bc38-e92f3ff7d2ec", false, "Philip" },
                    { "6f4bc586-751a-4a4b-8fec-4c7145b47a3e", 0, "84b24c4f-1a7c-475c-b547-e9db7054bba7", 200m, "Dimitrichko@gmail.com", false, false, null, "DIMITRICHKO@GMAIL.COM", "DIMITRICHKO", "AQAAAAEAACcQAAAAELIYGE2hDt15ccnUTOrzdIkkP9cNqMrA6EJw/Z3tJFz/QRJpM6dC4h+KcwT8kZi6iQ==", null, false, "d1f95a51-054d-43b5-b0e0-80b18b58c203", false, "Dimitrichko" },
                    { "77388c0c-698c-4df9-9ad9-cef29116b666", 0, "259fa712-497b-4eb2-b45e-6dd49468f013", 200m, "Vendor@gmail.com", false, false, null, "VENDOR@GMAIL.COM", "VENDOR", "AQAAAAEAACcQAAAAECNGHZIhuQXndT4vknyASIQssJDg5GXotbBkcZQoldXr0sB3jOEfEwbDNt2zh0Gi6w==", null, false, "237b8765-cbe2-4915-9f70-c32aa51b9d3e", false, "Vendor" },
                    { "799495ef-8794-491d-94d9-6bd37d51ba40", 0, "f7062c85-288e-4fb3-b494-eeff7c987eed", 200m, "Ivaylo@gmail.com", false, false, null, "IVAYLO@GMAIL.COM", "IVAYLO", "AQAAAAEAACcQAAAAEJlP7c1r6u+mVP4ZHoY6I/Ksg+c8mnX/R4rdb10bm1RNiuFVOO1vsNM/KNdlm9VWpg==", null, false, "ce9b2580-f8f2-49b1-8985-cbf3c2bd917d", false, "Ivaylo" },
                    { "b2451308-1197-4362-be78-f7ea7ca35fe9", 0, "3a8c4400-35a1-4821-8c3e-df468cf75dd7", 200m, "Alexander@gmail.com", false, false, null, "ALEXANDER@GMAIL.COM", "ALEXANDER", "AQAAAAEAACcQAAAAEA3JPz9EMpiLXnCJTjol6E/DkjLJjzIsK8ZQ16j2PoRFh0Ny6V2Q2uNanhJuQ9Z//g==", null, false, "1a61b55a-7a96-4363-9bd3-6ede3ac58fed", false, "Alexander" },
                    { "c5d9e543-7c2f-4345-a014-ebd860eef718", 0, "480df741-c371-472a-9b12-f6e4520ac3b5", 200m, "Matchmaker@gmail.com", false, false, null, "MATCHMAKER@GMAIL.COM", "MATCHMAKER", "AQAAAAEAACcQAAAAEL37yqzovoN8+KikAP1EkvLES9FdN9lMDhaPk/ahrMv9y0lCenwHMZvJ7iv9cN8pbw==", null, false, "32e932f0-0a4a-404f-b54f-b66e400c6971", false, "Matchmaker" },
                    { "c95011ef-d0e4-49c0-bbdd-1b9985bf7a74", 0, "403eeab0-4801-4163-a9aa-d5a93dc88f0b", 200m, "Walter@gmail.com", false, false, null, "WALTER@GMAIL.COM", "WALTER", "AQAAAAEAACcQAAAAENasbsRzcM6tpvB8Wgse0yd1nj9yRJYkIcrvKWIKktjkZuTRmup5jeIbStduy23W6Q==", null, false, "e53c68bd-93f5-49a8-a289-38f648776ed6", false, "Walter" },
                    { "cc1cb39b-c0cf-41ed-856c-d3943aec605a", 0, "6d4ccd78-2cc3-45b0-b4fa-38534322f5c9", 200m, "Joe@gmail.com", false, false, null, "JOE@GMAIL.COM", "JOE", "AQAAAAEAACcQAAAAEFWjcWWqH/myf0LNjrAuDZnDhTiYlIdv2FD1G7Y2C6HHPdWwg+wMho32REDVTdi7iA==", null, false, "f5beab33-0ebc-4b34-9b95-57f59238303f", false, "Joe" },
                    { "f3534aed-259b-4ff7-b816-15e8207e084a", 0, "aa5be5f6-3249-4332-b6e9-8e5c95c3b4ff", 200m, "Todor@gmail.com", false, false, null, "TODOR@GMAIL.COM", "TODOR", "AQAAAAEAACcQAAAAECFzP2WTD/b1y7cjGBxICsKV6whDMvGJYyRWPT3XrP2j4V+yqwgme0kOJfdbI4oEYg==", null, false, "d143d003-7748-4c30-afdc-e6ba4d8d8eb7", false, "Todor" },
                    { "f580c1f9-d41f-455e-b4ec-705b834e4b19", 0, "29d1efd7-af0c-4d66-81ef-737ccf80d0a1", 200m, "Hank@gmail.com", false, false, null, "HANK@GMAIL.COM", "HANK", "AQAAAAEAACcQAAAAEAzFI4uNwUFvN1DnVxnqKP/UpF7T8ArDOSu13wcO+qeTtWSDeEo16bLz6n8UvNbpcA==", null, false, "290bbb88-de71-4089-8bd7-7d1ff6045a70", false, "Hank" }
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
                    { 3, 0, "Charlie", 0 },
                    { 4, 0, "Delta", 0 }
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
                    { 2, 0, "Hard to spot in forest.", null, "Green outfit", 9, 50m, null },
                    { 3, 0, "Hard to spot in forest.", null, "Green outfit", 14, 50m, null },
                    { 4, 0, "Hard to spot in forest.", null, "Green outfit", 4, 50m, null },
                    { 5, 0, "Hard to spot in forest.", null, "Green outfit", 8, 50m, null },
                    { 6, 0, "Hard to spot in forest.", null, "Green outfit", 1, 50m, null },
                    { 7, 0, "Hard to spot in forest.", null, "Green outfit", 5, 50m, null },
                    { 8, 0, "Hard to spot in forest.", null, "Green Ghillie Suit", 11, 80m, null },
                    { 9, 0, "Hard to spot in forest.", null, "Green Ghillie Suit", 13, 80m, null },
                    { 10, 0, "Hard to spot in forest.", null, "Green Army camouflage", 2, 60m, null },
                    { 11, 0, "Hard to spot in forest.", null, "Green Army camouflage", 10, 60m, null },
                    { 12, 2, "Hard to spot in urban enviroment.", null, "Urban outfit", 12, 40m, null },
                    { 13, 2, "Hard to spot in urban enviroment.", null, "Urban outfit", 6, 40m, null },
                    { 14, 2, "Hard to spot in urban enviroment.", null, "Urban outfit", 7, 40m, null },
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
                values: new object[] { 1, new DateTime(2023, 1, 11, 15, 23, 1, 420, DateTimeKind.Local).AddTicks(2714), 40m, 1, 1, 0, 1, 1, "Alpha vs Bravo", true, null, 2, 124, 0, 1, -126, 0 });

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
                    { 1, 40, "Small pistol", 120.0, 300.0, "https://arms-bg.com/wp-content/uploads/imported/2.6412_17_links_2000_1125_0-600x600.jpg", "Glock 17", 12, 0, 20m, null, 0 },
                    { 2, 15, "Shotgun", 150.0, 100.0, "https://www.airsoft.bg/products/1334236938_160704__031226700_1656_02022011.jpg", "Benelli M3", 2, 1, 50m, null, 1 },
                    { 3, 90, "Popular Assault Rifle", 300.0, 666.0, "https://arms-bg.com/wp-content/uploads/2021/11/cyma-cm002a1-600x600.jpg", "M4A1", 1, 1, 100m, null, 3 },
                    { 4, 15, "Sniper Rifle", 500.0, 20.0, "https://cqb.bg/wp-content/uploads/1152193374_1.jpg", "AWP", 8, 2, 130m, null, 4 },
                    { 5, 120, "Good Smg", 110.0, 700.0, "https://nelo-mill.com/wp-content/uploads/2019/07/2.6311_MP5A5_links_ret_613_400_0.jpg", "Mp5", 14, 0, 70m, null, 2 },
                    { 6, 100, "Very fast fire rate", 280.0, 1200.0, "https://shop.crgroup.bg/media/t44s4/2543.jpg", "FAMAS", 7, 1, 80m, null, 3 },
                    { 7, 180, "Machine gun with good fire rate and good accuracy", 260.0, 900.0, "https://cqb.bg/wp-content/uploads/1152226012_6.jpg", "M249", 10, 1, 150m, null, 5 }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "AverageAmmoExpendedPerGame", "Description", "FeetPerSecond", "FireRate", "ImageUrl", "Name", "PlayerId", "PreferedEngagementDistance", "Price", "VendorId", "WeaponType" },
                values: new object[,]
                {
                    { 8, 20, "Old fashioned sniper rifle for classy people", 400.0, 15.0, "https://cqb.bg/wp-content/uploads/1152190150_3.jpg", "Kar98k", 9, 2, 110m, null, 4 },
                    { 9, 60, "Versatile assault rifle with good accuracy", 240.0, 600.0, "https://www.airsoft.bg/products/1333793093_Kalashnikov-AKM-AEG_CG120914_airsoft_zm.jpg", "AKM", 4, 1, 90m, null, 3 },
                    { 10, 300, "Overkill", 290.0, 3000.0, "https://www.evike.com/images/large/34905.jpg", "Minigun", 5, 0, 250m, null, 5 },
                    { 11, 40, "Small pistol. Good in tight quarters.", 110.0, 300.0, "https://i.pinimg.com/736x/92/86/dc/9286dcbb94e7faf0d648e63dd199de2f--products-is-.jpg", "USP", 11, 0, 25m, null, 0 },
                    { 12, 70, "Assault rifle good in most ranges.", 200.0, 666.0, "https://iwi.net/wp-content/uploads/2021/08/ACE_22_IWI_3687.jpg", "Galil", 3, 1, 60m, null, 3 },
                    { 13, 10, "Old sniper rifle frow WW2.", 240.0, 15.0, "https://static3.gunfire.com/eng_pl_Mosin-Nagant-1891-30-rifle-replica-with-PU-scope-1152227065_1.webp", "Mosin Nagant", 6, 2, 50m, null, 4 },
                    { 14, 90, "British bullpup assault rifle", 400.0, 650.0, "https://static4.gunfire.com/eng_pl_L85A2-Assault-Rifle-Replica-1152213851_1.webp", "L85A2", 13, 1, 80m, null, 3 },
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
                values: new object[] { 1, false, 1, 40m, 50m });

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
