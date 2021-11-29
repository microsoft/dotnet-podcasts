using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Podcast.Infrastructure.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedCategory",
                columns: table => new
                {
                    FeedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedCategory", x => new { x.FeedId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_FeedCategory_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedCategory_Feeds_FeedId",
                        column: x => x.FeedId,
                        principalTable: "Feeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shows_Feeds_FeedId",
                        column: x => x.FeedId,
                        principalTable: "Feeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Explicit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Published = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episodes_Shows_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Shows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Genre" },
                values: new object[,]
                {
                    { new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), "Mobile" },
                    { new Guid("4d41bc8e-ef5e-439f-80fa-5e9873ea7a4a"), "Web" },
                    { new Guid("5f923017-86da-4793-9332-7b74197acc51"), "Microsoft" },
                    { new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), "Desktop" },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), "Community" },
                    { new Guid("bee871ad-750b-400b-91b0-c34056c92297"), "M365" }
                });

            migrationBuilder.InsertData(
                table: "Feeds",
                columns: new[] { "Id", "IsFeatured", "Url" },
                values: new object[,]
                {
                    { new Guid("1d9a5366-4258-4355-9a04-80680d12e05c"), false, "https://www.m365devpodcast.com/feed.xml" },
                    { new Guid("2a57fb68-8755-4d9a-a6ee-86bf106d7874"), false, "http://www.pwop.com/feed.aspx?show=dotnetrocks&filetype=master" },
                    { new Guid("54179124-9094-4091-9891-f29868298575"), true, "http://feeds.gimletcreative.com/dotfuture" },
                    { new Guid("5660e7b9-7555-4d3f-b863-df658440820b"), false, "http://feeds.codenewbie.org/cnpodcast.xml" },
                    { new Guid("57da3b70-bdfc-454e-81f0-fb4ee7ba68d3"), true, "https://s.ch9.ms/Shows/Hello-World/feed/mp3" },
                    { new Guid("5ebb45a0-5fff-49ac-a5d5-691e6314ce71"), false, "https://thedotnetcorepodcast.libsyn.com/rss" },
                    { new Guid("5fb313f5-ca48-49cd-a9bd-7ea830cfa984"), false, "https://feeds.simplecast.com/gvtxUiIf" },
                    { new Guid("6d6b95a4-88f8-4e52-bacd-362c0024362c"), false, "https://microsoftmechanics.libsyn.com/rss" },
                    { new Guid("71a2df8c-cb34-4203-b045-375695439b8b"), false, "https://devchat.tv/podcasts/adventures-in-dotnet/feed/" },
                    { new Guid("76c2dd2f-7232-4842-9808-f6a389de510e"), false, "http://awayfromthekeyboard.com/feed/" },
                    { new Guid("7941709e-dbd5-4d04-8c90-e304a4645005"), false, "https://upwards.libsyn.com/rss" },
                    { new Guid("7dd803ce-d834-4ae2-8f37-6f6e0d1977cc"), false, "https://nullpointers.io/feed/podcast.rss" },
                    { new Guid("89a51256-4674-4a11-8f2a-bd44ce325d14"), false, "https://listenbox.app/f/NRGnlt0wQqB7" },
                    { new Guid("a8791dd6-0ad8-48b7-b66e-8c6d67719626"), false, "http://feeds.feedburner.com/NoDogmaPodcast" },
                    { new Guid("bc2cab2b-d6f4-48ae-9602-3041a55ee6be"), false, "https://feeds.fireside.fm/gonemobile/rss" },
                    { new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d"), false, "https://msdevshow.libsyn.com/rss" },
                    { new Guid("c2b49169-0bb5-444a-86a4-14a476cf7620"), false, "https://feeds.simplecast.com/cRTTfxcT" },
                    { new Guid("c843a675-02a4-46c7-aea1-a78fd98d7c7a"), true, "https://feeds.fireside.fm/xamarinpodcast/rss" },
                    { new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6"), false, "https://feeds.fireside.fm/mergeconflict/rss" },
                    { new Guid("da5fb742-7ceb-40cc-ac17-2d46253de3f9"), false, "https://feeds.buzzsprout.com/978640.rss" },
                    { new Guid("e2a825f2-1a5e-4b54-94dd-1544511349ab"), false, "https://feeds.soundcloud.com/users/soundcloud:users:941029057/sounds.rss" },
                    { new Guid("fa3da5bc-805e-401e-a590-f57776712170"), false, "https://intrazone.libsyn.com/rss" }
                });

            migrationBuilder.InsertData(
                table: "FeedCategory",
                columns: new[] { "CategoryId", "FeedId" },
                values: new object[,]
                {
                    { new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("1d9a5366-4258-4355-9a04-80680d12e05c") },
                    { new Guid("bee871ad-750b-400b-91b0-c34056c92297"), new Guid("1d9a5366-4258-4355-9a04-80680d12e05c") },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("2a57fb68-8755-4d9a-a6ee-86bf106d7874") },
                    { new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("54179124-9094-4091-9891-f29868298575") },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("5660e7b9-7555-4d3f-b863-df658440820b") },
                    { new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("57da3b70-bdfc-454e-81f0-fb4ee7ba68d3") },
                    { new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("5ebb45a0-5fff-49ac-a5d5-691e6314ce71") },
                    { new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), new Guid("5ebb45a0-5fff-49ac-a5d5-691e6314ce71") },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("5ebb45a0-5fff-49ac-a5d5-691e6314ce71") },
                    { new Guid("bee871ad-750b-400b-91b0-c34056c92297"), new Guid("5ebb45a0-5fff-49ac-a5d5-691e6314ce71") },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("5fb313f5-ca48-49cd-a9bd-7ea830cfa984") },
                    { new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("6d6b95a4-88f8-4e52-bacd-362c0024362c") },
                    { new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("71a2df8c-cb34-4203-b045-375695439b8b") },
                    { new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), new Guid("71a2df8c-cb34-4203-b045-375695439b8b") },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("71a2df8c-cb34-4203-b045-375695439b8b") },
                    { new Guid("bee871ad-750b-400b-91b0-c34056c92297"), new Guid("71a2df8c-cb34-4203-b045-375695439b8b") },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("76c2dd2f-7232-4842-9808-f6a389de510e") },
                    { new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("7941709e-dbd5-4d04-8c90-e304a4645005") },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("7dd803ce-d834-4ae2-8f37-6f6e0d1977cc") },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("89a51256-4674-4a11-8f2a-bd44ce325d14") },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("a8791dd6-0ad8-48b7-b66e-8c6d67719626") },
                    { new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("bc2cab2b-d6f4-48ae-9602-3041a55ee6be") },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("bc2cab2b-d6f4-48ae-9602-3041a55ee6be") },
                    { new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d") },
                    { new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d") },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d") },
                    { new Guid("bee871ad-750b-400b-91b0-c34056c92297"), new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d") },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("c2b49169-0bb5-444a-86a4-14a476cf7620") },
                    { new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("c843a675-02a4-46c7-aea1-a78fd98d7c7a") },
                    { new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("c843a675-02a4-46c7-aea1-a78fd98d7c7a") },
                    { new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), new Guid("c843a675-02a4-46c7-aea1-a78fd98d7c7a") },
                    { new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6") },
                    { new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6") },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6") },
                    { new Guid("bee871ad-750b-400b-91b0-c34056c92297"), new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6") },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("da5fb742-7ceb-40cc-ac17-2d46253de3f9") },
                    { new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("e2a825f2-1a5e-4b54-94dd-1544511349ab") },
                    { new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("fa3da5bc-805e-401e-a590-f57776712170") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_ShowId",
                table: "Episodes",
                column: "ShowId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedCategory_CategoryId",
                table: "FeedCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_FeedId",
                table: "Shows",
                column: "FeedId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "FeedCategory");

            migrationBuilder.DropTable(
                name: "Shows");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Feeds");
        }
    }
}
