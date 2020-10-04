using System.Runtime.Serialization.Json;
using System.Text;

namespace DominosGeolocation.Helper
{
    public static class ConvertHelper
    {
        public static double ToDouble(this string theValue)
        {
            double retNum;
            var result = double.TryParse(theValue, out retNum);
            return result ? retNum : 0;
        }

        public static string ToJson<T>(this T item, System.Text.Encoding encoding = null, System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = null)
        {
            encoding = encoding ?? Encoding.Default;
            serializer = serializer ?? new DataContractJsonSerializer(typeof(T));

            using (var stream = new System.IO.MemoryStream())
            {
                serializer.WriteObject(stream, item);
                var json = encoding.GetString((stream.ToArray()));

                return json;
            }
        }
    }
}
