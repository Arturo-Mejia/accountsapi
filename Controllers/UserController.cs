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
                var users = _db.Users.Where(U => U.Email == acc.Email).ToList();
                if (users.Count > 0)
                {
                    return new JsonResult(new { Message = "El email ya se encuentra registrado, por favor ingrese uno diferente"}) 
                    {
                        StatusCode = 400
                    };
                }
                else 
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
        public async Task<IActionResult> deleteUser(string idu)
        {   
            try
            {
                int id = SecurityPass.getIdUser(idu);
                if (id != -1)
                {
                    var user = _db.Users.SingleOrDefault(x => x.Id == id);
                    if (user != null)
                    {
                        //eliminar todas las cuentas del usuario
                        var accounts = _db.Accounts.Where(x => x.Iduser == id);
                        foreach (var acc in accounts) 
                        {
                            _db.Accounts.Remove(acc);
                        }

                        _db.Users.Remove(user);
                        _db.SaveChanges();

                        return StatusCode(StatusCodes.Status200OK, new { message = "Eliminado correctamente" });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontró al usuario" });
                    }
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
        [Route("getUser")]
        public async Task<IActionResult> getUser(string idu)
        {
            int id = SecurityPass.getIdUser(idu);
            if (id != -1)
            {
                var user = _db.Users.Find(id);
                if (user != null)
                {
                    var obj = new { Email = user.Email, Username = user.Username,
                        Password = SecurityPass.Decrypt(user.Password), Createdadate = user.CreatedDate }; 
                    return new JsonResult(obj)
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
            else 
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontró al usuario" });
            }
             
        }

   

        [HttpPost]
        [Route("login")]
        public  async Task<IActionResult> login(string email,string pass) 
        {
            var user = _db.Users.Where(u => u.Email == email).ToList();
            if (user.Count > 0)
            {
                string password = SecurityPass.Decrypt(user.First().Password);
                if (password.Equals(pass))
                {
                    string idu = SecurityPass.genIduser(user.First().Id);
                    return new JsonResult(new { message = "credenciales correctas", iduser = idu  })
                    {
                        StatusCode = 200
                    };
                }
                else 
                {
                    return new JsonResult(new { message = "credenciales incorrectas" })
                    {
                        StatusCode = 404
                    };
                }
               
            }
            else 
            {
                return new JsonResult(new { message = "credenciales incorrectas" })
                {
                    StatusCode = 404
                };
            }
            
        }


        [HttpPut]
        [Route("updatepassword")]
        public async Task<IActionResult> updatePassword(string idu, string newpass) 
        {
            try
            {
                int id = SecurityPass.getIdUser(idu);
                if (id != -1)
                {
                    var user = _db.Users.Find(id);
                    user.Password = SecurityPass.Encrypt(newpass);
                    _db.Users.Update(user);
                    _db.SaveChanges(); 
                    return StatusCode(StatusCodes.Status200OK, new { message = "Actualizado correctamente" });
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


    }
}
