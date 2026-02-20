using System;

namespace IdentityCore.Enums;

[Flags]
public enum Role
{
    Admin = 1 << 0,
    User = 1 << 1
}