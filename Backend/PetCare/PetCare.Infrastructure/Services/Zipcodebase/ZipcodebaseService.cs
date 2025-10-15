namespace PetCare.Infrastructure.Services.Zipcodebase;

using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetCare.Application.Interfaces;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Implementation of <see cref="IZipcodebaseService"/> using the Zipcodebase API.
/// </summary>
public sealed class ZipcodebaseService : IZipcodebaseService
{
    private readonly HttpClient httpClient;
    private readonly ZipcodebaseOptions options;
    private readonly ILogger<ZipcodebaseService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ZipcodebaseService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to send requests to the Zipcodebase API.</param>
    /// <param name="options">The configuration options containing the API key and settings.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when any required parameter is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when Zipcodebase configuration is invalid.</exception>
    public ZipcodebaseService(
        HttpClient httpClient,
        IOptions<ZipcodebaseOptions> options,
        ILogger<ZipcodebaseService> logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        if (string.IsNullOrWhiteSpace(this.options.BaseUrl))
        {
            throw new InvalidOperationException("Zipcodebase BaseUrl не налаштований");
        }

        if (string.IsNullOrWhiteSpace(this.options.ApiKey))
        {
            throw new InvalidOperationException("Zipcodebase ApiKey не налаштований");
        }

        this.logger.LogDebug(
            "ZipcodebaseService initialized with BaseUrl: {BaseUrl}, Country: {Country}, Language: {Language}",
            this.options.BaseUrl,
            this.options.Country,
            this.options.Language);
    }

    /// <summary>
    /// Resolves an address by postal code using external API.
    /// </summary>
    /// <param name="postalCode">The postal code to resolve.</param>
    /// <returns>The resolved <see cref="Address"/> or <c>null</c> if not found.</returns>
    public async Task<Address?> ResolveAddressAsync(string postalCode)
    {
        return await this.ResolveAddressAsync(postalCode, CancellationToken.None);
    }

    /// <summary>
    /// Resolves an address by postal code using external API with cancellation support.
    /// </summary>
    /// <param name="postalCode">The postal code to resolve.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The resolved <see cref="Address"/> or <c>null</c> if not found.</returns>
    /// <remarks>
    /// The <paramref name="cancellationToken"/> can be used to cancel long-running requests.
    /// </remarks>
    public async Task<Address?> ResolveAddressAsync(string postalCode, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(postalCode))
        {
            this.logger.LogWarning("Postal code is null or empty");
            return null;
        }

        postalCode = postalCode.Trim();

        // Валідація українського формату (5 цифр)
        if (!IsValidUkrainianPostalCode(postalCode))
        {
            this.logger.LogWarning("Invalid Ukrainian postal code format: {PostalCode}. Expected 5 digits.", postalCode);
            return null;
        }

        this.logger.LogInformation("Resolving address for postal code: {PostalCode}", postalCode);

        try
        {
            // Формуємо запит з усіма параметрами
            var queryParams = new Dictionary<string, string>
            {
                { "codes", postalCode },
                { "apikey", this.options.ApiKey },
                { "country", this.options.Country },
                { "language", this.options.Language },
                { "formats", "json" },
            };

            var uriBuilder = new UriBuilder($"{this.options.BaseUrl}/search");
            var query = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
            uriBuilder.Query = query;

            var fullUrl = uriBuilder.ToString();
            this.logger.LogDebug("Full request URL: {FullUrl}", fullUrl);

            var response = await this.httpClient.GetAsync(fullUrl, cancellationToken);

            this.logger.LogDebug(
                "Response status: {StatusCode} for postal code {PostalCode}",
                response.StatusCode,
                postalCode);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                this.logger.LogWarning(
                    "Zipcodebase API returned {StatusCode} for postal code {PostalCode}: {Content}",
                    response.StatusCode,
                    postalCode,
                    errorContent);
                return null;
            }

