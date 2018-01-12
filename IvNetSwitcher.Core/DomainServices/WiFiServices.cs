using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;
using SimpleWifi;

namespace IvNetSwitcher.Core.DomainServices
{
    public class WiFiServices: IServices
    {
        private readonly Wifi _wifi;

        public WiFiServices()
        {
            _wifi = new Wifi();
            _wifi.ConnectionStatusChanged += WifiConnectionStatusChanged;

            if (_wifi.NoWifiAvailable) throw new ApplicationException("WiFi not available");
        }

        #region Implementation of IServices

        public List<Network> ListAvailableNetworks()
        {
            var result = new List<Network>();

            var lst = ListAccessPoints();
            foreach (var zn in lst)
            {
                result.Add(new Network(zn.Name, zn.SignalStrength, zn.HasProfile, zn.IsSecure, zn.IsConnected));
            }

            return result;
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

        public void Disconnect()
        {
            _wifi.Disconnect();
        }

        public Result Status()
        {
            return Result.Ok(_wifi.ConnectionStatus == WifiStatus.Connected
                ? "Connected"
                : "Not connected");
        }

        public Result PrintProfileXml(int index)
        {
            var accessPoints = ListAccessPoints();
            if (index > accessPoints.ToArray().Length || accessPoints.ToArray().Length == 0)
            {
                return Result.Fail<string>($"Index {index} out of bounds");
            }
            AccessPoint selectedAp = accessPoints.ToList()[index];

            return Result.Ok(selectedAp.GetProfileXML());
        }

        public Result ShowAccessPointInfo(int index)
        {
            var accessPoints = ListAccessPoints();
            
            if (index > accessPoints.ToArray().Length || accessPoints.ToArray().Length == 0)
            {
                return Result.Fail<string>("Index out of bounds");
            }
            AccessPoint selectedAp = accessPoints.ToList()[index];

            return Result.Fail<string>(selectedAp.ToString());
        }

        public Result DeleteProfile(int index)
        {
            var accessPoints = ListAccessPoints();
            
            if (index > accessPoints.ToArray().Length || accessPoints.ToArray().Length == 0)
            {
                return Result.Fail("Index out of bounds");
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
