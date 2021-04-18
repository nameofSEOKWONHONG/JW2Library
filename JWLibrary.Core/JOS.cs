using System.Runtime.InteropServices;

namespace JWLibrary.Core {
    public class JOS {
        public static bool jIsWindows() {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public static bool jIsMac() {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }

        public static bool jIsLinux() {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }
    }
}