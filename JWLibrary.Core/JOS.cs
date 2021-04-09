using System.Runtime.InteropServices;

namespace JWLibrary.Core {
    public class JOS {
        public static bool isWindows() {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public static bool isMac() {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }

        public static bool isLinux() {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }
    }
}