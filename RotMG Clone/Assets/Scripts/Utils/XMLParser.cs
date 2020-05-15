using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using Color = System.Drawing.Color;

namespace Utils
{
    public static class XmlParser
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> input, Action<T> action)
        {
            foreach (T element in input) action(element);
            return input;
        }

        public static bool HasOwnProperty(this XElement element, XName name)
        {
            return element.Elements(name).Any();
        }
        
        public static string AttrDefault(this XElement element, XName name, string @default)
        {
            return element.Attributes(name).Any() ? element.Attribute(name).Value : @default;
        }

        public static string ElemDefault(this XElement element, XName name, string @default)
        {
            return element.Elements(name).Any() ? element.Element(name).Value : @default;
        }

        public static int ParseHex(this string input)
        {
            try
            {
                return Convert.ToInt32(input, 16);
            }
            catch (Exception e)
            {
                Debug.LogError("[Hex] Could not parse hex from string : '" + input + "'. \n " + e);
                throw;
            }
        }

        public static Color ParseHexColor(this string input)
        {
            try
            {
                Color c = Color.FromArgb(int.Parse(input.Replace("0x", ""), NumberStyles.HexNumber));
                c = Color.FromArgb(255, c.R, c.G, c.B);
                return c;
            }
            catch (Exception e)
            {
                Debug.LogError("Could not parse color from string : '" + input + "'. \n " + e);
                throw;
            }
        }

        public static int ParseInt(this string input)
        {
            try
            {
                return int.Parse(input, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                Debug.LogError("Could not parse int from string : '" + input + "'. \n " + e);
                throw;
            }
        }

        public static float ParseFloat(this string input)
        {
            try
            {
                return float.Parse(input, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                Debug.LogError("Could not parse float from string : '" + input + "'. \n " + e);
                throw;
            }
        }
    }
}