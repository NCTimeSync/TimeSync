﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using TimeSync.Model;

namespace TimeSync.DataAccess
{
    public static class SharepointClient
    {
        private static ClientContext _clientContext;

        public static int GetUserIdFromToolkit(ToolkitUser toolkitUser, Toolkit toolkit)
        {
            ClientContext clientContext = new ClientContext(toolkit.Url);
            UserCollection userCollection = clientContext.Web.SiteUsers;
            clientContext.Load(userCollection);
            clientContext.ExecuteQuery();


            var email = $"{toolkitUser.Name}@netcompany.com";
            User user = userCollection.GetByEmail(email);
            clientContext.Load(user);
            clientContext.ExecuteQuery();

            return user.Id;
        }

        public static int MakeTimeRegistration(Timeregistration timereg, ToolkitUser toolkitUser, ToolkitInfo toolkitInfo)
        {
            Toolkit toolkit;
            var success = toolkitInfo.Toolkits.TryGetValue(timereg.Customer, out toolkit);
            if (!success)
                throw new Exception("Customer not found");

            _clientContext = new ClientContext(toolkit.Url);
            _clientContext.Credentials = new NetworkCredential(toolkitUser.Name, toolkitUser.Password, toolkitUser.Domain);

            var timeregList = "tidsregistrering";
            var sharepointList = _clientContext.Web.Lists.GetByTitle(timeregList);
            _clientContext.Load(sharepointList);
            _clientContext.ExecuteQuery();

            var doneBy = new SPFieldLookupValue(toolkit.UserId, toolkitUser.Name);
            var author = new SPFieldLookupValue(toolkit.UserId, toolkitUser.Name);
            var toolkitCase = new SPFieldLookupValue(timereg.CaseId, $"{timereg.Customer}-{timereg.CaseId}");

            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
            ListItem sharepointListItem = sharepointList.AddItem(itemCreateInfo);

            sharepointListItem["Hours"] = timereg.Hours;
            sharepointListItem["DoneBy"] = doneBy;
            sharepointListItem["Author"] = author;
            sharepointListItem["Case"] = toolkitCase;
            sharepointListItem["DoneDate"] = timereg.DoneDate;

            sharepointListItem.Update();
            _clientContext.ExecuteQuery();

            return sharepointListItem.Id;
        }

        public static void MakeTimeregistrations(List<Timeregistration> timeregs)
        {
            //Do some loop over list where we create Microsoft.SharePoint.Client.ListItem and put into SP.List oList -- SEE UNIT TEST PROJECT
            //Send to Toolkit -- SEE UNIT TEST PROJECT
            throw new NotImplementedException();
        }
    }
}
