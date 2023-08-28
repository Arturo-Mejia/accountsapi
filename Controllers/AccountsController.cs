using apitest.Data;
using apitest.Model;
using Microsoft.AspNetCore.Mvc;

namespace apitest.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public async Task<IActionResult> delete(string idu, int idaccount) 
        {
            try
            {
                int id = SecurityPass.getIdUser(idu);
                if (id != -1)
                {
                    var acc = _db.Accounts.FirstOrDefault(x => x.Id == idaccount && x.Iduser==id);
                    if (acc != null)
                    {
                        _db.Accounts.Remove(acc);
                        _db.SaveChanges();
                        return StatusCode(StatusCodes.Status200OK, new { message = "Eliminado correctamente" });
                    }
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { message = "Cuenta no encontrada" });
                    }
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
                        List<EditAccount> list = new List<EditAccount>();
                        foreach (var acc in accounts) 
                        {
                            EditAccount ea = new EditAccount();
                            ea.id = acc.Id;
                            ea.Iduser = idu;
                            ea.descripcion = acc.Account1;
                            ea.useraccount = acc.useracc; 
                            ea.Pass = SecurityPass.Decrypt(acc.Pass);
                            list.Add(ea);
                        }
                        return StatusCode(StatusCodes.Status200OK, list);
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
        public async Task<IActionResult> getAccount(string idu, int idaccount)
        {
            try
            {
                int id = SecurityPass.getIdUser(idu);
                if (id != -1)
                {
                    var acc = _db.Accounts.Find(idaccount);
                    if (acc == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontraron datos" });
                    }
                    else
                    {

                        if (acc.Iduser == id)
                        {
                            EditAccount ea = new EditAccount();
                            ea.id = acc.Id;
                            ea.Iduser = idu;
                            ea.descripcion = acc.Account1;
                            ea.useraccount = acc.useracc;
                            ea.Pass = SecurityPass.Decrypt(acc.Pass);
                            return StatusCode(StatusCodes.Status200OK, ea);
                        }
                        else 
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, new { Message = "La cuenta no pertenece al usuario" });
                        }
                        
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

        [HttpPut]
        [Route("UpdateAccount")]
        public async Task<IActionResult>updateAccount([FromBody]EditAccount account) 
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

    }
}
