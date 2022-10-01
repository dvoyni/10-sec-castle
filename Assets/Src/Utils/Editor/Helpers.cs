using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TenSecCastle.Model;
using UnityEditor;
using UnityEngine;
using Attribute = TenSecCastle.Model.Attribute;

public static class Helpers {
    private static readonly string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    
    [MenuItem("10 Sec Castle/Reload Config")]
    public static async void ReloadConfig() {
        
        Debug.Log("...Loading data...");
        
        var itemsCSV = DownloadCSV("1hnbstt53_ZFGjA758OczVS4CTnIyeLhBmcRXxcmRi10", "970349213");
        await itemsCSV;
        
        var charactersCSV = DownloadCSV("1hnbstt53_ZFGjA758OczVS4CTnIyeLhBmcRXxcmRi10", "157130821");
        await charactersCSV;
        
        Debug.Log("...Data loaded...");
        
        var itemsData = ParseCSV(itemsCSV.Result);
        
        var charactersData = ParseCSV(charactersCSV.Result);
       
        Debug.Log("...Parse data...");
        
        CreateFile("Src/TenSecCastle/Game",itemsData,charactersData);
        
        Debug.Log("File successfully created!");
    }

    private static void CreateFile(string path, Dictionary<int, List<string>> itemsData,Dictionary<int, List<string>> charactersData) {
        string copyPath = $"Assets/{path}/GameConfig.cs";
        Debug.Log("Creating Classfile: " + copyPath);
        
        using (StreamWriter outfile = new StreamWriter(copyPath)) {
            
            outfile.WriteLine("using Rondo.Core.Lib.Containers;");
            outfile.WriteLine("using TenSecCastle.Model;");
            outfile.WriteLine("");
            outfile.WriteLine("namespace TenSecCastle.Game {");
            outfile.WriteLine("    public class GameConfig {");
            outfile.WriteLine("        public static L<Item> Items {");
            outfile.WriteLine("            get {");
            outfile.WriteLine("                var list = new L<Item>();");
            outfile.WriteLine("");
            foreach (var item in itemsData) {
                var parsedItem = ParseItem(item.Value);
                outfile.WriteLine("                list += new Item {");
                outfile.WriteLine($"                    Id = {item.Key},");
                outfile.WriteLine($"                    SlotKind = SlotKind.{parsedItem.SlotKind},");
                outfile.WriteLine("                    Attributes = new(");
                for (int i = 0; i < parsedItem.Attributes.Count; i++) {
                    outfile.WriteLine($"                        new Attribute {{ Kind = AttributeKind.{parsedItem.Attributes[i].Kind}" +
                                      $", Value = {parsedItem.Attributes[i].Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}f" +
                                      $", AttackType = AttackType.{parsedItem.Attributes[i].AttackType}" +
                                      $", AttackRange = AttackRange.{parsedItem.Attributes[i].AttackRange}}}"+
                                      (i+1<parsedItem.Attributes.Count?",":""));
                }
                outfile.WriteLine("                    ),");
                outfile.WriteLine("                };");
                outfile.WriteLine("");
            }
            outfile.WriteLine("                return list;");
            outfile.WriteLine("            }");
            outfile.WriteLine("        }");
            outfile.WriteLine("");
            outfile.WriteLine("        public static Unit BasicUnit => new() {");
            outfile.WriteLine($"            MaxHitPoints = {float.Parse(charactersData[0][0]).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}f,");
            outfile.WriteLine($"            HpRegen = {float.Parse(charactersData[1][0]).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}f,");
            outfile.WriteLine($"            PhysicsAttack = {float.Parse(charactersData[2][0]).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}f,");
            outfile.WriteLine($"            PhysicsDefense = {float.Parse(charactersData[3][0]).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}f,");
            outfile.WriteLine($"            MagicAttack = {float.Parse(charactersData[4][0]).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}f,");
            outfile.WriteLine($"            MagicDefense = {float.Parse(charactersData[5][0]).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}f,");
            outfile.WriteLine($"            MoveSpeed = {float.Parse(charactersData[6][0]).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}f, //cells per second");
            outfile.WriteLine($"            AttackSpeed = {float.Parse(charactersData[7][0]).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}f, //attacks per second");
            outfile.WriteLine($"            Income = {int.Parse(charactersData[8][0])}, //income per kill");
            outfile.WriteLine("        };");
            outfile.WriteLine("    }");
            outfile.WriteLine("}");
        }
    }

    private static Item ParseItem(List<string> data) {
        
        var item = new Item();
        switch (data[0]) {
            case "weapon":
                item.SlotKind = SlotKind.Weapon;
                break;
            case "armor":
                item.SlotKind = SlotKind.Armor;
                break;
            case "jewelry":
                item.SlotKind = SlotKind.Jewelry;
                break;
        }

        item.Attributes = new List<Attribute>();
        
        for (int i = 1; i <=5; i += 4) {
            if (data[i] != "none") {
                var attribute = new Attribute();

                attribute.Kind = GetKind(data[i], item);
                attribute.AttackType = data[i + 2] == "physical" ? AttackType.Physical :
                    data[i + 2] == "magic" ? AttackType.Magical : AttackType.None;
                attribute.AttackRange = data[i + 3] == "melee" ? AttackRange.Melee :
                    data[i + 3] == "range" ? AttackRange.Ranged : AttackRange.None;
                attribute.Value = float.Parse(data[i + 1]);
                
                item.Attributes.Add(attribute);
            }
        }
        
        return item;
    }

    private static AttributeKind GetKind(string key, Item item) {
        switch (key) {
            case "physical attack":
            case "magic attack":
                return item.SlotKind == SlotKind.Weapon ? AttributeKind.Weapon : AttributeKind.Attack;
            case "magic defense":
            case "physical defense":
                return AttributeKind.Defense;
                   
            case "hit points":
                return AttributeKind.HitPoints;
            case "hp regeneration":
                return AttributeKind.HitPointRegen;
            case "money for kill":
                return AttributeKind.Income;
        }

        throw new Exception("Wrong key");
    }

    private class Item {
        public ulong Id;
        public SlotKind SlotKind;
        public List<Attribute> Attributes;
    }


    private static async Task<string> DownloadCSV(string docId, string sheetId = null) {

        string url = "https://docs.google.com/spreadsheets/d/" + docId + "/export?format=csv";

        if (!string.IsNullOrEmpty(sheetId)) url += "&gid=" + sheetId;
        
        var webClient = new System.Net.WebClient();
        
        string source = await webClient.DownloadStringTaskAsync(url);
        
        if (string.IsNullOrEmpty(source)) {
            throw new Exception("Download error");
        }

        return source;
    }

    private static Dictionary<int, List<string>> ParseCSV(string text) {
        text = text.Replace(",,,", ",");
        var lines = Regex.Split(text, LINE_SPLIT_RE);
        lines = lines.Select(x => x.Replace("\"", "")).ToArray();

        var result = new Dictionary<int, List<string>>();
        var parsedLine = lines.Select(x => x.Split(',')).ToList();

        for (int i = 0; i < parsedLine.Count; i++) {
            
            var index = int.Parse(parsedLine[i][0]);
            
            result.Add(index, new List<string>());
            
            for (int j = 1; j < parsedLine[0].Length; j++)
            {
                result[index].Add(parsedLine[i][j].Replace(".",","));
            }
        }

        return result;
    }
}
