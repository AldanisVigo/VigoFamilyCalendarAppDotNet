﻿using System.Security.Claims;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using FamilyCalendarDotNet.Models;

namespace FamilyCalendarDotNet.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UsersController : Controller
{
    // This is the MySQL connection that will get injected
    readonly MySqlConnection connection;
    readonly IConfiguration iConfig;
    readonly JWTTokenBlacklistingService _tokenBlacklistingService;
    // This is the constructor with the injected MySQL connection
    public UsersController(MySqlConnection conn, IConfiguration iconf, JWTTokenBlacklistingService ts)
    {
        // Set the readonly connection
        connection = conn;
        iConfig = iconf;
        _tokenBlacklistingService = ts;
    }

    // This is the destructor
    ~UsersController()
    {
        // Close the MySQL connection
        connection.Close();
    }

    /*
     * This is a POST mapping for /RegisterNewUser
     */
    [HttpPost]
    [AllowAnonymous]
    [Route("RegisterNewUser")]
    public async Task<JsonResult> RegisterNewUser(DateTime registrationDate,string username, string password)
    {
        try
        {
            // Hash the incoming password
            string hashed_pass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: new byte[]{1,2,3,4,5,6,7,8,9,0},
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            Console.WriteLine($"Hashed Password for {username}: {hashed_pass}");

            // Open the MySQL connection asynchronously
            await connection.OpenAsync();

            // Create a new sql command
            MySqlCommand cmd = new()
            {
                Connection = connection, // Set it's connection to the injected connection
                // And set the command text to the query
                CommandText = "INSERT INTO users(username,password,registrationDate) VALUES(?username,?password,?registrationDate)"
            };

            // Replace the parameters with the incoming data
            cmd.Parameters.Add("?registrationDate", MySqlDbType.DateTime).Value = registrationDate;
            cmd.Parameters.Add("?username", MySqlDbType.VarChar).Value = username;
            cmd.Parameters.Add("?password", MySqlDbType.VarChar).Value = hashed_pass;

            await cmd.ExecuteNonQueryAsync();

            return new JsonResult(new Dictionary<string, string>(){
                {
                    "success", "User has been registered successfully."
                }
            });
        }
        catch (Exception err)
        {
            Console.WriteLine("There was an error while attempting to add an event.");
            Console.WriteLine(err.Message);
            return new JsonResult(new Dictionary<string, string>(){
                {
                    "error", err.Message
                }
            });
        }
    }

    /*
     * This is a function for generating a JWT token with some Claims for the given userId
     */
    private string CreateJwtToken(int userId)
    {
        Claim[] userClaims = new Claim[]
        {
            new Claim("id",userId.ToString())
        };

        // Grab the tokenKey from the configuration in appsettings.json
        string tokenKey = iConfig.GetValue<string>("AppSettings:TokenKey")!;

        Console.WriteLine("Retrieving server token key from appsettings.json...");
        Console.WriteLine($"Token Key: {tokenKey}");

        // Generate a symmetric security key with it
        SymmetricSecurityKey tokeyKey = new(Encoding.UTF8.GetBytes(tokenKey));

        // Sign the token
        SigningCredentials credentials = new(tokeyKey, SecurityAlgorithms.HmacSha512Signature);

        // Create the security token descriptor
        SecurityTokenDescriptor descriptor = new()
        {
            // Add the subject claims
            Subject = new ClaimsIdentity(userClaims),
            // Add the signing credentials to the token descriptor
            SigningCredentials = credentials,
            // Expire this token after 24hrs
            Expires = DateTime.Now.AddDays(1)
        };

        // Create the token handler
        JwtSecurityTokenHandler tokenHandler = new();

        // Create the token using the tokenHandler and passing the token descriptor
        SecurityToken secToken = tokenHandler.CreateToken(descriptor);

        Console.WriteLine($"Generating Token String...");

        // Generate the token string.
        string secTokenStr = tokenHandler.WriteToken(secToken);

        // Log the token to the terminal
        Console.WriteLine($"Token:${secTokenStr}");

        // Return the token string
        return secTokenStr;
    }

    /*
     * This is a POST mapping for /login
     * 
     */
    [HttpPost]
    [Route("Login")]
    [AllowAnonymous]
    public async Task<JsonResult> Login(UserDTO user)
    {
        try
        {
            // Open the MySQL connection asynchronously
            await connection.OpenAsync();

            // Create a new sql command
            MySqlCommand cmd = new()
            {
                Connection = connection, // Set it's connection to the injected connection
                // And set the command text to the query
                CommandText = "SELECT * FROM users WHERE username=?username LIMIT 1;"
            };

            // Replace the parameters with the incoming data
            cmd.Parameters.Add("?username", MySqlDbType.VarChar).Value = user.Username;


            // Execute the MySQL command and read the results
            using var reader = await cmd.ExecuteReaderAsync();

            // If there were some results
            if (reader.HasRows)
            {
                // Read the result
                reader.Read();

                // Grab the data out of the result
                string db_username = reader.GetString("username");
                string db_password = reader.GetString("password");
                int db_userId = reader.GetInt32("id");


                // Check if the password and the hash match
                // Hash the incoming password
                string hashed_pass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: user.Password,
                    salt: new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 },
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                ));

                // Create a flag that represents a match between the hash and the stored hash
                bool match = hashed_pass == db_password;

                // if there's a match
                if (match)
                {
                    // Return the JWT token
                    return new JsonResult(new Dictionary<string, string>
                    {
                        {
                            "token", CreateJwtToken(db_userId)
                        }
                    });
                }
                else // Otherwise
                {
                    // Return an error message.
                    return new JsonResult(new Dictionary<string, string>
                    {
                        {
                            "error","Password does not match the stored password."
                        }
                    });
                }
            }
            // Otherwise return an error message that the user was not found.
            else return new JsonResult(new Dictionary<string, string>(){
                {
                    "error","User was not found."
                }
            });

        }
        catch (Exception err)
        {
            Console.WriteLine($"Found an error while attempting to log in {user.Username}");
            Console.WriteLine(err.Message);
            return new JsonResult(new Dictionary<string, string>(){
                {
                    "error", err.Message
                }
            });
        }
    }

    /*
     * Endpoint for logging out and blacklisting the JWT token
     */
    [HttpPost]
    [Route("Logout")]
    public JsonResult Logout(string token)
    {
        try
        {
            //Console.WriteLine($"Blacklisting the following token {token}");
            //_tokenBlacklistingService.AddToBlacklist(token);
            //Console.WriteLine("The token has been blacklisted.");

            return new JsonResult(new Dictionary<string, string>()
            {
                {
                    "success", "Successfully logged out."
                }
            });
        }catch(Exception e)
        {
            Console.WriteLine($"Found an error while attempting to logout and blacklist {token}");
            Console.WriteLine(e.Message);
            return new JsonResult(new Dictionary<string, string>()
            {
                {
                    "error", e.Message
                }
            });
        }
    }


    /*
     * Endpoint for refreshing the token
     */
    [HttpGet("RefreshToken")]
    public JsonResult RefreshToken(int userId)
    {
        Console.WriteLine($"Generating a RefreshToken for {userId}");

        string token = CreateJwtToken(userId);

        Console.WriteLine($"Generated token {token}");

        // Get the token
        return new JsonResult(new Dictionary<string, string>(){
            {
                "token", token
            }
        });
    }
}


