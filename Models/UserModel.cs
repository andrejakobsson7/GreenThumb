﻿using EntityFrameworkCore.EncryptColumn.Attribute;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenThumb.Models
{
    public class UserModel
    {
        [Key]
        [Column("id")]
        public int UserId { get; set; }

        [Column("username")]
        public string Username { get; set; } = null!;

        [Column("password")]
        [EncryptColumn]
        public string Password { get; set; } = null!;

        //Navigation property
        public GardenModel? Garden { get; set; }

        public UserModel(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
