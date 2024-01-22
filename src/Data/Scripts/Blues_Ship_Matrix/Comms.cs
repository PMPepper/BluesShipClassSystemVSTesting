using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
    class Comms<T>
    {
        private readonly ushort MessageId;
        public Action<T, ulong> OnMessage;
        public Comms(ushort messageId) {
            MessageId = messageId;
            
            MyAPIGateway.Multiplayer.RegisterSecureMessageHandler(MessageId, MessageHandler);
        }

        private void MessageHandler(ushort handlerId, byte[] data, ulong playerId, bool arg4)
        {
            //if playerid = 0, this comes from the server
            ModSessionManager.Log($"[Comms] message recieved, id = {handlerId}, from = {playerId}");

            try
            {
                var message = MyAPIGateway.Utilities.SerializeFromBinary<T>(data);
                ModSessionManager.Log("[Comms] message de-serialised");

                if (OnMessage != null)
                {
                    OnMessage(message, playerId);
                }
            }
            catch (Exception e)
            {
                ModSessionManager.Log($"[Comms] message de-serialising error: {e.Message}", 2);
            }
        }

        public void SendMessage(T message, bool toServer) {
            if(!ModSessionManager.IsMultiplayer)
            {
                ModSessionManager.ClientDebug("Not sending message, game is in single player mode");
                return;
            }

            ModSessionManager.Log($"[Comms] sending message to target = {(toServer ? "server" : "players")}");
            byte[] messageData;

            try
            {
                messageData = MyAPIGateway.Utilities.SerializeToBinary(message);
                ModSessionManager.Log($"[Comms] message serialised, length={messageData.Length}");

            }
            catch (Exception e)
            {
                ModSessionManager.Log($"[Comms] message serialising error: {e.Message}", 2);

                throw e;
            }

            if (toServer)
            {
                //Sending to server
                if (ModSessionManager.IsServer) {
                    ModSessionManager.Log($"[Comms] message target cannot be 0 when running on the server", 2);

                    throw new ArgumentException("[Comms] message target cannot be 0 when running on the server");
                }

                MyAPIGateway.Multiplayer.SendMessageToServer(MessageId, messageData);
            }
            else {
                //trying to send to player
                if (!ModSessionManager.IsServer) {
                    //Players cannot send messages to other players
                    ModSessionManager.Log("[Comms] message target cannot sent to players when not the server", 2);

                    throw new ArgumentException("[Comms] message target cannot sent to players when not the server");
                }

                MyAPIGateway.Multiplayer.SendMessageToOthers(MessageId, messageData);
            }

            ModSessionManager.Log($"[Comms] message sent to {(toServer ? "server" : "players")}");
        }
    }
}
