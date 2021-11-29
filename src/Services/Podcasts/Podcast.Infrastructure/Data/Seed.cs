using Podcast.Infrastructure.Data.Models;

namespace Podcast.Infrastructure.Data;

internal static class Seed
{
    internal static Feed[] Feeds = {
        new(new Guid("5660e7b9-7555-4d3f-b863-df658440820b"), "http://feeds.codenewbie.org/cnpodcast.xml"),
        new(new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6"), "https://feeds.fireside.fm/mergeconflict/rss"),
        new(new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d"), "https://msdevshow.libsyn.com/rss" ),
        new(new Guid("5ebb45a0-5fff-49ac-a5d5-691e6314ce71"), "https://thedotnetcorepodcast.libsyn.com/rss" ),
        new(new Guid("71a2df8c-cb34-4203-b045-375695439b8b"), "https://devchat.tv/podcasts/adventures-in-dotnet/feed/" ),
        new(new Guid("5fb313f5-ca48-49cd-a9bd-7ea830cfa984"), "https://feeds.simplecast.com/gvtxUiIf" ),
        new(new Guid("54179124-9094-4091-9891-f29868298575"), "http://feeds.gimletcreative.com/dotfuture" , true ),
        new(new Guid("bc2cab2b-d6f4-48ae-9602-3041a55ee6be"), "https://feeds.fireside.fm/gonemobile/rss" ),
        new(new Guid("c843a675-02a4-46c7-aea1-a78fd98d7c7a"), "https://feeds.fireside.fm/xamarinpodcast/rss", true ),
        new(new Guid("6d6b95a4-88f8-4e52-bacd-362c0024362c"), "https://microsoftmechanics.libsyn.com/rss" ),
        new(new Guid("1d9a5366-4258-4355-9a04-80680d12e05c"), "https://www.m365devpodcast.com/feed.xml" ),
        new(new Guid("2a57fb68-8755-4d9a-a6ee-86bf106d7874"), "http://www.pwop.com/feed.aspx?show=dotnetrocks&filetype=master" ),
        new(new Guid("c2b49169-0bb5-444a-86a4-14a476cf7620"), "https://feeds.simplecast.com/cRTTfxcT" ),
        new(new Guid("fa3da5bc-805e-401e-a590-f57776712170"), "https://intrazone.libsyn.com/rss" ),
        new(new Guid("7941709e-dbd5-4d04-8c90-e304a4645005"), "https://upwards.libsyn.com/rss" ),
        new(new Guid("57da3b70-bdfc-454e-81f0-fb4ee7ba68d3"), "https://s.ch9.ms/Shows/Hello-World/feed/mp3", true ),
        new(new Guid("89a51256-4674-4a11-8f2a-bd44ce325d14"), "https://listenbox.app/f/NRGnlt0wQqB7" ),
        new(new Guid("7dd803ce-d834-4ae2-8f37-6f6e0d1977cc"), "https://nullpointers.io/feed/podcast.rss" ),
        new(new Guid("76c2dd2f-7232-4842-9808-f6a389de510e"), "http://awayfromthekeyboard.com/feed/" ),
        new(new Guid("da5fb742-7ceb-40cc-ac17-2d46253de3f9"), "https://feeds.buzzsprout.com/978640.rss" ),
        new(new Guid("a8791dd6-0ad8-48b7-b66e-8c6d67719626"), "http://feeds.feedburner.com/NoDogmaPodcast" ),
        new(new Guid("e2a825f2-1a5e-4b54-94dd-1544511349ab"), "https://feeds.soundcloud.com/users/soundcloud:users:941029057/sounds.rss")
    };

    internal static Category[] Categories = {
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), "Community"),
        new(new Guid("5f923017-86da-4793-9332-7b74197acc51"), "Microsoft"),
        new(new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), "Mobile"),
        new(new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), "Desktop"),
        new(new Guid("bee871ad-750b-400b-91b0-c34056c92297"), "M365"),
        new(new Guid("4d41bc8e-ef5e-439f-80fa-5e9873ea7a4a"), "Web")
    };

    internal static FeedCategory[] FeedCategories =
    {
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("5660e7b9-7555-4d3f-b863-df658440820b")),
        new(new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("bc2cab2b-d6f4-48ae-9602-3041a55ee6be")),
        new(new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("c843a675-02a4-46c7-aea1-a78fd98d7c7a")),
        new(new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("c843a675-02a4-46c7-aea1-a78fd98d7c7a")),
        new(new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), new Guid("c843a675-02a4-46c7-aea1-a78fd98d7c7a")),
        new(new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("6d6b95a4-88f8-4e52-bacd-362c0024362c")),
        new(new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("1d9a5366-4258-4355-9a04-80680d12e05c")),
        new(new Guid("bee871ad-750b-400b-91b0-c34056c92297"), new Guid("1d9a5366-4258-4355-9a04-80680d12e05c")),
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("bc2cab2b-d6f4-48ae-9602-3041a55ee6be")),
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("2a57fb68-8755-4d9a-a6ee-86bf106d7874")),
        new(new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("fa3da5bc-805e-401e-a590-f57776712170")),
        new(new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("7941709e-dbd5-4d04-8c90-e304a4645005")),
        new(new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("57da3b70-bdfc-454e-81f0-fb4ee7ba68d3")),
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("89a51256-4674-4a11-8f2a-bd44ce325d14")),
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("7dd803ce-d834-4ae2-8f37-6f6e0d1977cc")),
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("76c2dd2f-7232-4842-9808-f6a389de510e")),
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("da5fb742-7ceb-40cc-ac17-2d46253de3f9")),
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("c2b49169-0bb5-444a-86a4-14a476cf7620")),
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("a8791dd6-0ad8-48b7-b66e-8c6d67719626")),
        new(new Guid("5f923017-86da-4793-9332-7b74197acc51"), new Guid("54179124-9094-4091-9891-f29868298575")),
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("5fb313f5-ca48-49cd-a9bd-7ea830cfa984")),
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6")),
        new(new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6")),
        new(new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6")),
        new(new Guid("bee871ad-750b-400b-91b0-c34056c92297"), new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6")),
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d")),
        new(new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d")),
        new(new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d")),
        new(new Guid("bee871ad-750b-400b-91b0-c34056c92297"), new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d")),
        new(new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("5ebb45a0-5fff-49ac-a5d5-691e6314ce71")),
        new(new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), new Guid("5ebb45a0-5fff-49ac-a5d5-691e6314ce71")),
        new(new Guid("bee871ad-750b-400b-91b0-c34056c92297"), new Guid("5ebb45a0-5fff-49ac-a5d5-691e6314ce71")),
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("71a2df8c-cb34-4203-b045-375695439b8b")),
        new(new Guid("2f07481d-5f3f-4bbf-923f-60e62fcfe4e7"), new Guid("71a2df8c-cb34-4203-b045-375695439b8b")),
        new(new Guid("7322b307-1431-4203-bda8-9161b60c45d0"), new Guid("71a2df8c-cb34-4203-b045-375695439b8b")),
        new(new Guid("bee871ad-750b-400b-91b0-c34056c92297"), new Guid("71a2df8c-cb34-4203-b045-375695439b8b")),
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("5ebb45a0-5fff-49ac-a5d5-691e6314ce71")),
        new(new Guid("a5ae013c-14a1-4c2d-a731-47fbbd0ba527"), new Guid("e2a825f2-1a5e-4b54-94dd-1544511349ab"))
    };
}