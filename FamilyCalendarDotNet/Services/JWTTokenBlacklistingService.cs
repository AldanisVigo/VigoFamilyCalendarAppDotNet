/*
 *  This service allows us to store blacklisted JWT tokens and check wether 
 *  a given token has been added to this list.
 */

namespace FamilyCalendarDotNet;

public class JWTTokenBlacklistingService
{
    /*
        * This is where the blacklisted keys will be stored
        */
    private readonly HashSet<string> _blacklist = new();

    /*
        * Method for adding a token to the blacklist.
        */
    public void AddToBlacklist(string token)
    {
        lock (_blacklist)
        {
            _blacklist.Add(token);
        }
    }

    /*
        *  Method for checking if a token is blacklisted.
        */
    public bool IsTokenBlacklisted(string token)
    {
        lock (_blacklist)
        {
            return _blacklist.Contains(token);
        }
    }
}

