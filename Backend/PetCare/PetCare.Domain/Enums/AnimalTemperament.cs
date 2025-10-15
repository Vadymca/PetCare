namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the temperament or behavioral tendencies of an animal.
/// </summary>
/// <remarks>This enumeration can be used to classify animals based on their general behavior or disposition.
/// Common use cases include categorizing animals in a zoo, veterinary system, or behavioral analysis
/// application.</remarks>
public enum AnimalTemperament
{
    /// <summary>
    /// Represents a friendly entity or object.
    /// </summary>
    Friendly,              // Лагідний, відкритий до людей

    /// <summary>
    /// Represents a shy behavior or characteristic.
    /// </summary>
    Shy,                   // Сором'язливий, потребує часу

    /// <summary>
    /// Gets or sets a value indicating whether the entity requires social interaction.
    /// </summary>
    NeedsSocialization,    // Потребує соціалізації

    /// <summary>
    /// Represents an independent entity or concept. This class is designed to be used in scenarios where independence
    /// or isolation is a key characteristic.
    /// </summary>
    Independent,           // Самостійний, не надто прив'язаний

    /// <summary>
    /// Represents a type or member that is characterized by affection or displays a warm, caring nature.
    /// </summary>
    Affectionate,          // Дуже ласкавий, любить обійми

    /// <summary>
    /// Represents a protective mechanism or entity designed to safeguard against potential risks or harm.
    /// </summary>
    Protective,            // Захисний, може охороняти

    /// <summary>
    /// Represents a state of curiosity or inquisitiveness.
    /// </summary>
    Curious,               // Цікавий, досліджує все

    /// <summary>
    /// Represents the playful behavior or state of an entity.
    /// </summary>
    Playful,               // Грайливий, активний

    /// <summary>
    /// Represents a calm demeanor or state of being.
    /// </summary>
    Calm,                  // Спокійний, не метушливий

    /// <summary>
    /// Represents an energetic behavior or state of being.
    /// </summary>
    Energetic,             // Енергійний, потребує активності

    /// <summary>
    /// Represents a gentle nature or behavior.
    /// </summary>
    Gentle,                // Ніжний, делікатний

    /// <summary>
    /// Represents a vocal behavior or characteristic.
    /// </summary>
    Vocal,                 // Багато "говорить" (мяукає, гавкає, пищить)

    /// <summary>
    /// Represents a quiet demeanor or behavior.
    /// </summary>
    Quiet,                 // Тихий, не галасливий

    /// <summary>
    /// Represents a cuddly nature or behavior.
    /// </summary>
    Cuddly,                // Любить сидіти на руках, обійми

    /// <summary>
    /// Represents a nervous temperament or behavior.
    /// </summary>
    Nervous,               // Тривожний, може боятись нових ситуацій

    /// <summary>
    /// Represents a confident demeanor or behavior.
    /// </summary>
    Confident,             // Впевнений у собі

    /// <summary>
    /// Represents a food-motivated behavior or characteristic.
    /// </summary>
    FoodMotivated,         // Добре навчається через їжу

    /// <summary>
    /// Represents a trainable nature or behavior.
    /// </summary>
    Trainable,             // Легко навчається

    /// <summary>
    /// Represents a stubborn temperament or behavior.
    /// </summary>
    Stubborn,              // Впертий, має власну думку

    /// <summary>
    /// Represents a social nature or behavior.
    /// </summary>
    GoodWithKids,          // Добре ладнає з дітьми

    /// <summary>
    /// Represents an animal that gets along well with other animals.
    /// </summary>
    GoodWithOtherAnimals,  // Добре ладнає з іншими тваринами

    /// <summary>
    /// Represents an animal that requires an experienced owner.
    /// </summary>
    NeedsExperiencedOwner, // Потребує досвідченого власника

    /// <summary>
    /// Represents a senior and relaxed temperament or behavior.
    /// </summary>
    SeniorAndRelaxed,      // Старший, спокійний

    /// <summary>
    /// Represents a young animal that is still learning and developing.
    /// </summary>
    YoungAndLearning,      // Молодий, ще вчиться

    /// <summary>
    /// Represents an animal with special needs, either emotional or physical.
    /// </summary>
    SpecialNeeds,          // Має особливі потреби (емоційні чи фізичні)

    /// <summary>
    /// Represents a bonded pair temperament or behavior, indicating that the animal is part of a pair.
    /// </summary>
    BondedPair,            // Частина пари, яку бажано всиновити разом
}
