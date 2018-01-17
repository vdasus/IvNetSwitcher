using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;
using SimpleWifi;

namespace IvNetSwitcher.Core.DomainServices
{
    public class WiFiService: INetService
    {
        private readonly Wifi _wifi;

        public WiFiService()
        {
            _wifi = new Wifi();
            _wifi.ConnectionStatusChanged += WifiConnectionStatusChanged;

            // TODO don't forget to uncomment
            //if (_wifi.NoWifiAvailable) throw new ApplicationException("WiFi not available");
        }

        #region Implementation of IServices

        public IReadOnlyList<Network> ListAvailableNetworks()
        {
            var lst = ListAccessPoints();

            return lst.Select((zn, index) =>
                new Network(index, zn.Name, zn.SignalStrength, zn.HasProfile, zn.IsSecure, zn.IsConnected)).ToList();
        }

        public Result Connect(int index, string username, string password, string domain)
        {
            var accessPoints = ListAccessPoints();

            if (index > accessPoints.ToArray().Length || accessPoints.ToArray().Length == 0)
            {
                return Result.Fail("Index out of bounds");
            }

            AccessPoint selectedAp = accessPoints.ToList()[index];

            AuthRequest authRequest = new AuthRequest(selectedAp);
            bool overwrite = true;

            if (authRequest.IsPasswordRequired)
            {
                if (selectedAp.HasProfile)
                    overwrite = false;
                    

                if (overwrite)
                {
                    if (authRequest.IsUsernameRequired)
                    {
                        authRequest.Username = username;
                    }

                    bool validPassFormat = selectedAp.IsValidPassword(password);
                    if (!validPassFormat)
                        return Result.Fail("Password is not valid for this network type.");
                        
                    authRequest.Password = password;

                    if (!string.IsNullOrWhiteSpace(domain) && authRequest.IsDomainSupported)
                    {
                        authRequest.Domain = domain;
                    }
                }
            }

            selectedAp.ConnectAsync(authRequest, overwrite, OnConnectedComplete);
            return Result.Ok();
        }

        public Result CheckIsConnected()
        {
            return _wifi.ConnectionStatus == WifiStatus.Connected ? Result.Ok() : Result.Fail("Not connected");
        }

        public void Disconnect()
        {
            _wifi.Disconnect();
        }

        public string Status()
        {
            return _wifi.ConnectionStatus == WifiStatus.Connected
                ? "Connected"
                : "Not connected";
        }

        public Result<string> PrintProfileXml(int index)
        {
            var accessPoints = ListAccessPoints();
            if (index > accessPoints.ToArray().Length || accessPoints.ToArray().Length == 0)
            {
                return Result.Fail<string>($"Index {index} out of bounds");
            }
            AccessPoint selectedAp = accessPoints.ToList()[index];

            return Result.Ok(selectedAp.GetProfileXML());
        }

        public Result<string> ShowAccessPointInfo(int index)
        {
            var accessPoints = ListAccessPoints();
            
            if (index > accessPoints.ToArray().Length || accessPoints.ToArray().Length == 0)
            {
                return Result.Fail<string>("Index out of bounds");
            }
            AccessPoint selectedAp = accessPoints.ToList()[index];
            return Result.Ok(selectedAp.ToString());
        }

        public Result<string> DeleteProfile(int index)
        {
            var accessPoints = ListAccessPoints();
            
            if (index > accessPoints.ToArray().Length || accessPoints.ToArray().Length == 0)
            {
                return Result.Fail<string>("Index out of bounds");
            }

            AccessPoint selectedAp = accessPoints.ToList()[index];

            selectedAp.DeleteProfile();
            return Result.Ok(selectedAp.Name);
        }

        #endregion

        #region Privates
        private List<AccessPoint> ListAccessPoints()
        {
            return _wifi.GetAccessPoints().OrderByDescending(ap => ap.SignalStrength).ToList();
        }
        #endregion

        #region Temporary part
        // TODO Move outside

        private static void WifiConnectionStatusChanged(object sender, WifiStatusEventArgs e)
        {
            //Console.WriteLine($"New status: {e.NewStatus.ToString()}");
        }

        private static void OnConnectedComplete(bool success)
        {
            //Console.WriteLine($"OnConnectedComplete, success: {success}");
        }

        #endregion
    }
}
