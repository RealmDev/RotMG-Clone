using System.Collections.Generic;
using Assets;
using Networking.GameMap;
using Networking.Objects;
using RotMG_Net_Lib.Constants;
using RotMG_Net_Lib.Models;
using RotMG_Net_Lib.Networking;
using RotMG_Net_Lib.Networking.Packets;
using RotMG_Net_Lib.Networking.Packets.Incoming;
using RotMG_Net_Lib.Networking.Packets.Outgoing;
using UnityEngine;
using GameObject = Networking.Objects.GameObject;

namespace Networking.Connection
{
    public class GameServerConnectionConcrete : MonoBehaviour
    {
        private NetClient _client;

        private Player _player;

        private Map _map;
        private int _playerId;
        private int _charId;

        private GameSprite _gs;

        public bool CreateCharacter { get; set; }

        public void Start()
        {
            _client = new NetClient();
            _gs = new GameSprite();
        }

        public void Connect()
        {
            MapMessages();
        }

        private void MapMessages()
        {
            _client.Hook(PacketType.FAILURE, OnFailure);
            _client.Hook(PacketType.CREATE_SUCCESS, OnCreateSuccess);
            // _client.Hook(PacketType.SERVERPLAYERSHOOT, OnServerPlayerShoot);
            // _client.Hook(PacketType.DAMAGE, OnDamage);
            _client.Hook(PacketType.UPDATE, OnUpdate);
            // _client.Hook(PacketType.NOTIFICATION, OnNotification);
            // _client.Hook(PacketType.GLOBAL_NOTIFICATION, OnGlobalNotification);
            _client.Hook(PacketType.NEWTICK, OnNewTick);
            // _client.Hook(PacketType.SHOWEFFECT, OnShowEffect);
            _client.Hook(PacketType.GOTO, OnGoto);
            // _client.Hook(PacketType.INVRESULT, OnInvResult);
            // _client.Hook(PacketType.RECONNECT, OnReconnect);
            _client.Hook(PacketType.PING, OnPing);
            _client.Hook(PacketType.MAPINFO, OnMapInfo);
            // _client.Hook(PacketType.PIC, OnPic);
            // _client.Hook(PacketType.DEATH, OnDeath);
            // _client.Hook(PacketType.BUYRESULT, OnBuyResult);
            _client.Hook(PacketType.AOE, OnAoe);
            // _client.Hook(PacketType.ACCOUNTLIST, OnAccountList);
            // _client.Hook(PacketType.QUESTOBJID, OnQuestObjId);
            // _client.Hook(PacketType.NAMERESULT, OnNameResult);
            // _client.Hook(PacketType.GUILDRESULT, OnGuildResult);
            // _client.Hook(PacketType.ALLYSHOOT, OnAllyShoot);
            // _client.Hook(PacketType.ENEMYSHOOT, OnEnemyShoot);
            // _client.Hook(PacketType.TRADEREQUESTED, OnTradeRequested);
            // _client.Hook(PacketType.TRADESTART, OnTradeStart);
            // _client.Hook(PacketType.TRADECHANGED, OnTradeChanged);
            // _client.Hook(PacketType.TRADEDONE, OnTradeDone);
            // _client.Hook(PacketType.TRADEACCEPTED, OnTradeAccepted);
            // _client.Hook(PacketType.CLIENTSTAT, OnClientStat);
            // _client.Hook(PacketType.FILE, OnFile);
            // _client.Hook(PacketType.INVITEDTOGUILD, OnInvitedToGuild);
            // _client.Hook(PacketType.PLAYSOUND, OnPlaySound);
            // _client.Hook(PacketType.ACTIVEPETUPDATE, OnActivePetUpdate);
            // _client.Hook(PacketType.NEW_ABILITY, OnNewAbility);
            // _client.Hook(PacketType.PETYARDUPDATE, OnPetYardUpdate);
            // _client.Hook(PacketType.EVOLVE_PET, OnEvolvedPet);
            // _client.Hook(PacketType.DELETE_PET, OnDeletePet);
            // _client.Hook(PacketType.HATCH_PET, OnHatchPet);
            // _client.Hook(PacketType.IMMINENT_ARENA_WAVE, OnImminentArenaWave);
            // _client.Hook(PacketType.ARENA_DEATH, OnArenaDeath);
            // _client.Hook(PacketType.VERIFY_EMAIL, OnVerifyEmail);
            // _client.Hook(PacketType.RESKIN_UNLOCK, OnReskinUnlock);
            // _client.Hook(PacketType.PASSWORD_PROMPT, OnPasswordPrompt);
            // _client.Hook(PacketType.QUEST_FETCH_RESPONSE, OnQuestFetchResponse);
            // _client.Hook(PacketType.QUEST_REDEEM_RESPONSE, OnQuestRedeemResponse);
            // _client.Hook(PacketType.KEY_INFO_RESPONSE, OnKeyInfoResponse);
            // _client.Hook(PacketType.LOGIN_REWARD_MSG, OnLoginRewardResponse);
            // _client.Hook(PacketType.REALM_HERO_LEFT_MSG, OnRealmHeroesResponse);
        }


