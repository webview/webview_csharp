using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace SharpWebview
{
    /// <summary>
    /// This class includes all methods necessary to check for the loopback exception on windows.
    /// </summary>
    public class Loopback
    {
        private const string WebViewAppContainerName = "microsoft.win32webviewhost_cw5n1h2txyewy";

        /// <summary>
        /// This method checks, if the loopback exception is present for the webview on windows.
        /// </summary>
        /// <returns>True, if the exception is present.</returns>
        public bool IsWebViewLoopbackEnabled()
        {
            var webViewSids = GetWebViewAppContainerSids();
            return GetAllAppContainerConfigs().Any(c =>
            {
                ConvertSidToStringSid(c.Sid, out var currentSid);
                return webViewSids.Any(webViewSid => currentSid == webViewSid);
            });
        }

        private IEnumerable<string> GetWebViewAppContainerSids()
        {
            return GetAllAppContainers()
                .Where(a => a.appContainerName.ToLower() == WebViewAppContainerName)
                .Select(a => {
                    ConvertSidToStringSid(a.appContainerSid, out var webViewSid);
                    return webViewSid;
                });
        }

        private List<FirewallAppContainer> GetAllAppContainers()
        {
            var size = 0u;
            var arrayValue = IntPtr.Zero;
            var structSize = Marshal.SizeOf<FirewallAppContainer>();

            var handle_pdwCntPublicACs = GCHandle.Alloc(size, GCHandleType.Pinned);
            var handle_ppACs = GCHandle.Alloc(arrayValue, GCHandleType.Pinned);

            NetworkIsolationEnumAppContainers((int)NETISO_FLAG.NETISO_FLAG_MAX, out size, out arrayValue);

            var firewallApps = new List<FirewallAppContainer>();
            for (var i = 0; i < size; i++)
            {
                var cur = (FirewallAppContainer)Marshal.PtrToStructure<FirewallAppContainer>(arrayValue);
                firewallApps.Add(cur);
                arrayValue = new IntPtr((long)arrayValue + structSize);
            }

            handle_pdwCntPublicACs.Free();
            handle_ppACs.Free();

            return firewallApps;
        }

        private List<SidAndAttributes> GetAllAppContainerConfigs()
        {
            var size = 0u;
            var arrayValue = IntPtr.Zero;
            var structSize = Marshal.SizeOf<SidAndAttributes>();

            var handle_pdwCntPublicACs = GCHandle.Alloc(size, GCHandleType.Pinned);
            var handle_ppACs = GCHandle.Alloc(arrayValue, GCHandleType.Pinned);

            NetworkIsolationGetAppContainerConfig(out size, out arrayValue);

            var firewallAppConfigs = new List<SidAndAttributes>();
            for (var i = 0; i < size; i++)
            {
                var currentConfig = (SidAndAttributes)Marshal.PtrToStructure<SidAndAttributes>(arrayValue);
                firewallAppConfigs.Add(currentConfig);
                arrayValue = new IntPtr((long)arrayValue + structSize);
            }

            handle_pdwCntPublicACs.Free();
            handle_ppACs.Free();

            return firewallAppConfigs;
        }

        #region DllImports
        internal enum NETISO_FLAG
        {
            NETISO_FLAG_FORCE_COMPUTE_BINARIES  =  0x1,
            NETISO_FLAG_MAX                     =  0x2
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct FirewallAppContainer
        {
            internal IntPtr appContainerSid;
            internal IntPtr userSid;
            [MarshalAs(UnmanagedType.LPWStr)] public string appContainerName;
            [MarshalAs(UnmanagedType.LPWStr)] public string displayName;
            [MarshalAs(UnmanagedType.LPWStr)] public string description;
            internal FirewallAcCapabilities capabilities;
            internal FirewallAcBinaries binaries;
            [MarshalAs(UnmanagedType.LPWStr)] public string workingDirectory;
            [MarshalAs(UnmanagedType.LPWStr)] public string packageFullName;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct FirewallAcCapabilities
        {
            public uint count;
            public IntPtr capabilities;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct FirewallAcBinaries
        {
            public uint count;
            public IntPtr binaries;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SidAndAttributes
        {
            public IntPtr Sid;
            public uint Attributes;
        }

        [DllImport("FirewallAPI.dll")]
        internal static extern uint NetworkIsolationEnumAppContainers(uint Flags, out uint pdwCntPublicACs, out IntPtr ppACs);

        [DllImport("FirewallAPI.dll")]
        internal static extern uint NetworkIsolationGetAppContainerConfig(out uint pdwCntACs, out IntPtr appContainerSids);

        [DllImport("advapi32", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool ConvertSidToStringSid(IntPtr pSid, out string strSid);
        #endregion
    }
}
