using apitest.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apitest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        AmhapiPasswordsContext _db = new AmhapiPasswordsContext();


        [HttpPost]
        [Route("create")]
        public JsonResult create([FromBody] createUser acc) 
        {
            User useracc = new User();
            useracc.Email = acc.Email;
            useracc.Username = acc.Username;
            useracc.Password = acc.Password;
            _db.Users.Add(useracc);
            _db.SaveChanges();

            try 
            {
                return new JsonResult(new { message = "Registrado correctamente" })
                {
                    StatusCode = 200
                };
            }
            catch (Exception ex) 
            {
                return new JsonResult(new { message = ex.Message })
                {
                    StatusCode = 200
                };
            }

            
        }

        
    }
}
