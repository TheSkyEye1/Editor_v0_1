using Editor_v0_1;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;


public class EnemyTemplateConverter : JsonConverter<CEnemyTemplate>
{
    public override CEnemyTemplate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDoc = JsonDocument.ParseValue(ref reader);  // Создаем JSON-документ

        try
        {
            string type = jsonDoc.RootElement.GetProperty("$type").GetString();  // Получаем тип объекта

            switch (type)
            {
                case "CArmoredEnemyTemplate":
                    return JsonSerializer.Deserialize<CArmoredEnemyTemplate>(jsonDoc.RootElement.GetRawText(), options);
                case "CEnemyTemplate":
                    return JsonSerializer.Deserialize<CEnemyTemplate>(jsonDoc.RootElement.GetRawText(), options);
                default:
                    throw new NotSupportedException($"Unknown type: {type}");
            }
        }
        finally
        {
            jsonDoc.Dispose();  // Освобождаем ресурс
        }
    }

    public override void Write(Utf8JsonWriter writer, CEnemyTemplate value, JsonSerializerOptions options)
    {
        string type = value.GetType().Name;  // Определяем тип: CEnemyTemplate, CArmoredEnemyTemplate и т. д.

        //ТУТ ПРОИСХОДИТ ОБСЕР ПРЕПОДАВАТЕЛЯ КОМПИЛИРУЮЩЕГО КОД В ГОЛОВЕ
        string json = JsonSerializer.Serialize(value, value.GetType(), options);  // Сериализуем объект
        var jsonDoc = JsonDocument.Parse(json);

        try
        {
            writer.WriteStartObject();
            writer.WriteString("$type", type);  // Добавляем информацию о типе

            // Копируем все свойства
            foreach (var property in jsonDoc.RootElement.EnumerateObject())
            {
                property.WriteTo(writer);
            }

            writer.WriteEndObject();
        }
        finally
        {
            jsonDoc.Dispose();  // Освобождаем ресурс
        }
    }
}
