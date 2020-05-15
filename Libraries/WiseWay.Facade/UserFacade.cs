using Microsoft.IdentityModel.Tokens;
using System;
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
                        Email = dataRow["Email"].ToString()
                    };
                }
                catch (Exception)
                {
                    Console.WriteLine("no data is present in database");
                }
            }
            return loggedInUser;
        }
    }
}
