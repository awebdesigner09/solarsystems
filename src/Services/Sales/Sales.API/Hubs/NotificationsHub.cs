using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace Sales.API.Hubs
{
    // Add authorization if only authenticated users can connect
    // [Authorize]
    public class NotificationsHub : Hub
    {
        // This method will be called by the client after connecting
        public async Task JoinCustomerGroup(string customerId)
        {
            // In a real app, you'd validate that the authenticated user
            // is allowed to listen for this customerId.
            await Groups.AddToGroupAsync(Context.ConnectionId, GetCustomerGroupName(customerId));
        }

        public async Task LeaveCustomerGroup(string customerId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetCustomerGroupName(customerId));
        }

        public static string GetCustomerGroupName(string customerId) => $"customer-{customerId}";
    }
}
