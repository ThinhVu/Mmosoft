using System;
using System.Collections.Generic;
using System.Linq;

namespace Mmosoft.Utils
{
    public static class EnumParser
    {
        public static List<TEnum> Parse<TEnum>(TEnum value)
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("Not an enum");           
            int enumValue = Convert.ToInt32(value);
            List<int> enumValues = new List<int>();
            List<int> availableEnumValues = Enum.GetValues(typeof(TEnum)).Cast<int>().ToList();
            availableEnumValues.Sort();
            availableEnumValues.Reverse();
            for (int i = 0; i < availableEnumValues.Count; i++)
            {
                if (enumValue >= availableEnumValues[i])
                {
                    enumValues.Add(availableEnumValues[i]);
                    enumValue -= availableEnumValues[i];
                }
            }
            List<TEnum> tEnums = new List<TEnum>();
            foreach (var item in enumValues)
	        {
		        tEnums.Add((TEnum) Enum.Parse(typeof(TEnum), item.ToString()));
	        }
            return tEnums;
        }
    }
}
