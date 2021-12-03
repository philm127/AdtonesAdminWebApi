using AdtonesAdminWebApi.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services
{
    public class ProfileMatchRetrivalService
    {
        public int GetProfileMatchValue(string ch)
        {
            int data = 0;
            switch (ch)
            {
                case "A":
                    data = 0;
                    break;
                case "B":
                    data = 1;
                    break;
                case "C":
                    data = 2;
                    break;
                case "D":
                    data = 3;
                    break;
                case "E":
                    data = 4;
                    break;
                case "F":
                    data = 5;
                    break;
                case "G":
                    data = 6;
                    break;
                case "H":
                    data = 7;
                    break;
                case "I":
                    data = 8;
                    break;
                case "J":
                    data = 9;
                    break;
                case "K":
                    data = 10;
                    break;
                case "L":
                    data = 11;
                    break;
                case "M":
                    data = 12;
                    break;
                case "N":
                    data = 13;
                    break;
                case "O":
                    data = 14;
                    break;
                case "P":
                    data = 15;
                    break;
                case "Q":
                    data = 16;
                    break;
                case "R":
                    data = 17;
                    break;
                case "S":
                    data = 18;
                    break;
                case "T":
                    data = 19;
                    break;
                case "U":
                    data = 20;
                    break;
                case "V":
                    data = 21;
                    break;
                case "W":
                    data = 22;
                    break;
                case "X":
                    data = 23;
                    break;
                case "Y":
                    data = 24;
                    break;
                case "Z":
                    data = 25;
                    break;
            }
            return data;
        }
    }
}