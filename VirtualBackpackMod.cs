using Eleon;
using Eleon.Modding;
using System;
using System.IO;
using Newtonsoft.Json;

public class VirtualBackpackMod : ModInterface
{
  private ModGameAPI api;
  private string dataPath = Path.Combine(Path.GetDirectoryName(typeof(VirtualBackpackMod).Assembly.Location), "PlayerData");

  public void Game_Start(ModGameAPI dediAPI)
  {
    api = dediAPI;
    Directory.CreateDirectory(dataPath);
    api.Console_Write("VirtualBackpackMod loaded.");
  }

  public void Game_Exit()
  {
    api?.Console_Write("VirtualBackpackMod shutting down.");
  }

  public void Game_Update() { }

  public void Game_Event(CmdId eventId, ushort seqNr, object data)
  {
    if (eventId == CmdId.Event_ChatMessage)
    {
      var chat = (ChatInfo)data;

      if (chat.msg.Trim() == "/vb")
      {
        int playerId = chat.playerId;
        string playerKey = playerId.ToString();

        var items = LoadBackpack(playerKey);
        ShowBackpackUI(playerId, seqNr, items);
      }
    }

    // Called when the backpack window is changed or closed
    else if (eventId == CmdId.Event_Player_ItemExchange)
    {
      var exchange = (ItemExchangeInfo)data;
      int playerId = exchange.id;
      string playerKey = playerId.ToString();

      SaveBackpack(playerKey, exchange.items);
      api.Console_Write($"[Backpack] Saved backpack for player {playerId}");
    }
  }

  private void ShowBackpackUI(int playerId, ushort seqNr, ItemStack[] items)
  {
    ItemExchangeInfo ui = new ItemExchangeInfo()
    {
      id = playerId,
      title = "Virtual Backpack",
      desc = "Your personal storage",
      items = items,
      buttonText = "Close"
    };

    api.Game_Request(CmdId.Request_Player_ItemExchange, seqNr, ui);
  }

  private ItemStack[] LoadBackpack(string playerKey)
  {
    string path = Path.Combine(dataPath, playerKey + ".json");

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
        api.Console_Write("Error reading backpack: " + ex.Message);
      }
    }

    // Create new empty backpack if not found or error
    return new ItemStack[40];
  }

  private void SaveBackpack(string playerKey, ItemStack[] items)
  {
    try
    {
      string path = Path.Combine(dataPath, playerKey + ".json");
      var wrapper = new BackpackData { items = items };
      var json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
      File.WriteAllText(path, json);
    }
    catch (Exception ex)
    {
      api.Console_Write("Error saving backpack: " + ex.Message);
    }
  }
}

public class BackpackData
{
  public ItemStack[] items { get; set; }
}
