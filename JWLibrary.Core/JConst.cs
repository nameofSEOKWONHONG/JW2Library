using System.Threading;

namespace JWLibrary.Core {
    internal class JConst {
        public const int SLEEP_INTERVAL = 1;
        public const int LOOP_WARNING_COUNT = 5000;
        public const int LOOP_LIMIT = 500;

        public static void setInterval(int interval) {
            Thread.Sleep(interval);
        }
    }
}