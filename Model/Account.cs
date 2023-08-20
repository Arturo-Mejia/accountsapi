using System;
using System.Collections.Generic;

namespace apitest.Model;

public partial class Account
{
    public int Id { get; set; }

    public int? Iduser { get; set; }

    public string? Account1 { get; set; }

    public string? useracc {get; set;}

    public string? Pass { get; set; }

    public DateTime? Created { get; set; }
}


public class CreateAccount
{
    public int? Iduser { get; set; }
    public string? descripcion { get; set; }

    public string? useraccount { get; set; }

    public string? Pass { get; set; }

}