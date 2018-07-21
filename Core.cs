using System;
using System.Reflection;
using Harmony;
using Newtonsoft.Json;

namespace MechTonnageFloatingPointFixer
{
  public class Core
  {
    public const string ModName = "MechTonnageFloatingPointFixer";
    public const string ModId   = "com.joelmeador.MechTonnageFloatingPointFixer";

    internal static Settings ModSettings = new Settings();
    internal static string ModDirectory;

    public static void Init(string directory, string settingsJSON)
    {
      ModDirectory = directory;
      try
      {
        ModSettings = JsonConvert.DeserializeObject<Settings>(settingsJSON);
      }
      catch (Exception ex)
      {
        Logger.Error(ex);
        ModSettings = new Settings();
      }

      var harmony = HarmonyInstance.Create(ModId);
      harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
  }
}