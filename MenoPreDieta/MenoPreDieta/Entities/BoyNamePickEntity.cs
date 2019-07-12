﻿using SQLite;

namespace MenoPreDieta.Entities
{
    public class BoyNamePickEntity : INamePickEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int FirstNameId { get; set; }
        public int SecondNameId { get; set; }
        public int PickedNameId { get; set; }
        public bool IsNamePicked { get; set; }
    }
}