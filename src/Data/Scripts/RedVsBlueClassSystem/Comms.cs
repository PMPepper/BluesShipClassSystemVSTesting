using ProtoBuf;
using Sandbox.Game.GameSystems;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI;

namespace RedVsBlueClassSystem
{
    
    internal class Comms
    {
        private readonly ushort CommsId = 6412;

        public Comms()
        {
            MyAPIGateway.Multiplayer.RegisterSecureMessageHandler(CommsId, MessageHandler);
        }

        public void SendChangeGridClassMessage(long entityId, long gridClassId)
        {
            var messageData = MyAPIGateway.Utilities.SerializeToBinary(new ChangeGridClassMessage() { EntityId = entityId, GridClassId = gridClassId });
            var message = MyAPIGateway.Utilities.SerializeToBinary(new Message() { Type = MessageType.ChangeGridClass, Data = messageData });

            MyAPIGateway.Multiplayer.SendMessageToServer(CommsId, message);
        }

        private void MessageHandler(ushort handlerId, byte[] data, ulong playerId, bool unknown)
        {
            if (!Constants.IsServer)
            {
                throw new Exception("Only the server should be recieveing messages");
            }

            var message = MyAPIGateway.Utilities.SerializeFromBinary<Message>(data);

            switch(message.Type)
            {
                case MessageType.ChangeGridClass:
                    HandleChangeGridClassMessage(message.Data);
                    break;
                default:
                    Utils.Log("Unknown message type", 3);
                    break;
            }
        }

        private void HandleChangeGridClassMessage(byte[] data)
        {
            var message = MyAPIGateway.Utilities.SerializeFromBinary<ChangeGridClassMessage>(data);

            Utils.WriteToClient($"HandleChangeGridClassMessage: {message.EntityId}, {message.GridClassId}");

            var entity = MyAPIGateway.Entities.GetEntityById(message.EntityId);

            if(entity is IMyCubeGrid)
            {
                var gridLogic = (entity as IMyCubeGrid).GetGridLogic();

                if(gridLogic != null)
                {
                    if(ModSessionManager.IsValidGridClass(message.GridClassId))
                    {
                        gridLogic.GridClassId = message.GridClassId;
                    } else
                    {
                        Utils.Log($"HandleChangeGridClassMessage: Uknown grid class ID {message.GridClassId}", 3);
                    }
                }
            } else
            {
                Utils.Log($"HandleChangeGridClassMessage: Uknown entity {message.EntityId}", 3);
            }
        }
    }

    enum MessageType
    {
        ChangeGridClass,
    }

    [ProtoContract]
    internal struct Message
    {
        [ProtoMember(1)]
        public MessageType Type;
        [ProtoMember(2)]
        public byte[] Data;
    }

    [ProtoContract]
    internal struct ChangeGridClassMessage
    {
        [ProtoMember(1)]
        public long EntityId;
        [ProtoMember(2)]
        public long GridClassId;
    }
}
