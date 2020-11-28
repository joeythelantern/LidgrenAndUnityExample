using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
using Packets;

namespace Networking
{
	public class PlayerPosition
	{
		public float X { get; set; }
		public float Y { get; set; }
	}

	public class Client
	{
		public NetClient client { get; set; }

		public Client(int port, string server, string serverName) 
		{
			var config = new NetPeerConfiguration(serverName)
			{
				AutoFlushSendQueue = false
			};

			client = new NetClient(config);
			client.RegisterReceivedCallback(new SendOrPostCallback(ReceiveMessage));
			client.Start();

			client.Connect(server, port);
		}

		public void ReceiveMessage(object peer)
		{
			NetIncomingMessage message;

			while ((message = client.ReadMessage()) != null)
			{
				switch (message.MessageType)
				{
					case NetIncomingMessageType.Data:
						var packetType = (int)message.ReadByte();

						Debug.Log("Message type: " + packetType);

						Packet packet;

						switch(packetType)
						{
							case (int)PacketTypes.LocalPlayerPacket:
								packet = new LocalPlayerPacket();
								packet.NetIncomingMessageToPacket(message);
								ExtractLocalPlayerInformation((LocalPlayerPacket)packet);
								break;
							case (int)PacketTypes.PlayerDisconnectsPacket:
								packet = new PlayerDisconnectsPacket();
								packet.NetIncomingMessageToPacket(message);
								DisconnectPlayer((PlayerDisconnectsPacket)packet);
								break;
							case (int)PacketTypes.PositionPacket:
								packet = new PositionPacket();
								packet.NetIncomingMessageToPacket(message);
								UpdatePlayerPosition((PositionPacket)packet);
								break;
							case (int)PacketTypes.SpawnPacket:
								packet = new SpawnPacket();
								packet.NetIncomingMessageToPacket(message);
								SpawnPlayer((SpawnPacket)packet);
								break;
						}

						break;
					case NetIncomingMessageType.DebugMessage:
					case NetIncomingMessageType.ErrorMessage:
					case NetIncomingMessageType.WarningMessage:
					case NetIncomingMessageType.VerboseDebugMessage:
						string text = message.ReadString();
						Debug.Log(text);
						break;
					case NetIncomingMessageType.StatusChanged:
						NetConnectionStatus status = (NetConnectionStatus)message.ReadByte();
						string reason = message.ReadString();
						Debug.Log(status.ToString() + ": " + reason);
						break;
					default:
						Debug.Log("Unhandled type: " + message.MessageType + " " + message.LengthBytes + " bytes");
						break;
				}

				client.Recycle(message);
			}
		}

		public void SendPosition(float X, float Y)
		{
			Debug.Log("Sending position");

			NetOutgoingMessage message = client.CreateMessage();
			new PositionPacket(){ player = StaticManager.LocalPlayerID, X = X, Y = Y }.PacketToNetOutGoingMessage(message);
			client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
			client.FlushSendQueue();
		}

		public void SendDisconnect()
		{
			NetOutgoingMessage message = client.CreateMessage();
			new PlayerDisconnectsPacket(){ player = StaticManager.LocalPlayerID }.PacketToNetOutGoingMessage(message);
			client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
			client.FlushSendQueue();

			client.Disconnect("Bye!");
		}

		public void ExtractLocalPlayerInformation(LocalPlayerPacket packet)
		{
			Debug.Log("Local ID is " + packet.ID);

			StaticManager.LocalPlayerID = packet.ID;
		}

		public void SpawnPlayer(SpawnPacket packet)
		{
			Debug.Log("Spawning player " + packet.player);

			GameObject player = (GameObject)Resources.Load("Player");
			Vector3 position = new Vector3(packet.X, packet.Y);
			Quaternion rotation = new Quaternion();

			GameObject _player = MonoBehaviour.Instantiate(player, position, rotation);
			StaticManager.Players.Add(packet.player, _player);

			// If this is the local client, add controls!
			if (packet.player == StaticManager.LocalPlayerID)
			{
				_player.AddComponent<Controller>();
				_player.transform.name = "Local";
			}
			else
			{
				_player.transform.name = packet.player;
			}
		}

		public void UpdatePlayerPosition(PositionPacket packet)
		{
			Debug.Log("Moving player " + packet.player);

			StaticManager.Players[packet.player].gameObject.GetComponent<Movement>().SetMovePosition(new Vector3(packet.X, packet.Y));
		}

		public void DisconnectPlayer(PlayerDisconnectsPacket packet)
		{
			Debug.Log("Removing player " + packet.player);

			MonoBehaviour.Destroy(StaticManager.Players[packet.player]);
			StaticManager.Players.Remove(packet.player);
		}
	}
}