namespace JWLibrary.Core {
    public static class JCastExtension {
        public static TDest jCast<TDest>(this object src)
            where TDest : class {
            return src as TDest;
        }

        public static TDest jCast<TSrc, TDest>(this TSrc src)
            where TSrc : class
            where TDest : class {
            return src as TDest;
        }
    }
}