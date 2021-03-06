﻿using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using eXtensionSharp;
using JWLibrary.Util.CLI;

namespace JWLibrary.Utils
{
    /// <summary>
    ///     firewall port type
    /// </summary>
    public class ENUM_PORT_TYPE : XEnumBase<ENUM_PORT_TYPE>
    {
        public static readonly ENUM_PORT_TYPE TCP = Define("TCP");
        public static readonly ENUM_PORT_TYPE UDP = Define("UDP");
    }

    /// <summary>
    ///     firewall bound type
    /// </summary>
    public class ENUM_BOUND_TYPE : XEnumBase<ENUM_BOUND_TYPE>
    {
        public static readonly ENUM_BOUND_TYPE IN = Define("in");
        public static readonly ENUM_BOUND_TYPE OUT = Define("out");
    }

    public static class JNetworkHelper
    {
        public static readonly int[] DYNAMIC_UDP_TCP_PORT_RANGE = Enumerable.Range(49152, 65535).ToArray();

        public static bool IsTcpPortAvailable(this int tcpPort)
        {
            var ipgp = IPGlobalProperties.GetIPGlobalProperties();

            // Check ActiveConnection ports
            var conns = ipgp.GetActiveTcpConnections();
            foreach (var cn in conns)
                if (cn.LocalEndPoint.Port == tcpPort)
                    return false;

            // Check LISTENING ports
            var endpoints = ipgp.GetActiveTcpListeners();
            foreach (var ep in endpoints)
                if (ep.Port == tcpPort)
                    return false;

            return true;
        }

        public static void OpenPort(this int port, string name, ENUM_PORT_TYPE portType,
            ENUM_BOUND_TYPE boundType)
        {
            var arg = OpenPortCommand(name, boundType.ToString(), portType.ToString(), port);
            ProcessHandlerAsync.RunAsync("cmd.exe", arg,
                    output => { Debug.WriteLine(output); },
                    error => { Debug.WriteLine(error); })
                .GetAwaiter()
                .OnCompleted(() => { Debug.WriteLine("done"); });
        }

        public static void ClosePort(this int port, string name, ENUM_PORT_TYPE portType,
            ENUM_BOUND_TYPE boundType)
        {
            var arg = ClosePortCommand(name, boundType.ToString(), portType.ToString(), port);
            ProcessHandlerAsync.RunAsync("cmd.exe", arg,
                    output => { Debug.WriteLine(output); },
                    error => { Debug.WriteLine(error); })
                .GetAwaiter()
                .OnCompleted(() => { Debug.WriteLine("done"); });
        }

        public static void EnableFirewall()
        {
            var arg = EnableFirewallCommand();
            ProcessHandlerAsync.RunAsync("cmd.exe", arg,
                    output => { Debug.WriteLine(output); },
                    error => { Debug.WriteLine(error); })
                .GetAwaiter()
                .OnCompleted(() => { Debug.WriteLine("done"); });
        }

        public static void DisableFirewall()
        {
            var arg = DisableFirewallCommand();
            ProcessHandlerAsync.RunAsync("cmd.exe", arg,
                    output => { Debug.WriteLine(output); },
                    error => { Debug.WriteLine(error); })
                .GetAwaiter()
                .OnCompleted(() => { Debug.WriteLine("done"); });
        }

        /// <summary>
        ///     open port command
        /// </summary>
        /// <param name="boundType">in/out</param>
        /// <param name="portType">TCP/UDP</param>
        /// <param name="port"></param>
        /// <returns></returns>
        private static string OpenPortCommand(string name, string boundType, string portType, int port)
        {
            return
                $"/C netsh advfirewall firewall add rule name=\"{name} PORT OPEN {port}\" dir={boundType} action=allow protocol={portType} localport={port}";
        }

        private static string ClosePortCommand(string name, string boundType, string portType, int port)
        {
            return
                $"/C netsh advfirewall firewall delete rule name=\"{name} PORT OPEN {port}\" dir={boundType} protocol={portType} localport={port}";
        }

        private static string EnableFirewallCommand()
        {
            return "/C netsh advfirewall set allprofiles state on";
        }

        private static string DisableFirewallCommand()
        {
            return "/C netsh advfirewall set allprofiles state off";
        }
    }
}