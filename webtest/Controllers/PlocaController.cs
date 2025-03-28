using System.Collections.Generic;
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace webtest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlocaController : ControllerBase
    {
        private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=!popara02;Database=testapp";
        private readonly ILogger<PlocaController> logger;

        public PlocaController(ILogger<PlocaController> logger)
        {
            this.logger = logger;
        }

        [HttpGet(Name ="GetPloce")]
        public IActionResult Get()
        {
            int id;
            string naziv;
            string izvodjac;
            string zanr;
            int trajanje;
            Ploca? p =null;
            List<Ploca> ploce = new List<Ploca>();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connected to the PostgreSQL database!");

                    using (var command = new NpgsqlCommand("SELECT * FROM public.ploce", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                id = reader.GetInt32(reader.GetOrdinal("id"));
                                naziv = reader.GetString(reader.GetOrdinal("naziv"));
                                izvodjac = reader.GetString(reader.GetOrdinal("izvodjac"));
                                zanr = reader.GetString(reader.GetOrdinal("zanr"));
                                int tip_br= reader.GetInt32(reader.GetOrdinal("tip"));
                                trajanje = reader.GetInt32(reader.GetOrdinal("trajanje"));

                                p = new Ploca(id, naziv, izvodjac, zanr, (Tipovi)tip_br, trajanje);
                                ploce.Add(p);
                            }
                        }
                    }
                    return Ok(ploce);
                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return Conflict(ex.Message);
                }
            }

        }
        [HttpPost(Name ="PostPloca")]
        public IActionResult Post([FromBody] Ploca p)
        {
           
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connected to the PostgreSQL database!");

                    string cmd_txt = string.Format("INSERT INTO public.ploce (naziv,izvodjac,zanr,tip,trajanje) VALUES ('{0}','{1}','{2}', {3},{4})", p.naziv,p.izvodjac,p.zanr,(int)p.tip,p.trajanje);
                    var command = new NpgsqlCommand(cmd_txt, connection);
                    command.ExecuteNonQuery();
                    return Ok(p);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return Conflict(ex.Message);
                }
            }
        }

        [HttpPut(Name = "PutPloca")]

        public IActionResult Put([FromBody] Ploca p)
        {
            using (var connection=new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string cmd_txt = string.Format("UPDATE public.ploce SET naziv='{0}',izvodjac='{1}',tip={2},zanr='{3}',trajanje={4} WHERE id={5}",p.naziv,p.izvodjac,(int)p.tip,p.zanr,p.trajanje,p.id);
                    var command = new NpgsqlCommand(cmd_txt, connection);
                    int aff = command.ExecuteNonQuery();
                    return Ok(aff);
                }
                catch(Exception ex)
                {
                    return Conflict(ex.Message);
                }
            }
        }

        [HttpDelete(Name ="DeletePloca")]
        public IActionResult Delete([FromBody] int id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string cmd_txt = string.Format("DELETE FROM public.ploce WHERE id={0}", id);
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

        [HttpGet("SearchPloca/{term}")]
        public IActionResult Search([FromRoute] string term)
        {
            List<Ploca> ploce= new List<Ploca>();
            Ploca? p = null;
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string cmd_txt = $"select * from public.ploce where naziv ilike '%{term}%' or izvodjac ilike '%{term}%' or zanr ilike '%{term}%';";
                    using(var command=new NpgsqlCommand(cmd_txt, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read()) {
                                int r_id = reader.GetInt32(reader.GetOrdinal("id"));
                                string r_naziv = reader.GetString(reader.GetOrdinal("naziv"));
                                string r_izvodjac = reader.GetString(reader.GetOrdinal("izvodjac"));
                                string r_zanr = reader.GetString(reader.GetOrdinal("zanr"));
                                int tip_br = reader.GetInt32(reader.GetOrdinal("tip"));
                                int r_trajanje = reader.GetInt32(reader.GetOrdinal("trajanje"));

                                p = new Ploca(r_id, r_naziv, r_izvodjac, r_zanr, (Tipovi)tip_br, r_trajanje);
                                ploce.Add(p);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Conflict(ex.Message);
                }
            }
            return Ok(ploce);
        }

    }
}
