using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WiseWay.Core;
using WiseWay.Extender;

namespace WiseWay.Facade
{
    public class UserFacade
    {
        /// <summary>
        /// After validating user it create token and return that token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="secret"></param>
        /// <returns>After validating user it create token and return that token</returns>
        public static User Authenticate(User user, string secret, int expiryDays)
        {
            var authenticatedUser = UserFacade.AuthenticateUser(user);

            // return null if user not found
            if (authenticatedUser == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, authenticatedUser.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(expiryDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            authenticatedUser.Token = tokenHandler.WriteToken(token);

            return authenticatedUser.WithoutPassword();
        }

        /// <summary>
        /// pass user model with username & password and validate it then return list of that selected row
        /// </summary>
        /// <param name="users"></param>       
        /// <returns>If validate successfully then return user data of login user </returns>
        public static User AuthenticateUser(User user)
        {
            DataSet dataSet = new DataSet();
            using (SqlConnection sqlcon = new SqlConnection(DBUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Login", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                    sqlcon.Open();
                    sqlDataAdapter.Fill(dataSet);
                    sqlcon.Close();
                }
            }
            User loggedInUser = null;
            loggedInUser = GetUserData(dataSet, loggedInUser);
            return loggedInUser;
        }

        private static User GetUserData(DataSet dataSet, User loggedInUser)
        {
            if (dataSet.HasData())
            {
                DataRow dataRow = dataSet.Tables[0].Rows[0];
                try
                {
                    loggedInUser = new User()
                    {
                        Id = dataRow["UserId"].ToLong(),
                        UserName = dataRow["UserName"].ToString(),
                        Password = dataRow["Password"].ToString(),
                        FirstName = dataRow["FirstName"].ToString(),
                        LastName = dataRow["LastName"].ToString(),
                        Email = dataRow["Email"].ToString(),
                        PhoneNo = dataRow["PhoneNo"].ToString(),
                        Address = dataRow["Address"].ToString(),
                        UserType = dataRow["UserType"].ToString()
                    };
                    loggedInUser.Msg = "Get user data successfully";
                }
                catch (Exception)
                {
                    Console.WriteLine("no data is present in database");
                }
            }
            else
            {
                loggedInUser.Msg = "Invalid user id";
            }
            return loggedInUser;
        }

        public static User AddUpdateUser(User objModel)
        {
            DataSet dataSet = new DataSet();
            User tempUser = new User();
            using (SqlConnection con = new SqlConnection(DBUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_AddUpdateUser", con))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserName", objModel.UserName);
                        cmd.Parameters.AddWithValue("@FirstName", objModel.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", objModel.LastName);
                        cmd.Parameters.AddWithValue("@Password", objModel.Password);
                        cmd.Parameters.AddWithValue("@Phone", objModel.PhoneNo);
                        cmd.Parameters.AddWithValue("@Email", objModel.Email);
                        cmd.Parameters.AddWithValue("@Address", objModel.Address);
                        cmd.Parameters.AddWithValue("@UserType", objModel.UserType);
                        cmd.Parameters.AddWithValue("@ID", objModel.Id);

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                        con.Open();
                        sqlDataAdapter.Fill(dataSet);
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        //throw;
                        if (ex.Message.Contains("Email_Users"))
                        {
                            tempUser.Msg = "Email already exist";
                        }
                        return tempUser;
                    }
                }
            }
            tempUser = GetUserData(dataSet, tempUser);
            if (objModel.Id == 0)
            { tempUser.Msg = "User data added successfully"; }
            else { tempUser.Msg = "User data updated successfully"; }
            return tempUser;
        }

        public static List<User> GetUsers()
        {
            DataSet dataSet = userList(0);
            List<User> User = new List<User>();
            if (dataSet.Tables.Count > 0)
            {
                foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                {
                    try
                    {
                        User objUser = new User()
                        {
                            Id = dataRow["UserId"].ToLong(),
                            UserName = dataRow["UserName"].ToString(),
                            Password = dataRow["Password"].ToString(),
                            FirstName = dataRow["FirstName"].ToString(),
                            LastName = dataRow["LastName"].ToString(),
                            Email = dataRow["Email"].ToString(),
                            PhoneNo = dataRow["PhoneNo"].ToString(),
                            Address = dataRow["Address"].ToString(),
                            UserType = dataRow["UserType"].ToString()
                        };
                        User.Add(objUser);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("no data is present in database");
                    }
                }
            }
            return User;
        }
        public static string GetUserList(int IsJsonFormat = 1)
        {
            DataSet dataSet = userList(IsJsonFormat);
            string JSONresult = "{\"msg\":\"Data is not available\"}";
            if (dataSet.Tables.Count > 0)
            {
                DataTable dt = dataSet.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    JSONresult = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        JSONresult += dt.Rows[i][0];
                    }
                }
            }
            return JSONresult;
        }

        private static DataSet userList(int IsJsonFormat)
        {
            DataSet dataSet = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(DBUtil.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("usp_GetUserList", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@IsJsonFormat", IsJsonFormat);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlConnection.Open();
                    sqlDataAdapter.Fill(dataSet);
                    sqlConnection.Close();
                }
            }
            return dataSet;
        }

        public static string DeleteUser(int Id)
        {
            DataSet dataSet = new DataSet();
            using (SqlConnection con = new SqlConnection(DBUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_DeleteUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                    con.Open();
                    sqlDataAdapter.Fill(dataSet);
                    con.Close();
                }
            }
            string JSONresult = "{\"msg\":\"User id is not available\"}";
            if (dataSet.Tables.Count > 0)
            {
                DataTable dt = dataSet.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    JSONresult = "{\"msg\":\"User Deleted Successfully.\"}";
                }
            }
            return JSONresult;
        }

        public static string ChangeUserStatus(int Id, bool IsActive)
        {
            DataSet dataSet = new DataSet();
            using (SqlConnection con = new SqlConnection(DBUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ChangeUserStatus", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", Id);
                    cmd.Parameters.AddWithValue("@IsActive", IsActive);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                    con.Open();
                    sqlDataAdapter.Fill(dataSet);
                    con.Close();
                }
            }
            string JSONresult = "{\"msg\":\"User id is not available\"}";
            if (dataSet.Tables.Count > 0)
            {
                DataTable dt = dataSet.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    JSONresult = "{\"msg\":\"User status changed successfully.\"}";
                }
            }
            return JSONresult;
        }

        public static User GetUserDetailById(int UserId)
        {
            DataSet dataSet = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(DBUtil.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("usp_GetUserById", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Id", UserId);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlConnection.Open();
                    sqlDataAdapter.Fill(dataSet);
                    sqlConnection.Close();
                }
            }
            User tempUser = new User();
            tempUser = GetUserData(dataSet, tempUser);
            return tempUser;
        }

    }
}
