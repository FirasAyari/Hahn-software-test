﻿using System;

namespace Application.Core.Dtos;

public class ApiEntryDto
{
    public string API { get; set; }
    public string Description { get; set; }
    public string Auth { get; set; }
    public bool HTTPS { get; set; }
    public string Cors { get; set; }
    public string Link { get; set; }
    public string Category { get; set; }
}