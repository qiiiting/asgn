using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Password_Hashing
{
    public partial class Success : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static byte[] Key = null;
        static byte[] IV = null;

        static byte[] email = null;
        static byte[] ccnum = null;
        static byte[] cvv = null;
        static byte[] expdate = null;
        static string userid = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserID"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    userid = Session["UserID"].ToString();
                    displayUserProfile(userid);

                }
            }

            else
            {
                Response.Redirect("Login.aspx", false);

        
            }

            
        }

        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
                            plainText = srDecrypt.ReadToEnd();
                        
                        }
                    }
                }
            }
            
            
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return plainText;
        }


        protected void displayUserProfile(string userid)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["email"] != DBNull.Value)
                        {
                            lb_email.Text = reader["email"].ToString();
                        }

                        if (reader["ccNo"] != DBNull.Value)
                        {

                            ccnum = Convert.FromBase64String(reader["ccNo"].ToString());

                        }

                        if (reader["cvv"] != DBNull.Value)
                        {
                            cvv = Convert.FromBase64String(reader["cvv"].ToString());

                        }

                        if (reader["ccExpiry"] != DBNull.Value)
                        {
                            expdate = Convert.FromBase64String(reader["ccExpiry"].ToString());

                        }

                        if (reader["IV"] != DBNull.Value)
                        {
                            IV = Convert.FromBase64String(reader["IV"].ToString());

                        }
                        if (reader["Key"] != DBNull.Value)
                        {
                            Key = Convert.FromBase64String(reader["Key"].ToString());

                        }
                    }
                    lb_ccNum.Text = decryptData(ccnum);
                    lb_cvv.Text = decryptData(cvv);
                    lb_exp.Text = decryptData(expdate);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                connection.Close();
            }
        }

        protected void btn_logout_click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Console.WriteLine("User " + userid + " logged out");

            Response.Redirect("Login.aspx", false);

            if (Request.Cookies["ASP.NET_SessionID"] != null)
            {
                Response.Cookies["ASP.NET_SessionID"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionID"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }

        }
    }

    
}