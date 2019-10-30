using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DotNetNuke.Services.FileSystem
{
    public static class MimeTypeParser
    {
        [DllImport("urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
        static extern int FindMimeFromData(IntPtr pBC,
           [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
           [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I1, SizeParamIndex=3)]
            byte[] pBuffer,
           int cbSize,
           [MarshalAs(UnmanagedType.LPWStr)] string pwzMimeProposed,
           int dwMimeFlags,
           out IntPtr ppwzMimeOut,
           int dwReserved);


        public static List<string> GetMimeTypeFromList(string sFileNameOrPath)
        {
            List<string> sMimeTypes = null;
            string sExtensionWithoutDot = Path.GetExtension(sFileNameOrPath).Substring(1).ToLower();

            if (!String.IsNullOrEmpty(sExtensionWithoutDot) && spDicMIMETypes.ContainsKey(sExtensionWithoutDot))
            {
                sMimeTypes = spDicMIMETypes[sExtensionWithoutDot];
            }

            return sMimeTypes;
        }

        public static string GetMimeTypeFromRegistry(string sFileNameOrPath)
        {
            string sMimeType = null;
            string sExtension = Path.GetExtension(sFileNameOrPath).ToLower();
            RegistryKey pKey = Registry.ClassesRoot.OpenSubKey(sExtension);

            if (pKey != null && pKey.GetValue("Content Type") != null)
            {
                sMimeType = pKey.GetValue("Content Type").ToString();
            }

            return sMimeType;
        }

        public static string GetMimeTypeFromFile(string file, Stream fileContent)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = fileContent.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                buffer = ms.ToArray();
            }

            IntPtr mimeout;

            int MaxContent = (int)buffer.Length;
            if (MaxContent > 4096) MaxContent = 4096;

            int result = FindMimeFromData(IntPtr.Zero, file, buffer, MaxContent, null, 0, out mimeout, 0);

            if (result != 0)
                throw Marshal.GetExceptionForHR(result);
            string mime = Marshal.PtrToStringUni(mimeout);
            Marshal.FreeCoTaskMem(mimeout);


            return mime;
        }

        private static readonly Dictionary<string, List<string>> spDicMIMETypes = new Dictionary<string, List<string>>
        {
            {"ai", new List<string>{"application/postscript" } },
            {"aif", new List<string>{"audio/x-aiff"}},
            {"aifc", new List<string>{"audio/x-aiff"}},
            {"aiff", new List<string>{"audio/x-aiff"}},
            {"asc", new List<string>{"text/plain"}},
            {"atom", new List<string>{"application/atom+xml"}},
            {"au", new List<string>{"audio/basic"}},
            {"avi", new List<string>{"video/x-msvideo"}},
            {"bcpio", new List<string>{"application/x-bcpio"}},
            {"bin", new List<string>{"application/octet-stream"}},
            {"bmp", new List<string>{"image/bmp"}},
            {"cdf", new List<string>{"application/x-netcdf"}},
            {"cgm", new List<string>{"image/cgm"}},
            {"class", new List<string>{"application/octet-stream"}},
            {"cpio", new List<string>{"application/x-cpio"}},
            {"cpt", new List<string>{"application/mac-compactpro"}},
            {"csh", new List<string>{"application/x-csh"}},
            {"css", new List<string>{"text/css"}},
            {"dcr", new List<string>{"application/x-director"}},
            {"dif", new List<string>{"video/x-dv"}},
            {"dir", new List<string>{"application/x-director"}},
            {"djv", new List<string>{"image/vnd.djvu"}},
            {"djvu", new List<string>{"image/vnd.djvu"}},
            {"dll", new List<string>{"application/octet-stream"}},
            {"dmg", new List<string>{"application/octet-stream"}},
            {"dms", new List<string>{"application/octet-stream"}},
            {"doc", new List<string>{"application/msword"}},
            {"docx", new List<string>{"application/vnd.openxmlformats-officedocument.wordprocessingml.document"}},
            {"dotx", new List<string>{"application/vnd.openxmlformats-officedocument.wordprocessingml.template"}},
            {"docm", new List<string>{"application/vnd.ms-word.document.macroEnabled.12"}},
            {"dotm", new List<string>{"application/vnd.ms-word.template.macroEnabled.12"}},
            {"dtd", new List<string>{"application/xml-dtd"}},
            {"dv", new List<string>{"video/x-dv"}},
            {"dvi", new List<string>{"application/x-dvi"}},
            {"dxr", new List<string>{"application/x-director"}},
            {"eps", new List<string>{"application/postscript"}},
            {"eot", new List<string>{"application/octet-stream"}},
            {"etx", new List<string>{"text/x-setext"}},
            {"exe", new List<string>{"application/x-msdownload"}},
            {"ez", new List<string>{"application/andrew-inset"}},
            {"gif", new List<string>{"image/gif"}},

            {"gram", new List<string>{"application/srgs"}},
            {"grxml", new List<string>{"application/srgs+xml"}},
            {"gtar", new List<string>{"application/x-gtar"}},
            {"hdf", new List<string>{"application/x-hdf"}},
            {"hqx", new List<string>{"application/mac-binhex40"}},
            {"htc", new List<string>{"text/x-component"}},
            {"htm", new List<string>{"text/html"}},
            {"html", new List<string>{"text/html"}},
            {"htmltemplate", new List<string>{"plain/text"}},
            {"ice", new List<string>{"x-conference/x-cooltalk"}},
            {"ico", new List<string>{"image/x-icon"}},
            {"ics", new List<string>{"text/calendar"}},
            {"ief", new List<string>{"image/ief"}},
            {"ifb", new List<string>{"text/calendar"}},
            {"iges", new List<string>{"model/iges"}},
            {"igs", new List<string>{"model/iges"}},
            {"jnlp", new List<string>{"application/x-java-jnlp-file"}},
            {"jp2", new List<string>{"image/jp2"}},
            {"jpe", new List<string>{"image/pjpeg","image/jpeg"}},
            {"jpeg", new List<string>{"image/pjpeg","image/jpeg","image/jpg"}},
            {"jpg", new List<string>{"image/pjpeg","image/jpeg"}},
            {"js", new List<string>{"application/x-javascript"}},
            {"kar", new List<string>{"audio/midi"}},
            {"latex", new List<string>{"application/x-latex"}},
            {"lha", new List<string>{"application/octet-stream"}},
            {"lzh", new List<string>{"application/octet-stream"}},
            {"m3u", new List<string>{"audio/x-mpegurl"}},
            {"m4a", new List<string>{"audio/mp4a-latm"}},
            {"m4b", new List<string>{"audio/mp4a-latm"}},
            {"m4p", new List<string>{"audio/mp4a-latm"}},
            {"m4u", new List<string>{"video/vnd.mpegurl"}},
            {"m4v", new List<string>{"video/x-m4v"}},
            {"mac", new List<string>{"image/x-macpaint"}},
            {"man", new List<string>{"application/x-troff-man"}},
            {"mathml", new List<string>{"application/mathml+xml"}},
            {"me", new List<string>{"application/x-troff-me"}},
            {"mesh", new List<string>{"model/mesh"}},
            {"mid", new List<string>{"audio/midi"}},
            {"midi", new List<string>{"audio/midi"}},
            {"mif", new List<string>{"application/vnd.mif"}},
            {"mov", new List<string>{"video/quicktime","application/octet-stream"}},
            {"movie", new List<string>{"video/x-sgi-movie"}},
            {"mp2", new List<string>{"audio/mpeg"}},
            {"mp3", new List<string>{"audio/mpeg"}},
            {"mp4", new List<string>{"video/mp4","application/octet-stream"}},
            {"mpe", new List<string>{"video/mpeg"}},
            {"mpeg", new List<string>{"video/mpeg"}},
            {"mpg", new List<string>{"video/mpeg"}},
            {"mpga", new List<string>{"audio/mpeg"}},
            {"ms", new List<string>{"application/x-troff-ms"}},
            {"msh", new List<string>{"model/mesh"}},
            {"mxu", new List<string>{"video/vnd.mpegurl"}},
            {"nc", new List<string>{"application/x-netcdf"}},
            {"oda", new List<string>{"application/oda"}},
            {"ogg", new List<string>{"application/ogg"}},
            {"pbm", new List<string>{"image/x-portable-bitmap"}},
            {"pct", new List<string>{"image/pict"}},
            {"pdb", new List<string>{"chemical/x-pdb"}},
            {"pdf", new List<string>{"application/pdf"}},
            {"pgm", new List<string>{"image/x-portable-graymap"}},
            {"pgn", new List<string>{"application/x-chess-pgn"}},
            {"pic", new List<string>{"image/pict"}},
            {"pict", new List<string>{"image/pict"}},
            {"png", new List<string>{"image/x-png","image-png","image/png"}},
            {"pnm", new List<string>{"image/x-portable-anymap"}},
            {"pnt", new List<string>{"image/x-macpaint"}},
            {"pntg", new List<string>{"image/x-macpaint"}},
            {"ppm", new List<string>{"image/x-portable-pixmap"}},
            {"ppt", new List<string>{"application/vnd.ms-powerpoint","application/mspowerpoint"}},
            {"pptx", new List<string>{"application/vnd.openxmlformats-officedocument.presentationml.presentation"}},
            {"potx", new List<string>{"application/vnd.openxmlformats-officedocument.presentationml.template"}},
            {"ppsx", new List<string>{"application/vnd.openxmlformats-officedocument.presentationml.slideshow"}},
            {"ppam", new List<string>{"application/vnd.ms-powerpoint.addin.macroEnabled.12"}},
            {"pptm", new List<string>{"application/vnd.ms-powerpoint.presentation.macroEnabled.12"}},
            {"potm", new List<string>{"application/vnd.ms-powerpoint.template.macroEnabled.12"}},
            {"ppsm", new List<string>{"application/vnd.ms-powerpoint.slideshow.macroEnabled.12"}},
            {"ps", new List<string>{"application/postscript"}},
            {"qt", new List<string>{"video/quicktime"}},
            {"qti", new List<string>{"image/x-quicktime"}},
            {"qtif", new List<string>{"image/x-quicktime"}},
            {"ra", new List<string>{"audio/x-pn-realaudio"}},
            {"ram", new List<string>{"audio/x-pn-realaudio"}},
            {"rar", new List<string>{"application/x-zip-compressed","application/x-compressed" }},
            {"ras", new List<string>{"image/x-cmu-raster"}},
            {"rdf", new List<string>{"application/rdf+xml"}},
            {"rgb", new List<string>{"image/x-rgb"}},
            {"rm", new List<string>{"application/vnd.rn-realmedia"}},
            {"roff", new List<string>{"application/x-troff"}},
            {"rtf", new List<string>{"text/rtf"}},
            {"rtx", new List<string>{"text/richtext"}},
            {"sgm", new List<string>{"text/sgml"}},
            {"sgml", new List<string>{"text/sgml"}},
            {"sh", new List<string>{"application/x-sh"}},
            {"shar", new List<string>{"application/x-shar"}},
            {"silo", new List<string>{"model/mesh"}},
            {"sit", new List<string>{"application/x-stuffit"}},
            {"skd", new List<string>{"application/x-koan"}},
            {"skm", new List<string>{"application/x-koan"}},
            {"skp", new List<string>{"application/x-koan"}},
            {"skt", new List<string>{"application/x-koan"}},
            {"smi", new List<string>{"application/smil"}},
            {"smil", new List<string>{"application/smil"}},
            {"snd", new List<string>{"audio/basic"}},
            {"so", new List<string>{"application/octet-stream"}},
            {"spl", new List<string>{"application/x-futuresplash"}},
            {"src", new List<string>{"application/x-wais-source"}},
            {"sv4cpio", new List<string>{"application/x-sv4cpio"}},
            {"sv4crc", new List<string>{"application/x-sv4crc"}},
            {"svg",   new List<string>{"image/svg+xml","img/svg+xml","text/xml"}},
            {"swf", new List<string>{"application/x-shockwave-flash"}},
            {"t", new List<string>{"application/x-troff"}},
            {"tar", new List<string>{"application/x-tar"}},
            {"tcl", new List<string>{"application/x-tcl"}},
            {"template", new List<string>{"plain/text","text/plain"}},
            {"tex", new List<string>{"application/x-tex"}},
            {"texi", new List<string>{"application/x-texinfo"}},
            {"texinfo", new List<string>{"application/x-texinfo"}},
            {"tif", new List<string>{"image/tiff"}},
            {"tiff", new List<string>{"image/tiff"}},
            {"tr", new List<string>{"application/x-troff"}},
            {"tsv", new List<string>{"text/tab-separated-values"}},
            {"txt", new List<string>{"text/plain"}},
            {"ttf", new List<string>{"image/x-icon","image/x-ttf"}},

            {"ustar", new List<string>{"application/x-ustar"}},
            {"vcd", new List<string>{"application/x-cdlink"}},
            {"vrml", new List<string>{"model/vrml"}},
            {"vxml", new List<string>{"application/voicexml+xml"}},
            {"wav", new List<string>{"audio/x-wav","video/x-msvideo"}},
            {"wbmp", new List<string>{"image/vnd.wap.wbmp"}},
            {"wbmxl", new List<string>{"application/vnd.wap.wbxml"}},
            {"wml", new List<string>{"text/vnd.wap.wml"}},
            {"wmlc", new List<string>{"application/vnd.wap.wmlc"}},
            {"wmls", new List<string>{"text/vnd.wap.wmlscript"}},
            {"wmlsc", new List<string>{"application/vnd.wap.wmlscriptc"}},
            {"wrl", new List<string>{"model/vrml"}},
            {"xbm", new List<string>{"image/x-xbitmap"}},
            {"xht", new List<string>{"application/xhtml+xml"}},
            {"xhtml", new List<string>{"application/xhtml+xml"}},
            {"xls", new List<string>{"application/vnd.ms-excel","application/excel"}},
            {"xml", new List<string>{"application/xml","text/xml"}},
            {"xpm", new List<string>{"image/x-xpixmap"}},
            {"xsl", new List<string>{"application/xml","text/xml"}},
            {"xlsx", new List<string>{"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"}},
            {"xltx", new List<string>{"application/vnd.openxmlformats-officedocument.spreadsheetml.template"}},
            {"xlsm", new List<string>{"application/vnd.ms-excel.sheet.macroEnabled.12"}},
            {"xltm", new List<string>{"application/vnd.ms-excel.template.macroEnabled.12"}},
            {"xlam", new List<string>{"application/vnd.ms-excel.addin.macroEnabled.12"}},
            {"xlsb", new List<string>{"application/vnd.ms-excel.sheet.binary.macroEnabled.12"}},
             {"xsd", new List<string>{"text/plain","text/xml"}},
            {"xslt", new List<string>{"application/xslt+xml"}},
            {"xul", new List<string>{"application/vnd.mozilla.xul+xml"}},
            {"xwd", new List<string>{"image/x-xwindowdump"}},
            {"xyz", new List<string>{"chemical/x-xyz"}},
            {"zip", new List<string>{"application/x-zip-compressed","application/x-compressed"}}
        };
    }
}
