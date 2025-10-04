namespace Cloaked;

public static class Alphabets
{
    /// <summary>
    /// All digits [0, 9].
    /// </summary>
    public const string Digits = "0123456789";

    /// <summary>
    /// English hexadecimal with lowercase characters.
    /// </summary>
    public const string HexadecimalLowercase = Digits + "abcdef";

    /// <summary>
    /// English hexadecimal with uppercase characters.
    /// </summary>
    public const string HexadecimalUppercase = Digits + "ABCDEF";

    /// <summary>
    /// Lowercase English alphabet letters.
    /// </summary>
    public const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// Uppercase English alphabet letters.
    /// </summary>
    public const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// Lowercase and uppercase English alphabet letters.
    /// </summary>
    public const string Letters = LowercaseLetters + UppercaseLetters;

    /// <summary>
    /// Lowercase English alphabet letters and digits.
    /// </summary>
    public const string LowercaseLettersAndDigits = Digits + LowercaseLetters;

    /// <summary>
    /// Uppercase English alphabet letters and digits.
    /// </summary>
    public const string UppercaseLettersAndDigits = Digits + UppercaseLetters;

    /// <summary>
    /// English alphabet letters and digits.
    /// </summary>
    public const string LettersAndDigits = Digits + LowercaseLetters + UppercaseLetters;

    /// <summary>
    /// English alphabet letters and digits without lookalikes: 1, l, I, 0, O, o, u, v, 5, S, s, 2, Z.
    /// </summary>
    public const string NoLookAlikes =
        SubAlphabets.NoLookAlikeDigits + SubAlphabets.NoLookAlikeLetters;

    /// <summary>
    /// English alphabet letters and digits without lookalikes (1, l, I, 0, O, o, u, v, 5, S, s, 2, Z)
    /// and with removed vowels and the following letters: 3, 4, x, X, V.
    /// This list should protect you from accidentally getting obscene words in generated strings.
    /// </summary>
    public const string NoLookAlikesSafe =
        SubAlphabets.NoLookAlikeSafeDigits + SubAlphabets.NoLookAlikeSafeLetters;

    /// <summary>
    /// Includes ascii digits, letters and the symbols '_' and '-'.
    /// </summary>
    public const string Default = SubAlphabets.Symbols + LettersAndDigits;

    /// <summary>
    /// Used for composition and documentation in building proper Alphabets.
    /// </summary>
    public static class SubAlphabets
    {
        /// <summary>
        /// All digits that don't look similar to other digits or letters.
        /// </summary>
        public const string NoLookAlikeDigits = "346789";

        /// <summary>
        /// All lowercase letters that don't look similar to other letters.
        /// </summary>
        public const string NoLookAlikeLettersLowercase = "abcdefghijkmnpqrtwxyz";

        /// <summary>
        /// All uppercase letters that don't look similar to other letters.
        /// </summary>
        public const string NoLookAlikeLettersUppercase = "ABCDEFGHJKLMNPQRTUVWXY";

        /// <summary>
        /// All letters that don't look similar to other letters.
        /// </summary>
        public const string NoLookAlikeLetters =
            NoLookAlikeLettersUppercase + NoLookAlikeLettersLowercase;

        /// <summary>
        /// All digits that don't look similar to other digits or letters
        /// and prevent potential obscene words from appearing in generated ids.
        /// </summary>
        public const string NoLookAlikeSafeDigits = "6789";

        /// <summary>
        /// All lowercase letters that don't look similar to other digits or letters
        /// and prevent potential obscene words from appearing in generated ids.
        /// </summary>
        public const string NoLookAlikeSafeLettersLowercase = "bcdfghjkmnpqrtwz";

        /// <summary>
        /// All uppercase letters that don't look similar to other digits or letters
        /// and prevent potential obscene words from appearing in generated ids.
        /// </summary>
        public const string NoLookAlikeSafeLettersUppercase = "BCDFGHJKLMNPQRTW";

        /// <summary>
        /// All letters that don't look similar to other digits or letters
        /// and prevent potential obscene words from appearing in generated ids.
        /// </summary>
        public const string NoLookAlikeSafeLetters =
            NoLookAlikeSafeLettersUppercase + NoLookAlikeSafeLettersLowercase;

        /// <summary>
        /// URL safe symbols. Part of the default alphabet.
        /// </summary>
        public const string Symbols = "_-";
    }
}
