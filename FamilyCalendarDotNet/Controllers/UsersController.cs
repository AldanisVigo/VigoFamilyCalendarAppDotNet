using System.Security.Claims;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using FamilyCalendarDotNet.Models;
using FamilyCalendarDotNet.Utilities;
using FamilyCalendarDotNet.Interfaces;

namespace FamilyCalendarDotNet.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UsersController : Controller
{
    // This is the MySQL connection that will get injected
    readonly MySqlConnection connection;
    readonly IConfiguration iConfig;
    private readonly IMailService mailService;

    // This is the constructor with the injected MySQL connection
    public UsersController(MySqlConnection conn, IConfiguration iconf, IMailService ms)
    {
        // Set the readonly connection
        connection = conn;
        iConfig = iconf;
        mailService = ms;
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
    public async Task<JsonResult> RegisterNewUser(UserRegistrationDTO user)
    {
        try
        {
            if (ModelState.IsValid)
            {
                // Check that the username is available first and there's no users
                // With that username already
                if (await UsernameIsAvaialable(user.Username))
                {
                    // Check that the password and confirmation passwords match
                    if (user.Password == user.PasswordConfirmation)
                    {
                        // Hash the incoming password
                        string hashed_pass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: user.Password,
                            salt: new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 },
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8
                        ));

                        Console.WriteLine($"Hashed Password for {user.Username}: {hashed_pass}");

                        // Open the MySQL connection asynchronously
                        //await connection.OpenAsync();

                        // Create a new sql command
                        MySqlCommand cmd = new()
                        {
                            Connection = connection, // Set it's connection to the injected connection
                            // And set the command text to the query
                            CommandText = "INSERT INTO users(fullname,email,username,password,registrationDate) VALUES(?fullname,?email,?username,?password,?registrationDate)"
                        };

                        // Replace the parameters with the incoming data
                        cmd.Parameters.Add("?fullname", MySqlDbType.VarChar).Value = user.FullName;
                        cmd.Parameters.Add("?email", MySqlDbType.VarChar).Value = user.Email;
                        cmd.Parameters.Add("?registrationDate", MySqlDbType.DateTime).Value = user.RegistrationDate;
                        cmd.Parameters.Add("?username", MySqlDbType.VarChar).Value = user.Username;
                        cmd.Parameters.Add("?password", MySqlDbType.VarChar).Value = hashed_pass;

                        await cmd.ExecuteNonQueryAsync();

                        return new JsonResult(new Dictionary<string, string>(){
                            {
                                "success", "User has been registered successfully."
                            }
                        });
                    }
                    else
                    {
                        return new JsonResult(new Dictionary<string, string>()
                        {
                            {
                                "error", "The password and password confirmation do not match. Please check your passwords and try again."
                            }
                        });
                    }
                }
                else
                {
                    return new JsonResult(new Dictionary<string, string>() {
                        {
                            "error", "The username provided is already taken. Please either recover your existing account or create a new account with a different username."
                        }
                    });
                }
            }
            else
            {
                // Get the validation results from the ModelState and return an error
                return new JsonResult(new Dictionary<string, string>(){
                    {
                        "error", ModelState.ValidationState.ToString()
                    }
                });
            }
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
     * This is a function for checking if a username is already taken
     */
    private async Task<bool> UsernameIsAvaialable(string username)
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
            cmd.Parameters.Add("?username", MySqlDbType.VarChar).Value = username;


            // Execute the MySQL command and read the results
            using var reader = await cmd.ExecuteReaderAsync();

            // If there were some results
            if (reader.HasRows)
            {
                // Then return false because the username is taken
                return false;
            }
            else
            {
                // Otherwise return true because the username is available.
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"There was an error while checking the username {username}");
            Console.WriteLine(e.Message);
            return false;
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
            // This is handled by a middleware service now.
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

    /*
     *  Endpoint for sending recovery link
     */
    [HttpPost]
    [AllowAnonymous]
    [Route("Recover")]
    public async Task<JsonResult> SendRecovery(string identifier)
    {
        try
        {
            // Check if the identifier is a username
            var identifierIsAUsername = !(await UsernameIsAvaialable(identifier));


            if (identifierIsAUsername)
            {
                // The identifier is a username
                // Query for the email

                // Create a new sql command
                MySqlCommand cmd = new()
                {
                    Connection = connection, // Set it's connection to the injected connection
                                             // And set the command text to the query
                    CommandText = "SELECT * FROM users WHERE username=?username LIMIT 1;"
                };

                // Replace the parameters with the incoming data
                cmd.Parameters.Add("?username", MySqlDbType.VarChar).Value = identifier;


                // Execute the MySQL command and read the results
                using var reader = await cmd.ExecuteReaderAsync();

                // If there were some results
                if (reader.HasRows)
                {
                    // Read the result
                    reader.Read();

                    // Grab the data out of the result
                    string db_email = reader.GetString("email");
                    string db_fullName = reader.GetString("fullName");

                    // This identifier is a valid email address
                    if (mailService.SendMail(
                        new MailData()
                        {
                            EmailSubject = "Account Recovery Link",
                            EmailBody = "Some generated link here.",
                            EmailToId = db_email,
                            EmailToName = db_fullName
                        })
                    )
                    {
                        return new JsonResult(new Dictionary<string, string>()
                        {
                            {
                                "success", "We've sent you a recovery email. Please click the link included to recover your account."
                            }
                        });
                    }
                    else
                    {
                        return new JsonResult(new Dictionary<string, string>()
                        {
                            {
                                "error", "There was an error while sending you the recovery link. Please try again later."
                            }
                        });
                    }
                }
                else
                {
                    return new JsonResult(new Dictionary<string, string>()
                    {
                        {
                            "error" , "Username provided does not belong to any active accounts."
                        }
                    });
                }
            }
            else
            {
                // The identifier is most likely an email address.
                if (EmailValidator.IsEmailValid(identifier))
                {
                    Console.WriteLine("The identifier provided is an email.");
                    // This identifier is a valid email address
                    if (mailService.SendMail(
                        new MailData()
                        {
                            EmailSubject = "Account Recovery Link",
                            EmailBody = "Some generated link here.",
                            EmailToId = identifier,
                            EmailToName = ""
                        })
                    )
                    {
                        return new JsonResult(new Dictionary<string, string>()
                        {
                            {
                                "success", "We've sent you a recovery email. Please click the link included to recover your account."
                            }
                        });
                    }
                    else
                    {
                        return new JsonResult(new Dictionary<string, string>()
                        {
                            {
                                "error","There was an error while sending you the recovery link. Please try again later."
                            }
                        });
                    }
                }
                else
                {
                    return new JsonResult(new Dictionary<string, string>()
                        {
                            {
                                "error", "The email provided is not valid."
                            }
                        });
                }
            }

            //return new JsonResult("");
        }catch(Exception e)
        {
            Console.WriteLine("There was an error while attempting to send a recovery link.");
            Console.WriteLine(e.Message);
            return new JsonResult(new Dictionary<string, string>()
            {
                {
                    "error" , e.Message
                }
            });
        }
    }
}


