﻿namespace TutoringSystemMobile.Models.Dtos.Student
{
    public class StudentDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlRate { get; set; }
        public string Note { get; set; }
        public string ProfilePictureFirebaseUrl { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({Username})";
        }
    }
}