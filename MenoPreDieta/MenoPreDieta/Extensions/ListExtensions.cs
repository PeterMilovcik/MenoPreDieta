﻿using System;
using System.Collections.Generic;

namespace MenoPreDieta.Extensions
{
    public static class ListExtensions
    {
        private static readonly Random random = new Random((int) DateTime.Now.Ticks);

        public static T RandomItem<T>(this IList<T> list) => list[random.Next(list.Count - 1)];
    }
}