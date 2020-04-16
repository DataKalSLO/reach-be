﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HourglassServer.Data.Persistent;

namespace HourglassServer.Data
{
    public partial class StoryEntityCreator
    {
        public static void create(EntityTypeBuilder<Story> entity)
        {
            entity.ToTable("story", "public");
        }
    }
}
