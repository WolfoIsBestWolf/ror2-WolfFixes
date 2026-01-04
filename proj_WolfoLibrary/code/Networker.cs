using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace WolfoLibrary
{
    public static class Networker
    {
        public static readonly Dictionary<Type, byte> message_to_index = new Dictionary<Type, byte>();
        public static readonly Dictionary<byte, Type> index_to_message = new Dictionary<byte, Type>();


        public static void SendWQoLMessage(ChatMessageBase message)
        {
            SendBroadcastChat(message, QosChannelIndex.chat.intVal);
        }
        public static void SendBroadcastChat(ChatMessageBase message, int channelIndex)
        {
            if (WConfig.cfgTestMultiplayer.Value)
            {
                Debug.Log("SendBroadcastChat");
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.StartMessage(20024);
            networkWriter.Write(message_to_index[message.GetType()]);
            networkWriter.Write(message);
            networkWriter.FinishMessage();
            foreach (NetworkConnection networkConnection in NetworkServer.connections)
            {
                if (networkConnection != null)
                {
                    networkConnection.SendWriter(networkWriter, channelIndex);
                }
            }
        }

        public static ChatMessageBase InstantiateMessage(byte typeIndex)
        {
            Type type = index_to_message[typeIndex];
            if (ChatMessageBase.cvChatDebug.value)
            {
                Debug.LogFormat("Received chat message typeIndex={0} type={1}", new object[]
                {
                    typeIndex,
                    (type != null) ? type.Name : null
                });
            }
            if (type != null)
            {
                return (ChatMessageBase)Activator.CreateInstance(type);
            }
            return null;
        }

        [NetworkMessageHandler(msgType = 20024, client = true)]
        public static void HandleBroadcastChat(NetworkMessage netMsg)
        {
            if (WConfig.cfgTestMultiplayer.Value)
            {
                Debug.Log("HandleBroadcastChat");
            }
            ChatMessageBase chatMessageBase = InstantiateMessage(netMsg.reader.ReadByte());
            if (chatMessageBase != null)
            {
                chatMessageBase.Deserialize(netMsg.reader);
                Chat.AddMessage(chatMessageBase);
            }
        }
    }
}