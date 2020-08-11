using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace DustSwier.OnSiteList.Models.DataAccess
{
    /// <summary>
    /// Converts data to and from: JSON to Model Data.
    /// </summary>
    public class JSONdataProvider : BaseDataProvider
    {
        //public override string FilterTypes => "JSON file (*.json)|*.json";

        protected override string Extention => ".json";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool TryOpen<T>(string filePath, out T data)
        {
            try
            {
                using (StreamReader file = File.OpenText(filePath))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        data = serializer.Deserialize<T>(reader);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                data = default(T);
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool Save<T>(string filePath, T data)
        {
            try
            {
                using (StreamWriter file = File.CreateText(filePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, data);

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}