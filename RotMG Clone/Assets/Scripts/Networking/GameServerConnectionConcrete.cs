using System.Collections.Generic;
using NLog.Fluent;
using RotMG_Net_Lib;
using RotMG_Net_Lib.Constants;
using RotMG_Net_Lib.Models;
using RotMG_Net_Lib.Networking;
using RotMG_Net_Lib.Networking.Packets;
using RotMG_Net_Lib.Networking.Packets.Incoming;
using RotMG_Net_Lib.Networking.Packets.Outgoing;
using UnityEngine;

namespace Networking
{
    public class GameServerConnectionConcrete : MonoBehaviour
    {
        private NetClient _client;

        public void Start()
        {
            _client = new NetClient();
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

        private void OnAoe(IncomingPacket p)
        {
            AoePacket aoe = new AoePacket();
            _client.SendPacket(new AoeAckPacket {Position = aoe.Pos, Time = _client.GetTimer()});
        }

        private void OnGoto(IncomingPacket p)
        {
            _client.SendPacket(new GotoAckPacket() {Time = GetTimer()});
        }

        private void OnPing(IncomingPacket packet)
        {
            PingPacket pingPacket = (PingPacket) packet;
            _client.SendPacket(new PongPacket() {Serial = pingPacket.Serial, Time = GetTimer()});
        }

        private void OnFailure(IncomingPacket p)
        {
            Log.Info("Failure: " + ((FailurePacket) p).ErrorDescription + " with bot " + Account.Email + ".");

            FailurePacket failure = (FailurePacket) p;

            if (failure.ErrorDescription.Contains("Email Verification needed"))
            {
                _client.Disconnect(DisconnectReason.EmailVerificationNeeded.SetDetails(failure.ErrorDescription));
            }
            else if (failure.ErrorDescription.Contains("Protocol error"))
            {
                _client.Disconnect(DisconnectReason.ProtocolError.SetDetails(failure.ErrorDescription));
            }
            else if (failure.ErrorDescription.Contains("Account in use") || failure.ErrorDescription.Contains("Account is already in use"))
            {
                _client.Disconnect(DisconnectReason.AccountInUse.SetDetails(failure.ErrorDescription));
            }
            else
            {
                Log.Error("UNHANDLED FAILURE : " + failure.ErrorDescription);
            }
        }

        private void OnNewTick(IncomingPacket p)
        {
            NewTickPacket newTickPacket = (NewTickPacket) p;
            foreach (var objectStatusData in newTickPacket.Statuses)
            {
                if (objectStatusData.ObjectId == Player.ObjectId)
                {
                    Player.Position = objectStatusData.Pos;
                }
            }

            _client.SendPacket(new MovePacket() {TickId = newTickPacket.TickId, Time = GetTimer(), NewPosition = Player.Position, Records = new List<MoveRecord>()});
        }

        private void OnUpdate(IncomingPacket p)
        {
            SendPacket(new UpdateAckPacket());

            UpdatePacket updatePacket = (UpdatePacket) p;
            foreach (ObjectData updatePacketNewObject in updatePacket.NewObjects)
            {
                if (updatePacketNewObject.Status.ObjectId == Player.ObjectId)
                {
                    Player.Position = updatePacketNewObject.Status.Pos;
                    UpdateMyPlayerGameObject(updatePacketNewObject.Status);
                }
            }
        }

        private void OnCreateSuccess(IncomingPacket packet)
        {
            Log.Info(Account.Email + " Has successfully connected!");

            CreateSuccessPacket createSuccessPacket = (CreateSuccessPacket) packet;
            Player = new Player {ObjectId = createSuccessPacket.ObjectId};
        }

        private void OnMapInfo(IncomingPacket p)
        {
            if (CreateNewCharacter)
            {
                Log.Debug("Creating new character...");

                CreatePacket createPacket = new CreatePacket() {ClassType = Class.Wizard.Id, SkinType = 0};

                SendPacket(createPacket);
            }

            else
            {
                Log.Debug("Loading existing character...");

                LoadPacket load = new LoadPacket() {CharId = CharacterId, IsFromArena = false};
                SendPacket(load);
            }
        }
    }
}