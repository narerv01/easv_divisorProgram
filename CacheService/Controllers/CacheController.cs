using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;
using Dapper;


namespace CacheService.Controllers;

[ApiController]
[Route("[controller]")]
public class CacheController : ControllerBase
{
    private static IDbConnection divisorCache = new MySqlConnection("Server=cache-db;Database=cache-database;Uid=div-cache;Pwd=C@ch3d1v;");

    [HttpGet]
    public int Get(long number)
    {
        return divisorCache.QueryFirstOrDefault<int>("SELECT divisors FROM counters WHERE number = @number", new { number = number });
    }

    [HttpPost]
    public void Post([FromQuery] long number, [FromQuery] int divisorCounter)
    {
        divisorCache.Execute("INSERT INTO counters (number, divisors) VALUES (@number, @divisors)", new { number = number, divisors = divisorCounter });
    }
    public List<string> Tables()
    {
        return (List<string>)divisorCache.Query<string>("SHOW TABLES LIKE 'counters'");

    }
    public void CreateTable()
    {
        divisorCache.Execute("CREATE TABLE counters (number BIGINT NOT NULL PRIMARY KEY, divisors INT NOT NULL)");
    }
    public void Open()
    {
        divisorCache.Open();
    }

    public void Close()
    {
        divisorCache.Close();
    }
}