﻿namespace BandManagerPWA.Utils.Models
{
    public class GroupDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<UserDTO>? Users { get; set; }
    }
}
