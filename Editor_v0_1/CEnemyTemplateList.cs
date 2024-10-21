using Editor_v0_1.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Xml;

namespace Editor_v0_1
{
    public class CEnemyTemplateList
    {
        public List<CEnemyTemplate> Enemies { get; set; } = new List<CEnemyTemplate>();

        public CEnemyTemplateList() 
        {
            Enemies = new List<CEnemyTemplate>();
        }

        public string addEnemy(CEnemyTemplate enemy)
        {
            Enemies.Add(enemy);

            return "added";
        }

        public CEnemyTemplate getEnemyByName(string name)
        {
            foreach (CEnemyTemplate enemy in Enemies)
                if (enemy.Name == name)
                    return enemy;

            return null;
        }

        public CEnemyTemplate getEnemyByIndex(int ind)
        {
            if (ind >= 0 & ind < Enemies.Count)
                return Enemies[ind];

            return null;
        }


        public void deleteEnemyByName(string name)
        {
            for(int i = 0; i < Enemies.Count; i++)
                if (Enemies[i].Name == name)
                {
                    Enemies.RemoveAt(i);
                    break;
                }
        }

        public void deleteEnemyByIndex(int ind)
        {
            if (ind >= 0 & ind < Enemies.Count)
                Enemies.RemoveAt(ind);
        }

        public List<string> getListOfEnemyNames()
        {
            List<string> names = new List<string>();

            for (int i = 0; i < Enemies.Count; i++)
            {
                names.Add(Enemies[i].Name);
            }

            return names;
        }

        public void SaveToFile()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new EnemyTemplateConverter() }  // Добавляем наш конвертер
            };

            string json = JsonSerializer.Serialize(Enemies, options);
            File.WriteAllText("enemies.json", json);
        }

        public void LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var options = new JsonSerializerOptions
                {
                    Converters = { new EnemyTemplateConverter() }
                };

                string json = File.ReadAllText(filePath);
                Enemies = JsonSerializer.Deserialize<List<CEnemyTemplate>>(json, options) ?? new List<CEnemyTemplate>();
            }
        }

    }
}
