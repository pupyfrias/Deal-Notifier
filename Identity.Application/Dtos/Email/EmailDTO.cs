﻿namespace Identity.Application.Dtos.Email
{
    public class EmailDTO
    {
        public string Body { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
    }
}