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
            Utils.Log($"[Comms] message recieved, id = {handlerId}, from = {playerId}");

            try
            {
                var message = MyAPIGateway.Utilities.SerializeFromBinary<T>(data);
                Utils.Log("[Comms] message de-serialised");

                if (OnMessage != null)
                {
                    OnMessage(message, playerId);
                }
            }
            catch (Exception e)
            {
                Utils.Log($"[Comms] message de-serialising error: {e.Message}", 2);
            }
        }

        public void SendMessage(T message, bool toServer) {
            if(!Constants.IsMultiplayer)
            {
                Utils.ClientDebug("Not sending message, game is in single player mode");
                return;
            }

            Utils.Log($"[Comms] sending message to target = {(toServer ? "server" : "players")}");
            byte[] messageData;

            try
            {
                messageData = MyAPIGateway.Utilities.SerializeToBinary(message);
                Utils.Log($"[Comms] message serialised, length={messageData.Length}");

            }
            catch (Exception e)
            {
                Utils.Log($"[Comms] message serialising error: {e.Message}", 2);

                Utils.WriteToClient("Error serialising message, see logs for details");

                return;
            }

            if (toServer)
            {
                //Sending to server
                if (Constants.IsServer) {
                    Utils.Log($"[Comms] message target cannot be 0 when running on the server", 2);

                    throw new ArgumentException("[Comms] message target cannot be 0 when running on the server");
                }

                MyAPIGateway.Multiplayer.SendMessageToServer(MessageId, messageData);
            }
            else {
                //trying to send to player
                if (!Constants.IsServer) {
                    //Players cannot send messages to other players
                    Utils.Log("[Comms] message target cannot sent to players when not the server", 2);

                    throw new ArgumentException("[Comms] message target cannot sent to players when not the server");
                }

                MyAPIGateway.Multiplayer.SendMessageToOthers(MessageId, messageData);
            }

            Utils.Log($"[Comms] message sent to {(toServer ? "server" : "players")}");
        }
    }
}
