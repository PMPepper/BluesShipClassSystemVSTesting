/*using ProtoBuf;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
    public sealed class Comms
    {
        private static Dictionary<MessageType, List<Action<byte[]>>> MessageTypeHandlers = new Dictionary<MessageType, List<Action<byte[]>>>();

        // static constructor
        static Comms()
        {
            MyAPIGateway.Multiplayer.RegisterSecureMessageHandler(Settings.COMMS_MESSAGE_ID, MessageHandler);
        }

        public static void AddMessageTypeHandler(MessageType messageType, Action<byte[]> handler)
        {
            if(!MessageTypeHandlers.ContainsKey(messageType))
            {
                MessageTypeHandlers[messageType] = new List<Action<byte[]>>();
            }

            MessageTypeHandlers[messageType].Add(handler);
        }

        public static void RemoveMessageTypeHandler(MessageType messageType, Action<byte[]> handler)
        {
            if (MessageTypeHandlers.ContainsKey(messageType))
            {
                MessageTypeHandlers[messageType].Remove(handler);
            }
        }

        public static void SendMessage(MessageType type, byte[] data, bool toServer)
        {
            if (!Constants.IsMultiplayer)
            {
                Utils.ClientDebug("Not sending message, game is in single player mode");
                return;
            }

            Utils.Log($"[Comms] sending message to target = {(toServer ? "server" : "players")}");
            var message = new Message() { Type = type, Data = data };
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
                MyAPIGateway.Multiplayer.SendMessageToServer(Settings.COMMS_MESSAGE_ID, messageData);
            }
            else
            {
                MyAPIGateway.Multiplayer.SendMessageToOthers(Settings.COMMS_MESSAGE_ID, messageData);
            }

            Utils.Log($"[Comms] message sent to {(toServer ? "server" : "players")}");
        }

        private static void MessageHandler(ushort handlerId, byte[] data, ulong playerId, bool arg4)
        {
            //if playerid = 0, this comes from the server
            Utils.Log($"[Comms] message recieved, id = {handlerId}, from = {playerId}");

            try
            {
                var message = MyAPIGateway.Utilities.SerializeFromBinary<Message>(data);
                Utils.Log("[Comms] message de-serialised");

                var handlers = MessageTypeHandlers[message.Type];

                if(handlers  == null || handlers.Count == 0)
                {
                    Utils.Log($"[Comms] no message handler registered for message type {Enum.GetName(typeof(MessageType), message.Type)}", 1);
                } else
                {
                    foreach(var handler in handlers)
                    {
                        handler(message.Data);
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Log($"[Comms] message de-serialising error: {e.Message}", 2);
            }
        }
    }

    public enum MessageType
    {
        ShipClass
    }

    [ProtoContract(SkipConstructor = true)]
    internal struct Message
    {
        [ProtoMember(1)]
        public MessageType Type;
        [ProtoMember(2)]
        public byte[] Data;
    }
}
*/