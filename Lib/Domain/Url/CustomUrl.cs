﻿using System.ComponentModel.DataAnnotations;
using MyPersonalShortner.Lib.Domain.CustomValidators;

namespace MyPersonalShortner.Lib.Domain.Url
{
    public class CustomUrl
    {
        public int Id { get; set; }
        [Required]
        [Url(ErrorMessage = "The url is invalid.")]
        public string Url { get; set; }
        [Required]
        [MaxLength(10)]
        public string CustomPart { get; set; }
    }
}