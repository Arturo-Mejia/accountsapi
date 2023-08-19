using System;
using System.Collections.Generic;

namespace apitest.Model;

public partial class User
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public DateTime? CreatedDate { get; set; } = default(DateTime?);
}

public class createUser 
{
    public string? Email { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

}