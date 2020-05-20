using Microsoft.EntityFrameworkCore;
using HourglassServer.Models.Persistent;
using System;
using System.ComponentModel.DataAnnotations;

namespace HourglassServer.Custom.Upload
{
    public class CovidUnemploymentUploadModel
    {
        [Required]
        CovidUnemployment[] CovidUnemployment;
    }
}
