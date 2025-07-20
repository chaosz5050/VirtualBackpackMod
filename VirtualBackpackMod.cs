using Eleon;
using Eleon.Modding;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class VirtualBackpackMod : ModInterface
{
  private ModGameAPI api;
  private string dataPath = Path.Combine(Path.GetDirectoryName(typeof(VirtualBackpackMod).Assembly.Location), "PlayerData");
  private Dictionary<int, int> activeBackpackIndex = new Dictionary<int, int>();

  public void Game_Start(ModGameAPI dediAPI)
  {
    api = dediAPI;
    Directory.CreateDirectory(dataPath);
    api.Console_Write("VirtualBackpackMod loaded.");
  }

  public void Game_Exit()
  {
    // Save any open backpacks before shutdown
    foreach (var kvp in activeBackpackIndex)
    {
      api.Console_Write($"[Backpack] Cleaning up tracking for player {kvp.Key}");
    }
    activeBackpackIndex.Clear();
    
    api?.Console_Write("VirtualBackpackMod shutting down.");
  }

  public void Game_Update() { }

  public void Game_Event(CmdId eventId, ushort seqNr, object data)
  {
    if (eventId == CmdId.Event_ChatMessage)
    {
      var chat = (ChatInfo)data;
      string msg = chat.msg.Trim().ToLowerInvariant();

      int backpackNumber = 0;

      if (msg == "/vb1")
      {
        backpackNumber = 1;
      }
      else if (msg == "/vb2")
      {
        backpackNumber = 2;
      }

      if (backpackNumber > 0)
      {
        int playerId = chat.playerId;
        string playerKey = playerId.ToString();

        // Track which backpack this player is using
        activeBackpackIndex[playerId] = backpackNumber;

        var items = LoadBackpack(playerKey, backpackNumber);
        ShowBackpackUI(playerId, seqNr, items, backpackNumber);
      }
    }

    // Called when the backpack window is changed or closed
    else if (eventId == CmdId.Event_Player_ItemExchange)
    {
      var exchange = (ItemExchangeInfo)data;
      int playerId = exchange.id;
      string playerKey = playerId.ToString();

      // Get which backpack this player was using
      if (activeBackpackIndex.TryGetValue(playerId, out int backpackNumber))
      {
        SaveBackpack(playerKey, exchange.items, backpackNumber);
        api.Console_Write($"[Backpack] Saved backpack {backpackNumber} for player {playerId}");
        
        // DON'T remove from tracking - keep it so multiple saves work
        // activeBackpackIndex.Remove(playerId);
      }
    }
  }

  private void ShowBackpackUI(int playerId, ushort seqNr, ItemStack[] items, int backpackNumber)
  {
    ItemExchangeInfo ui = new ItemExchangeInfo()
    {
      id = playerId,
      title = $"Virtual Backpack {backpackNumber}",
      desc = "Your personal storage",
      items = items,
      buttonText = "Close"
    };

    api.Game_Request(CmdId.Request_Player_ItemExchange, seqNr, ui);
  }

  private ItemStack[] LoadBackpack(string playerKey, int backpackNumber)
  {
    string path = Path.Combine(dataPath, playerKey + ".vb" + backpackNumber + ".json");

    if (File.Exists(path))
    {
      try
      {
        var json = File.ReadAllText(path);
        var wrapper = JsonConvert.DeserializeObject<BackpackData>(json);
        return wrapper?.items ?? new ItemStack[40];
      }
      catch (Exception ex)
      {
        api.Console_Write($"Error reading backpack {backpackNumber}: " + ex.Message);
      }
    }

    // Create new empty backpack if not found or error
    return new ItemStack[40];
  }

  private void SaveBackpack(string playerKey, ItemStack[] items, int backpackNumber)
  {
    try
    {
      string path = Path.Combine(dataPath, playerKey + ".vb" + backpackNumber + ".json");
      var wrapper = new BackpackData { items = items };
      var json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
      File.WriteAllText(path, json);
    }
    catch (Exception ex)
    {
      api.Console_Write($"Error saving backpack {backpackNumber}: " + ex.Message);
    }
  }
}

public class BackpackData
{
  public ItemStack[] items { get; set; }
}