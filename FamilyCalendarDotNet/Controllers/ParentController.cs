using System.Security.Claims;
using FamilyCalendarDotNet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FamilyCalendarDotNet.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize]
public class ParentController : Controller
{
    [HttpPost]
    [Route("AddChild")]
    public JsonResult AddChild(AddChildDTO child)
    {
        try
        {
            // Make sure the username provided is not taken
            // Make sure the email provided is not taken

            //// Grab the bearer token from the headers of the request
            //var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            //// Open the token to retrieve the userId
            //var currentUser = HttpContext.User;
            //if(currentUser.HasClaim(c => c.Type == "userId")){
            //    Console.WriteLine($"User has a userId claim of: {currentUser.Claims.ToList()[0].Value}");
            //}
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                //string? userId = identity.FindFirst("userId")!.Value;
                // Use the AddChildDTO to retrieve the data for the child account

                IEnumerable<Claim>? v = claims;
                Int32 userId = Int32.Parse(v.First().Value ?? "");
                return new JsonResult(new Dictionary<string, string>()
                {
                    {
                        "success", "You have successfully added a kid to your account."
                    },
                    {
                        "userId" , userId.ToString()
                    }
                });
            }
            else
            {
                return new JsonResult(new Dictionary<string, string>()
                {
                    {
                        "error", "The provided token is missing necessary claims."
                    }
                });
            }
        }
        catch(Exception e)
        {
            Console.WriteLine("Found an error while attempting");
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


