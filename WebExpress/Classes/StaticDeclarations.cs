using System;
using System.IO;

namespace WebExpress
{
    class StaticDeclarations
    {
        public static string Bookmarkspath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\bookmarks.json");

        public static string Historypath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\history.json");

        public static bool IsIncognito;
        public static bool IsFullscreen;

    }
}
