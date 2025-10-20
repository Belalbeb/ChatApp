using ChatApi.Dto;
using ChatApi.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Writers;
using Microsoft.VisualBasic;

namespace ChatApi.Hubs
{
    public class ChatHub :Hub
    {
        private readonly ChatServices chatServices;

        public ChatHub(ChatServices chatServices)
        {
            this.chatServices = chatServices;
        }
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId,"Come2chat");
            await Clients.Caller.SendAsync("UserConnected");

        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Come2chat");
            var user = chatServices.GetUserByConnectionId(Context.ConnectionId);
            
            chatServices.RemoveUser(user);
            await DisplayOnlineUser();
            await base.OnDisconnectedAsync(exception);
        }
        public async Task AddUserConnectionId(string Name)
        {
            chatServices.AddUserConnectionId(Name, Context.ConnectionId);
            await DisplayOnlineUser();
        }
        public async Task DisplayOnlineUser()
        {
            var OnlineUser = chatServices.GetOnLineUsers();
            await Clients.Groups("Come2chat").SendAsync("OnlineUsres", OnlineUser);

        }
        public async Task ReceivesMessages(MessageDto messageDto)
        {
           
                await Clients.Groups("Come2chat").SendAsync("ReceivedMessages", messageDto);


        }
        public async Task CreatePrivateChat(MessageDto messageDto)
        {
            string privateGroupName = GetPrivateGroubName(messageDto.From, messageDto.To);
            await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnId = chatServices.GetUserByKey(messageDto.To);
            await Groups.AddToGroupAsync(toConnId, privateGroupName);
            await Clients.Client(toConnId).SendAsync("PrivateChat", messageDto);

        }
        public async Task RecivePrivateMessage(MessageDto messageDto)
        {
            string privateGroupName = GetPrivateGroubName(messageDto.From, messageDto.To);
            await Clients.Group(privateGroupName).SendAsync("NewPrivateChat", messageDto);


        }
        public async Task RemovePrivateChat(string From,string To)
        {
            string privateGroupName = GetPrivateGroubName(From,To);
            await Clients.Group(privateGroupName).SendAsync("ClosePrivateChat");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnId = chatServices.GetUserByKey(To);
            await Groups.RemoveFromGroupAsync(toConnId, privateGroupName);

        }
        private String GetPrivateGroubName(string from,string to)
        {
            var stringCompare = string.CompareOrdinal(from, to) < 0;
            return stringCompare ? $"{from}-{to}" : $"{to}-{from}";

        }
        //public async Task SendVoice(string base64Audio, string sender)
        //{
           
        //    await Clients.Others.SendAsync("ReceiveVoice", base64Audio, sender);
        //}

    }
}
