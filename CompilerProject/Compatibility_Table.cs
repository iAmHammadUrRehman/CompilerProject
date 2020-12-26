using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerProject
{
    class Compatibility_Table
    {
        string left_type;
        string right_type;
        string oper;
        string return_type;

        public Compatibility_Table()
        {
            
        }
        public  Compatibility_Table(string left_type, string right_type,string oper,string return_type)
        {
            this.left_type = left_type;
            this.right_type = right_type;
            this.oper = oper;
            this.return_type = return_type;
        }
        public string compatibility(string left_type, string right_type, string oper)
        {
            try
            {
                
               // Console.WriteLine("\t\t" + left_type + " -> " + right_type + " " + oper);
                foreach (var item in CompilerProject.comp)
                {
                    
                    if (item.left_type == left_type && item.right_type == right_type && item.oper == oper)
                    {
                     //   Console.WriteLine(left_type + right_type + oper);
                       /// Console.WriteLine(item.return_type + "here");
                        return item.return_type;

                    }

                }
            }
            catch (Exception)
            {
                
                Console.WriteLine("IN compatibility exception");
            }

            return "null";
        }
    }
}
