using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace webtest.Controllers
   
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=!popara02;Database=testapp";

        [HttpPost(Name = "PostUserTarget")]
        public IActionResult Post([FromBody] User user)
        {
            UserVM? userVM = null;
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connected to the PostgreSQL database!");

                    string cmd_txt = $"SELECT * FROM public.users WHERE username='{user.username}' AND password='{user.password}';";
                    var command = new NpgsqlCommand(cmd_txt, connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetInt32(reader.GetOrdinal("id"));
                            string name = reader.GetString(reader.GetOrdinal("name"));
                            string surname = reader.GetString(reader.GetOrdinal("surname"));
                            string username = reader.GetString(reader.GetOrdinal("username"));
                            userVM = new UserVM(id, name, surname, username);
                        }
                    }
                    return Ok(userVM);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return Conflict(ex.Message);
                }
            }

        }
        [HttpPost("PostUser")]
        public IActionResult Post([FromBody] UserVM user) {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try {
                    connection.Open();
                    string cmd_txt = string.Format("INSERT INTO public.users (name,surname,username) VALUES ('{0}','{1}','{2}');",user.name,user.surname,user.username);
                    var command=new NpgsqlCommand(cmd_txt,connection);
                    int aff=command.ExecuteNonQuery();
                    return Ok(aff);
                }
                catch(Exception ex){
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return Conflict(ex.Message);
                }
            }
        }

        [HttpPut(Name = "PutUser")]
        public IActionResult Put([FromBody] UserVM user) {
            using (var connection = new NpgsqlConnection(connectionString)) {
                try
                {
                    connection.Open();
                    string cmd_txt = string.Format("UPDATE public.users SET name='{0}',surname='{1}',username='{2}' WHERE id={3}",user.name,user.surname,user.username,user.id);
                    var command = new NpgsqlCommand(cmd_txt, connection);
                    int aff = command.ExecuteNonQuery();
                    return Ok(aff);
                }
                catch (Exception ex)
                {
                    return Conflict(ex.Message);
                }
            }
        }
        [HttpGet(Name = "GetUser")]
        public IActionResult Get()
        {
            UserVM? user= null;
            List<UserVM> users = new List<UserVM>();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try {
                    connection.Open();
                    string cmd_text = "SELECT * FROM public.users;";
                    var command= new NpgsqlCommand(cmd_text, connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetInt32(reader.GetOrdinal("id"));
                            string name = reader.GetString(reader.GetOrdinal("name"));
                            string surname = reader.GetString(reader.GetOrdinal("surname"));
                            string username = reader.GetString(reader.GetOrdinal("username"));
                            user = new UserVM(id, name, surname, username);
                            users.Add(user);
                        }
                    }
                    return Ok(users);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return Conflict(ex.Message);
                }
            }
        }

        [HttpDelete(Name = "DeleteUser")]
        public IActionResult Delete([FromBody] int id) {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try {
                    connection.Open();
                    string cmd_txt = $"DELETE FROM public.users WHERE id={id}";
                    var command=new NpgsqlCommand(cmd_txt, connection);
                    int aff=command.ExecuteNonQuery();
                    return Ok(aff);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return Conflict(ex.Message);
                }
            }
        }
    
        
    }

}
