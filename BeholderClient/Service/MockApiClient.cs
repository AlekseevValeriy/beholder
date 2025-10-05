namespace Beholder.Service;

class MockApiClient : IApiClient
{
    async public Task<ApiResponse<Boolean>> IsActiveAsync()
    {
        return new(await Task.FromResult(true));
    }

    async public Task<ApiResponse<List<ChannelResponse>>> GetChannelsAsync()
    {
        List<ChannelResponse> channels =
        [
            new (0, "Первый канал", 1, "Крупнейший телеканал России с охватом почти 100% аудитории. Позиционируется как главный телеканал страны, вещает из телецентра «Останкино».", "https://avatars.mds.yandex.net/i?id=979d579b84f454ca9abbb856d45ad7363ebc506a-16874457-images-thumbs&n=13", "развлекательный,познавательный,увлекательный"),
            new (1, "Россия-1",  2, "Один из двух национальных телеканалов с почти полным охватом территории России. Его визитной карточкой являются информационные программы «Вести».", "https://static.wikia.nocookie.net/tvpedia/images/9/90/%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D1%8F-1_2012_%28%D0%B7%D0%B5%D1%80%D0%BA%D0%B0%D0%BB%D1%8C%D0%BD%D1%8B%D0%B9%29.png/revision/latest?cb=20220318095737&path-prefix=ru", "развлекательный,познавательный,увлекательный"),
            new (2, "Матч ТВ", 3, "Федеральный спортивный телеканал, начавший вещание в 2015 году. Был создан на базе телеканала «Россия-2» по инициативе государства и «Газпрома».", "https://avatars.mds.yandex.net/i?id=b57ed05890974b83fa552e92de99733b777eedc7-16288467-images-thumbs&n=13", "развлекательный,познавательный,увлекательный"),
            new (3, "НТВ", 4, "Общероссийский телеканал, вещающий с 1993 года. Его сигнал также распространяется на страны СНГ, Балтии и другие государства через канал «НТВ Мир».", "https://avatars.mds.yandex.net/i?id=da9beacdb9b1656560f65509194c7288bb0c362f-4777526-images-thumbs&n=13", "развлекательный,познавательный,увлекательный"),
            new (4, "Пятый канал", 5, "Старейший телеканал России с центром вещания из Санкт-Петербурга. Был создан как преемник ГТРК «Петербург» на её частотах.", "https://avatars.mds.yandex.net/i?id=fd8820f7d26c1ddc0d5d435acd934ac181bb7037-16330514-images-thumbs&n=13", "развлекательный,познавательный,увлекательный")
        ];

        return new ApiResponse<List<ChannelResponse>>(await Task.FromResult(channels));
    }

