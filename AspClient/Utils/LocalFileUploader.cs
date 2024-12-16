using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using MimeTypes;

namespace AspClient.Utils {
    /// <summary>
    /// מחלקה המממשת העלאת קבצים לשירות לוקלי
    /// </summary>
    public class LocalFileUploader : IFileUploader {
        private readonly string _uploadDirectory;  // תיקיית הקבצים

        public LocalFileUploader(string uploadDirectory = null) {
            _uploadDirectory = uploadDirectory 
                ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");

            if (!Directory.Exists(_uploadDirectory)) {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }


        /// <summary>
        /// מעלה קובץ לתיקייה ומחזיר את כתובת הגישה אליו
        /// </summary>
        /// <param name="file">הקובץ להעלאה</param>
        /// <returns>כתובת URL לגישה לקובץ</returns>
        /// <exception cref="InvalidOperationException">כאשר נכשל לעלות קובץ</exception>
        public async Task<string> UploadFileAsync(File file) {
            try {
                string fileName = Guid.NewGuid().ToString() + GetExtension(file.ContentType);
                string filePath = Path.Combine(_uploadDirectory, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                    await file.Stream.CopyToAsync(fileStream);
                }

                return filePath;
            } catch (Exception ex) {
                throw new InvalidOperationException($"Failed to upload file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// מחשב סיומת של קובץ
        /// </summary>
        /// <param name="contentType">טיפוס הקובץ</param>
        /// <returns>סיומת הקובץ</returns>
        /// <exception cref="InvalidOperationException">כאשר טיפוס הקובץ לא נתמך</exception>
        private static string GetExtension(string contentType) {
            return MimeTypeMap.GetExtension(contentType);
        }
    }
}