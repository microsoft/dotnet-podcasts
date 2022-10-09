using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Podcast.Infrastructure.Data.Models;
using Podcast.Infrastructure.Http.Feeds;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                    Order = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Explicit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Published = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    { new Guid("9bd02984-df0e-4bac-890a-a13ad7b3e2df"), false, "https://rss.app/feeds/7axIqSwHjwOe3tsB.xml" },
                    //{ new Guid("1d9a5366-4258-4355-9a04-80680d12e05c"), false, "https://www.m365devpodcast.com/feed.xml" },
                    //{ new Guid("2a57fb68-8755-4d9a-a6ee-86bf106d7874"), false, "http://www.pwop.com/feed.aspx?show=dotnetrocks&filetype=master" },
                    //{ new Guid("54179124-9094-4091-9891-f29868298575"), true, "http://feeds.gimletcreative.com/dotfuture" },
                    //{ new Guid("5660e7b9-7555-4d3f-b863-df658440820b"), false, "http://feeds.codenewbie.org/cnpodcast.xml" },
                    //{ new Guid("57da3b70-bdfc-454e-81f0-fb4ee7ba68d3"), true, "https://s.ch9.ms/Shows/Hello-World/feed/mp3" },
                    //{ new Guid("5ebb45a0-5fff-49ac-a5d5-691e6314ce71"), false, "https://thedotnetcorepodcast.libsyn.com/rss" },
                    //{ new Guid("5fb313f5-ca48-49cd-a9bd-7ea830cfa984"), false, "https://feeds.simplecast.com/gvtxUiIf" },
                    //{ new Guid("6d6b95a4-88f8-4e52-bacd-362c0024362c"), false, "https://microsoftmechanics.libsyn.com/rss" },
                    //{ new Guid("71a2df8c-cb34-4203-b045-375695439b8b"), false, "https://devchat.tv/podcasts/adventures-in-dotnet/feed/" },
                    //{ new Guid("76c2dd2f-7232-4842-9808-f6a389de510e"), false, "http://awayfromthekeyboard.com/feed/" },
                    //{ new Guid("7941709e-dbd5-4d04-8c90-e304a4645005"), false, "https://upwards.libsyn.com/rss" },
                    //{ new Guid("7dd803ce-d834-4ae2-8f37-6f6e0d1977cc"), false, "https://nullpointers.io/feed/podcast.rss" },
                    //{ new Guid("89a51256-4674-4a11-8f2a-bd44ce325d14"), false, "https://listenbox.app/f/NRGnlt0wQqB7" },
                    //{ new Guid("a8791dd6-0ad8-48b7-b66e-8c6d67719626"), false, "http://feeds.feedburner.com/NoDogmaPodcast" },
                    //{ new Guid("bc2cab2b-d6f4-48ae-9602-3041a55ee6be"), false, "https://feeds.fireside.fm/gonemobile/rss" },
                    //{ new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d"), false, "https://msdevshow.libsyn.com/rss" },
                    //{ new Guid("c2b49169-0bb5-444a-86a4-14a476cf7620"), false, "https://feeds.simplecast.com/cRTTfxcT" },
                    //{ new Guid("c843a675-02a4-46c7-aea1-a78fd98d7c7a"), true, "https://feeds.fireside.fm/xamarinpodcast/rss" },
                    //{ new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6"), false, "https://feeds.fireside.fm/mergeconflict/rss" },
                    //{ new Guid("da5fb742-7ceb-40cc-ac17-2d46253de3f9"), false, "https://feeds.buzzsprout.com/978640.rss" },
                    //{ new Guid("e2a825f2-1a5e-4b54-94dd-1544511349ab"), false, "https://feeds.soundcloud.com/users/soundcloud:users:941029057/sounds.rss" },
                    //{ new Guid("fa3da5bc-805e-401e-a590-f57776712170"), false, "https://intrazone.libsyn.com/rss" }
                });

            migrationBuilder.InsertData(
                table: "FeedCategory",
                columns: new[] { "CategoryId", "FeedId" },
                values: new object[,]
                {
                   { new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("9bd02984-df0e-4bac-890a-a13ad7b3e2df") },
                    //{ new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("1d9a5366-4258-4355-9a04-80680d12e05c") },
                    //{ new Guid("bee871ad-750b-400b-91b0-c34056c92297"), new Guid("1d9a5366-4258-4355-9a04-80680d12e05c") },
                    //{ new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("2a57fb68-8755-4d9a-a6ee-86bf106d7874") },
                    //{ new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("54179124-9094-4091-9891-f29868298575") },
                    //{ new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("5660e7b9-7555-4d3f-b863-df658440820b") },
                    //{ new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("57da3b70-bdfc-454e-81f0-fb4ee7ba68d3") },
                    //{ new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("5ebb45a0-5fff-49ac-a5d5-691e6314ce71") },
                    //{ new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), new Guid("5ebb45a0-5fff-49ac-a5d5-691e6314ce71") },
                    //{ new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("5ebb45a0-5fff-49ac-a5d5-691e6314ce71") },
                    //{ new Guid("bee871ad-750b-400b-91b0-c34056c92297"), new Guid("5ebb45a0-5fff-49ac-a5d5-691e6314ce71") },
                    //{ new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("5fb313f5-ca48-49cd-a9bd-7ea830cfa984") },
                    //{ new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("6d6b95a4-88f8-4e52-bacd-362c0024362c") },
                    //{ new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("71a2df8c-cb34-4203-b045-375695439b8b") },
                    //{ new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), new Guid("71a2df8c-cb34-4203-b045-375695439b8b") },
                    //{ new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("71a2df8c-cb34-4203-b045-375695439b8b") },
                    //{ new Guid("bee871ad-750b-400b-91b0-c34056c92297"), new Guid("71a2df8c-cb34-4203-b045-375695439b8b") },
                    //{ new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("76c2dd2f-7232-4842-9808-f6a389de510e") },
                    //{ new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("7941709e-dbd5-4d04-8c90-e304a4645005") },
                    //{ new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("7dd803ce-d834-4ae2-8f37-6f6e0d1977cc") },
                    //{ new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("89a51256-4674-4a11-8f2a-bd44ce325d14") },
                    //{ new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("a8791dd6-0ad8-48b7-b66e-8c6d67719626") },
                    //{ new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("bc2cab2b-d6f4-48ae-9602-3041a55ee6be") },
                    //{ new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("bc2cab2b-d6f4-48ae-9602-3041a55ee6be") },
                    //{ new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d") },
                    //{ new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d") },
                    //{ new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d") },
                    //{ new Guid("bee871ad-750b-400b-91b0-c34056c92297"), new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d") },
                    //{ new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("c2b49169-0bb5-444a-86a4-14a476cf7620") },
                    //{ new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("c843a675-02a4-46c7-aea1-a78fd98d7c7a") },
                    //{ new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("c843a675-02a4-46c7-aea1-a78fd98d7c7a") },
                    //{ new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), new Guid("c843a675-02a4-46c7-aea1-a78fd98d7c7a") },
                    //{ new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6") },
                    //{ new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6") },
                    //{ new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6") },
                    //{ new Guid("bee871ad-750b-400b-91b0-c34056c92297"), new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6") },
                    //{ new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("da5fb742-7ceb-40cc-ac17-2d46253de3f9") },
                    //{ new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("e2a825f2-1a5e-4b54-94dd-1544511349ab") },
                    //{ new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("fa3da5bc-805e-401e-a590-f57776712170") }
                });

            migrationBuilder.InsertData(
               table: "Shows",
               columns: new[] {
                    "Id",
                    "Title",
                    "Author",
                    "Description",
                    "Image",
                    "Updated",
                    "Link",
                    "Email",
                    "Language",
                    "FeedId",
               },
               values: new object[,]
                {
                    {
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                        "IDBC - Fel vagy véve! Munkaerőpiaci podcast fröccs",
                        "Illés József",
                        "Meg akarod tartani a kulcs szakembereidet a versenytársaiddal szemben? Szeretnél válságálló HR stratégiákat megismerni? Tudni szeretnéd, mennyit érsz a piacon? Munkahely váltáskor szeretnél jól dönteni? Kíváncsi vagy a legújabb toborzási trendekre, újdonságokra? Vállalkozóként érdekel, hogyan találj növekedési tartalékokat a cégedben és skálázódj felfelé? Amennyiben úgy érzed, hogy valamelyik kérdés megszólított, te vagy a mi emberünk! :) Hallgasd a Fel vagy véve! Munkaerőpiaci podcast fröccsöt!\r\nMűködésfejlesztési tanácsadóként építettünk egy saját digitális recruitment céget, az IDBC-t. Magunkon próbáltuk ki azokat a cégépítési módszereket, amelyeket korábban az ügyfeleinknek javasoltunk. Hallgasd meg mi az ami sikerült és melyek voltak a baklövéseink. Illés József vagyok a podcast házigazdája, az IDBC társ alapítója, vezetője, korábban több multinacionális és hazai IT cég értékesítési és üzletfejlesztési vezetője, megrögzött innovátor, aki töretlenül hisz abban, hogy a recruitment digitalizálása mellett nagyon is emberinek lehet maradni! Egészségetekre!",
                        "https://is4-ssl.mzstatic.com/image/thumb/Podcasts115/v4/4b/bb/0e/4bbb0eb7-95a3-2ebd-a439-bdfbf1a6a916/mza_1700765425663584119.jpg/1200x1200bb.jpg",
                        DateTime.Now,
                        "https://idbc.hu/",
                        "info@idbc.hu",
                        "hu",
                        new Guid("9bd02984-df0e-4bac-890a-a13ad7b3e2df")
                     },
               });

            migrationBuilder.InsertData(
            table: "Episodes",
               columns: new[] {
                  "Id",
                  "Order",
                  "Title",
                  "Description",
                  "Explicit",
                  "Published",
                  "Duration",
                  "Url",
                  "Image",
                  "ShowId",
               },
               values: new object[,]
                {
                    {
                        Guid.NewGuid(),
                        "1",
                        "S01E01 - Változó munkavállalói igények - válságálló HR stratégiák | Bemutatkozik a Fel vagy véve! Podcast",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,41,20),
                        "https://www.buzzsprout.com/1762317/9314430-ep1-valtozo-munkavallaloi-igenyek-valsagallo-hr-strategiak-bemutatkozik-a-fel-vagy-veve-podcast.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCSGhSbkFJPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--fdf94a087aae469a9a2ec8d76159946602e6b15c/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP1.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "2",
                        "S01E02 - Toborzási sakk-matt! - Harc a jó szakemberekért!",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,50,20),
                        "https://www.buzzsprout.com/1762317/9402143-ep2-toborzasi-sakk-matt-harc-a-jo-szakemberekert.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCSlpKcEFJPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--418760fd3166fc028b20f1a3e1b8fa25a30d8361/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP2_v2.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "3",
                        "S01E03 - A titkosügynök, aki segít, hogy jól érezd magad! – Employee Experience napszemüvegben",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,44,35),
                        "https://www.buzzsprout.com/1762317/9488646-ep3-a-titkosugynok-aki-segit-hogy-jol-erezd-magad-employee-experience-napszemuvegben.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCRlU5ckFJPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--804d7d0f9ec7e501edac0fac2502270911d0635f/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP3.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "4",
                        "S01E04 - Eladási és toborzási stratégiák a LinkedIn-en - Lélektan és automatizáció",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,44,36),
                        "https://www.buzzsprout.com/1762317/9557035-ep4-eladasi-es-toborzasi-strategiak-a-linkedin-en-lelektan-es-automatizacio.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCS0Jyc2dJPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--5648c2741c38f9c522545ab13285318556770019/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP4.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "5",
                        "S01E05 - Fizetek főúr, volt egy feketém! Vidéki turizmus és vendéglátás munkaerőpiaci szemmel",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,46,58),
                        "https://www.buzzsprout.com/1762317/9644897-ep5-fizetek-four-volt-egy-feketem-videki-turizmus-es-vendeglatas-munkaeropiaci-szemmel.mp3?",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCRDB6dXdJPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--1a2c6c579e2db0326331f3d0a0f06a09a64a9a0a/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP5_final.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "6",
                        "S01E06 - A legnagyobb hazai bankfúzió PMO szemmel – a mesterterv és sztori, ami mögötte van",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(1,2,7),
                        "https://www.buzzsprout.com/1762317/9725316-ep6-a-legnagyobb-hazai-bankfuzio-pmo-szemmel-a-mesterterv-es-sztori-ami-mogotte-van.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCTnN0d3dJPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--791a50b6b1ed2b3d4d7506f0f150e6ca5cd528fe/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP6.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "7",
                        "S01E07 - Kimaxoljuk a szabadságot! - Freelancer életstratégiák az online network marketing businesstől a content marketingig.",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(1,6,58),
                        "https://www.buzzsprout.com/1762317/9760194-ep7-kimaxoljuk-a-szabadsagot-freelancer-eletstrategiak-az-online-network-marketing-businesstol-a-content-marketingig.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCQ09PeGdJPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--08b6ec0d66ee3428dd55aa1d77777d5e5d12dbac/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP7.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "8",
                        "S01E08 - A nagy recruitment előrejelzés 2022-re - viharos digitális szelek, munkaerő fagyok, a félelmek nagyok…",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(1,10,1),
                        "https://www.buzzsprout.com/1762317/9836543-ep8-a-nagy-recruitment-elorejelzes-2022-re-viharos-digitalis-szelek-munkaero-fagyok-a-felelmek-nagyok.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCQ1NZemdJPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--886186f6cf0d6d76e81bf5afda764412a82b33cb/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP8.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "9",
                        "S01E09 - VOIS – az Üzleti Támogató Központok új generációja - a multikulturális tehetség show",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(1,4,28),
                        "https://www.buzzsprout.com/1762317/9919279-ep9-vois-az-uzleti-tamogato-kozpontok-uj-generacioja-a-multikulturalis-tehetseg-show.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCRzhBMlFJPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--3aa463236427e1df62a7a618e5f14bc08a0f1d81/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP9_v5.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "10",
                        "S01E10 - Multisból vállalkozó - karrierugrás védőháló nélkül",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,41,54),
                        "https://www.buzzsprout.com/1762317/9998222-ep10-multisbol-vallalkozo-karrierugras-vedohalo-nelkul.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCSTh6NFFJPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--dbf972fbe40f150df46b746e8dafdcf1cc3239ba/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP10_cover_final.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "11",
                        "S01E11 - A márka szerepe a kommunikációban – perszónákról személyesen",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,44,30),
                        "https://www.buzzsprout.com/1762317/10091444-ep11-a-marka-szerepe-a-kommunikacioban-perszonakrol-szemelyesen.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCQUF2NndJPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--584e8e713f00c964c269c989c761ff842a9e3230/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP11_V3.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "12",
                        "S01E12 - Az egyházi munkaerőpiacról szabadon - tréner, coach, supervisor, nővér, tanító",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,35,47),
                        "https://www.buzzsprout.com/1762317/10180533-ep12-az-egyhazi-munkaeropiacrol-szabadon-trener-coach-supervisor-nover-tanito.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCS3QyOUFJPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--9476a759f897ee43db2661998f151d1ab6901fbb/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP12.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "13",
                        "S01E13 - Harapós tanulságok a jelölt piacon - amikor a cápa úszni tanít! - vendégünk CV Shark",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,42,51),
                        "https://www.buzzsprout.com/1762317/10266720-ep13-harapos-tanulsagok-a-jelolt-piacon-amikor-a-capa-uszni-tanit-vendegunk-cv-shark.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCQ2FmL0FJPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--e3bae7af1fe19bcc300d635409eede6546a93097/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP13.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "14",
                        "S01E14 - Behind the scenes - a recruitmentről kendőzetlenül Ramival - hogy látja egy Senior Lead Recruiter",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,52,07),
                        "https://www.buzzsprout.com/1762317/10353161-ep14-behind-the-scenes-a-recruitmentrol-kendozetlenul-ramival-hogy-latja-egy-senior-lead-recruiter.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCQStlQkFNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--467296baa586ef81ae583e1dc93fa4ee31791e4a/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP14_V2.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "15",
                        "S01E15 - Az egyházi munkaerőpiacról szabadon 2 - Szólj be a papnak! Vitakultúra, közösség építés, toborzás",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,56,24),
                        "https://www.buzzsprout.com/1762317/10438079-ep15-az-egyhazi-munkaeropiacrol-szabadon-2-szolj-be-a-papnak-vitakultura-kozosseg-epites-toborzas.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCRStwREFNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--012a1e80b6f0d60402b3e47db4dc32615ec1639b/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP15.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "16",
                        "S01E16 - Banki digitalizációs karrier -  Te készen állsz?",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,50,35),
                        "https://www.buzzsprout.com/1762317/10604931-ep16-banki-digitalizacios-karrier-te-keszen-allsz.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCTTI3SEFNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--febe88dd4d80a9bd8c8703fce7a19606d4559ea3/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP16.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "17",
                        "S01E17 - A diverzitás cégformáló ereje - a zöld 64 árnyalata",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,51,38),
                        "https://www.buzzsprout.com/1762317/10724006-ep17-a-diverzitas-cegformalo-ereje-a-zold-64-arnyalata.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCTlJPS3dNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--1fe1aa23a1e0c150a0e1775084bd27f549cdd587/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP17_v2.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "18",
                        "S01E18 - Hogyan ugorjunk  a Top5 munkaerő közvetítő cég közé? – feat Papp Emma üzletág vezető",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,51,26),
                        "https://www.buzzsprout.com/1762317/10808585-ep18-hogyan-ugorjunk-a-top5-munkaero-kozvetito-ceg-koze-feat-papp-emma-uzletag-vezeto.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCRWxjT0FNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--f3908cb12cf9525750804c035227141fa4a6e16d/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP18.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "19",
                        "S01E19 - Mit tanulhatunk az NBA-ből? - toborzás, draft, csapatépítés, kultúrák, történetek Bazskával",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(1,7,35),
                        "https://www.buzzsprout.com/1762317/10882608-ep19-mit-tanulhatunk-az-nba-bol-toborzas-draft-csapatepites-kulturak-tortenetek-bazskaval.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCQVBZUWdNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--373bf22b40a08dea8005dc0b616bc6fa94ff87ac/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP19_V3.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "20",
                        "S01E20 - Őrült lengyel SSC feelingtől a hazai banki recruitmentig – humán road trip Veres Tamással",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,45,15),
                        "https://www.buzzsprout.com/1762317/10960317-ep20-orult-lengyel-ssc-feelingtol-a-hazai-banki-recruitmentig-human-road-trip-veres-tamassal.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCRHN6VGdNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--b573ded17d662574b193fc9b05553dfe1c5afca5/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP20.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                     {
                        Guid.NewGuid(),
                        "21",
                        "S01E21 - Fel lehet-e állni egy Covid tragédia és egy munkahelyet ért háborús csapásból?",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(1,08,41),
                        "https://www.buzzsprout.com/1762317/11035264-ep21-fel-lehet-e-allni-egy-covid-tragedia-es-egy-munkahelyet-ert-haborus-csapasbol.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCS3Z0V1FNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--ee36b28967f54ec22912978b5ce87941c861860a/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP21.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                      {
                        Guid.NewGuid(),
                        "22",
                        "S01E22 - A tanácsadási szakma kihívásai a digitális korban – beszélgetés Komjáthy Csaba Deloitte partnerrel",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(1,15,11),
                        "https://www.buzzsprout.com/1762317/11158809-ep22-a-tanacsadasi-szakma-kihivasai-a-digitalis-korban-beszelgetes-komjathy-csaba-deloitte-partnerrel.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCTVpVY3dNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--b0e25083d771b9f54a97fcd238ef325a33ca1b99/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP22_V3.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                       {
                        Guid.NewGuid(),
                        "23",
                        "S01E23 - Hol a helye a karrierportáloknak a toborzási ökoszisztémában? – hogy látja a piacvezető?",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(1,13,18),
                        "https://www.buzzsprout.com/1762317/11235894-ep23-hol-a-helye-a-karrierportaloknak-a-toborzasi-okoszisztemaban-hogy-latja-a-piacvezeto.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCTHh3aUFNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--344e9f2b4dc96567ced757bdccce014516ebb9c3/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP23.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                    {
                        Guid.NewGuid(),
                        "24",
                        "S01E24 - Hogyan teremtsünk gyorsan recruitment kapacitást? RPO tapasztalatok, sikerek nehéz terepen",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(0,49,55),
                        "https://www.buzzsprout.com/1762317/11330780-ep24-hogyan-teremtsunk-gyorsan-recruitment-kapacitast-rpo-tapasztalatok-sikerek-nehez-terepen.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCTlBPbndNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--6c7c7ed9a16c50e515a25afb4b3f03ab3a4bc6a2/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP24.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    },
                     {
                        Guid.NewGuid(),
                        "25",
                        "S01E25 - Behind the Scenes 2 - A recruitmentről kendőzetlenül - szezonzáró Ramival és Dórival",
                        "",
                        "",
                        DateTime.Now,
                        new TimeSpan(1,02,29),
                        "https://www.buzzsprout.com/1762317/11412632-s1e25-behind-the-scenes-2-a-recruitmentrol-kendozetlenul-szezonzaro-ramival-es-dorival.mp3",
                        "https://www.buzzsprout.com/rails/active_storage/representations/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCQ3BKdFFNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--6e4771514c0f7dbf40b9a054c5e1bb405172ecc6/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaDdDem9MWm05eWJXRjBTU0lJYW5CbkJqb0dSVlE2QzNKbGMybDZaVWtpRHpFME1EQjRNVFF3TUY0R093WlVPZ3huY21GMmFYUjVTU0lMWTJWdWRHVnlCanNHVkRvTFpYaDBaVzUwU1NJT01UUXdNSGd4TkRBd0Jqc0dWRG9NY1hWaGJHbDBlV2xWT2c5amIyeHZjbk53WVdObFNTSUpjMUpIUWdZN0JsUT0iLCJleHAiOm51bGwsInB1ciI6InZhcmlhdGlvbiJ9fQ==--ba61da96b1aafb226473d067fb9b416582e45878/EP25_S_E.jpg",
                        new Guid("d8aa2de5-dcad-4a0b-8fa2-77e9207c7c83"),
                    }

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
