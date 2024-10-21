using Editor_v0_1.Properties;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Editor_v0_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CIconList iconList = new CIconList(100, 100, 240, 400);
        CEnemyTemplateList enemies = new CEnemyTemplateList();
        string type = "Normal";
        public MainWindow()
        {
            InitializeComponent();

            iconList.Load(@"/Resources/MonsterIcons/");

            List<CIcon> icons = iconList.getIcons();

            foreach (CIcon icon in icons)
                scene.Children.Add(icon.getIcon());

            int delta = iconList.getDeltaY();

            if (delta > 0)
                img_scroll.Maximum = delta;
        }

        private void img_scroll_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double delta = e.OldValue - e.NewValue;
            iconList.scroll(delta);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            enemy_icon.Children.Clear();
            //получение координат мыши в координатах объекта Canvas с именем scene
            Point mousePosition = Mouse.GetPosition(scene);

            CIcon icon = iconList.isMouuseOver(mousePosition);

            if (icon != null)
            {
                lb_iconName.Content = icon.Name;
                enemy_icon.Children.Add(icon.CloneIcon());
            }
            else
            {
                lb_iconName.Content = "";
            }
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            string iconName = lb_iconName.Content.ToString();
            string name = tb_enemyName.Text;
            int baseLife = int.Parse(tb_enemyBaseLife.Text);
            double lifeModifier = double.Parse(tb_enemyLifeModifier.Text, System.Globalization.CultureInfo.InvariantCulture);
            int baseGold = int.Parse(tb_enemyBaseGold.Text);
            double goldModifier = double.Parse(tb_enemyGoldModifier.Text, System.Globalization.CultureInfo.InvariantCulture);
            double spawnChance = double.Parse(tb_enemySpawnChance.Text, System.Globalization.CultureInfo.InvariantCulture);


            if(type == "Normal")
            {
                CEnemyTemplate newEnemy = new CEnemyTemplate { Name = name, BaseGold = baseGold, BaseLife = baseLife, GoldModifier = goldModifier, IconName = iconName, LifeModifier = lifeModifier, SpawnChance = spawnChance };
                string result = enemies.addEnemy(newEnemy);

                if (result == "added")
                    lb_enemy_list.Items.Add(name);
            }
            if(type == "Armored")
            {
                int armor = int.Parse(tb_enemyArmor.Text, System.Globalization.CultureInfo.InvariantCulture);
                CArmoredEnemyTemplate newEnemy = new CArmoredEnemyTemplate { Name = name, BaseGold = baseGold, BaseLife = baseLife, GoldModifier = goldModifier, IconName = iconName, LifeModifier = lifeModifier, SpawnChance = spawnChance, Armor = armor};
                string result = enemies.addEnemy(newEnemy);

                if (result == "added")
                    lb_enemy_list.Items.Add(name);
            }

            

        }

        private void lb_enemy_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb_enemy_list.SelectedIndex > - 1)
            {
                CEnemyTemplate et = enemies.getEnemyByIndex(lb_enemy_list.SelectedIndex);

                if (et != null)
                {
                    enemy_icon.Children.Clear();
                    enemy_icon.Children.Add(iconList.findByName(et.IconName).CloneIcon());

                    lb_iconName.Content = et.IconName;
                    tb_enemyName.Text = et.Name;

                    tb_enemyBaseLife.Text = et.LifeModifier.ToString();
                    tb_enemyGoldModifier.Text = et.GoldModifier.ToString();

                    tb_enemyBaseGold.Text = et.BaseGold.ToString();
                    tb_enemyGoldModifier.Text = et.GoldModifier.ToString();

                    tb_enemySpawnChance.Text = et.SpawnChance.ToString();
                }
            }
        }

        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            if (lb_enemy_list.SelectedIndex > -1)
            {
                enemies.deleteEnemyByIndex(lb_enemy_list.SelectedIndex);
                lb_enemy_list.Items.RemoveAt(lb_enemy_list.SelectedIndex);
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            enemies.SaveToFile();
        }

        private void Load_Button_Click(object sender, RoutedEventArgs e)
        {
            enemies.LoadFromFile("null");
        }

        private void rb_NormalType_Checked(object sender, RoutedEventArgs e)
        {
            type = "Normal";
        }

        private void rb_ArmoredType_Checked(object sender, RoutedEventArgs e)
        {
            type = "Armored";
        }
    }
}
