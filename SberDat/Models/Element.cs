using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SberDat.Models
{
    public class Element
    {
        public string name { get; set; }
        public bool required { get; set; }
        public static string[] properties = { "state", "need of investment", "amount of investment", "notes" };
        public static string[] elements = { "facade", "roof", "door & windows", "water connection", "gas", "wiring" };

        public Element(string name, bool required)
        {
            this.name = name;
            this.required = required;
        }

        private static string getFlags(string cellName, bool required)
        {
            string flags = "";
            if (required && !cellName.Equals("notes"))
                flags += "required ";
            if (cellName.Equals("state"))
                flags += "pattern=\"[1234]\"";
            if (cellName.Equals("need of investment"))
                flags += "pattern=\"[1234]\"";
            if (cellName.Equals("amount of investment"))
                flags += "pattern=\"[123]\"";
            if (cellName.Equals("notes"))
                flags += "maxlength=\"1999\"";

            return flags;
        }

        private static string generateCell(Element element, string cellName)
        {
            string cell = "<td>" + cellName + ": </td><td>";
            string name = element.name + "_" + cellName;
            cell += "<input type =\"text\" " + getFlags(cellName, element.required) + " name=" + name + " ";
            cell += "/></td>";
            return cell;
        }

        public static string generateTable(List<Element> elements)
        {
            string table = "<table border=\"1\">";
            
            foreach (Element element in elements)
            {
                table += "<tr>" + "<td colspan=2><b>" + element.name + "</b></td>" + "</tr>";
                foreach (string property in properties)
                {
                    table += "<tr>" + generateCell(element, property) + "</tr>";
                }
            }
            
            table += "</table>";
            return table;
        }
    }
}