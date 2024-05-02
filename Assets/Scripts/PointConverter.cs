using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

public class PointConverter : JsonConverter<Point>
{
    public override Point ReadJson(JsonReader reader, Type objectType, Point existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);

        // Determine the type of point based on JSON properties
        if (jsonObject["navigateToImage"] != null)
        {
            return jsonObject.ToObject<NavigationPoint>(serializer);
        }
        else if (jsonObject["answers"] != null)
        {
            return jsonObject.ToObject<QuizPoint>(serializer);
        }
        else
        {
            return jsonObject.ToObject<InfoPoint>(serializer);
        }
    }

    public override void WriteJson(JsonWriter writer, Point value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}