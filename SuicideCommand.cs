using System;
using System.Collections.Generic;
using System.Linq;
using Eleon.Modding;




public partial class InventorySorting: ModInterface
{
    static ModGameAPI GameAPI;
    
    public void Game_Start(ModGameAPI dediAPI)
    {
        InventorySorting.GameAPI = dediAPI;
        LogFile("chat.txt", "Mod Loaded");
        GameAPI.Console_Write("Inventory Sorting Launched! ");
    }

    private void LogFile(String FileName, String FileData)
    {
        if (!System.IO.File.Exists("Content\\Mods\\SuicideCommand\\" + FileName))
        {
            System.IO.File.Create("Content\\Mods\\SuicideCommand\\" + FileName);
        }
        string FileData2 = FileData + Environment.NewLine;
        System.IO.File.AppendAllText("Content\\Mods\\SuicideCommand\\" + FileName, FileData2);
    }


    public void Game_Event(CmdId eventId, ushort seqNr, object data)
    {

        GameAPI.Console_Write($"ID:EVENT! {eventId} - {seqNr}");
        try
        {
            switch (eventId)
            {

                case CmdId.Event_Player_Connected:
                    int id = (int)data;
                    GameAPI.Console_Write("SuicideCommand : player " + id + "connected! ");
                    LogFile("chat.txt", "Player " + id + "connected");
                    break;
                case CmdId.Event_ChatMessage:
                    ChatInfo ci = (ChatInfo)data;
                    if (ci.msg.StartsWith("s! "))
                    {
                        ci.msg = ci.msg.Remove(0, 3);
                    }
                    ci.msg = ci.msg.ToLower();
                    if (ci.msg.StartsWith("/suicide"))
                    {
                        int playerId = ci.playerId;
                        LogFile("chat.txt", "Player " + playerId + "commited suicide");
                        GameAPI.Game_Request(CmdId.Request_Player_SetPlayerInfo, 2015, new PlayerInfoSet()
                        {
                            health = 0
                        });
                        
                    }
                    break;
               

                default:
                    GameAPI.Console_Write($"event: {eventId}");
                    var outmessage = "NO DATA";
                    if (data != null)
                    {
                        outmessage = "data: " + data.ToString();
                    }
                    GameAPI.Console_Write(outmessage);
                    
                    break;
            }
        }
        catch (Exception ex)
        {
            GameAPI.Console_Write(ex.Message);
            GameAPI.Console_Write(ex.ToString());
        }
    }


    public void Game_Update()
    {
    }

    public void Game_Exit()
    {
    }

}