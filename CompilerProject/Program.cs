using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace CompilerProject

{
    public class token
    {
       public string cls_part;
       public string val_part;
       public  int line_no;
        public token(string class_part,string value_part,int Line_no)
        {
            cls_part = class_part;
            val_part = value_part;
            line_no = Line_no;
        }
    }
    class LexicalAnalyzer
    {
       public static List<token> t =new List<token>();
        static int token_ind=0;


        static Dictionary<string, string> keywords = new Dictionary<string, string>();
        static Dictionary<string, string> Operators = new Dictionary<string, string>();
        static Dictionary<string, string> Punctuators = new Dictionary<string, string>();
        static Regex Identifier = new Regex(@"[A-Za-z_][A-Za-z_@0-9]*");
        static Regex Integer = new Regex(@"[0-9]+");
        static Regex Float = new Regex(@"[0-9]*[.][0-9]+");
      //  static Regex String = new Regex("\"[a-zA-Z0-9\\s\\t\\n\"\'\\\\]*\"");
        //static Regex String2 = new Regex("^\"((\\\\[\\\\\a\b\n\v\f\'\"])+([^\\\\\'\"]))*\"$");
        static Regex String = new Regex("^\"" + "[^\\\\\"]*((\\\\[\\\\'\"abfnrtv])*[^\\\\\"\']*(\\\\[\\\\'\"abfnrtv])*[^\\\\\"]*)*" + "\"$");
        static string abc;
        static StringBuilder str;
        static Char ch;
        static int flag = 0;
        static bool word_continue = false;
        static Match match;
        static char j;
        static int i;
        static int NewLineCounter = 1;
        
       

        public LexicalAnalyzer()
        {
             
    
    
            using (StreamWriter write = new StreamWriter(@"C:\Users\justh\Desktop\abc1.wc", true))
             {
                 
                write.Write("\n");
                write.Close();
            }
            using (StreamReader reader = new StreamReader(@"C:\Users\justh\Desktop\abc1.wc", true))
                   {
                       abc = reader.ReadToEnd();
                       reader.Close();
                   }
            //Keywords
            keywords.Add("None", "None");
            keywords.Add("True", "Bool_Const");
            keywords.Add("False", "Bool_Const");
            keywords.Add("Begin", "Begin");
            keywords.Add("Bool", "DT");
            keywords.Add("Char", "DT");
            keywords.Add("Num", "DT");
            keywords.Add("Dec", "DT");
            keywords.Add("string", "DT");
            keywords.Add("Loop", "Loop");
            keywords.Add("Check", "Check");
            keywords.Add("Then", "Then");
            keywords.Add("Default", "Default");
            keywords.Add("return", "return");
            keywords.Add("Enum", "Enum");
            keywords.Add("Continue","Continue");
            keywords.Add("Break", "Break");
            keywords.Add("Create", "Create");
            keywords.Add("Public", "AM");
            keywords.Add("Private", "AM");
        
            keywords.Add("Start", "Start");
            keywords.Add("Class", "Class");

            //Operators
            Operators.Add("+", "PM");
            Operators.Add("-", "PM");
            Operators.Add("*", "MDM");
            Operators.Add("/", "MDM");
            Operators.Add("%", "MDM");
            Operators.Add("=", "=");
            Operators.Add(">", "RO");
            Operators.Add("<", "RO");
            Operators.Add("+=", "ASS_OP");
            Operators.Add("-=", "ASS_OP");
            Operators.Add("*=", "ASS_OP");
            Operators.Add("%=", "ASS_OP");
            Operators.Add("/=", "ASS_OP");
            Operators.Add("++", "++");
            Operators.Add("--", "--");
            Operators.Add("==", "RO");
            Operators.Add("<=", "RO");
            Operators.Add(">=", "RO");
            Operators.Add("&&", "&&");
            Operators.Add("||", "||");
            Operators.Add("!", "!");
            Operators.Add("<-", "Cast Operator");
            Operators.Add("!=", "ASS_OP");
            Operators.Add("|", "|");
            Operators.Add("&", "&");
                      
            

            //Punctuators
            Punctuators.Add("\\","MOP");
            Punctuators.Add("[", "[");
            Punctuators.Add("]", "]");
            Punctuators.Add("{", "{");
            Punctuators.Add("}", "}");
            Punctuators.Add("(", "(");
            Punctuators.Add(")", ")");
            Punctuators.Add(",", ",");
            Punctuators.Add(":", ":");
            Punctuators.Add(".", ".");
       
            Write();
            t.Add(new token("$", "$", NewLineCounter));
            token_ind++;
            display();
            
            syntax S1 = new syntax(t);
        }
        public static void Write()
        {
            
            str = new StringBuilder();
            if (ch == '\n')
            {
                NewLineCounter++;
            }
            for (i = 0; i < abc.Length; i++)
            {
                ch = abc[i];
                             
                if (abc[i] == '/' && abc[i + 1] == '/')
                    flag = 6;
                if (flag == 6)
                {
                    if (ch == '\r')
                    {
                        flag = 0;
                        continue;
                    }
                    continue;
                }

                if (abc[i] == '/' && abc[i + 1] == '*')
                    flag = 7;
                if (flag == 7)
                {
                    if (ch == '*' && abc[i + 1] == '/')
                    {
                        flag = 0;
                        i++;
                        continue;
                    }
                    continue;
                }
               

                if (!word_continue)
                {
                    if (ch == '@' || ch >= 65 && ch <= 90 || ch >= 97 && ch <= 122 || ch == '_')
                    {
                        word_continue = true;
                        flag = 2;

                    }
                    else if (ch >= 48 && ch <= 57)
                    {
                        word_continue = true;
                        flag = 4;
                    }
                    else if(ch=='`'||ch=='^'||ch=='&'||ch=='$'||ch=='#'||ch=='~'||ch=='?'||ch==';')
                    {
                        word_continue = true;
                        flag=1;
                    }
                    else if (ch == '"')
                    {
                        word_continue = true;
                        str.Append(ch);
                        flag = 3;
                        continue;
                    }
                   

               }
                
                if (Operators.ContainsKey(ch.ToString()) || Punctuators.ContainsKey(ch.ToString()) || ch == ' ' ||ch=='\r'|| ch == '\n'||ch=='"'||ch=='\\')
                {
                    switch (flag)
                    {
                        case 1:
                            word_continue = false;
                            InvalidLexeme();
                            break;
                        case 2:
                            word_continue = false;
                            if (keywords.ContainsKey(str.ToString()))
                            {
                                create_token(keywords[str.ToString()]);
                               // Console.WriteLine("(" + keywords[str.ToString()] + "," + str + "," + NewLineCounter + ")");
                                str = new StringBuilder();
                            }
                            else
                            {
                                match = Identifier.Match(str.ToString());
                                if (match.Value == str.ToString())
                                {
                                    create_token("ID");
                                   // Console.WriteLine("(ID" + "," + str + "," + NewLineCounter + ")");
                                    str = new StringBuilder();
                                }
                                else
                                {
                                    InvalidLexeme();
                                }

                            }
                            break;
                            
                        case 3:
                            j = abc[i - 1];
                            if (ch == '"' && j != '\\' || ch == '\r'||ch=='\n'||ch=='"'&&j=='\\'&&abc[i-2]=='\\')
                            {
                                word_continue = false;
                                if(ch=='"')
                                str.Append(ch);
                                match = String.Match(str.ToString());
                                if (match.Value == str.ToString())
                                {
                                    create_token("string_Const");
                                    str = new StringBuilder();
                                }
                                else
                                {
                                    InvalidLexeme();
                                }

                            }
                            else
                            {
                                str.Append(ch);
                                continue;
                            }
                                break;

                        case 4:

                            if (ch.ToString() == ".")
                            {
                                try
                                {
                                    j = abc[i + 1];
                                    if (j >= 48 && j <= 57)
                                    {
                                        str.Append(ch);
                                        flag = 5;
                                        continue;
                                    }
                                }
                                catch (Exception e) { }
                            }
                            word_continue = false;
                            match = Integer.Match(str.ToString());
                            if (match.Value == str.ToString())
                            {
                                create_token("Num_Const");
                                

                                str = new StringBuilder();
                            }
                            else
                            {
                                InvalidLexeme();
                            }
                            break;

                        case 5:
                            word_continue = false;
                            match = Float.Match(str.ToString());
                            if (match.Value == str.ToString())
                            {
                                create_token("Dec_Const");
                              
                                str = new StringBuilder();
                            }
                            else
                            {
                                InvalidLexeme();
                            }

                            break;
                    }//switch
                    flag = 0;
                    if(ch!='"')
                    Word_breaker();

                }//if
                else
                    str.Append(ch);
               
                if (ch == '\n')
                {
                    NewLineCounter++;
                }

            }//for

        }//method

        public static void InvalidLexeme()
        {
            Console.WriteLine("(InvalidLexeme," + str.ToString() + "," + NewLineCounter + ")");
        }

        public static void Word_breaker()
        {
            if (Punctuators.ContainsKey(ch.ToString()))
            {
                str.Append(ch);
                create_token(Punctuators[ch.ToString()]);
                
            }
            else if (ch.ToString() != " " && ch.ToString() != "\"" && ch.ToString() != "\n" && i != abc.Length - 1 && ch != '\r')
            {
                j = abc[i + 1];
                str.Append(ch);
                str.Append(j);
                if (Operators.ContainsKey(str.ToString()))
                {
                  
                   create_token(Operators[str.ToString()]); 
                   i++;
                }
                else
                {
                    
                    str = new StringBuilder();
                    str.Append(ch);
                    create_token(Operators[ch.ToString()]);
                    
                }
            }
            str = new StringBuilder();
        }

        public static void create_token(string x)
        {
            t.Add(new token(x,str.ToString(),NewLineCounter));
            token_ind++;
            
        }
        void display()
        {
            for (int i = 0; i <token_ind; i++)
            {
                 Console.WriteLine("(" + t[i].cls_part + "," + t[i].val_part + "," + t[i].line_no + ")");
            }

        }
    }
    class CompilerProject
    {
        public static List<Compatibility_Table> comp = new List<Compatibility_Table>();
   
        static void Main(string[] args)
        {
            comp_table();
            LexicalAnalyzer l1 = new LexicalAnalyzer();
            
        }

        public static void comp_table()
        {

           // comp = new List<Compatibility_Table>();

           
            //AOP
            comp.Add(new Compatibility_Table("Num", "Num", "+", "Num"));
            comp.Add(new Compatibility_Table("Num", "Num", "-", "Num"));
            comp.Add(new Compatibility_Table("Num", "Num", "*", "Num"));
            comp.Add(new Compatibility_Table("Num", "Num", "/", "Num"));
            comp.Add(new Compatibility_Table("Num", "Num", "%", "Num"));
            
            comp.Add(new Compatibility_Table("Dec", "Dec", "+", "Dec"));
            comp.Add(new Compatibility_Table("Dec", "Dec", "-", "Dec"));
            comp.Add(new Compatibility_Table("Dec", "Dec", "*", "Dec"));
            comp.Add(new Compatibility_Table("Dec", "Dec", "/", "Dec"));
            comp.Add(new Compatibility_Table("Dec", "Dec", "%", "Dec"));

            comp.Add(new Compatibility_Table("Dec", "Num", "+", "Dec"));
            comp.Add(new Compatibility_Table("Dec", "Num", "-", "Dec"));
            comp.Add(new Compatibility_Table("Dec", "Num", "*", "Dec"));
            comp.Add(new Compatibility_Table("Dec", "Num", "/", "Dec"));
            comp.Add(new Compatibility_Table("Dec", "Num", "%", "Dec"));

            comp.Add(new Compatibility_Table("Num", "Dec", "+", "Dec"));
            comp.Add(new Compatibility_Table("Num", "Dec", "-", "Dec"));
            comp.Add(new Compatibility_Table("Num", "Dec", "*", "Dec"));
            comp.Add(new Compatibility_Table("Num", "Dec", "/", "Dec"));
            comp.Add(new Compatibility_Table("Num", "Dec", "%", "Dec"));
            
            //inc_dec
            comp.Add(new Compatibility_Table("null", "Num", "++", "Num"));
            comp.Add(new Compatibility_Table("null", "Num", "--", "Num"));
            comp.Add(new Compatibility_Table("Num", "null", "++", "Num"));
            comp.Add(new Compatibility_Table("Num", "null", "--", "Num"));

            //ROP
            comp.Add(new Compatibility_Table("Num", "Num", ">=", "Bool"));
            comp.Add(new Compatibility_Table("Num", "Num", "<=", "Bool"));
            comp.Add(new Compatibility_Table("Num", "Num", ">", "Bool"));
            comp.Add(new Compatibility_Table("Num", "Num", "<", "Bool"));
            comp.Add(new Compatibility_Table("Num", "Num", "==", "Bool"));
            comp.Add(new Compatibility_Table("Num", "Num", "!=", "Bool"));

            comp.Add(new Compatibility_Table("Num", "Dec", ">=", "Bool"));
            comp.Add(new Compatibility_Table("Num", "Dec", "<=", "Bool"));
            comp.Add(new Compatibility_Table("Num", "Dec", ">", "Bool"));
            comp.Add(new Compatibility_Table("Num", "Dec", "<", "Bool"));
            comp.Add(new Compatibility_Table("Num", "Dec", "==", "Bool"));
            comp.Add(new Compatibility_Table("Num", "Dec", "!=", "Bool"));

            comp.Add(new Compatibility_Table("Dec", "Num", ">=", "Bool"));
            comp.Add(new Compatibility_Table("Dec", "Num", "<=", "Bool"));
            comp.Add(new Compatibility_Table("Dec", "Num", ">", "Bool"));
            comp.Add(new Compatibility_Table("Dec", "Num", "<", "Bool"));
            comp.Add(new Compatibility_Table("Dec", "Num", "==", "Bool"));
            comp.Add(new Compatibility_Table("Dec", "Num", "!=", "Bool"));

            comp.Add(new Compatibility_Table("Dec", "Dec", ">=", "Bool"));
            comp.Add(new Compatibility_Table("Dec", "Dec", "<=", "Bool"));
            comp.Add(new Compatibility_Table("Dec", "Dec", ">", "Bool"));
            comp.Add(new Compatibility_Table("Dec", "Dec", "<", "Bool"));
            comp.Add(new Compatibility_Table("Dec", "Dec", "==", "Bool"));
            comp.Add(new Compatibility_Table("Dec", "Dec", "!=", "Bool"));

            ///AS_OP
            comp.Add(new Compatibility_Table("Dec", "Dec", "+=", "True"));
            comp.Add(new Compatibility_Table("Dec", "Dec", "-=", "True"));
            comp.Add(new Compatibility_Table("Dec", "Dec", "*=", "True"));
            comp.Add(new Compatibility_Table("Dec", "Dec", "/=", "True"));
            comp.Add(new Compatibility_Table("Dec", "Dec", "%=", "True"));
            comp.Add(new Compatibility_Table("Dec", "Dec", "=", "True"));

            comp.Add(new Compatibility_Table("Dec", "Num", "+=", "True"));
            comp.Add(new Compatibility_Table("Dec", "Num", "-=", "True"));
            comp.Add(new Compatibility_Table("Dec", "Num", "*=", "True"));
            comp.Add(new Compatibility_Table("Dec", "Num", "/=", "True"));
            comp.Add(new Compatibility_Table("Dec", "Num", "%=", "True"));
            comp.Add(new Compatibility_Table("Dec", "Num", "=", "True"));

            comp.Add(new Compatibility_Table("Num", "Num", "+=", "True"));
            comp.Add(new Compatibility_Table("Num", "Num", "-=", "True"));
            comp.Add(new Compatibility_Table("Num", "Num", "*=", "True"));
            comp.Add(new Compatibility_Table("Num", "Num", "/=", "True"));
            comp.Add(new Compatibility_Table("Num", "Num", "%=", "True"));
            comp.Add(new Compatibility_Table("Num", "Num", "=", "True"));

            //
            comp.Add(new Compatibility_Table("string", "string", "+", "string"));
            comp.Add(new Compatibility_Table("string", "string", "+=", "True"));
            comp.Add(new Compatibility_Table("string", "string", "==", "Bool"));
            comp.Add(new Compatibility_Table("string", "string", "!=", "Bool"));
            comp.Add(new Compatibility_Table("string", "string", "=", "True"));

            comp.Add(new Compatibility_Table("Bool", "Bool", "||", "Bool"));
            comp.Add(new Compatibility_Table("Bool", "Bool", "&&", "Bool"));
            comp.Add(new Compatibility_Table("Bool", "Bool", "=", "True"));
            comp.Add(new Compatibility_Table("Bool", "Bool", "!=", "Bool"));
            comp.Add(new Compatibility_Table("Bool", "Bool", "==", "Bool"));
            comp.Add(new Compatibility_Table("null", "Bool", "!", "Bool"));
            comp.Add(new Compatibility_Table("Bool", "Bool", "=", "Bool"));
        }
    }
}
