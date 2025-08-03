// <copyright file="Slug.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.ValueObjects;
using PetCare.Domain.Common;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Represents a URL-friendly slug as a value object with validation and normalization.
/// </summary>
public sealed class Slug : ValueObject
{
    private static readonly Regex SlugRegex = new(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.Compiled);

    private Slug(string value) => this.Value = value;

    /// <summary>
    /// Gets the slug string value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new <see cref="Slug"/> instance from a raw slug string.
    /// </summary>
    /// <param name="slug">The input slug string.</param>
    /// <returns>A new <see cref="Slug"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when the slug is invalid.</exception>
    public static Slug Create(string slug)
    {
        var normalized = GenerateFromName(slug);

        if (!IsValid(normalized))
        {
            throw new ArgumentException("Slug не дійсний.", nameof(slug));
        }

        return new Slug(normalized);
    }

    /// <summary>
    /// Generates a normalized slug from a name (e.g. title or label).
    /// Converts Ukrainian letters to Latin, then normalizes the string.
    /// </summary>
    /// <param name="name">The name to convert.</param>
    /// <returns>A normalized slug string.</returns>
    public static string GenerateFromName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Вхідне значення не може бути порожнім.", nameof(name));
        }

        // 1. Транслітерація українських символів у латиницю
        string transliterated = Transliterate(name);

        // 2. Нормалізація (trim, lowercase, заміна пробілів, видалення непотрібних символів)
        var normalized = transliterated.Trim().ToLowerInvariant();
        normalized = Regex.Replace(normalized, @"[\s_]+", "-");
        normalized = Regex.Replace(normalized, @"[^a-z0-9\-]", string.Empty);
        normalized = Regex.Replace(normalized, @"-+", "-");
        normalized = normalized.Trim('-');

        return normalized;
    }

    /// <summary>
    /// Checks whether a given string is a valid slug.
    /// </summary>
    /// <param name="value">The slug string to check.</param>
    /// <returns><c>true</c> if valid; otherwise <c>false</c>.</returns>
    public static bool IsValid(string value)
    {
        return !string.IsNullOrWhiteSpace(value) && SlugRegex.IsMatch(value);
    }

    /// <inheritdoc/>
    public override string ToString() => this.Value;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents() => new[] { this.Value };

    /// <summary>
    /// Simple Ukrainian-to-Latin transliteration.
    /// </summary>
    /// <param name="text">Text to transliterate.</param>
    /// <returns>Transliterated string.</returns>
    private static string Transliterate(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        var map = new Dictionary<char, string>()
        {
            ['а'] = "a",
            ['б'] = "b",
            ['в'] = "v",
            ['г'] = "h",
            ['ґ'] = "g",
            ['д'] = "d",
            ['е'] = "e",
            ['є'] = "ie",
            ['ж'] = "zh",
            ['з'] = "z",
            ['и'] = "y",
            ['і'] = "i",
            ['ї'] = "i",
            ['й'] = "i",
            ['к'] = "k",
            ['л'] = "l",
            ['м'] = "m",
            ['н'] = "n",
            ['о'] = "o",
            ['п'] = "p",
            ['р'] = "r",
            ['с'] = "s",
            ['т'] = "t",
            ['у'] = "u",
            ['ф'] = "f",
            ['х'] = "kh",
            ['ц'] = "ts",
            ['ч'] = "ch",
            ['ш'] = "sh",
            ['щ'] = "shch",
            ['ь'] = string.Empty,
            ['ю'] = "iu",
            ['я'] = "ia",

            // Uppercase letters mapped to lowercase Latin (to preserve casing, convert to lower later)
            ['А'] = "A",
            ['Б'] = "B",
            ['В'] = "V",
            ['Г'] = "H",
            ['Ґ'] = "G",
            ['Д'] = "D",
            ['Е'] = "E",
            ['Є'] = "Ie",
            ['Ж'] = "Zh",
            ['З'] = "Z",
            ['И'] = "Y",
            ['І'] = "I",
            ['Ї'] = "I",
            ['Й'] = "I",
            ['К'] = "K",
            ['Л'] = "L",
            ['М'] = "M",
            ['Н'] = "N",
            ['О'] = "O",
            ['П'] = "P",
            ['Р'] = "R",
            ['С'] = "S",
            ['Т'] = "T",
            ['У'] = "U",
            ['Ф'] = "F",
            ['Х'] = "Kh",
            ['Ц'] = "Ts",
            ['Ч'] = "Ch",
            ['Ш'] = "Sh",
            ['Щ'] = "Shch",
            ['Ь'] = string.Empty,
            ['Ю'] = "Iu",
            ['Я'] = "Ia",
        };

        var sb = new StringBuilder(text.Length);

        foreach (var ch in text)
        {
            if (map.TryGetValue(ch, out var latin))
            {
                sb.Append(latin);
            }
            else
            {
                sb.Append(ch);
            }
        }

        return sb.ToString();
    }
}
