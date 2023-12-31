﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CashFlow.Models;

public class BankAccount : Entity
{
    public int Id { get; set; }
    [Column(TypeName = "nvarchar(50)")]
    public BankAccountType Type { get; set; }

    public string Name { get; set; } = string.Empty;// Name should be created as user surname+bankaccountid+type
    [Column(TypeName = "decimal(18, 2)")]
    public double Balance { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public double CreditBalance { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public BankAccount()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = CreatedAt;
    }
    public int UserId { get; set; }
    public User? User { get; set; }
}