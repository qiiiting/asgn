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
using System.Text.RegularExpressions;
using System.Drawing;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Net.Mail;

namespace Password_Hashing
{
    public partial class Registration : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        static string line = "\r";

        //static string isDebug = ConfigurationManager.AppSettings["isDebug"].ToString();


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string pwd = tb_pwd.Text.ToString().Trim();

            if (checkInputs() == false)
            {
                lb_error.Text = lb_error.Text = "One or more field values invalid. Please try again";
                lb_error.ForeColor = Color.Red;
                return;
            }
            if (checkID(tb_email.Text.ToString()) == false)
            {
                lb_error.Text = lb_error.Text = "Email in use";
                lb_error.ForeColor = Color.Red;
                return;
            }

            try
            {
                if (ValidateCaptcha())
                {
                        //password hashing

                        //Generate random "salt" 
                        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                        byte[] saltByte = new byte[8];

                        //Fills array of bytes with a cryptographically strong sequence of random values.
                        rng.GetBytes(saltByte);
                        salt = Convert.ToBase64String(saltByte);

                        SHA512Managed hashing = new SHA512Managed();

                        string pwdWithSalt = pwd + salt;
                        byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                        finalHash = Convert.ToBase64String(hashWithSalt);

                        //encrypting cc info

                        RijndaelManaged cipher = new RijndaelManaged();
                        cipher.GenerateKey();
                        Key = cipher.Key;
                        IV = cipher.IV;



                        createAccount();

                        Console.WriteLine("Account created with userid " + tb_email.Text.Trim());

                        Response.Redirect("Login.aspx", false);


                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { };

            
        }


        protected void createAccount()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(" +
                        "@firstName,@lastName,@ccNo,@cvv,@ccExpiry,@email,@password,@birthDate,@key,@IV,@salt"
                        + ")"))

                       
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@firstName", tb_fname.Text.Trim());
                            cmd.Parameters.AddWithValue("@lastName", tb_lname.Text.Trim());
                            cmd.Parameters.AddWithValue("@birthDate", tb_dob.Text.Trim());
                            cmd.Parameters.AddWithValue("@email", tb_email.Text.Trim());

                            //creditcard vals
                            cmd.Parameters.AddWithValue("@ccNo", Convert.ToBase64String(encryptData(tb_ccNo.Text.Trim())));
                            cmd.Parameters.AddWithValue("@cvv", Convert.ToBase64String(encryptData(tb_cvv.Text.Trim())));
                            cmd.Parameters.AddWithValue("@ccExpiry", Convert.ToBase64String(encryptData(tb_exp.Text.Trim())));

                            //pw
                            cmd.Parameters.AddWithValue("@password", finalHash);

                            //salt key iv
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@salt", salt);

                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return cipherText;
        }

        protected void tb_dob_TextChanged(object sender, EventArgs e)
        {

        }

        private int checkPwd(string pw)
        {
            int score = 0;

            //pw length
            if (pw.Length >= 12)
            {score += 1;}

            //lowercase
            if (Regex.IsMatch(pw, "[a-z]"))
            { score += 1; }

            //uppercase
            if (Regex.IsMatch(pw, "[A-Z]"))
            { score += 1; }

            //numeric
            if (Regex.IsMatch(pw, "[0-9]"))
            { score += 1; }

            //special characters
            if (Regex.IsMatch(pw, "[^A-Za-z0-9]"))
            { score += 1; }

            return score;

        }

        private bool checkCVV()
        {
            var str = tb_cvv.Text.Trim().ToString();

            if (Regex.IsMatch(str, "[0-9]{3,3}") && str.Length == 3)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool checkCC()
        {
           var str = tb_ccNo.Text.Trim().ToString();

            if (Regex.IsMatch(str, "[0-9]{16,16}") && str.Length == 16)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkEXP()
        {
            var str = tb_exp.Text.Trim().ToString();

            if (Regex.IsMatch(str, "[0-9]{4,4}") && str.Length == 4)
            {
                var month = Convert.ToInt32(str.Substring(0, 1));
                if (month < 13)
                {
                    return true;
                       
                }
            }

            return false;
            
        }

        private bool checkEmail()
        {
            string mail = tb_email.Text.ToString().Trim();
            Regex rgx = new Regex(@"/^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i");

            bool isValid = false;

            try
            {
                MailAddress address = new MailAddress(mail);
                isValid = (address.Address == mail);
            }
            catch (FormatException)
            {
                // address is invalid
            }

            return isValid;
        }

        private bool checkInputs()
        {
            string pwd = tb_pwd.Text.ToString().Trim();
            if (checkPwd(pwd) < 5)
            {
                lb_pwdError.Text = "pwd";
                return false;
            }
            if (checkCC() == false)
            {
                lb_pwdError.Text = "cc";
                return false;
            }
            if(checkCVV() == false)
            {
                lb_pwdError.Text = "cvv";
                return false;
            }
            if(checkEXP() == false)
            {
                lb_pwdError.Text = "exp";
                return false;
            }
            if(checkEmail() == false)
            {
                lb_pwdError.Text = "mail";
                return false;
            }

            return true;
        }

        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        public bool ValidateCaptcha()
        {
            bool result = true;

            //When user submits the recaptcha form, the user gets a response POST parameter. 
            //captchaResponse consist of the user click pattern. Behaviour analytics! AI :) 
            string captchaResponse = Request.Form["g-recaptcha-response"];

            //To send a GET request to Google along with the response and Secret key.
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
           (" https://www.google.com/recaptcha/api/siteverify?secret=6Lfyc24eAAAAAM6LcRn0NNzAyNXU3z8sWsdgDwzb &response=" + captchaResponse);


            try
            {

                //Codes to receive the Response in JSON format from Google Server
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        //The response in JSON format
                        string jsonResponse = readStream.ReadToEnd();


                        JavaScriptSerializer js = new JavaScriptSerializer();

                        //Create jsonObject to handle the response e.g success or Error
                        //Deserialize Json
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        //Convert the string "False" to bool false or "True" to bool true
                        result = Convert.ToBoolean(jsonObject.success);//

                    }
                }

                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        public bool checkID(string userid)
        {
            var accExists = true;

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
                            accExists = false;
                        }
                        else
                        {
                            accExists = true;
                        }

                    }
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

            return accExists;
        }


    }


}