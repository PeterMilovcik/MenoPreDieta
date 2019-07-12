﻿using SQLite;

namespace MenoPreDieta.Entities
{
    public class GirlNameEntity : INameEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Value { get; set; }
    }
}