﻿namespace CashFlow.Dtos.Authorization;

public class LoginUserDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}