            var data = await response.Content.ReadFromJsonAsync<ZipcodebaseResponse>(
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                },
                cancellationToken);

            if (data == null)
            {
                this.logger.LogWarning("Failed to deserialize response for postal code: {PostalCode}", postalCode);
                return null;
            }

            this.logger.LogDebug("Parsed response. Results count: {Count}", data.Results?.Count ?? 0);

            var availableKeys = data.Results?.Keys != null
                ? string.Join(", ", data.Results.Keys)
                : "none";
            this.logger.LogDebug("Available postal codes in response: {Keys}", availableKeys);

            if (data.Results?.TryGetValue(postalCode, out var locations) == true && locations?.Any() == true)
            {
                var result = locations.First();

                var addressString = FormatBeautifulUkrainianAddress(result);

                this.logger.LogInformation(
                    "Found beautiful Ukrainian address for {PostalCode}: {Address}",
                    postalCode,
                    addressString);

                return Address.Create(addressString);
            }

            this.logger.LogWarning(
                "No locations found for postal code: {PostalCode}. Available results: {AvailableKeys}",
                postalCode,
                availableKeys);
            return null;
        }
        catch (HttpRequestException httpEx)
        {
            this.logger.LogError(
                httpEx,
                "HTTP request error for postal code {PostalCode}: {Message}",
                postalCode,
                httpEx.Message);
            return null;
        }
        catch (JsonException jsonEx)
        {
            this.logger.LogError(jsonEx, "JSON deserialization error for postal code {PostalCode}", postalCode);
            return null;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error calling Zipcodebase API for postal code {PostalCode}", postalCode);
            return null;
        }
    }

    /// <summary>
    /// Formats address in beautiful Ukrainian format with proper localization (without postal code).
    /// </summary>
    private static string FormatBeautifulUkrainianAddress(ZipcodebaseResult result)
    {
        var parts = new List<string>();

        // Населений пункт (місто, село, смт)
        if (!string.IsNullOrEmpty(result.City))
        {
            var cityName = TranslateCityToUkrainian(result.City.Trim());
            parts.Add(cityName);
        }

        // Область (регіон)
        if (!string.IsNullOrEmpty(result.State))
        {
            var regionName = TranslateRegionToUkrainian(result.State.Trim());
            if (!string.IsNullOrEmpty(regionName))
            {
                parts.Add(regionName);
            }
        }

        return string.Join(", ", parts);
    }

    /// <summary>
    /// Translates English city names to Ukrainian.
    /// </summary>
    private static string TranslateCityToUkrainian(string englishCity)
    {
        // Якщо назва вже українською (містить українські літери), повертаємо як є
        if (ContainsUkrainianCharacters(englishCity))
        {
            return englishCity;
        }

        // Словник для перекладу популярних міст
        return englishCity.ToLower() switch
        {
            // Чернівецька область
            "chernivtsi" or "chernovtsy" => "м. Чернівці",

            // Київ
            "kyiv" or "kiev" => "м. Київ",

            // Львів
            "lviv" or "lvov" => "м. Львів",

            // Одеса
            "odesa" or "odessa" => "м. Одеса",

            // Харків
            "kharkiv" or "kharkov" => "м. Харків",

            // Дніпро
            "dnipro" or "dnepropetrovsk" => "м. Дніпро",

            // Запоріжжя
            "zaporizhia" or "zaporozhye" => "м. Запоріжжя",

            // Вінниця
            "vinnytsia" or "vinnitsa" => "м. Вінниця",

            // Ужгород
            "uzhhorod" or "uzhgorod" => "м. Ужгород",

            // Івано-Франківськ
            "ivano-frankivsk" or "ivano-frankovsk" => "м. Івано-Франківськ",

            // Тернопіль
            "ternopil" or "ternopol" => "м. Тернопіль",

            // Луцьк
            "lutsk" => "м. Луцьк",

            // Рівне
            "rivne" or "rovno" => "м. Рівне",

            // Житомир
            "zhytomyr" or "zhitomir" => "м. Житомир",

            // Полтава
            "poltava" => "м. Полтава",

            // Чернігів
            "chernihiv" or "chernigov" => "м. Чернігів",

            // Суми
            "sumy" => "м. Суми",

            // Херсон
            "kherson" => "м. Херсон",

            // Миколаїв
            "mykolaiv" or "nikolaev" => "м. Миколаїв",

            // Кропивницький
            "kirovohrad" or "kirovograd" => "м. Кропивницький",

            // Черкаси
            "cherkasy" or "cherkassy" => "м. Черкаси",

            // Хмельницький
            "khmelnytskyi" or "khmelnitsky" => "м. Хмельницький",

            // Біла Церква
            "bila tserkva" or "belaya tserkov" => "м. Біла Церква",

            _ => englishCity // Якщо не розпізнано, повертаємо оригінал
        };
    }

    /// <summary>
    /// Translates English region names to Ukrainian.
    /// </summary>
    private static string TranslateRegionToUkrainian(string englishRegion)
    {
        // Якщо назва вже українською, повертаємо як є
        if (ContainsUkrainianCharacters(englishRegion))
        {
            return englishRegion;
        }

        // Розширений словник перекладів областей
        return englishRegion.ToLower() switch
        {
            // Області
            "chernivetska oblast" or "chernivtsi oblast" or "chernivetska" => "Чернівецька область",
            "kyiv oblast" or "kiev oblast" or "kyiv" or "kiev" => "Київська область",
            "lviv oblast" or "lviv" => "Львівська область",
            "odesa oblast" or "odessa oblast" or "odesa" or "odessa" => "Одеська область",
            "kharkiv oblast" or "kharkiv" => "Харківська область",
            "dnipropetrovsk oblast" or "dnepropetrovsk oblast" or "dnipropetrovsk" => "Дніпропетровська область",
            "donetsk oblast" or "donetsk" => "Донецька область",
            "zhytomyr oblast" or "zhytomyr" or "zhitomir" => "Житомирська область",
            "zakarpattia oblast" or "transcarpathian oblast" or "zakarpattia" => "Закарпатська область",
            "ivano-frankivsk oblast" or "ivano-frankivsk" => "Івано-Франківська область",
            "ternopil oblast" or "ternopil" or "ternopol" => "Тернопільська область",
            "rivne oblast" or "rivne" or "rovno" => "Рівненська область",
            "volyn oblast" or "volyn" => "Волинська область",
            "khmelnytskyi oblast" or "khmelnytskyi" or "khmelnitsky" => "Хмельницька область",
            "vinnytsia oblast" or "vinnytsia" or "vinnitsa" => "Вінницька область",
            "sumy oblast" or "sumy" => "Сумська область",
            "cherkasy oblast" or "cherkasy" or "cherkassy" => "Черкаська область",
            "kirovohrad oblast" or "kirovohrad" or "kirovograd" => "Кіровоградська область",
            "mykolaiv oblast" or "mykolaiv" or "nikolaev" => "Миколаївська область",
            "kherson oblast" or "kherson" => "Херсонська область",
            "zaporizhzhia oblast" or "zaporizhzhia" or "zaporozhye" => "Запорізька область",

            // Автономна республіка
            "crimea" or "avtonomna respublika krym" => "Автономна Республіка Крим",

            // Міста з обласним статусом
            "kyiv city" or "kiev city" => "м. Київ",
            "sevastopol city" => "м. Севастополь",

            _ => englishRegion // Якщо не розпізнано, повертаємо оригінал
        };
    }

    /// <summary>
    /// Checks if string contains Ukrainian characters.
    /// </summary>
    private static bool ContainsUkrainianCharacters(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return false;
        }

        var ukrainianChars = "абвгґдеєжзиіїйклмнопрстуфхцчшщьюяєіїґ";
        return text.ToLower().Any(c => ukrainianChars.Contains(c));
    }

    /// <summary>
    /// Validates Ukrainian postal code format (5 digits).
    /// </summary>
    private static bool IsValidUkrainianPostalCode(string postalCode)
    {
        return postalCode.Length == 5 && postalCode.All(char.IsDigit);
    }
}