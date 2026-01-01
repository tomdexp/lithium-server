using System.Globalization;
using System.Reflection;

namespace Lithium.Server.Core.Commands;

internal static class CommandArgumentBinder
{
    public static object[] Bind(ParameterInfo[] parameters, string[] args)
    {
        if (args.Length != parameters.Length)
        {
            Console.WriteLine("Invalid arguments count.");
            return [];
        }

        var result = new object[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
            result[i] = ConvertArg(args[i], parameters[i].ParameterType);

        return result;
    }

    private static object ConvertArg(string value, Type targetType)
    {
        var nullable = Nullable.GetUnderlyingType(targetType);

        if (nullable is not null)
        {
            if (string.Equals(value, "null", StringComparison.OrdinalIgnoreCase))
                return null!;

            targetType = nullable;
        }

        if (targetType == typeof(string))
            return value;

        if (targetType == typeof(bool))
            return bool.Parse(value);

        if (targetType.IsEnum)
            return Enum.Parse(targetType, value, ignoreCase: true);

        return Convert.ChangeType(
            value,
            targetType,
            CultureInfo.InvariantCulture
        );
    }
}