    async public Task<ApiResponse<List<ChannelResponse>>> GetChannelsByQueryAsync(String query)
    {
        List<ChannelResponse> channels =
        [
            new (0, "Первый канал", 1, "Крупнейший телеканал России с охватом почти 100% аудитории. Позиционируется как главный телеканал страны, вещает из телецентра «Останкино».", "https://static.wikia.nocookie.net/tvpedia/images/1/16/%D0%9F%D0%B5%D1%80%D0%B2%D1%8B%D0%B9_%D0%BA%D0%B0%D0%BD%D0%B0%D0%BB_2000.svg/revision/latest?cb=20210912151320&path-prefix=ru", "развлекательный,познавательный,увлекательный"),
            new (1, "Россия-1",  2, "Один из двух национальных телеканалов с почти полным охватом территории России. Его визитной карточкой являются информационные программы «Вести».", "https://static.wikia.nocookie.net/tvpedia/images/9/90/%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D1%8F-1_2012_%28%D0%B7%D0%B5%D1%80%D0%BA%D0%B0%D0%BB%D1%8C%D0%BD%D1%8B%D0%B9%29.png/revision/latest?cb=20220318095737&path-prefix=ru", "развлекательный,познавательный,увлекательный"),
            new (2, "Матч ТВ", 3, "Федеральный спортивный телеканал, начавший вещание в 2015 году. Был создан на базе телеканала «Россия-2» по инициативе государства и «Газпрома».", "https://static.wikia.nocookie.net/tvpedia/images/1/11/%D0%9C%D0%B0%D1%82%D1%87_%D0%A2%D0%92_%282015%2C_%D0%B7%D0%BE%D0%BB%D0%BE%D1%82%D1%8B%D0%B5_%D0%B1%D1%83%D0%BA%D0%B2%D1%8B%2C_%D1%87%D1%91%D1%80%D0%BD%D1%8B%D0%B9_%D1%84%D0%BE%D0%BD%2C_%D0%B8%D1%81%D0%BF%D0%BE%D0%BB%D1%8C%D0%B7%D1%83%D0%B5%D1%82%D1%81%D1%8F_%D0%BD%D0%B0_%D1%81%D0%B0%D0%B9%D1%82%D0%B5%29.svg/revision/latest?cb=20211026193729&path-prefix=ru", "развлекательный,познавательный,увлекательный"),
            new (3, "НТВ", 4, "Общероссийский телеканал, вещающий с 1993 года. Его сигнал также распространяется на страны СНГ, Балтии и другие государства через канал «НТВ Мир».", "https://static.wikia.nocookie.net/tvpedia/images/1/1a/NTV_logo_2003.svg/revision/latest?cb=20220403101423&path-prefix=ru", "развлекательный,познавательный,увлекательный"),
            new (4, "Пятый канал", 5, "Старейший телеканал России с центром вещания из Санкт-Петербурга. Был создан как преемник ГТРК «Петербург» на её частотах.", "https://static.wikia.nocookie.net/tvpedia/images/6/6f/%D0%9F%D1%8F%D1%82%D1%8B%D0%B9_%D0%BA%D0%B0%D0%BD%D0%B0%D0%BB_8.svg/revision/latest?cb=20231128062057&path-prefix=ru", "развлекательный,познавательный,увлекательный")
        ];

        return new(await Task.FromResult(channels));
    }

    async public Task<ApiResponse<ChannelResponse>> GetChannelAsync(Int32 id)
    {
        return new(await Task.FromResult(new ChannelResponse(0, "Первый канал", 1, "Крупнейший телеканал России с охватом почти 100% аудитории. Позиционируется как главный телеканал страны, вещает из телецентра «Останкино».", "https://static.wikia.nocookie.net/tvpedia/images/1/16/%D0%9F%D0%B5%D1%80%D0%B2%D1%8B%D0%B9_%D0%BA%D0%B0%D0%BD%D0%B0%D0%BB_2000.svg/revision/latest?cb=20210912151320&path-prefix=ru", "развлекательный,познавательный,увлекательный,позитивный,новостной")));
    }

    async public Task<ApiResponse<List<ScheduleResponse>>> GetScheduleAsync(Int32 programId, DateTime date)
    {
        if (date < DateTime.Today)
        {
            return new(await Task.FromResult(new List<ScheduleResponse>()
            {
                new ScheduleResponse("Изучение синтаксиса языка Rust на жизненных примерах", "Первый канал", DateTime.Today.AddDays(-1), DateTime.Today.AddDays(-1)),
                new ScheduleResponse("Изучение синтаксиса языка Rust на жизненных примерах", "Первый канал", DateTime.Today.AddDays(-1), DateTime.Today.AddDays(-1)),
                new ScheduleResponse("Изучение синтаксиса языка Rust на жизненных примерах", "Первый канал", DateTime.Today.AddDays(-1), DateTime.Today.AddDays(-1)),
                new ScheduleResponse("Изучение синтаксиса языка Rust на жизненных примерах", "Первый канал", DateTime.Today.AddDays(-1), DateTime.Today.AddDays(-1)),
            }));
        }
        else if (date > DateTime.Today)
        {
            return new(await Task.FromResult(new List<ScheduleResponse>()
            {
                new ScheduleResponse("Изучение синтаксиса языка Rust на жизненных примерах", "Первый канал", DateTime.Today.AddDays(1), DateTime.Today.AddDays(1)),
                new ScheduleResponse("Изучение синтаксиса языка Rust на жизненных примерах", "Первый канал", DateTime.Today.AddDays(1), DateTime.Today.AddDays(1)),
                new ScheduleResponse("Изучение синтаксиса языка Rust на жизненных примерах", "Первый канал", DateTime.Today.AddDays(1), DateTime.Today.AddDays(1)),
                new ScheduleResponse("Изучение синтаксиса языка Rust на жизненных примерах", "Первый канал", DateTime.Today.AddDays(1), DateTime.Today.AddDays(1)),
            }));
        }

        return new(await Task.FromResult(new List<ScheduleResponse>()
        {
            new ScheduleResponse("Изучение синтаксиса языка Rust на жизненных примерах", "Первый канал", DateTime.Today, DateTime.Today),
            new ScheduleResponse("Изучение синтаксиса языка Rust на жизненных примерах", "Первый канал", DateTime.Today, DateTime.Today),
            new ScheduleResponse("Изучение синтаксиса языка Rust на жизненных примерах", "Первый канал", DateTime.Today, DateTime.Today),
            new ScheduleResponse("Изучение синтаксиса языка Rust на жизненных примерах", "Первый канал", DateTime.Today, DateTime.Today),
        }));
    }

