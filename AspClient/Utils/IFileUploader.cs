using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AspClient.Utils {
    /// <summary>
    /// מחלקה המייצגת קובץ להעלאה
    /// </summary>
    public class File {
        /// <summary>
        /// זרם הנתונים של הקובץ
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// סוג התוכן של הקובץ (Content Type)
        /// </summary>
        public string ContentType { get; set; }
    }

    /// <summary>
    /// ממשק להעלאת קבצים המאפשר מימושים שונים כגון העלאה לשרת מרוחק או לתיקייה מקומית
    /// </summary>
    public interface IFileUploader {
        /// <summary>
        /// מעלה קובץ ומחזיר את כתובת ה-URL שלו
        /// </summary>
        /// <param name="file">אובייקט המכיל את פרטי הקובץ להעלאה</param>
        /// <returns>כתובת URL של הקובץ שהועלה</returns>
        Task<string> UploadFileAsync(File file);
    }
}