        #region Packet Handling

        private void OnAoe(IncomingPacket p)
        {
            AoePacket aoe = new AoePacket();

            _client.SendPacket(new AoeAckPacket
            {
                Position = aoe.Pos,
                Time = _client.GetTimer()
            });
        }

        private void OnGoto(IncomingPacket p)
        {
            _client.SendPacket(new GotoAckPacket()
            {
                Time = _client.GetTimer()
            });
        }

        private void OnPing(IncomingPacket packet)
        {
            PingPacket pingPacket = (PingPacket) packet;

            _client.SendPacket(new PongPacket()
            {
                Serial = pingPacket.Serial, Time = _client.GetTimer()
            });
        }

        private void OnFailure(IncomingPacket p)
        {
            FailurePacket failure = (FailurePacket) p;

            Debug.Log("Failure: " + ((FailurePacket) p).ErrorDescription + ".");

            /* Possible errors : 
             * "Email Verification needed"
             * "Protocol error"
             * "Account in use"
             * "Account is already in use"
             * and more...
             */
        }

        private void OnNewTick(IncomingPacket p)
        {
            NewTickPacket newTick = (NewTickPacket) p;

            Move(newTick.TickId, _player);

            foreach (var statusData in newTick.Statuses)
            {
                ProcessObjectStatus(statusData, newTick.TickTime, newTick.TickId);
            }
        }

        private void Move(int newTickTickId, Player player)
        {
            _client.SendPacket(new MovePacket()
            {
                TickId = newTickTickId,
                Time = _client.GetTimer(),
                NewPosition = _player.Position,
                Records = new List<MoveRecord>()
            });
        }

        /**
         * This method handles the update packet :
         *     Add ground tiles in the game world
         *     Add objects
         *     Remove objects
         *
         * These objects are then updated by the NewTick packet.
         */
        private void OnUpdate(IncomingPacket p)
        {
            _client.SendPacket(new UpdateAckPacket());

            UpdatePacket updatePacket = (UpdatePacket) p;

            foreach (GroundTileData groundTile in updatePacket.Tiles)
            {
                Map.SetGroundTile(groundTile.X, groundTile.Y, groundTile.Type);
            }

            foreach (ObjectData newObject in updatePacket.NewObjects)
            {
                AddObject(newObject);
            }

            foreach (int drop in updatePacket.Drops)
            {
                Map.RemoveObject(drop);
            }
        }

        private void AddObject(ObjectData newObject)
        {
            GameObject o = ObjectLibrary.GetObjectFromType(newObject.ObjectType);

            ObjectStatusData objectStatusData = newObject.Status;

            o.SetObjectId(objectStatusData.ObjectId);

            if (o is Player player)
            {
                HandleNewPlayer(player);
            }

            ProcessObjectStatus(objectStatusData, 0, -1);
        }

        private void HandleNewPlayer(Player player)
        {
            if (player.ObjectId == _playerId)
            {
                this._player = player;
                // TODO set camera focus to player
            }
        }

        private void OnCreateSuccess(IncomingPacket packet)
        {
            Debug.Log("Has successfully connected!");

            CreateSuccessPacket createSuccessPacket = (CreateSuccessPacket) packet;

            _playerId = createSuccessPacket.ObjectId;
            _charId = createSuccessPacket.CharId;
            CreateCharacter = false;
        }


        private void OnMapInfo(IncomingPacket p)
        {
            _gs.ApplyMapInfo((MapInfoPacket) p);

            if (CreateCharacter)
            {
                Create();
            }

            else
            {
                Load();
            }
        }

        #endregion


        private void Create()
        {
            Debug.Log("Creating new character...");
            CreatePacket createPacket = new CreatePacket()
            {
                ClassType = Class.Wizard.Id, SkinType = 0
            };

            _client.SendPacket(createPacket);
        }


        private void Load()
        {
            Debug.Log("Loading existing character...");

            LoadPacket load = new LoadPacket()
            {
                CharId = _charId, IsFromArena = false
            };
            _client.SendPacket(load);
        }


        #region Projectiles


        public void PlayerShoot(int time, Projectile proj)
        {
            
        }
        

        #endregion
        
        private void ProcessObjectStatus(ObjectStatusData statusData, int newTickTickTime, int newTickTickId)
        {
        }
    }


}