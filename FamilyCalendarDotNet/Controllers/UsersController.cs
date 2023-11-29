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
                Console.WriteLine("The user model is valid.");
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
                            CommandText = "INSERT INTO users(fullname,email,username,password,accountType,registrationDate) VALUES(?fullname,?email,?username,?password,?accountType,?registrationDate)"
                        };

                        // Replace the parameters with the incoming data
                        cmd.Parameters.Add("?fullname", MySqlDbType.VarChar).Value = user.FullName;
                        cmd.Parameters.Add("?email", MySqlDbType.VarChar).Value = user.Email;
                        cmd.Parameters.Add("?username", MySqlDbType.VarChar).Value = user.Username;
                        cmd.Parameters.Add("?password", MySqlDbType.VarChar).Value = hashed_pass;
                        cmd.Parameters.Add("?accountType", MySqlDbType.Int32).Value = user.UserAccountType ?? AccountType.Parent;
                        cmd.Parameters.Add("?registrationDate", MySqlDbType.DateTime).Value = user.RegistrationDate;


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
        string code = RecoveryCodeGenerator.GenerateCode();
        string recoveryLink = "http://localhost:5173/complete_recovery?recoveryCode=" + code;
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

                    // Close the reader
                    reader.Close();

                    // Store the recovery code
                    MySqlCommand storeCodeCommand = new()
                    {
                        Connection = connection,
                        CommandText = "INSERT INTO recovery(email,code) VALUES(?email,?code)"
                    };

                    storeCodeCommand.Parameters.Add("?email", MySqlDbType.VarChar).Value = db_email;
                    storeCodeCommand.Parameters.Add("?code", MySqlDbType.VarChar).Value = code;

                    await storeCodeCommand.ExecuteNonQueryAsync();

                    // This identifier is a valid email address
                    return SendRecoveryEmail(db_email, db_fullName, recoveryLink);
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

                    // Store the recovery code
                    MySqlCommand storeCodeCommand = new()
                    {
                        Connection = connection,
                        CommandText = "INSERT INTO recovery(email,code) VALUES(?email,?code)"
                    };

                    storeCodeCommand.Parameters.Add("?email", MySqlDbType.VarChar).Value = identifier;
                    storeCodeCommand.Parameters.Add("?code", MySqlDbType.VarChar).Value = code;

                    await storeCodeCommand.ExecuteNonQueryAsync();

                    // This identifier is a valid email address
                    return SendRecoveryEmail(identifier, "", recoveryLink);
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

    /*
     * Method sends out a recovery email
     */
    private JsonResult SendRecoveryEmail(string email,string name, string recoveryLink)
    {
        // This identifier is a valid email address
        if (mailService.SendMail(
            new MailData()
            {
                EmailSubject = "Account Recovery Link",
                HtmlEmailBody = "<a href='" + recoveryLink + "'>Click here to recover account.</a>",
                EmailToId = email,
                EmailToName = name
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

    /*
     * Endpoint for changing a given account's password given a recovery link code and a new password 
     */
    [HttpPost]
    [Route("RecoveryChangePassword")]
    [AllowAnonymous]
    public async Task<JsonResult> RecoveryChangePassword(RecoveryChangePasswordDTO data)
    {
        try
        {
            // Make sure the password and password confirmation match
            if (data.NewPassword == data.NewPasswordConfirmation)
            {
                // Query the email associated with the code
                await connection.OpenAsync();

                // Create a new sql command
                MySqlCommand cmd = new()
                {
                    Connection = connection, // Set it's connection to the injected connection
                                             // And set the command text to the query
                    CommandText = "SELECT * FROM recovery WHERE code=?code LIMIT 1;"
                };

                // Replace the parameters with the incoming data
                cmd.Parameters.Add("?code", MySqlDbType.VarChar).Value = data.Code;


                // Execute the MySQL command and read the results
                using var reader = await cmd.ExecuteReaderAsync();

                // If there were some results
                if (reader.HasRows)
                {
                    // Read the result
                    reader.Read();

                    // Grab the data out of the result
                    string db_email = reader.GetString("email");

                    // Update the password for the user with that email to the given password
                    reader.Close();

                    // Check that the code and password and confirmation are valid
                    if (ModelState.IsValid)
                    {
                        // Get the hash of the new password
                        string hashed_pass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: data.NewPassword,
                            salt: new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 },
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8
                        ));

                        // Update the existing password with the hash of the new password
                        MySqlCommand updatePasswordCommand = new()
                        {
                            Connection = connection,
                            CommandText = "UPDATE users SET password=?password WHERE email=?email"
                        };

                        // Prepare que query statement with the new password
                        updatePasswordCommand.Parameters.Add("?password", MySqlDbType.VarChar).Value = hashed_pass;
                        updatePasswordCommand.Parameters.Add("?email", MySqlDbType.VarChar).Value = db_email;

                        // Fire off the update query
                        await updatePasswordCommand.ExecuteNonQueryAsync();

                        // Delete the recovery code
                        MySqlCommand deleteCodeCommand = new()
                        {
                            Connection = connection,
                            CommandText = "DELETE FROM recovery WHERE code=?code"
                        };

                        deleteCodeCommand.Parameters.Add("?code", MySqlDbType.VarChar).Value = data.Code;

                        // Fire off the recovery code deletion
                        await deleteCodeCommand.ExecuteNonQueryAsync();

                        // Let the client know the password was updated successfuly.
                        return new JsonResult(new Dictionary<string, string>() {
                            {
                                "success", "Your password has been updated successfuly. We'll redirect you to the login page now."
                            }
                        });
                    }
                    else
                    {
                        // Get the errors and let the client know
                        return new JsonResult(new Dictionary<string, string>(){
                            {
                                "error", ModelState.ValidationState.ToString()
                            }
                        });
                    }
                }
                else
                {
                    return new JsonResult(new Dictionary<string, string>()
                    {
                        {
                            "error", "This recovery link is invalid. Please try recovering your account again."
                        }
                    });
                }
            }
            else
            {
                return new JsonResult(new Dictionary<string, string>()
                {
                    {
                        "error", "The password and password confirmation provided do not match. Please check your passwords and try again."
                    }
                });
            }
        }catch(Exception e)
        {
            Console.WriteLine("Found an error while updating a user account password from a recovery link code.");
            Console.WriteLine(e.Message);
            return new JsonResult(new Dictionary<string, string>()
            {
                {
                    "error", e.Message
                }
            });
        }
    }
}


