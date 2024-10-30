using Library.Core.Common.Helpers;
using Library.Core.Contracts.HelperServices;

namespace Library.Core.Services.HelperServices
{
    public class EnumService : IEnumService
    {
        public IEnumerable<KeyValue> GetEnumValues<TEnum>() where TEnum : struct, Enum
        {
            return Enum.GetValues(typeof(TEnum))
                              .Cast<TEnum>()
                              .Select(x => new KeyValue()
                              {
                                  Key = Convert.ToInt32(x).ToString(),
                                  Value = x.ToString()
                              })
                              .ToList();
        }
    }
}
