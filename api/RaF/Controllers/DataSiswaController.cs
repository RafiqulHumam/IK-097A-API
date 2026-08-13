using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

namespace RaF.Controllers;

[Controller]
[Route("api/[controller]/[action]")]
public class DataSiswaController : ControllerBase
{
    public class DataUser
    {
        public string? userid { get; set; }
        public string? namauser { get; set; }
        public string? pass { get; set; }
    }
    public class APIResult
    {
        public string? code { get; set; }
        public string? status { get; set; }
        public string? message { get; set; }
        public string? data { get; set; }
    }


    public class DataPayload
    {
        public string? userid { get; set; }
        public string? pass { get; set; }
    }


    public List<DataPayload> lstpayload = new List<DataPayload>();
    public List<APIResult> listapiresult = new List<APIResult>();
    public List<DataUser> listuser = new List<DataUser>();

    [HttpPost]
    public IActionResult Login([FromForm] string payload)
    {

        lstpayload.Add(JsonSerializer.Deserialize<DataPayload>(payload));
        foreach (var item in lstpayload)
        {
            if (item.userid == "001" && item.pass == "123")
            {
                listapiresult.Add(new APIResult { code = "200", status = "Success", message = "Logged In", data = "Andi" });
                return Ok(listapiresult);
            }
            else
            {
                listapiresult.Add(new APIResult { code = "401", status = "Unauthorized", message = "Failed", data = "null" });
                return Unauthorized(listapiresult);
            }
        }
        return BadRequest();
    }

    [HttpPost]
    public IActionResult Users([FromForm] string payload)
    {
        lstpayload.Add(JsonSerializer.Deserialize<DataPayload>(payload));

        listuser.Add(new DataUser { userid = "001", namauser = "Rafiq", pass = "123" });
        listuser.Add(new DataUser { userid = "002", namauser = "Rafiqul Humam", pass = "1234" });
        string? res = null;
        foreach (var item in listuser)
        {
            if (item.userid == lstpayload[0].userid && item.pass == lstpayload[0].pass)
            {
                res = item.namauser;
                Console.WriteLine("Login Berhasil");
                break;
            }
        }

        return Ok(listuser);
    }

    [HttpPost]
    public IActionResult quiz([FromForm] string payload)
    {
        if (string.IsNullOrEmpty(payload))
        {
            return BadRequest(new APIResult
            {
                code = "400",
                status = "Bad Request",
                message = "Failed",
                data = "null"
            });
        }

        try
        {
            var dataPayload = JsonSerializer.Deserialize<DataPayload>(payload);
            if (dataPayload == null)
            {
                return BadRequest(new APIResult
                {
                    code = "400",
                    status = "Bad Request",
                    message = "Payload tidak valid",
                    data = "null"
                });
            }

            var users = new List<DataUser>
            {
                new DataUser { userid = "001", namauser = "Rafiq", pass = "123" },
                new DataUser { userid = "002", namauser = "Rafiqul Humam", pass = "1234"},
            };

            var user = users.FirstOrDefault(u => u.userid == dataPayload.userid && u.pass == dataPayload.pass);

            if (user != null)
            {
                Console.WriteLine("Login Berhasil");
                return Ok(new APIResult
                {
                    code = "200",
                    status = "Success",
                    message = "Logged In",
                    data = user.namauser
                });
            }

            return Unauthorized(new APIResult
            {
                code = "401",
                status = "Unauthorized",
                message = "Failed",
                data = null
            });

        }
        catch (JsonException)
        {
            return BadRequest(new APIResult
            {
                code = "400",
                status = "Bad Request",
                message = "Format Json Tidak Valid",
                data = null
            });
        }
    }
}