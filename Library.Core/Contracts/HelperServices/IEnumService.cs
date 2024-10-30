using Library.Core.Common.Helpers;

namespace Library.Core.Contracts.HelperServices
{
    public interface IEnumService
    {
        /// <summary>
        /// Returns an enum in the form of a <see cref="Dictionary{TKey, TValue}"/> with a <see cref="string"/>
        /// key and <see cref="int"/> value
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns>a <see cref="List{T}"/> with a <see cref="KeyValue"/></returns>
        IEnumerable<KeyValue> GetEnumValues<TEnum>() where TEnum : struct, Enum;
    }
}
