using apitest.Data;
using apitest.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace apitest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        AmhapiPasswordsContext _db = new AmhapiPasswordsContext();


        [HttpPost]
        [Route("create")]
        public async Task<JsonResult> create([FromBody] createUser acc)
        {
            try
            {

                string pass = SecurityPass.Encrypt(acc.Password);
                User useracc = new User();
                useracc.Email = acc.Email;
                useracc.Username = acc.Username;
                useracc.Password = pass;
                useracc.CreatedDate = DateTime.Now;
                _db.Users.Add(useracc);
                _db.SaveChanges();

                return new JsonResult(new { message = "Registrado correctamente" })
                {
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { message = ex.Message })
                {
                    StatusCode = 500
                };
            }


        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> deleteUser(int id)
        {
            try
            {
                var user = _db.Users.SingleOrDefault(x => x.Id == id);
                if (user != null)
                {
                    _db.Users.Remove(user);
                    _db.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { message = "Eliminado correctamente" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontró al usuario" });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex });
            }
        }

        [HttpGet]
        [Route("getUsers")]
        public IActionResult getUsers()
        {
            var users = _db.Users.ToList();

            return new JsonResult(users);
        }


        [HttpGet]
        [Route("getUser")]
        public async Task<IActionResult> getUser(int id)
        {
            var user = _db.Users.Find(id);
            if (user != null)
            {
                return new JsonResult(user)
                {
                    StatusCode = 200
                };
            }
            else
            {
                return new JsonResult(new { message = "Usuario no encontrado" })
                {
                    StatusCode = 404
                };
            }

        }

        [HttpGet]
        [Route("decrypt")]
        public async Task<IActionResult> DecryptPassUser(int id)
        {
            var user = _db.Users.Find(id);
            if (user != null)
            {
                string password = SecurityPass.Decrypt(user.Password);
                return new JsonResult(password)
                {
                    StatusCode = 200
                };
            }
            else
            {
                return new JsonResult(new { message = "Usuario no encontrado" })
                {
                    StatusCode = 404
                };
            }
        }

        [HttpPost]
        [Route("login")]
        public  async Task<IActionResult> login(string email,string pass) 
        {
            var user = _db.Users.Where(u => u.Username == email || u.Email == email).ToList();
            if (user.Count > 0)
            {
                string password = SecurityPass.Decrypt(user.First().Password);
                if (password.Equals(pass))
                {
                    return new JsonResult(new { message = "credenciales correctas" })
                    {
                        StatusCode = 200
                    };
                }
                else 
                {
                    return new JsonResult(new { message = "credenciales correctas" })
                    {
                        StatusCode = 200
                    };
                }
               
            }
            else 
            {
                return new JsonResult(new { message = "credenciales correctas" })
                {
                    StatusCode = 404
                };
            }
            
        }

    }
}
