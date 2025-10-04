using Sqids;

namespace Cloaked.Sqids;

internal sealed class SqidsCloak : ICloak
{
    private readonly SqidsEncoder _sqidsEncoder;

    public SqidsCloak(SqidsEncoder sqidsEncoder)
    {
        _sqidsEncoder = sqidsEncoder;
    }

    public string Encode(object value, Type valueType)
    {
        var actualType = Nullable.GetUnderlyingType(valueType) ?? valueType;
        return actualType switch
        {
            not null when actualType == typeof(int) => _sqidsEncoder.Encode((int)value),
            not null when actualType == typeof(IEnumerable<int>) => _sqidsEncoder.Encode(
                (IEnumerable<int>)value
            ),
            _ => throw new NotSupportedException(
                $"Type '{actualType}' is not supported for encoding."
            ),
        };
    }

    public object Decode(string encodedValue, Type targetType)
    {
        var actualType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        try
        {
            var (decodedValue, canonicalEncoding) = actualType switch
            {
                not null when actualType == typeof(int) => DecodeInt(encodedValue),
                _ => throw new NotSupportedException(
                    $"Type '{actualType}' is not supported for decoding."
                ),
            };

            // Validate canonical encoding - prevents multiple IDs from resolving to the same value
            if (encodedValue != canonicalEncoding)
            {
                throw new ArgumentException(
                    $"Invalid non-canonical encoding '{encodedValue}'. The canonical encoding for this value is '{canonicalEncoding}'.",
                    nameof(encodedValue)
                );
            }

            return decodedValue;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ArgumentException(
                $"Unable to decode '{encodedValue}' to type {actualType?.Name}.",
                nameof(encodedValue),
                ex
            );
        }
    }

    private (object DecodedValue, string CanonicalEncoding) DecodeInt(string encodedValue)
    {
        var result = _sqidsEncoder.Decode(encodedValue);
        if (result.Count == 0)
        {
            throw new ArgumentException(
                $"Unable to decode '{encodedValue}' - invalid format.",
                nameof(encodedValue)
            );
        }

        var decodedValue = result[0];
        var canonicalEncoding = _sqidsEncoder.Encode(decodedValue);
        return (decodedValue, canonicalEncoding);
    }
}
