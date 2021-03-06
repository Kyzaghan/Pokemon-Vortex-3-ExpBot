﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;

namespace ControlGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const string AuthPath = "Config/authentication.json";
        private const string ConfigPath = "Config/config.json";
        private const string MapPath = "Config/map.json";
        private Process _cmd;
        public MainWindow()
        {
            InitializeComponent();
            LoadUi();
        }
        public dynamic LoadJson(string file)
        {
            try
            {
                using (StreamReader r = new StreamReader(file))
                {
                    string json = r.ReadToEnd();
                    dynamic array = JsonConvert.DeserializeObject(json);
                    return array;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
                return null;
            }
        }

        private void saveAuth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dynamic tmpAuth = LoadJson(AuthPath);
                if (tmpAuth != null)
                {
                    tmpAuth.Server = UiServer.Text;
                    tmpAuth.Username = UiUsername.Text;
                    tmpAuth.Password = UiPassword.Password;
                    tmpAuth.proxy.http = UiProxy.Text;
                    string json = JsonConvert.SerializeObject(tmpAuth, Formatting.Indented);
                    File.WriteAllText(AuthPath, json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

        private void LoadUi()
        {
            try
            {
                LoadAuth();
                LoadConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

        private void LoadAuth()
        {
            try
            {
                dynamic tmpAuth = LoadJson(AuthPath);
                if (tmpAuth != null)
                {
                    UiServer.Text = tmpAuth.Server;
                    UiUsername.Text = tmpAuth.Username;
                    UiPassword.Password = tmpAuth.Password;
                    UiProxy.Text = tmpAuth.proxy.http;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

        private void LoadConfig()
        {
            try
            {
                LoadConfigCombobox();
                LoadStaticComboBox();
                dynamic tmpConfig = LoadJson(ConfigPath);
                if (tmpConfig != null)
                {
                    UiLanguage.SelectedItem = UiLanguage.ItemsSource
                        .Cast<KeyValuePair<string, string>>()
                        .ToList().FirstOrDefault(x => x.Value == tmpConfig.Language.ToString());
                    UiCurrentMap.SelectedItem = UiCurrentMap.ItemsSource
                        .Cast<KeyValuePair<string, string>>()
                        .ToList().FirstOrDefault(x => x.Value == tmpConfig.Catcher.CurrentMap.ToString());
                    UiPokeBall.SelectedItem = new KeyValuePair<string, string>(tmpConfig.Catcher.PokeBall.ToString(), tmpConfig.Catcher.PokeBall.ToString());
                    UiSleepSecondsAfterBattle.Text = tmpConfig.Catcher.SleepSecondsAfterBattle;
                    UiAutoBuyPokeBall.IsChecked = tmpConfig.Catcher.AutoBuyPokeBall;
                    UiCatchOnlyLegendaryPokemon.IsChecked = tmpConfig.Catcher.CatchOnlyLegendaryPokemon;
                    UiCatchOnlyLegendaryPokemonIgnoreTypes.IsChecked = tmpConfig.Catcher.CatchOnlyLegendaryPokemonIgnoreTypes;
                    UiCatchOnlyWithPokemonFilter.IsChecked = tmpConfig.Catcher.CatchOnlyWithPokemonFilter;
                    UiCatchOnlyWithPokemonFilterIgnoreTypes.IsChecked = tmpConfig.Catcher.CatchOnlyWithPokemonFilterIgnoreTypes;
                    UiCatchPokemonNotInPokedex.IsChecked = tmpConfig.Catcher.CatchPokemonNotInPokedex;
                    UiDay.IsChecked = tmpConfig.Catcher.DayOrNight == "Day";
                    UiNight.IsChecked = tmpConfig.Catcher.DayOrNight == "Night";
                    UiDontPrintNoPokemonFoundText.IsChecked = tmpConfig.Catcher.DontPrintNoPokemonFoundText;
                    UiPokeball.Text = tmpConfig.Catcher.PokeBallBuyList["Poke Ball"];
                    UiGreatBall.Text = tmpConfig.Catcher.PokeBallBuyList["Great Ball"];
                    UiUltraBall.Text = tmpConfig.Catcher.PokeBallBuyList["Ultra Ball"];
                    UiMasterBall.Text = tmpConfig.Catcher.PokeBallBuyList["Master Ball"];
                    UiUserAgent.Text = tmpConfig.UserAgent;
                    UiTrainer.Text = tmpConfig.ExpBot.Traniner;
                    UiSleepSecondsAfterBattleExpBot.Text = tmpConfig.ExpBot.SleepSecondsAfterBattle;
                    UiSleepSecondsAfterAttack.Text = tmpConfig.ExpBot.SleepSecondsAfterAttack;
                    UiAttackToUse.SelectedItem = UiAttackToUse.ItemsSource
                        .Cast<KeyValuePair<string, int>>()
                        .ToList().FirstOrDefault(x => x.Value == Convert.ToInt32(tmpConfig.ExpBot.AttackToUse));
                    UiCatchLegyWithPokemonFilter.IsChecked = tmpConfig.Catcher.CatchLegyWithPokemonFilter;
                    UiPotionToUse.SelectedItem = UiPotionToUse.ItemsSource
                        .Cast<KeyValuePair<string, string>>()
                        .ToList().FirstOrDefault(x => x.Value == tmpConfig.ExpBot.PotionToUse.ToString());
                    UiUseWhenHPBelow.Text = Convert.ToString(tmpConfig.ExpBot.UseWhenHPBelow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

        private void LoadConfigCombobox()
        {
            try
            {
                dynamic tmpMap = LoadJson(MapPath);
                List<KeyValuePair<string, string>> tmpList = new List<KeyValuePair<string, string>>();
                foreach (var forMap in tmpMap["MapList"])
                {
                    foreach (var subitem in forMap)
                    {
                        tmpList.Add(new KeyValuePair<string, string>(subitem["Type"].ToString(), subitem["main"].ToString()));
                    }
                }
                UiCurrentMap.ItemsSource = tmpList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

        private void LoadStaticComboBox()
        {
            try
            {
                List<KeyValuePair<string, string>> tmpLanguageKeyValuePairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("English", "en"),
                    new KeyValuePair<string, string>("Turkish", "tr")
                };
                UiLanguage.ItemsSource = tmpLanguageKeyValuePairs;


                List<KeyValuePair<string, string>> tmpPokeBallsKeyValuePairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Poke Ball", "Poke Ball"),
                    new KeyValuePair<string, string>("Great Ball", "Great Ball"),
                    new KeyValuePair<string, string>("Ultra Ball", "Ultra Ball"),
                    new KeyValuePair<string, string>("Master Ball", "Master Ball")
                };
                UiPokeBall.ItemsSource = tmpPokeBallsKeyValuePairs;

                List<KeyValuePair<string, int>> tmpAttactKeyValuePairs = new List<KeyValuePair<string, int>>
                {
                    new KeyValuePair<string, int>("Attack 1", 1),
                    new KeyValuePair<string, int>("Attack 2", 2),
                    new KeyValuePair<string, int>("Attack 3", 3),
                    new KeyValuePair<string, int>("Attack 4", 4),
                };
                UiAttackToUse.ItemsSource = tmpAttactKeyValuePairs;


                List<KeyValuePair<string, string>> tmpPotionToUseKeyValuePairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Potion", "Potion"),
                    new KeyValuePair<string, string>("Super Potion", "Super Potion"),
                    new KeyValuePair<string, string>("Hyper Potion", "Hyper Potion"),
                    new KeyValuePair<string, string>("Full Heal", "Full Potion"),
                };
                UiPotionToUse.ItemsSource = tmpPotionToUseKeyValuePairs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

        private void StartBot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcessStartInfo cmdStartInfo = new ProcessStartInfo
                {
                    FileName = "run.bat",
                    UseShellExecute = true
                };

                _cmd = new Process
                {
                    StartInfo = cmdStartInfo
                };
                _cmd.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        }

        private void SaveConfigCatcher_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                dynamic tmpConfigCatcher = LoadJson(ConfigPath);
                if (tmpConfigCatcher != null)
                {
                    tmpConfigCatcher.Language = UiLanguage.SelectedItem != null ? ((KeyValuePair<string, string>)UiLanguage.SelectedItem).Value : "en";
                    tmpConfigCatcher.Catcher.CurrentMap = UiCurrentMap.SelectedItem != null ? ((KeyValuePair<string, string>)UiCurrentMap.SelectedItem).Value : "1";
                    tmpConfigCatcher.Catcher.PokeBall = UiPokeBall.SelectedItem != null ? ((KeyValuePair<string, string>)UiPokeBall.SelectedItem).Value : "Poke Ball";
                    tmpConfigCatcher.Catcher.SleepSecondsAfterBattle = UiSleepSecondsAfterBattle.Text;
                    tmpConfigCatcher.Catcher.AutoBuyPokeBall = UiAutoBuyPokeBall.IsChecked;
                    tmpConfigCatcher.Catcher.CatchOnlyLegendaryPokemon = UiCatchOnlyLegendaryPokemon.IsChecked;
                    tmpConfigCatcher.Catcher.CatchOnlyLegendaryPokemonIgnoreTypes = UiCatchOnlyLegendaryPokemonIgnoreTypes.IsChecked;
                    tmpConfigCatcher.Catcher.CatchOnlyWithPokemonFilter = UiCatchOnlyWithPokemonFilter.IsChecked;
                    tmpConfigCatcher.Catcher.CatchOnlyWithPokemonFilterIgnoreTypes = UiCatchOnlyWithPokemonFilterIgnoreTypes.IsChecked;
                    tmpConfigCatcher.Catcher.CatchPokemonNotInPokedex = UiCatchPokemonNotInPokedex.IsChecked;
                    tmpConfigCatcher.Catcher.DayOrNight = UiDay.IsChecked != null && UiDay.IsChecked.Value ? "Day" : "Night";
                    tmpConfigCatcher.Catcher.DontPrintNoPokemonFoundText = UiDontPrintNoPokemonFoundText.IsChecked;
                    tmpConfigCatcher.Catcher.PokeBallBuyList["Poke Ball"] = UiPokeball.Text;
                    tmpConfigCatcher.Catcher.PokeBallBuyList["Great Ball"] = UiGreatBall.Text;
                    tmpConfigCatcher.Catcher.PokeBallBuyList["Ultra Ball"] = UiUltraBall.Text;
                    tmpConfigCatcher.Catcher.PokeBallBuyList["Master Ball"] = UiMasterBall.Text;
                    tmpConfigCatcher.Catcher.CatchLegyWithPokemonFilter = UiCatchLegyWithPokemonFilter.IsChecked;
                    tmpConfigCatcher.UserAgent = tmpConfigCatcher.UserAgent;
                    string json = JsonConvert.SerializeObject(tmpConfigCatcher, Formatting.Indented);
                    File.WriteAllText(ConfigPath, json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

        private void SaveConfigExpBot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dynamic tmpConfigExpBot = LoadJson(ConfigPath);
                if (tmpConfigExpBot != null)
                {
                    tmpConfigExpBot.ExpBot.Traniner = UiTrainer.Text;
                    tmpConfigExpBot.ExpBot.SleepSecondsAfterBattle = Convert.ToInt32(UiSleepSecondsAfterBattleExpBot.Text);
                    tmpConfigExpBot.ExpBot.SleepSecondsAfterAttack = Convert.ToInt32(UiSleepSecondsAfterAttack.Text);
                    tmpConfigExpBot.ExpBot.AttackToUse = ((KeyValuePair<string, int>?)UiAttackToUse.SelectedItem)?.Value ?? 1;
                    tmpConfigExpBot.ExpBot.PotionToUse = ((KeyValuePair<string, string>?)UiPotionToUse.SelectedItem)?.Value ?? "Potion";
                    tmpConfigExpBot.ExpBot.UseWhenHPBelow = Convert.ToInt32(UiUseWhenHPBelow.Text);
                    string json = JsonConvert.SerializeObject(tmpConfigExpBot, Formatting.Indented);
                    File.WriteAllText(ConfigPath, json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }
    }
}
