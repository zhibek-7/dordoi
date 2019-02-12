using Microsoft.AspNetCore.SignalR;

namespace Localization.Hubs
{
    public class DataImportHub : Hub
    {

        /// <summary>
        /// Clients listen to event with this name.
        /// </summary>
        public static readonly string LogUpdatedEventName = "LogUpdated";

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

    }
}
