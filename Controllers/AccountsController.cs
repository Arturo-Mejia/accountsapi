using apitest.Data;
using apitest.Model;
using Microsoft.AspNetCore.Mvc;

namespace apitest.Controllers
{
    public class AccountsController : ControllerBase
    {
        AmhapiPasswordsContext _db = new AmhapiPasswordsContext();

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> create([FromBody] CreateAccount account)
        { 
            try 
            {
                int id = SecurityPass.getIdUser(account.Iduser);
                if (id != -1)
                {
                    Account acc = new Account();
                    acc.Account1 = account.descripcion;
                    acc.Iduser = id;
                    acc.Pass = SecurityPass.Encrypt(account.Pass);
                    acc.useracc = account.useraccount;
                    _db.Accounts.Add(acc);
                    _db.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK, new { message = "Guardado correctamente" });
                }
                else 
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontró al usuario" });
                }
                   
            }catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> delete(int id) 
        {
            try 
            {      
                var acc = _db.Accounts.FirstOrDefault(x => x.Id == id);
                if (acc != null) 
                {
                    _db.Accounts.Remove(acc);
                    _db.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK, new { message = "Eliminado correctamente" });
                }
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "Cuenta no encontrada" });
                }
                 
            }catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("getAccounts")]
        public async Task<IActionResult> getAccounts(string idu)
        {
            try
            {
                int id = SecurityPass.getIdUser(idu);
                if (id != -1)
                {
                    var accounts = _db.Accounts.Where(acc => acc.Iduser == id).ToList();
                    if (accounts.Count > 0)
                    {
                        return StatusCode(StatusCodes.Status200OK, accounts);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontraron registros" });
                    }

                }
                else 
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontró al usuario" });
                }
                  
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("getAccount")]
        public async Task<IActionResult> getAccount(int id)
        {
            try
            {
                var acc = _db.Accounts.Find(id);
                if (acc == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontraron datos" });
                }
                else 
                {
                    return StatusCode(StatusCodes.Status200OK, acc);
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("UpdateAccount")]
        public async Task<IActionResult>updateAccount(EditAccount account) 
        {
            int id = SecurityPass.getIdUser(account.Iduser);
            if (id != -1)
            {
                var acc = _db.Accounts.Find(account.id);
                if (acc != null)
                {
                    acc.Account1 = account.descripcion;
                    acc.Iduser = id;
                    acc.Pass = SecurityPass.Encrypt(account.Pass);
                    acc.useracc = account.useraccount;
                    _db.Accounts.Add(acc);
                    _db.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK, new { message = "Actualizado correctamente" });
                }
                else 
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontró la cuenta" });
                }
            }
            else 
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontró al usuario" });
            }
              
        }

        [HttpGet]
        [Route("decryptpasswordaccount")]
        public async Task<IActionResult> decryptPasswordAccount(string password) 
        {
            try
            {
                string pass = SecurityPass.Decrypt(password); 
                return StatusCode(StatusCodes.Status200OK, new { password = pass });
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
