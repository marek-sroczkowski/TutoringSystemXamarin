﻿using System;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.ReservationDtos
{
    public class ReservationDto
    {
        public long Id { get; set; }
        public double Cost { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public ReservationPlace Place { get; set; }
        public string SubjectName { get; set; }
        public string Tutor { get; set; }
        public string Student { get; set; }
    }
}
