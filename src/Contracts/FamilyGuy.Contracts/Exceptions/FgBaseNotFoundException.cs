﻿using System;

namespace FamilyGuy.Contracts.Exceptions
{
    public class FgBaseNotFoundException : Exception
    {
        public FgBaseNotFoundException(string message) : base(message)
        {
        }
    }
}
