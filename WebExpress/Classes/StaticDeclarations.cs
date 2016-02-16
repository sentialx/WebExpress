using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebExpress
{
    class StaticDeclarations
    {
        public static string Bookmarkslayoutpath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\bookmarks-layout.html");

        public static string Bookmarkspath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\bookmarks-data.html");

        public static string Bookspath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WebExpress\\user data\\bookmarks.txt");

        public static string Historylayoutpath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\history-layout.html");

        public static string Historypath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\history-data.html");

        public static string Suggestionspath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WebExpress\\user data\\suggestions.txt");
    }
}
