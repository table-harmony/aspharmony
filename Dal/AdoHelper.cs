using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Security.Cryptography;

/// <summary>
/// פעולות עזר לשימוש במסד נתונים  מסוג 
/// SQL SERVER
///  App_Data המסד ממוקם בתקיה 
/// </summary>
public class AdoHelper
{
    private static string fileName = "Database1.mdf";

    // טענת כניסה: אין
    // טענת יציאה: קישור למסד נתונים
    public static SqlConnection ConnectToDb() {
        string path = HttpContext.Current.Server.MapPath("/App_Data/" + fileName); //מאתר את מיקום מסד הנתונים מהשורש ועד התקייה בה ממוקם המסד
        string connString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + path + "; Integrated Security = True";

        SqlConnection conn = new SqlConnection(connString);
        return conn;
    }

    // טענת כניסה: שאילתה, עצם המכיל פרמטרים, האם השאילתה היא פרוצדורה
    // טענת יציאה: טבלה המכילה את תוצאות השאילתה
    public static DataTable GetDataTable(string sql, object parameters = null, bool isSp = false) {
        SqlConnection conn = ConnectToDb();

        SqlCommand command = new SqlCommand(sql, conn);
        if (isSp) command.CommandType = CommandType.StoredProcedure;  // אם השאילתה היא פרוצדורה

        // עבור כל פרמטר נכניס לשאילתה את השם והערך שלו
        foreach (var property in parameters.GetType().GetProperties())
            command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(parameters));

        SqlDataAdapter tableAdapter = new SqlDataAdapter(command);
        DataTable dt = new DataTable();

        try {
            conn.Open();
            tableAdapter.Fill(dt);
        }
        catch (Exception e) {
        }
        finally {
            conn.Close();
        }

        return dt;
    }

}

