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
                Account acc = new Account();
                acc.Account1 = account.Account;
                acc.Iduser = account.Iduser;
                acc.Pass = account.Pass;
                _db.Accounts.Add(acc);
                _db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message ="Guardado correctamente" });
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
        public async Task<IActionResult> getAccounts(int iduser)
        {
            try
            {
                var accounts = _db.Accounts.Where(acc => acc.Iduser == iduser).ToList();
                if (accounts.Count > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, accounts);
                }
                else 
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontraron registros" });
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
    }
}
