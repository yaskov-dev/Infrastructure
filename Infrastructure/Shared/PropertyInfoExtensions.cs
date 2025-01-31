using System.Reflection;

namespace Infrastructure.Shared;

public static class PropertyInfoExtensions
{
    public static void SetTypedValue(this PropertyInfo property, object targetObj, object value)
    {
        object realValue;
        if (property.PropertyType == StringType)
            realValue = Convert.ToString(value)!;
        else if (property.PropertyType == IntType)
            realValue = Convert.ToInt32(value);
        else if (property.PropertyType == BoolType)
            realValue = Convert.ToBoolean(value);
        else if (property.PropertyType == FloatType)
            realValue = (float)Convert.ToDouble(value);
        else if (property.PropertyType == DoubleType)
            realValue = Convert.ToDouble(value);
        else
            throw new NotSupportedException($"Type if not supported. Type: {property.PropertyType.Name}");

        property.SetValue(targetObj, realValue);
    }

    private static readonly Type StringType = typeof(string);
    private static readonly Type IntType = typeof(int);
    private static readonly Type FloatType = typeof(float);
    private static readonly Type DoubleType = typeof(double);
    private static readonly Type BoolType = typeof(bool);
}