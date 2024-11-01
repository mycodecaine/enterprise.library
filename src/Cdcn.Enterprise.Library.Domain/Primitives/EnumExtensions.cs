namespace Cdcn.Enterprise.Library.Domain.Primitives
{
    public static class EnumExtensions
    {
        public static bool IsValidEnumValue<T>(this T value)
            where T : struct, Enum
        {
            return Enum.IsDefined(typeof(T), value);
        }
    }
}
