using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Localization.Hubs
{
    public static class IClientProxyExtensions
    {

        public static async Task Log(this IClientProxy clientProxy, string message)
        {
            await clientProxy.SendAsync(DataImportHub.LogUpdatedEventName, new LogEntry()
            {
                Date = DateTime.Now,
                Message = message
            });
        }

    }
}
