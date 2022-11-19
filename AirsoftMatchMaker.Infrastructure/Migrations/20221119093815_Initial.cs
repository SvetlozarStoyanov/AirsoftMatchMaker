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
                    PlayerStatus = table.Column<int>(type: "int", nullable: false),
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
                    TeamBlueId = table.Column<int>(type: "int", nullable: false),
                    TeamBluePoints = table.Column<int>(type: "int", nullable: false)
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
                name: "Weapons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FeetPerSecond = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FireRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    TeamRedRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TeamBlueRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
                    { "202efe8b-7748-49ca-834c-fd1c37978ab2", 0, "9029861f-1e35-4d84-b969-d862e0a9eb0b", 200m, "Georgi@gmail.com", false, false, null, "GEORGI@GMAIL.COM", "GEORGI", "AQAAAAEAACcQAAAAECIPBzDKA6Hp+MkLSHxdWHVU3RGjO0ffXGYYUC+anj2/ncK5+PuUiHsXx8wMy/jwLA==", null, false, "77b936cc-3260-4e97-9976-9a49d43faf88", false, "Georgi" },
                    { "2a1bd8b6-6d06-470b-9dda-fe88aa1bf5e8", 0, "7ad2039d-e240-4eab-a808-87fdced2c671", 200m, "Ivan@gmail.com", false, false, null, "IVAN@GMAIL.COM", "IVAN", "AQAAAAEAACcQAAAAEH6JDRNPg7qmPxyHqvt2sX106P1+TiPV3URBFxXxWTnRZRROxb8cjAyM50lDersMGw==", null, false, "705cf84f-f613-49c9-ad98-c93b2d3ac06d", false, "Ivan" },
                    { "4d64daba-17d4-452c-af3e-5d731a250283", 0, "e9da12ad-613c-498a-8609-10395f5659fb", 200m, "Michael@gmail.com", false, false, null, "MICHAEL@GMAIL.COM", "MICHAEL", "AQAAAAEAACcQAAAAEJxR+QXTqD/C0BruHkp4MsoQCaFFRBr7sG5eFzNxTdPawyj8YRlegplK73orVRFJ0g==", null, false, "24c27739-3fb4-476d-b985-d6e980442ca4", false, "Michael" },
                    { "56d661fd-2339-498a-bd7e-c95f37908b28", 0, "57433a61-ffe3-470f-8840-dace1d202926", 200m, "Petar@gmail.com", false, false, null, "PETAR@GMAIL.COM", "PETAR", "AQAAAAEAACcQAAAAEN0sxerOBWqctbEWiWr8oyS3tSWzP41Q3QP8PsxquM6+hPeyZh8HE++KSypzpcomhw==", null, false, "ff1be511-23c2-4561-b6ba-2dc75f761386", false, "Petar" },
                    { "77388c0c-698c-4df9-9ad9-cef29116b666", 0, "ac7d599b-6d63-4160-94e4-243609519404", 200m, "Vasil@gmail.com", false, false, null, "VASIL@GMAIL.COM", "VASIL", "AQAAAAEAACcQAAAAEHqKiNANa1scU6pg4tOLezFE36OoXGL/O32yik2SE94uNNz4vNaxxaUW3uTcP3tcfg==", null, false, "e498f9e7-05a4-4281-970c-ba2906be8275", false, "Vasil" },
                    { "b2451308-1197-4362-be78-f7ea7ca35fe9", 0, "9c4cfdd7-cc82-4c24-9df3-d94c33ebed4d", 200m, "Alexander@gmail.com", false, false, null, "ALEXANDER@GMAIL.COM", "ALEXANDER", "AQAAAAEAACcQAAAAEJ7sbNECx9v/RgJi0zjyhQcRKGYGF/1yq3k3guCPR5m4dB+rv8JMYD33BHnS7tDwTw==", null, false, "bf45fc94-1ea1-46b9-a00f-6b2d385965f4", false, "Alexander" },
                    { "c5d9e543-7c2f-4345-a014-ebd860eef718", 0, "d9b33de3-54f8-4082-bbc6-d7daaa393899", 200m, "Krum@gmail.com", false, false, null, "KRUM@GMAIL.COM", "KRUM", "AQAAAAEAACcQAAAAEAfVSBevSD9RMHE1T7R9FjOkxbyGRGTPUtnxVTCQpkpw7CYFQ+8XvDR345/WCrB4bw==", null, false, "8f81acbc-b685-4387-adfe-4de36ea902f5", false, "Krum" },
                    { "cc1cb39b-c0cf-41ed-856c-d3943aec605a", 0, "c4b3c204-bcfa-4c56-bd6c-beb7ecc04396", 200m, "Joe@gmail.com", false, false, null, "JOE@GMAIL.COM", "JOE", "AQAAAAEAACcQAAAAEDm5pQl+aFw1BpkNpS1Gqug/S1y28dIXj+G8x0v6D4wd97lkaA1tWuUSpK7mZU+Kpg==", null, false, "eb0c4a83-b586-4154-9bf4-1dc0739ea593", false, "Joe" },
                    { "f3534aed-259b-4ff7-b816-15e8207e084a", 0, "75d140cc-65b7-4480-81a6-ea1d51e775d9", 200m, "Todor@gmail.com", false, false, null, "TODOR@GMAIL.COM", "TODOR", "AQAAAAEAACcQAAAAEPtet7Oqauogo6hdjN+9y75iBOMqu8sX5V9bLtW1fEEI99oW6ulO8PjlRnAfJuP/Mw==", null, false, "4a152b3c-80af-4b54-9066-7c7452e371fc", false, "Todor" },
                    { "f580c1f9-d41f-455e-b4ec-705b834e4b19", 0, "08d52d2f-d147-4fcc-a691-2f1d1e55a049", 200m, "Hank@gmail.com", false, false, null, "HANK@GMAIL.COM", "HANK", "AQAAAAEAACcQAAAAEJ15moQfa6NzbPsmYTJvpgqWBoY0hn45jxEHh4qSUfYC+zJ2584LqAQdv2WzUpTeYA==", null, false, "8058f92f-2ebb-45b5-b6cf-c820b9bf7358", false, "Hank" }
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
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "202efe8b-7748-49ca-834c-fd1c37978ab2" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "202efe8b-7748-49ca-834c-fd1c37978ab2" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "2a1bd8b6-6d06-470b-9dda-fe88aa1bf5e8" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "2a1bd8b6-6d06-470b-9dda-fe88aa1bf5e8" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "4d64daba-17d4-452c-af3e-5d731a250283" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "4d64daba-17d4-452c-af3e-5d731a250283" },
                    { "52f73adc-3c27-40de-b00e-2e2b382da84c", "56d661fd-2339-498a-bd7e-c95f37908b28" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "56d661fd-2339-498a-bd7e-c95f37908b28" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "77388c0c-698c-4df9-9ad9-cef29116b666" },
                    { "d0bd950a-e2d5-46cf-a6c1-1f0efa4144ce", "77388c0c-698c-4df9-9ad9-cef29116b666" },
                    { "6b3c10a1-4a55-411a-8dca-49574cb55e74", "b2451308-1197-4362-be78-f7ea7ca35fe9" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "b2451308-1197-4362-be78-f7ea7ca35fe9" },
                    { "b48af83e-7873-4ecd-82de-5d517e7b31f9", "c5d9e543-7c2f-4345-a014-ebd860eef718" },
                    { "fc9628b0-fa92-4be1-9f1f-9095d66f1ff8", "c5d9e543-7c2f-4345-a014-ebd860eef718" },
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
                    { 1, 1, "Large forest map in Norway", 1, null, 2, "Bjorn Forest", 1 },
                    { 2, 0, "Small Field in California", 2, null, 0, "Clear Field", 2 },
                    { 3, 2, "Extra Large snowy map in Russia", 2, null, 3, "Snow Villa", 3 }
                });

            migrationBuilder.InsertData(
                table: "Matchmakers",
                columns: new[] { "Id", "IsActive", "UserId" },
                values: new object[] { 1, true, "c5d9e543-7c2f-4345-a014-ebd860eef718" });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Ammo", "IsActive", "PlayerClassId", "PlayerStatus", "SkillLevel", "SkillPoints", "TeamId", "UserId" },
                values: new object[,]
                {
                    { 1, 100, true, 1, 2, 0, 100, 1, "202efe8b-7748-49ca-834c-fd1c37978ab2" },
                    { 2, 100, true, 2, 2, 0, 100, 1, "2a1bd8b6-6d06-470b-9dda-fe88aa1bf5e8" },
                    { 3, 600, true, 6, 2, 0, 100, 2, "4d64daba-17d4-452c-af3e-5d731a250283" },
                    { 4, 200, true, 7, 2, 0, 100, 2, "b2451308-1197-4362-be78-f7ea7ca35fe9" },
                    { 5, 450, true, 3, 1, 0, 100, null, "f3534aed-259b-4ff7-b816-15e8207e084a" },
                    { 6, 450, true, 3, 1, 0, 100, null, "f580c1f9-d41f-455e-b4ec-705b834e4b19" }
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
                    { 1, 0, "Hard to spot in forest.", null, "Green outfit", 4, 50m, null },
                    { 2, 0, "Hard to spot in forest.", null, "Green outfit", null, 50m, 1 },
                    { 3, 0, "Hard to spot in forest.", null, "Green outfit", 1, 50m, null },
                    { 4, 0, "Hard to spot in forest.", null, "Green Ghillie Suit", 5, 80m, null },
                    { 5, 0, "Hard to spot in forest.", null, "Green Ghillie Suit", null, 80m, 1 },
                    { 6, 0, "Hard to spot in forest.", null, "Green Army camouflage", null, 60m, 1 },
                    { 7, 0, "Hard to spot in forest.", null, "Green Army camouflage", null, 60m, 1 },
                    { 8, 2, "Hard to spot in urban enviroment.", null, "Urban outfit", null, 40m, 1 },
                    { 9, 2, "Hard to spot in urban enviroment.", null, "Urban outfit", 3, 40m, null },
                    { 10, 2, "Hard to spot in urban enviroment.", null, "Urban outfit", null, 40m, 1 },
                    { 11, 2, "Hard to spot in urban enviroment.", null, "Urban outfit", null, 40m, 1 },
                    { 12, 2, "Hard to spot in urban enviroment.", null, "Gray tracksuit", null, 20m, 1 },
                    { 13, 3, "Hard to spot in field.", null, "Brown Army outfit ", 2, 30m, null },
                    { 14, 3, "Hard to spot in field.", null, "Brown Army outfit ", null, 30m, 1 },
                    { 15, 3, "Hard to spot in field.", null, "Brown Army outfit ", null, 30m, 1 },
                    { 16, 3, "Hard to spot in field.", null, "Brown Ghillie suit", null, 45m, 1 },
                    { 17, 3, "Hard to spot in field.", null, "Brown Ghillie suit", null, 45m, 1 },
                    { 18, 3, "Hard to spot in field.", null, "Brown Ghillie suit", 6, 45m, null },
                    { 19, 1, "Hard to spot in snow.", null, "White outfit", null, 40m, 1 },
                    { 20, 1, "Hard to spot in snow.", null, "White outfit", null, 40m, 1 },
                    { 21, 1, "Hard to spot in snow.", null, "White Ghillie suit", null, 55m, 1 },
                    { 22, 1, "Hard to spot in snow.", null, "White Ghillie suit", null, 55m, 1 },
                    { 23, 1, "Hard to spot in snow.", null, "White Ghillie suit", null, 55m, 1 }
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "Date", "EntryFee", "GameModeId", "GameStatus", "MapId", "MatchmakerId", "Name", "Result", "TeamBlueId", "TeamBluePoints", "TeamRedId", "TeamRedPoints" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 11, 19, 12, 38, 14, 446, DateTimeKind.Local).AddTicks(8791), 40m, 1, 0, 1, 1, "First Game", null, 2, 0, 1, 0 },
                    { 2, new DateTime(2022, 11, 20, 11, 38, 14, 446, DateTimeKind.Local).AddTicks(8827), 40m, 1, 0, 2, 1, "Rematch Game", null, 1, 0, 2, 0 }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "AverageAmmoExpendedPerGame", "Description", "FeetPerSecond", "FireRate", "ImageUrl", "Name", "PlayerId", "PreferedEngagementDistance", "Price", "VendorId", "WeaponType" },
                values: new object[,]
                {
                    { 1, 40, "Small pistol", 1.20m, 300m, "https://arms-bg.com/wp-content/uploads/imported/2.6412_17_links_2000_1125_0-600x600.jpg", "Glock 17", 3, 0, 20m, null, 0 },
                    { 2, 15, "Shotgun", 1.30m, 100m, "https://www.airsoft.bg/products/1334236938_160704__031226700_1656_02022011.jpg", "Benelli M3", 1, 1, 50m, null, 1 },
                    { 3, 90, "Popular Assault Rifle", 1.45m, 666m, "https://arms-bg.com/wp-content/uploads/2021/11/cyma-cm002a1-600x600.jpg", "M4A1", 4, 1, 100m, null, 3 },
                    { 4, 15, "Sniper Rifle", 1.60m, 20m, "https://cqb.bg/wp-content/uploads/1152193374_1.jpg", "AWP", 5, 2, 130m, null, 4 },
                    { 5, 120, "Good Smg", 1.10m, 700m, "https://nelo-mill.com/wp-content/uploads/2019/07/2.6311_MP5A5_links_ret_613_400_0.jpg", "Mp5", null, 0, 70m, 1, 2 },
                    { 6, 100, "Very fast fire rate", 1.25m, 1200m, "https://shop.crgroup.bg/media/t44s4/2543.jpg", "FAMAS", null, 1, 80m, 1, 3 },
                    { 7, 180, "Machine gun with good fire rate and good accuracy", 1.35m, 900m, "https://cqb.bg/wp-content/uploads/1152226012_6.jpg", "M249", null, 1, 150m, 1, 5 },
                    { 8, 20, "Old fashioned sniper rifle for classy people", 1.45m, 15m, "https://cqb.bg/wp-content/uploads/1152190150_3.jpg", "Kar98k", null, 2, 110m, 1, 4 },
                    { 9, 60, "Versatile assault rifle with good accuracy", 1.30m, 600m, "https://www.airsoft.bg/products/1333793093_Kalashnikov-AKM-AEG_CG120914_airsoft_zm.jpg", "AKM", 2, 1, 90m, null, 3 },
                    { 10, 300, "Overkill", 1.50m, 3000m, "https://www.evike.com/images/large/34905.jpg", "Minigun", null, 0, 250m, 1, 5 }
                });

            migrationBuilder.InsertData(
                table: "Bets",
                columns: new[] { "Id", "BetStatus", "CreditsBet", "GameId", "TeamBlueRate", "TeamRedRate", "UserId", "WinningTeamId" },
                values: new object[] { 1, 0, 20m, 1, 0.65m, 1.25m, "b2451308-1197-4362-be78-f7ea7ca35fe9", 1 });

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
                name: "RoleRequests");

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