    async public Task<ApiResponse<List<FavoriteResponse>>> GetFavoritesAsync(Int32 userId)
    {
        List<FavoriteResponse> favorite =
        [
            new (0, "Первый канал", 1, "Крупнейший телеканал России с охватом почти 100% аудитории. Позиционируется как главный телеканал страны, вещает из телецентра «Останкино».", "https://avatars.mds.yandex.net/i?id=979d579b84f454ca9abbb856d45ad7363ebc506a-16874457-images-thumbs&n=13"),
            new (1, "Россия-1",  2, "Один из двух национальных телеканалов с почти полным охватом территории России. Его визитной карточкой являются информационные программы «Вести».", "https://static.wikia.nocookie.net/tvpedia/images/9/90/%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D1%8F-1_2012_%28%D0%B7%D0%B5%D1%80%D0%BA%D0%B0%D0%BB%D1%8C%D0%BD%D1%8B%D0%B9%29.png/revision/latest?cb=20220318095737&path-prefix=ru"),
            new (2, "Матч ТВ", 3, "Федеральный спортивный телеканал, начавший вещание в 2015 году. Был создан на базе телеканала «Россия-2» по инициативе государства и «Газпрома».", "https://avatars.mds.yandex.net/i?id=b57ed05890974b83fa552e92de99733b777eedc7-16288467-images-thumbs&n=13"),
            new (3, "НТВ", 4, "Общероссийский телеканал, вещающий с 1993 года. Его сигнал также распространяется на страны СНГ, Балтии и другие государства через канал «НТВ Мир».", "https://avatars.mds.yandex.net/i?id=da9beacdb9b1656560f65509194c7288bb0c362f-4777526-images-thumbs&n=13"),
            new (4, "Пятый канал", 5, "Старейший телеканал России с центром вещания из Санкт-Петербурга. Был создан как преемник ГТРК «Петербург» на её частотах.", "https://avatars.mds.yandex.net/i?id=fd8820f7d26c1ddc0d5d435acd934ac181bb7037-16330514-images-thumbs&n=13")];
        return new(await Task.FromResult(favorite));
    }

    async public Task<ApiResponse<Boolean>> HasFavoriteAsync(Int32 userId, Int32 channelId)
    {
        return new(await Task.FromResult(new Random().Next(0, 2) == 1));
    }

    async public Task<ApiResponse<Boolean>> AddFavoritesAsync(Int32 programId, Int32 userId)
    {
        return new(await Task.FromResult(false));
    }

    async public Task<ApiResponse<Boolean>> DeleteFavoritesAsync(Int32 programId, Int32 userId)
    {
        return new(await Task.FromResult(false));
    }

    async public Task<ApiResponse<Int32>> GetUserAsync(String login, String password)
    {
        return new(await Task.FromResult(5));
    }

    async public Task<ApiResponse<UserCreateResponse>> AddUserAsync(String login, String password)
    {
        return new(await Task.FromResult(new UserCreateResponse(0, "", "")));
    }

    async public Task<ApiResponse<Boolean>> DeleteUserAsync(Int32 userId, String login, String password)
    {
        return new(await Task.FromResult(false));
    }
}
