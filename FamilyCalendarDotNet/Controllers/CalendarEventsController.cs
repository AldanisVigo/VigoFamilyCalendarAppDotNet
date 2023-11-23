using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Microsoft.AspNetCore.Authorization;

namespace FamilyCalendarDotNet.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class CalendarEventsController : Controller
{
    // This is the MySQL connection that will get injected
    readonly MySqlConnection connection;

    // This is the constructor with the injected MySQL connection
    public CalendarEventsController(MySqlConnection conn)
    {
        // Set the readonly connection
        connection = conn;
    }

    // This is the destructor
    ~CalendarEventsController()
    {
        // Close the MySQL connection
        connection.Close();
    }

    /*
     *  This is a GET mapping for /GetCalendarEvents
     */
    [HttpGet(Name = "GetCalendarEvents")]
    public async Task<JsonResult> GetEvents()
    {
        try
        {
            // Validate the API key
                
            // Open the MySQL connection asynchronously
            await connection.OpenAsync();

            // Create a MySQL command for selecting all events
            using var command = new MySqlCommand("SELECT * FROM events;", connection);

            // Execute the MySQL command and read the results
            using var reader = await command.ExecuteReaderAsync();

            // Create a list of results
            List<CalendarEvent> results = new();

            // Log reader
            while (reader.Read())
            {
                var date = reader.GetValue(1);
                var content = reader.GetValue(2);
                string rowStr = "Date: " + date + " Content:" + content;
                Console.WriteLine(rowStr);
                results.Add(new CalendarEvent(DateTime.Parse(date.ToString() ?? ""), content.ToString() ?? ""));
            }

            // Return the results as JSON
            return new JsonResult(results);
        }catch(Exception err)
        {
            Console.WriteLine("There was an error while retrieving the calendar events.");
            Console.WriteLine(err.Message);
            return new JsonResult(new Dictionary<string, string>(){
                {
                    "error", err.Message
                }
            });
        }
    }

    /*
     *  This is a GET mapping for /GetCalendarEvent
     */
    [HttpGet]
    [Route("GetEvent")]
    public async Task<JsonResult> GetEvent(int id)
    {
        try
        {
            // Open the MySQL connection asynchronously
            await connection.OpenAsync();

            // Create a MySQL command for selecting all events
            using MySqlCommand command = new()
            {
                CommandText = "SELECT * FROM events WHERE id=?id LIMIT 1;",
                Connection = connection
            };

            command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;

            // Execute the MySQL command and read the results
            using var reader = await command.ExecuteReaderAsync();

            // Create a list of results
            List<CalendarEvent> results = new();

            // Log reader
            while (reader.Read())
            {
                var date = reader.GetValue(1);
                var content = reader.GetValue(2);
                string rowStr = "Date: " + date + " Content:" + content;
                Console.WriteLine(rowStr);
                results.Add(new CalendarEvent(DateTime.Parse(date.ToString() ?? ""), content.ToString() ?? ""));
            }

            // Return the results as JSON
            return new JsonResult(results);
        }
        catch (Exception err)
        {
            Console.WriteLine("There was an error while retrieving the calendar events.");
            Console.WriteLine(err.Message);
            return new JsonResult(new Dictionary<string, string>(){
                {
                    "error", err.Message
                }
            });
        }
    }

    /*
     * This is a POST mapping for /AddCalendarEvent
     */
    [HttpPost(Name = "AddCalendarEvent")]
    public async Task<JsonResult> AddEvent(DateTime eventTime, string eventContent)
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
                CommandText = "INSERT INTO events(timestamp,eventContent) VALUES(?timestamp,?content)"
            };

            // Replace the parameters with the incoming data
            cmd.Parameters.Add("?timestamp", MySqlDbType.DateTime).Value = eventTime;
            cmd.Parameters.Add("?content", MySqlDbType.VarChar).Value = eventContent;

            // Wait for the query to be executed
            await cmd.ExecuteNonQueryAsync();

            // Let the user know the event was added.
            return new JsonResult(new Dictionary<string, string>(){
                {
                    "success" , "The event has been added."
                }
            });
        }
        catch (Exception err)
        {
            Console.WriteLine("There was an error while attempting to add an event.");
            Console.WriteLine(err.Message);
            return new JsonResult(new Dictionary<string, string>() {
                {
                    "error" , err.Message
                }
            });
        }
    }

    /*
     * This is a DELETE mapping for /deleteCalendarEvent
     */
    [HttpDelete(Name = "DeleteCalendarEvent")]
    public async Task<JsonResult> DeleteEvent(int id)
    {
        try
        {
            await connection.OpenAsync();

            MySqlCommand cmd = new()
            {
                Connection = connection,
                CommandText = "DELETE FROM events WHERE id=?id"
            };

            cmd.Parameters.Add("?id", MySqlDbType.Int32).Value = id;

            await cmd.ExecuteNonQueryAsync();

            return new JsonResult(new Dictionary<string, string>(){
                {
                    "success", "Event has been deleted"
                }
            });
        }catch(Exception err)
        {
            Console.WriteLine("There was an error while deleting the event with id " + id);
            Console.WriteLine(err.Message);
            return new JsonResult(new Dictionary<string, string>(){
                {
                    "error", err.Message
                }
            });
        }
    }
}