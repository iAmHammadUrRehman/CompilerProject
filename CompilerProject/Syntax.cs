using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerProject
{
    class syntax
    {
        string CurrentClass = "";
        //
        Compatibility_Table CT = new Compatibility_Table();

        MainClassTable m = new MainClassTable();
        List<MemberTable> linq;
        List<MethodTable> link = null;
        bool Flag=false;

        public static Stack<int> scope_stack = new Stack<int>();
        int ScopeCount = 0;
        int scope = 0;
        //
        
        
       
       // Compatibility_Table comp = new Compatibility_Table();

      


        //
        List<token> T;
        int i;
        public int count1 = 0,countVariable = 0, countPush = 0;
        public string WriteIC = null;
        string NL1 = null, NL2 = null;



        public syntax(List<token> input)
        {
            
            
            T = input;
            this.i = 0;
            display();
            //Console.WriteLine(CompilerProject.c_tab[0].name + CompilerProject.c_tab[0].type);
            //Console.WriteLine();
            //Console.WriteLine(WriteIC);
        }
        void display()
        {
            if (S())
            {
                Console.WriteLine("Parse Successfull");
            }
            else
            {
                Console.WriteLine("Syntax Error");
                Console.WriteLine("You cannot write \"" + T[i].val_part + "\" here in line number " + T[i].line_no);
            }
        }

        //Intermediate code

        public string CreateLable()
        {
            string s1 = "L" + count1++;
            return s1;
        }

        public void generate(string s)
        {
            WriteIC += s + Environment.NewLine;
        }

        public string CreateVariable()
        {
            string s1 = "V" + countVariable++;
            return s1;
        }

        public int createScope()
        {
            scope_stack.Push(ScopeCount);
            ScopeCount++;
            return scope_stack.Peek();
        }
        public void DestroyScope()
        {
            scope_stack.Pop();
        }
 

        //
        public bool S()
        {
            if (MainClass())
            {
                if (cls())
                {
                    return true;
                }

            }

            
            return false;

        }
        public bool MainClass()
        {

            if (T[i].cls_part == "Class")
            {
                i++;
                if (T[i].cls_part == "Start")
                {

                    m.ClassInsert("Start", "null", "Class");
                    linq = m.getmemberlink("Start");
                    i++;
                    if (T[i].cls_part == "{")
                    {
                        i++;
                        if (T[i].cls_part == "None")
                        {
                            i++;
                            if (T[i].cls_part == "Begin")
                            {
                                m.MemberInsert_Fn("Begin", "Public", "None");
                                link = m.getmethodlink("Begin");
                                i++;
                                if (T[i].cls_part == "(")
                                {
                                    i++;
                                    if (T[i].cls_part == ")")
                                    {
                                        i++;
                                        if (T[i].cls_part == "{")
                                        {
                                            scope = createScope();
                                            generate("Main Proc");
                                            i++;
                                            if (Body("None"))
                                            {
                                                DestroyVariableScope();
                                                DestroyScope();
                                                link = null;
                                                if (T[i].cls_part == "}")
                                                {
                                                    
                                                    generate("Main Endp");
                                                    i++;
                                                    if (cls_body())
                                                    {
                                                        if (T[i].cls_part == "}")
                                                        {
                                                            linq = null;
                                                            i++;
                                                            return true;
                                                        }

                                                    }

                                                }

                                            }

                                        }


                                    }

                                }


                            }

                        }

                    }

                }

            }
            
            return false;
        }
        public bool cls()
        {
            string N_ = null, A_M = null,s2;   
            if (T[i].cls_part == "AM")
            {
                if (AM(ref A_M))
                {
                    if (T[i].cls_part == "Class")
                    {
                        i++;
                        if (T[i].cls_part == "ID")
                        {
                            N_ = T[i].val_part;

                            s2 = m.Clookup(N_);
                            if (s2!="null")
                            {
                                Console.WriteLine(N_ + " Redeclaration" + " at line number " + T[i].line_no);
                            }
                            else
                            {
                                m.ClassInsert(N_, A_M, "Class");
                                CurrentClass = N_;
                                linq = m.getmemberlink(CurrentClass); 
                            }
                            i++;

                            if (T[i].cls_part == "{")
                            {
                                
                                i++;

                                if (cls_body())
                                {

                                    if (T[i].cls_part == "}")
                                    {
                                        linq = null;
                                        i++;

                                        if (cls())
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (T[i].cls_part == "$")
            {
                return true;
            }
           
            return false;
        }
        public bool cls_body()
        {
            string A_M = "null";
            if (T[i].cls_part == "AM")
            {
                if (AM(ref A_M))
                {
                    if (AM1(A_M))
                    {
                        return true;
                    }
                }
                
            }
            else if (T[i].cls_part == "}")
            {
                return true;
            }
            return false;
        }
        public bool AM1(string A_M)
        {
            string N_ , Tn = "null" ;
            if (T[i].cls_part == "Enum")
            {
                
                if (Enum())
                {
                    if (cls_body())
                    {
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "ID")
            {
                N_ = T[i].val_part;

                string s1 = T[i].val_part;
                i++;
               
                if (AM2(s1,N_,A_M))
                {
                    return true;
                }

            }
            else if (T[i].cls_part == "DT")
            {
                Tn = T[i].val_part;
                i++;
                if (T[i].cls_part == "ID")
                {

                    N_ = T[i].val_part;

                    if (m.Mlookup(N_,linq) == "null" ) 
                    {
                        m.MemberInsert(N_, A_M, Tn);
                    }
                    else
                    {
                        Console.WriteLine(N_ + " Redeclaration" + " at line number " + T[i].line_no);
                    }
                    string s1 = T[i].val_part;
                    i++;

                    if (AM3(s1, N_ ,Tn,A_M))
                    {
                        return true;
                    }

                }

            }
            
            return false;
        }
        public bool AM(ref string A_M)
        {
            if (T[i].cls_part == "AM")
            {
                A_M = T[i].val_part;
                i++;
                return true;
            }
            Console.WriteLine("Are you forgetting Access Modifier ?");
            return false;
        }
        public bool AM2(string s1, string N_, string A_M)
        {
           
            string TY = "", PL_D = null, PL_V = null ;
            if (T[i].cls_part == "ID")
            {
                if (obj_dec(N_,A_M))
                {
                    if (T[i].cls_part == ".")
                    {
                        i++;
                        if (cls_body())
                        {
                            return true;
                        }
                    }

                }

            }
            else if (T[i].cls_part == ":")
            {
                
                i++;
                if (T[i].cls_part == "DT" || T[i].cls_part == "None")
                {
                    if (T[i].cls_part == "DT")
                    {
                        TY = T[i].val_part;
                    }
                    else
                    {
                        TY = "None";
                    }
                    i++;
                    if (T[i].cls_part == "(")
                    {
                        i++;

                        if (Per_cre(ref PL_D,ref PL_V))
                        {
                            if (T[i].cls_part == ")")
                            {
                                i++;
                                N_ += ">" + PL_D;
                                string temp = m.Mlookup(N_, linq);
                                if (temp != "null")
                                {
                                    string[] name1 = N_.Split('>');

                                    Console.WriteLine(name1[0] + " Method redeclaration" + " at line number " + T[i].line_no);
                                }
                                else
                                {
                                    m.MemberInsert_Fn(N_, A_M, TY);
                                    link = m.getmethodlink(N_);
                                }
                                if (T[i].cls_part == "{")
                                {
                                    scope = createScope();
                                    generate(s1 + " _Proc");
                                    i++;
                                    int counter = 0;
                                    string[] Temp_D = PL_D.Split(',');
                                    string[] Temp_V = PL_V.Split(',');
                                    foreach (string item in Temp_D)
                                    {
                                        if (counter > 0)
                                        {
                                            string t = m.Mlookup(Temp_V[counter], linq);
                                            if (t == "null")
                                            {
                                                t = m.flookup(Temp_V[counter],scope, link);
                                                if (t == "null")
                                                {
                                                    m.MethodInsert(Temp_V[counter], item, scope);
                                                }
                                                else
                                                {
                                                    Console.WriteLine(Temp_V[0] + " Redeclaration" + " at line number " + T[i].line_no);
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine(Temp_V[0] + " Already defined in parent scope" + " at line number " + T[i].line_no);
                                            }
                                        }
                                        else
                                        {
                                            string t = m.Mlookup(Temp_V[counter], linq);
                                            if (t == "null")
                                            {
                                                m.MethodInsert(Temp_V[counter], item, scope);
                                            }
                                            else
                                            {
                                                Console.WriteLine(Temp_V[0] + " Already defined in parent scope" + " at line number " + T[i].line_no) ;
                                            }
                                        }
                                    }



                                    if (Body(TY))
                                    {

                                        if (T[i].cls_part == "}")
                                        {

                                            DestroyVariableScope();
                                            
                                            DestroyScope();
                                            link = null;
                                            generate(s1 + " _Endp");
                                            i++;
                                            if (cls_body())
                                            {
                                                return true;
                                            }

                                        }

                                    }

                                }


                            }

                        }

                    }

                }

            }
                
           else if (T[i].cls_part == "(")
            {
               

                i++;
                if (Per_cre(ref PL_D, ref PL_V))
                {

                    if (T[i].cls_part == ")")
                    {
                        i++;
                        if (N_ != CurrentClass)
                        {
                            Console.WriteLine("Contructor name should be same as Class name Or Method should have return type" + " at line number " + T[i].line_no );
                        }
                        else
                        {
                            N_ += ">" + PL_D;
                                string temp = m.Mlookup(N_, linq);
                                if (temp != "null")
                                {
                                    Console.WriteLine(N_ + " Method redeclaration" + " at line number " + T[i].line_no );
                                }
                                else
                                {
                                    m.MemberInsert_Fn(N_, A_M, "None");
                                    link = m.getmethodlink(N_);
                                }
                        }
                        if (T[i].cls_part == "{")
                        {
                            scope = createScope();

                            generate(CurrentClass + "" + s1 + " _Proc");
                            i++;
                            int counter = 0;
                            string[] Temp_D = null,Temp_V = null;
                            
                            try
                            {
                                if (PL_D.Contains(',') && PL_V.Contains(','))
                                {

                                    Temp_D = PL_D.Split(',');
                                    Temp_V = PL_V.Split(',');
                                }
                               
                            }
                            catch (Exception e)
                            {
                                Temp_D[0] = PL_D;
                                Temp_V[0] = PL_V;
                            }

                            if (Temp_D != null)
                            {
                                foreach (string item in Temp_D)
                                {
                                    if (counter > 0)
                                    {
                                        string t = m.Mlookup(Temp_V[counter], linq);
                                        if (t == "null")
                                        {
                                            t = m.flookup(Temp_V[counter], scope, link);
                                            if (t == "null")
                                            {
                                                m.MethodInsert(Temp_V[counter], item, scope);
                                                //counter = 0;
                                            }
                                            else
                                            {
                                                Console.WriteLine(Temp_V[0] + " Redeclaration" + " at line number " + T[i].line_no );
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine(Temp_V[0] + " Already defined in parent scope" + " at line number " + T[i].line_no );
                                        }
                                    }

                                    else
                                    {
                                        string t = m.Mlookup(Temp_V[counter], linq);
                                        if (t == "null")
                                        {
                                            m.MethodInsert(Temp_V[counter], item, scope);
                                        }
                                        else
                                        {
                                            Console.WriteLine(Temp_V[0] + " Already defined in parent scope" + " at line number " + T[i].line_no );
                                        }
                                    }
                                }
                            }
                          
                            i++;
                            if (Body(TY))
                            {
                                if (T[i].cls_part == "}")
                                {
                                    DestroyVariableScope();
                                    DestroyScope();
                                    link = null;
                                    generate(s1 + " Endp");
                                    i++;
                                    if (cls_body())
                                    {
                                        return true;
                                    }
                                }

                            }

                        }


                    }

                }

            }

            
            return false;
        }

        private void DestroyVariableScope()
        {
            int x = scope_stack.Peek();

            try
            {
                foreach (var mem in link)
                {
                    if (mem.Scope == x)
                    {
                        mem.Scope = -1;
                    }
                }
            }
            
               
            catch (Exception e)
            {
                
            }
        }

        public bool AM3(string s1, string N_ , string Tn, string A_M)
        {
            string temp = null;
            if (T[i].cls_part == "," || T[i].cls_part == "=" || T[i].cls_part == ".")
            {


                if (go(s1,Tn,A_M))
                {

                    if (T[i].cls_part == ".")
                    {
                        i++;
                        if (cls_body())
                        {
                            return true;
                        }

                    }

                }

            }
            else if (T[i].cls_part == "[")
            {
                string Tn1 = null;
                i++;
                if (exp(ref Tn1))
                {
                    if (Tn1 != "Num")
                    {
                        Console.WriteLine("Invalid indexer type" + " at line number " + T[i].line_no);
                    }
                    if (T[i].cls_part == "]")
                    {
                        i++;
                        string r;
                        if (link == null)
                        {
                            r = m.Mlookup(N_, linq);
                            if (r == "null")
                            {
                                temp = Tn + ">[]";
                                m.MethodInsert(N_, Tn, scope);

                            }
                            else
                            {
                                Console.WriteLine("Redeclaration" + " at line number " + T[i].line_no);
                            }
                        }
                        else
                        {
                            r = m.Mlookup(N_, linq);

                            if (r == "null")
                            {
                                r = m.flookup(N_, scope, link);
                                if (r == "null")
                                {
                                    temp = Tn + ">[]";
                                    m.MethodInsert(N_, temp, scope);

                                }
                                else
                                {
                                    Console.WriteLine("Redeclaration" + " at line number " + T[i].line_no);
                                }
                            }
                            else
                            {
                                Console.WriteLine(N_ + " Already exist in Parent scope" + " at line number " + T[i].line_no);
                            }
                        }
                        if (opt(s1,Tn,A_M))
                        {
                            if (T[i].cls_part == ".")
                            {
                                i++;
                                if (cls_body())
                                {
                                    return true;
                                }


                            }


                        }

                    }


                }

            }
            return false;

        }
        public bool Enum()
        {
           
            if (T[i].cls_part == "Enum")
            {
                i++;
                if (T[i].cls_part == "ID")
                {

                    i++;

                    if (type())
                    {
                        if (T[i].cls_part == "{")
                        {

                            i++;
                            if (e_body())
                            {
                                if (T[i].cls_part == "}")
                                {
                                    i++;
                                    return true;
                                }

                            }


                        }


                    }

                }


            }

            return false;

        }
        public bool e_body()
        {

            if (T[i].cls_part == "ID")
            {
                i++;
                if (more())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool more()
        {

            if (T[i].cls_part == ",")
            {
                i++;
                if (T[i].cls_part == "ID")
                {

                    i++;
                    if (more())
                    {
                        return true;
                    }

                }

            }
            else if (T[i].cls_part == "}")
            {
                return true;
            }

            return false;

        }
        public bool type()
        {

            if (T[i].cls_part == ":")
            {
                i++;

                if (ty())
                {
                    return true;
                }

            }

            return false;

        }
        public bool ty()
        {
            if (T[i].cls_part == "DT")
            {
                i++;

                return true;
            }

            return false;

        }
        public bool obj_dec(string N_,string A_M)
        {
            string N1_;
            if (T[i].cls_part == "ID")
            {
                N1_ = T[i].val_part;

                string s2 = m.Clookup(N_);
                if (s2!=null)
                {
                    if (N_==N1_)
                    {
                        Console.WriteLine("Error : Instance name can not be same as class" + " at line number " + T[i].line_no);
                    }
                    else
                    {
                        linq = m.getmemberlink(CurrentClass);
                        string s3 = m.Mlookup(N1_, linq);
                        if (s3!=null)
                        {
                            Console.WriteLine("Redeclaration" + " at line number " + T[i].line_no);
                        }
                        else
                        {
                            m.MemberInsert(N1_, A_M, N_);
                        }
                    }
                }
                else
                {
                    Console.WriteLine(N_ + " is not a Class" + " at line number " + T[i].line_no);
                }

                i++;
                if (xcon(N_,A_M))
                {
                    if (xcon1(N_,A_M))
                    {

                        return true;
                    }

                }

            }
            return false;
        }
        public bool xcon(string N_, string A_M)
        {
            string N1_ = null, PL = null,T_ = null;
            if (T[i].cls_part == "Create")
            {
                i++;
                if (T[i].cls_part == "ID")
                {
                    N1_ = T[i].val_part;
                    string vp = T[i].val_part;
                    i++;
                    if (T[i].cls_part == "(")
                    {
                        i++;
                        countPush = 0;
                        if (Per_call(ref PL))
                        {

                            generate(CreateVariable() + "= Call " + vp + "," + countPush);
                            countPush = 0;
                            if (T[i].cls_part == ")")
                            {
                                i++;
                                if (N_!=N1_)
                                {
                                    Console.WriteLine("Invalid Contructor" + N1_ + " at line number " + T[i].line_no);
                                }
                                else
                                {
                                    
                                    N1_ += ">" + PL;
                                    if (m.Mlookup(N1_,linq)== null)
                                    {
                                        Console.WriteLine("Constructor not declared" + " at line number " + T[i].line_no);
                                    }

                                }
                                return true;
                            }
                        }

                    }

                }

            }
            else if (T[i].cls_part == "[")
            {

                i++;
                if (exp(ref T_))
                {
                    if (T_!="Num")
                    {
                        Console.WriteLine("Invalid Indexer type" + " at line number " + T[i].line_no);
                    }

                if (T[i].cls_part == "]")
                {

                    i++;

                    string s2 = m.Clookup(N_);
                    if (s2 != null)
                    {
                        if (N_ == N1_)
                        {
                            Console.WriteLine("Error : Instance name can not be same as class" + " at line number " + T[i].line_no);
                        }
                        else
                        {
                           
                            string s3 = m.Mlookup(N1_, linq);
                            if (s3 != null)
                            {
                                Console.WriteLine("Redeclaration" + " at line number " + T[i].line_no);
                            }
                            else
                            {
                                N_ += ">[]";
                                m.MemberInsert(N1_, A_M, N_);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine(N_ + " is not a Class" + " at line number " + T[i].line_no);
                    }

                    if (T[i].cls_part == "Create")
                    {
                        i++;
                        if (T[i].cls_part == "ID")
                        {
                            string vp = T[i].val_part;
                            i++;
                            if (T[i].cls_part == "(")
                            {
                                i++;
                                countPush = 0;
                                if (Per_call(ref PL))
                                {
                                    generate(CreateVariable() + "= Call " + vp + "," + countPush);
                                    countPush = 0;
                                    if (T[i].cls_part == ")")
                                    {
                                        i++;
                                        if (N_ != N1_)
                                        {
                                            Console.WriteLine("Invalid Contructor" + N1_ + " at line number " + T[i].line_no);
                                        }
                                        else
                                        {

                                            N1_ += ">" + PL;
                                            if (m.Mlookup(N1_, linq) == null)
                                            {
                                                Console.WriteLine("Constructor not declared" + " at line number " + T[i].line_no);
                                            }

                                        }
                                        return true;
                                    }
                                }
                            }
                        }

                    }
                }
                }
            }

            else if (T[i].cls_part == "," || T[i].cls_part == "AM" || T[i].cls_part == ".")
            {
                return true;
            }
            return false;

        }
        public bool xcon1(string N_, string A_M)
        {

            if (T[i].cls_part == ",")
            {
                i++;
                if (obj_dec(N_,A_M))
                {
                    return true;
                }
            }
            else if (T[i].cls_part == "AM" || T[i].cls_part == ".")
            {
                return true;
            }
            return false;
        }
        public bool obj_init(string N_, string A_M)
        {
            string N1 = null, PL = null, Tn = null; 
            if (T[i].cls_part == "Create")
            {
                i++;
                if (T[i].cls_part == "ID")
                {
                    N1 = T[i].val_part;
                    string vp = T[i].val_part;
                    i++;
                    if (T[i].cls_part == "(")
                    {
                        i++;
                        countPush = 0;
                        if (Per_call(ref PL))
                        {
                            if (N1 != N_)
                            {
                                Console.WriteLine("Invalid Constructor " + N1 + " at line number " + T[i].line_no);
                            }
                            else
                            {
                                N1 += ">" + PL;
                                Tn = m.Mlookup(N1, linq);
                                if (Tn == "null")
                                {
                                    Console.WriteLine("Constructor not declared" + " at line number " + T[i].line_no);
                                }
                            }
                            
                            generate(CreateVariable() + "= Call " + vp + "," + countPush);
                            countPush = 0;
                            if (T[i].cls_part == ")")
                            {
                                i++;
                                return true;
                            }
                        }
                    }

                }
            }

            return false;
        }
        //public bool Dec()
        //{
        //    if (T[i].cls_part == "DT")
        //    {
        //        i++;
        //        if (T[i].cls_part == "ID")
        //        {
        //            string s1 = T[i].val_part;
        //            i++;
        //            if (go(s1))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
        public bool go(string s1,string Tn, string A_M)
        {

            if (T[i].cls_part == "," || T[i].cls_part == ".")
            {

                if (list(s1,Tn,A_M))
                {
                    return true;
                }
            }
            else if (T[i].cls_part == "=")
            {

                if (Sing_AS(s1,Tn,A_M))
                {

                    return true;
                }
            }
            return false;

        }
        public bool list(string s1, string Tn, string A_M)
        {
            string N_ = null;
            if (T[i].cls_part == ",")
            {
                i++;
                if (T[i].cls_part == "ID")
                {
                    N_ = T[i].val_part;
                    s1 = T[i].val_part;
                    i++;

                    if (m.Mlookup(N_,linq) == "null")
                    {
                        if (link != null)
                        {
                            m.MethodInsert(N_, Tn, scope);
                        }
                        else
                        m.MemberInsert(N_, A_M, Tn);

                    }
                    else
                    {
                        Console.WriteLine("Redeclaration" + " at line number " + T[i].line_no);
                    }

                    if (go(s1,Tn,A_M))
                    {
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == ".")
            {

                return true;
            }
            return false;

        }
        public bool Sing_AS(string s1, string RT, string A_M)
        {
            string OP, Tn = null, RT1 = null;
            if (T[i].cls_part == "=")
            {
                OP = T[i].val_part;
                i++;
                string v1 = CreateVariable();
                if (AS_ST(v1,ref RT1))
                {
                    Tn = CT.compatibility(RT, RT1, OP);
                   
                    if (Tn == "null")
                    {
                        Console.WriteLine(RT + " and " + RT1 + "Miss Match" + " at line number " + T[i].line_no);
                        Console.WriteLine(Tn);
                    }
                    generate(s1 + "=" + v1);
                    if (list(s1,RT,A_M))
                    {
                        return true;
                    }
                }
            }
            return false;

        }
        public bool Per_call(ref string PL)
        {
            if (T[i].cls_part == "ID" || T[i].cls_part == "," || T[i].cls_part == "!" || T[i].cls_part == "(" || T[i].cls_part == "++" || T[i].cls_part == "--" || T[i].cls_part == "Num_Const" || T[i].cls_part == "Dec_Const" || T[i].cls_part == "string_Const" || T[i].cls_part == "Char_Const" || T[i].cls_part == "Bool_Const")
            {
                if (val(ref PL))
                {
                    return true;
                }
            }
            else if (T[i].cls_part == ")")
            {
                    return true;   
            }
            return false;
        }
        
        public bool val(ref string PL)
        {
            string RT = null;
            if (T[i].cls_part == "ID" || T[i].cls_part == "!" || T[i].cls_part == "(" || T[i].cls_part == "++" || T[i].cls_part == "--" || T[i].cls_part == "Num_Const" || T[i].cls_part == "Dec_Const" || T[i].cls_part == "string_Const" || T[i].cls_part == "Char_Const" || T[i].cls_part == "Bool_Const")
            {
                //string s1 = T[i-1].val_part;
                if (f_exp(ref RT))
                {
                    if (RT!= null)
                    {
                        PL += RT;
                    }
                    countPush++;
                    generate("Push " + CreateVariable());   
                    
                    if (val2(ref PL))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public bool val2(ref string PL)
        {
            string RT = null;
            if (T[i].cls_part == ",")
            {
                i++;
                if (f_exp(ref RT))
                {
                    PL += "," + RT;
                    countPush++;
                    generate("Push " + CreateVariable());
                    if (val2(ref PL))
                    {
                        return true;
                    }
                }
            }

            else if (T[i].cls_part == ")")
            {
                return true;
            }
            return false;
        }
        public bool f_exp(ref string RT)
        {
            string N_, RT1 = null; ;
            if (T[i].cls_part == "ID")
            {
                N_ = T[i].val_part;
                string s1 = T[i].val_part;
                i++;
               
                if (etc(s1,N_,ref RT1))
                {
                  
                    if (terao(RT1, ref RT))
                    {
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "!")
            {
                i++;
                if (F(ref RT1))
                {

                    if ( CT.compatibility("null","Bool","!") == "Bool")
                    {
                        RT = RT1;
                    }
                    else
                    {
                        Console.WriteLine(RT + "and" + RT1 + "Miss match" + " at line number " + T[i].line_no);
                    }

                    if (terao(RT1, ref RT))
                    {
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "(")
            {
                i++;
                if (f_exp(ref RT1))
                {
                    if (T[i].cls_part == ")")
                    {
                        i++;
                        if (terao(RT1,ref RT))
                        {
                            return true;
                        }
                    }
                }
            }
            else if (T[i].cls_part == "++")
            {
                string SB = null;
                i++;
                if (T[i].cls_part == "ID")
                {
                    N_ = T[i].val_part;
                    i++;
                    if (A(ref SB))
                    {
                        if (link == null)
                        {
                            RT1 = m.Mlookup(N_, linq);
                            if (RT1 == "null")
                            {
                                Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                            }
                            else if (SB == null)
                            {
                                if (!RT1.Contains('['))
                                {

                                }
                                else
                                {
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                            }
                            else if (SB.Contains('['))
                            {
                                if (RT1.Contains('['))
                                {
                                    string[] RT2 = RT1.Split('>');
                                    RT1 = RT2[0];
                                }
                                else
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }
                            else if (RT1 != "null")
                            {

                            }
                        }
                        else
                        {
                            RT1 = m.flookup(N_,scope, link);
                            if (RT1 == "null")
                            {
                                RT1 = m.Mlookup(N_, linq);
                                if (RT1 == "null")
                                {
                                    Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                                }
                                else if (SB == null)
                                {
                                    if (!RT1.Contains('['))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                    }
                                }
                                else if (SB.Contains('['))
                                {
                                    if (RT1.Contains('['))
                                    {
                                        string[] RT2 = RT1.Split('>');
                                        RT1 = RT2[0];
                                    }
                                    else
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                                else if (RT1 != "null")
                                {

                                }
                            }
                            else
                            {
                                if (SB == null)
                                {
                                    if (!RT1.Contains('['))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                    }
                                }
                                else if (SB.Contains('['))
                                {
                                    if (RT1.Contains('['))
                                    {
                                        string[] RT2 = RT1.Split('>');
                                        RT1 = RT2[0];
                                    }
                                    else
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                                else if (RT1 != "null")
                                {

                                }
                            }
                        }
                        if (RT1 == "Num" || RT1 == "Dec")
                        {
                            RT = RT1;
                        }
                        else
                        {
                            Console.WriteLine(RT + "and" + RT1 + "Miss Match" + " at line number " + T[i].line_no);
                        }
                        if (terao(RT1 ,ref RT))
                        {
                            return true;
                        }
                    }
                }
            }
            else if (T[i].cls_part == "--")
            {
                string SB = null;
                i++;
                if (T[i].cls_part == "ID")
                {
                    N_ = T[i].val_part;
                    i++;
                    if (A(ref SB))
                    {
                        if (link == null)
                        {
                            RT1 = m.Mlookup(N_, linq);
                            if (RT1 == "null")
                            {
                                Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                            }
                            else if (SB == null)
                            {
                                if (!RT1.Contains('['))
                                {

                                }
                                else
                                {
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                            }
                            else if (SB.Contains('['))
                            {
                                if (RT1.Contains('['))
                                {
                                    string[] RT2 = RT1.Split('>');
                                    RT1 = RT2[0];
                                }
                                else
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }
                            else if (RT1 != "null")
                            {

                            }
                        }
                        else
                        {
                            RT1 = m.flookup(N_,scope, link);
                            if (RT1 == "null")
                            {
                                RT1 = m.Mlookup(N_, linq);
                                if (RT1 == "null")
                                {
                                    Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                                }
                                else if (SB == null)
                                {
                                    if (!RT1.Contains('['))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                    }
                                }
                                else if (SB.Contains('['))
                                {
                                    if (RT1.Contains('['))
                                    {
                                        string[] RT2 = RT1.Split('>');
                                        RT1 = RT2[0];
                                    }
                                    else
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                                else if (RT1 != "null")
                                {

                                }
                            }
                            else
                            {
                                if (SB == null)
                                {
                                    if (!RT1.Contains('['))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                    }
                                }
                                else if (SB.Contains('['))
                                {
                                    if (RT1.Contains('['))
                                    {
                                        string[] RT2 = RT1.Split('>');
                                        RT1 = RT2[0];
                                    }
                                    else
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                                else if (RT1 != "null")
                                {

                                }
                            }
                        }
                        if (RT1 == "Num" || RT1 == "Dec")
                        {
                            RT = RT1;
                        }
                        else
                        {
                            Console.WriteLine(RT + "and" + RT1 + "Miss Match" + " at line number " + T[i].line_no);
                        }
                        if (terao(RT1, ref RT))
                        {
                            return true;
                        }
                    }
                }
            }
            else if (T[i].cls_part == "Num_Const" || T[i].cls_part == "Dec_Const" || T[i].cls_part == "string_Const" || T[i].cls_part == "Char_Const" || T[i].cls_part == "Bool_Const")
            {
                if (T[i].cls_part == "Num_Const" )
                {
                    RT1 = "Num";
                }

                else if (T[i].cls_part == "Dec_Const" )
                {
                    RT1 = "Dec";
                }

                else if (T[i].cls_part == "Bool_Const")
                {
                    RT1 = "Bool";
                }
                else if (T[i].cls_part == "string_Const")
                {
                    RT1 = "string";
                }
                else if (T[i].cls_part == "Char_Const")
                {
                    RT1 = "Char";
                }
                i++;
                if (terao(RT1, ref RT))
                {
                    return true;
                }
            }
            return false;
        }
        public bool exp(ref string RT1)
        {
            string RT2 = null;
            if (T1(ref RT2))
            {

                if (E_(RT2,ref RT1))
                {
                   
                    return true;
                }

            }

            return false;
        }
        public bool terao(string RT1, ref string RT)
        {

            if (T[i].cls_part == "MDM" || T[i].cls_part == "PM" || T[i].cls_part == "RO" || T[i].cls_part == "&&" || T[i].cls_part == "||" || T[i].cls_part == ")" || T[i].cls_part == "." || T[i].cls_part == "," || T[i].cls_part == "}")
            {
                if (T_(RT1,ref RT))
                {

                    if (E_(RT1, ref RT))
                    {
                        
                        if (RE_(RT1, ref RT))
                        {
                            
                            if (And_(RT1, ref RT))
                            {
                                
                                if (OR_(RT1, ref RT))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool T_( string RT1, ref string RT)
        {
            string RT2 = null, OP;
            if (T[i].cls_part == "MDM")
            {
                OP = T[i].val_part;
                i++;
                if (F(ref RT2))
                {
                    RT2 = CT.compatibility(RT1, RT2, OP);
                    if (RT2 == "null")
                    {
                        Console.WriteLine(RT + "and" + RT1 + "Type miss match" + " at line number " + T[i].line_no);
                    }
                  
                    if (T_(RT2,ref RT))
                    {

                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "]" || T[i].cls_part == "PM" || T[i].cls_part == "RO" || T[i].cls_part == "&&" || T[i].cls_part == "||" || T[i].cls_part == ")" || T[i].cls_part == "." || T[i].cls_part == "," || T[i].cls_part == "}")
            {
               
                RT = RT1;
                return true;
            }
            return false;

        }
        public bool E_(string RT1,ref string RT)
        {
            string RT2 = null, OP;
            if (T[i].cls_part == "PM")
            {
                OP = T[i].val_part;
                i++;
                
                if (T1(ref RT2))
                {
                    RT2 = CT.compatibility(RT1, RT2, OP);
                    if (RT2 == "null")
                    {
                        Console.WriteLine(RT + "and" + RT2 + "Type miss match" + " at line number " + T[i].line_no);
                    }
                  
                    if (E_(RT2,ref RT))
                    {
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "]" || T[i].cls_part == "RO" || T[i].cls_part == "&&" || T[i].cls_part == "||" || T[i].cls_part == ")" || T[i].cls_part == "." || T[i].cls_part == "," || T[i].cls_part == "}")
            {
                RT = RT1;
                return true;
            }
            return false;
        }
        public bool RE_(string RT1, ref string RT)
        {
            
            string RT2 = null, OP;
            if (T[i].cls_part == "RO")
            {
                OP = T[i].val_part;
                i++;
                if (exp(ref RT2))
                {
                    RT2 = CT.compatibility(RT1, RT2, OP);
                   
                    if (RT2 == "null")
                    {
                        Console.WriteLine(RT + "and" + RT2 + "Type miss match" + " at line number " + T[i].line_no);
                    }
                    if (RE_(RT2,ref RT))
                    {
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "&&" || T[i].cls_part == "||" || T[i].cls_part == ")" || T[i].cls_part == "." || T[i].cls_part == "," || T[i].cls_part == "}")
            {
                
                RT = RT1;
                return true;
            }
            return false;
        }
        public bool And_(string RT1,ref string RT)
        {
            
            string RT2 = null, OP;
            if (T[i].cls_part == "&&")
            {
                OP = T[i].val_part;
                i++;
                if (RE(ref RT2))
                {
                    RT2 = CT.compatibility(RT1, RT2, OP);
                    if (RT2 == "null")
                    {
                        Console.WriteLine(RT1 + "and" + RT2 + "Type miss match" + " at line number " + T[i].line_no);
                    }
                   
                    if (And_(RT2,ref RT))
                    {
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "||" || T[i].cls_part == ")" || T[i].cls_part == "." || T[i].cls_part == "," || T[i].cls_part == "}")
            {
               
                RT = RT1;   
                return true;
            }
            return false;
        }
        public bool OR_(string RT1, ref string RT)
        {
            string RT2 = null,OP;
            if (T[i].cls_part == "||")
            {
                OP = T[i].val_part;
                i++;
                if (And(ref RT2))
                {
                    RT2 = CT.compatibility(RT1, RT2, OP);
                    if (RT2 == "null")
                    {
                        Console.WriteLine(RT1 + "and" + RT2 + "Type miss match" + " at line number " + T[i].line_no);
                    }
                    if (OR_(RT2,ref RT))
                    {
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == ")" || T[i].cls_part == "." || T[i].cls_part == "," || T[i].cls_part == "}")
            {
               
                    RT = RT1;
               
                return true;
            }
            return false;
        }
        public bool F(ref string RT)
        {
            string N_ = null, SB = null, RT1 = null, OP;
            if (T[i].cls_part == "ID")
            {
                N_ = T[i].val_part;
                string s1 = T[i].val_part;
                i++;
                
                if (etc(s1,N_,ref RT))
                {
                    return true;
                }
            }
            else if (T[i].cls_part == "!")
            {
                OP = T[i].val_part;
                i++;
                if (F(ref RT1))
                {
                    if (CT.compatibility("null", "Bool", "!") == "Bool")
                    {
                        RT = RT1;
                    }
                    else
                    {
                        Console.WriteLine(RT + "and" + RT1 + "Miss match" + " at line number " + T[i].line_no);
                    }
                    return true;
                }
            }
            else if (T[i].cls_part == "(")
            {
                i++;
                if (f_exp(ref RT))
                {
                    if (T[i].cls_part == ")")
                    {
                        i++;
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "++")
            {
                OP = T[i].val_part;
                i++;
                if (T[i].cls_part == "ID")
                {
                    N_ = T[i].val_part;
                    i++;
                    if (A(ref SB))
                    {
                        if (link!= null)
                        {
                            RT1 = m.Mlookup(N_, linq);
                            if (RT1 == "null")
                            {
                                Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                            }
                            else if (SB == null)
                            {
                                if (!RT1.Contains('['))
                                {

                                }
                                else
                                {
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                            }
                            else if (SB.Contains('['))
                            {
                                if (RT1.Contains('['))
                                {
                                    string[] RT2 = RT1.Split('>');
                                    RT1 = RT2[0];
                                }
                                else
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }
                            else if (RT1!="null")
                            {
                                
                            }
                        }
                        else
                        {
                            RT1 = m.flookup(N_,scope, link);
                            if (RT1== "null")
                            {
                                RT1 = m.Mlookup(N_, linq);
                                if (RT1 == "null")
                                {
                                    Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                                }
                                else if (SB == null)
                                {
                                    if (!RT1.Contains('['))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                    }
                                }
                                else if (SB.Contains('['))
                                {
                                    if (RT1.Contains('['))
                                    {
                                        string[] RT2 = RT1.Split('>');
                                        RT1 = RT2[0];
                                    }
                                    else
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                                else if (RT1 != "null")
                                {

                                }  
                            }
                            else
                            {
                                if (SB == null)
                                {
                                    if (!RT1.Contains('['))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                    }
                                }
                                else if (SB.Contains('['))
                                {
                                    if (RT1.Contains('['))
                                    {
                                        string[] RT2 = RT1.Split('>');
                                        RT1 = RT2[0];
                                    }
                                    else
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                                else if (RT1 != "null")
                                {

                                }  
                            }
                        }
                        if (RT1 == "Num" || RT1 == "Dec")
                        {
                            RT = RT1;
                        }
                        else
                        {
                            Console.WriteLine(RT + "and" + RT1 + "Miss Match" + " at line number " + T[i].line_no);
                        }
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "--")
            {
                i++;
                if (T[i].cls_part == "ID")
                {
                    N_ = T[i].val_part;
                    i++;
                    if (A(ref SB))
                    {
                        if (link == null)
                        {
                            RT1 = m.Mlookup(N_, linq);
                            if (RT1 == "null")
                            {
                                Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                            }
                            else if (SB == null)
                            {
                                if (!RT1.Contains('['))
                                {

                                }
                                else
                                {
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                            }
                            else if (SB.Contains('['))
                            {
                                if (RT1.Contains('['))
                                {
                                    string[] RT2 = RT1.Split('>');
                                    RT1 = RT2[0];
                                }
                                else
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }
                            else if (RT1 != "null")
                            {

                            }
                        }
                        else
                        {
                            RT1 = m.flookup(N_,scope, link);
                            if (RT1 == "null")
                            {
                                RT1 = m.Mlookup(N_, linq);
                                if (RT1 == "null")
                                {
                                    Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                                }
                                else if (SB == null)
                                {
                                    if (!RT1.Contains('['))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                    }
                                }
                                else if (SB.Contains('['))
                                {
                                    if (RT1.Contains('['))
                                    {
                                        string[] RT2 = RT1.Split('>');
                                        RT1 = RT2[0];
                                    }
                                    else
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                                else if (RT1 != "null")
                                {

                                }
                            }
                            else
                            {
                                if (SB == null)
                                {
                                    if (!RT1.Contains('['))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                    }
                                }
                                else if (SB.Contains('['))
                                {
                                    if (RT1.Contains('['))
                                    {
                                        string[] RT2 = RT1.Split('>');
                                        RT1 = RT2[0];
                                    }
                                    else
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                                else if (RT1 != "null")
                                {

                                }
                            }
                        }
                        if (RT1 == "Num" || RT1 == "Dec")
                        {
                            RT = RT1;
                        }
                        else
                        {
                            Console.WriteLine(RT + "and" + RT1 + "Miss Match" + " at line number " + T[i].line_no);
                        }
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "Num_Const" || T[i].cls_part == "Dec_Const" || T[i].cls_part == "string_Const" || T[i].cls_part == "Char_Const" || T[i].cls_part == "Bool_Const")
            {

                if (T[i].cls_part == "Num_Const")
                {
                    RT1 = "Num";
                }

                else if (T[i].cls_part == "Dec_Const")
                {
                    RT1 = "Dec";
                }

                else if (T[i].cls_part == "Bool_Const")
                {
                    RT1 = "Bool";
                }
                else if (T[i].cls_part == "string_Const")
                {
                    RT1 = "string";
                }
                else if (T[i].cls_part == "Char_Const")
                {
                    RT1 = "Char";
                }
                RT = RT1;
                i++;
                return true;
            }
            return false;
        }
        public bool etc(string vp, string N_, ref string RT)
        {
            string PL = null;
            string RT1 = null;
            string SB = null;
            if (T[i].cls_part == "]" || T[i].cls_part == "[" || T[i].cls_part == "}" || T[i].cls_part == "MOP" || T[i].cls_part == "MDM" || T[i].cls_part == "PM" || T[i].cls_part == "RO" || T[i].cls_part == "&&" || T[i].cls_part == "||" || T[i].cls_part == ")" || T[i].cls_part == "." || T[i].cls_part == "," || T[i].cls_part == "ASS_OP" || T[i].cls_part == "=" || T[i].cls_part == "Create")
            {
                if (A(ref SB))
                {
                   
                    if (link == null)
                    {
                        
                        RT1 = m.Mlookup(N_, linq);
                        if (RT1 == "null")
                        {
                            Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                        }
                        else if (SB == null)
                        {
                            if (!RT1.Contains('['))
                            {

                            }
                            else
                            {
                                Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }
                        }
                        else if (SB.Contains('['))
                        {
                            if (RT1.Contains('['))
                            {
                                string[] RT2 = RT1.Split('>');
                                RT1 = RT2[0];
                            }
                            else
                                Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                        }
                        else if (RT1 != "null")
                        {

                        }
                    }
                    else
                    {
                        RT1 = m.flookup(N_,scope, link);
                        
                        if (RT1 == "null")
                        {
                            RT1 = m.Mlookup(N_, linq);
                            if (RT1 == "null")
                            {
                                Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                            }
                            else if (SB == null)
                            {
                                if (!RT1.Contains('['))
                                {

                                }
                                else
                                {
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                            }
                            else if (SB.Contains('['))
                            {
                                if (RT1.Contains('['))
                                {
                                    string[] RT2 = RT1.Split('>');
                                    RT1 = RT2[0];
                                }
                                else
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }
                            else if (RT1 != "null")
                            {

                            }
                        }
                        else
                        {
                            //if (RT1.Contains('[') && SB.Contains('['))
                            //{
                            //    string[] RT2 = RT1.Split('>');
                            //    RT1 = RT2[0];
                            //}
                            //else if (!(RT1.Contains('[') && SB.Contains('[')))
                            //{
                            //    Console.WriteLine(RT1 + " " + SB);
                            //    Console.WriteLine("Error : invalid use of " + N_ + " at line number " + T[i].line_no);
                            //}
                            if (SB == null)
                            {
                                if (!RT1.Contains('['))
                                {
                                    
                                }
                                else
                                {
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                            }
                            else if (SB.Contains('['))
                            {
                                if (RT1.Contains('['))
                                {
                                    string[] RT2 = RT1.Split('>');
                                    RT1 = RT2[0];
                                }
                                else
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }

                            else if (RT1 != "null")
                            {

                            }
                        }
                    }
                  
                    if (Inc_Dec(RT1))
                    {
                        if (MOP(N_,RT1,ref RT))
                        {
                            return true;
                        }
                    }
                }
            }

           
            else if (T[i].cls_part == "(")
            {
                i++;
                countPush = 0;
                if (Per_call(ref PL))
                {
                    
                    generate(CreateVariable() + "= Call " + vp + "," + countPush);
                    countPush = 0;
                    if (T[i].cls_part == ")")
                    {
                        
                        i++;
                        N_ += ">" + PL;
                        RT1 = m.Mlookup(N_, linq);

                        if (RT1 != "null")
                        {
                            RT = RT1;
                        }
                        else
                        {
                            Console.WriteLine("Function Not declared " + N_ + " at line number " + T[i].line_no);
                        }
                        return true;
                    }
                }
            }
            return false;
        }
        //public bool Ass()
        //{
        //    if (T[i].cls_part == "ID")
        //    {
        //        string s1 = T[i].val_part;
        //        i++;
        //        if (A())
        //        {
        //            if (Obj_ref())
        //            {
        //                if (init(s1))
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}
        public bool A(ref string SB)
        {
            string RT1 = null;
            
            if (T[i].cls_part == "[")
            {
                i++;
                
                if (exp(ref RT1))
                {
                    if (RT1!= "null")
                    {
                        Console.WriteLine("Invalid indexer type" + " at line number " + T[i].line_no);
                    }
                   
                    if (T[i].cls_part == "]")
                    {

                        i++;
                        SB = "[]";
                        return true;
                    }

                }
            }
            else if (T[i].cls_part == "]" || T[i].cls_part == "(" || T[i].cls_part == "++" || T[i].cls_part == "--" || T[i].cls_part == "MOP" || T[i].cls_part == "MDM" || T[i].cls_part == "PM" || T[i].cls_part == "RO" || T[i].cls_part == "&&" || T[i].cls_part == "||" || T[i].cls_part == ")" || T[i].cls_part == "," || T[i].cls_part == "." || T[i].cls_part == "}" || T[i].cls_part == "ASS_OP" || T[i].cls_part == "=" || T[i].cls_part == "Create")
            {
                return true;
            }
            return false;
        }
        public bool Obj_ref(string N_, string RT1, ref string RT)
        {
            string N1 = null, SB = null;
            //string RT1 = null;
            
            if (T[i].cls_part == "MOP")
            {
                if (m.Clookup(N_) == "Class")
                {

                }
                else
                {
                    Console.WriteLine("Member Operator can not be apply on non object operand" + " at line number " + T[i].line_no);
                }
                i++;
                if (T[i].cls_part == "ID")
                {
                    N1 = T[i].val_part;
                    i++;
                    if (A(ref SB))
                    {
                        if (link == null)
                        {
                            RT1 = m.Mlookup(N_, linq);
                            if (RT1 == "null")
                            {
                                Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                            }
                            else if (SB == null)
                            {
                                if (!RT1.Contains('['))
                                {

                                }
                                else
                                {
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                            }
                            else if (SB.Contains('['))
                            {
                                if (RT1.Contains('['))
                                {
                                    string[] RT2 = RT1.Split('>');
                                    RT1 = RT2[0];
                                }
                                else
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }
                            else if (RT1 != "null")
                            {

                            }
                        }
                        else
                        {
                            RT1 = m.flookup(N_, scope, link);
                            if (RT1 == "null")
                            {
                                RT1 = m.Mlookup(N_, linq);
                                if (RT1 == "null")
                                {
                                    Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                                }
                                else if (SB == null)
                                {
                                    if (!RT1.Contains('['))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                    }
                                }
                                else if (SB.Contains('['))
                                {
                                    if (RT1.Contains('['))
                                    {
                                        string[] RT2 = RT1.Split('>');
                                        RT1 = RT2[0];
                                    }
                                    else
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                                else if (RT1 != "null")
                                {

                                }
                            }
                            else
                            {
                                if (SB == null)
                                {
                                    if (!RT1.Contains('['))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                    }
                                }
                                else if (SB.Contains('['))
                                {
                                    if (RT1.Contains('['))
                                    {
                                        string[] RT2 = RT1.Split('>');
                                        RT1 = RT2[0];
                                    }
                                    else
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                                else if (RT1 != "null")
                                {

                                }
                            }
                        }
                        if (Obj_ref(N1,RT1,ref RT))
                        {
                            return true;
                        }
                    }
                }

            }
            else if (T[i].cls_part == "ASS_OP" || T[i].cls_part == "=")
            {
                RT = RT1;
                return true;
            }
            return false;
        }
        public bool init(string s1, string RT)
        {
            string RT1 = null, OP, Tn = null;
            if (T[i].cls_part == "ASS_OP" || T[i].cls_part == "=")
            {
                OP = T[i].val_part;
                i++;
                
                if (AS_ST(s1,ref RT1))
                {
                    Tn = CT.compatibility(RT, RT1, OP);
                    if (Tn == "null")
                    {
                        Console.WriteLine(RT + "and" + RT1 + "Miss match" + " at line number " + T[i].line_no);
                    }
                    return true;
                }
            }
            return false;
        }
        public bool AS_ST(string s2, ref string RT1)
        {
            string N_ = null;
            if (T[i].cls_part == "ID")
            {
                N_ = T[i].val_part;
                string s1 = T[i].val_part;
                i++;
                
                if (AS_ST2(s1,N_,ref RT1))
                {
                    return true;
                }
            }
            else if (T[i].cls_part == "!")
            {
                string OP = T[i].val_part;
                string RT2 = null;
                i++;
                if (F(ref RT2))
                {
                    RT2 = CT.compatibility("null", "Bool", "!");
                    if (RT2 != "null")
                    {
                    
                    }
                    else
                    {
                        Console.WriteLine(RT1 + "and" + RT2 + "Miss match" + " at line number " + T[i].line_no);
                    }
                    if (terao(RT2,ref RT1))
                    {
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "(")
            {
                string RT2 = null;
                i++;
                if (f_exp(ref RT2))
                {

                    if (T[i].cls_part == ")")
                    {
                        i++;
                        if (terao(RT2,ref RT1))
                        {
                            return true;
                        }
                    }
                }
            }
            else if (T[i].cls_part == "++")
            {
                string RT = null, SB = null;

                i++;
                if (T[i].cls_part == "ID")
                {
                    N_ = T[i].val_part;
                    i++;

                    if (A(ref SB))
                    {
                        if (link == null)
                        {
                            RT = m.Mlookup(N_, linq);
                            if (RT == "null")
                            {
                                Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                            }
                            else if (RT.Contains('[') && SB.Contains('['))
                            {
                                string[] RT2 = RT.Split('>');
                                RT = RT2[0];
                            }
                            else if (!(RT.Contains('[') && SB.Contains('[')))
                            {
                                Console.WriteLine("Error : invalid use of " + N_ + " at line number " + T[i].line_no);
                            }
                            else if (RT != "null")
                            {

                            }
                        }
                        else
                        {
                            RT = m.flookup(N_,scope, link);
                            if (RT == "null")
                            {
                                RT = m.Mlookup(N_, linq);
                                if (RT == "null")
                                {
                                    Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                                }
                                else if (RT.Contains('[') && SB.Contains('['))
                                {
                                    string[] RT2 = RT.Split('>');
                                    RT = RT2[0];
                                }
                                else if (!(RT.Contains('[') && SB.Contains('[')))
                                {
                                    Console.WriteLine("Error : invalid use of " + N_ + " at line number " + T[i].line_no);
                                }
                                else if (RT != "null")
                                {

                                }
                            }
                            else
                            {
                                if (RT.Contains('[') && SB.Contains('['))
                                {
                                    string[] RT2 = RT.Split('>');
                                    RT = RT2[0];
                                }
                                else if (!(RT.Contains('[') && SB.Contains('[')))
                                {
                                    Console.WriteLine("Error : invalid use of " + N_ + " at line number " + T[i].line_no);
                                }
                                else if (RT != "null")
                                {

                                }
                            }
                        }
                        if (RT == "Num" || RT == "Dec")
                        {
                           // RT1 = RT;
                        }
                        else
                        {
                            Console.WriteLine(RT + "and" + RT1 + "Miss Match" + " at line number " + T[i].line_no);
                        }
                        if (terao(RT, ref RT1))
                        {
                            return true;
                        }
                    }
                }

            }
            else if (T[i].cls_part == "--")
            {
                string RT = null, SB = null; 
                i++;
                if (T[i].cls_part == "ID")
                {
                    i++; 
                    if (A(ref SB))
                    {
                        if (link == null)
                        {
                            RT = m.Mlookup(N_, linq);
                            if (RT == "null")
                            {
                                Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                            }
                            else if (RT.Contains('[') && SB.Contains('['))
                            {
                                string[] RT2 = RT.Split('>');
                                RT = RT2[0];
                            }
                            else if (!(RT.Contains('[') && SB.Contains('[')))
                            {
                                Console.WriteLine("Error : invalid use of " + N_ + " at line number " + T[i].line_no);
                            }
                            else if (RT != "null")
                            {

                            }
                        }
                        else
                        {
                            RT = m.flookup(N_, scope, link);
                            if (RT == "null")
                            {
                                RT = m.Mlookup(N_, linq);
                                if (RT == "null")
                                {
                                    Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                                }
                                else if (RT.Contains('[') && SB.Contains('['))
                                {
                                    string[] RT2 = RT.Split('>');
                                    RT = RT2[0];
                                }
                                else if (!(RT.Contains('[') && SB.Contains('[')))
                                {
                                    Console.WriteLine("Error : invalid use of " + N_ + " at line number " + T[i].line_no);
                                }
                                else if (RT != "null")
                                {

                                }
                            }
                            else
                            {
                                if (RT.Contains('[') && SB.Contains('['))
                                {
                                    string[] RT2 = RT.Split('>');
                                    RT = RT2[0];
                                }
                                else if (!(RT.Contains('[') && SB.Contains('[')))
                                {
                                    Console.WriteLine("Error : invalid use of " + N_ + " at line number " + T[i].line_no);
                                }
                                else if (RT != "null")
                                {

                                }
                            }
                        }
                        if (RT == "Num" || RT == "Dec")
                        {
                            // RT1 = RT;
                        }
                        else
                        {
                            Console.WriteLine(RT + "and" + RT1 + "Miss Match" + " at line number " + T[i].line_no);
                        }
                        if (terao(RT, ref RT1))
                        {
                            return true;
                        }
                    }
                }
            }
            else if (T[i].cls_part == "Num_Const" || T[i].cls_part == "Dec_Const" || T[i].cls_part == "string_Const" || T[i].cls_part == "Char_Const" || T[i].cls_part == "Bool_Const")
            {
                string RT2 = null;
                if (T[i].cls_part == "Num_Const")
                {
                    RT2 = "Num";
                }

                else if (T[i].cls_part == "Dec_Const")
                {
                    RT2 = "Dec";
                }

                else if (T[i].cls_part == "Bool_Const")
                {
                    RT2 = "Bool";
                }
                else if (T[i].cls_part == "string_Const")
                {
                    RT2 = "string";
                }
                else if (T[i].cls_part == "Char_Const")
                {
                    RT2 = "Char";
                }
              //  RT = RT2;
                i++;
                if (terao(RT2,ref RT1))
                {
                    return true;
                }
            }
            return false;
        }
        public bool AS_ST2(string vp, string N_, ref string RT)
        {
            string PL = null;
            string RT1 = null;
            string SB = null;
          
          if (T[i].cls_part == "++" || T[i].cls_part == "--" || T[i].cls_part == "["  || T[i].cls_part == "MOP" ||  T[i].cls_part == "MDM" || T[i].cls_part == "PM" || T[i].cls_part == "RO" || T[i].cls_part == "&&" || T[i].cls_part == "||"  || T[i].cls_part == "."  || T[i].cls_part == "ASS_OP" || T[i].cls_part == "=" )
            {

                if (A(ref SB))
                {
                    if (link == null)
                    {
                        RT1 = m.Mlookup(N_, linq);
                        if (RT1 == "null")
                        {
                            Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                        }
                        else if (SB == null)
                        {
                            if (!RT1.Contains('['))
                            {

                            }
                            else
                            {
                                Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }
                        }
                        else if (SB.Contains('['))
                        {
                            if (RT1.Contains('['))
                            {
                                string[] RT2 = RT1.Split('>');
                                RT1 = RT2[0];
                            }
                            else
                                Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                        }
                        else if (RT1 != "null")
                        {

                        }
                    }
                    else
                    {
                        RT1 = m.flookup(N_, scope, link);
                        if (RT1 == "null")
                        {
                            RT1 = m.Mlookup(N_, linq);
                            if (RT1 == "null")
                            {
                                Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                            }
                            else if (SB == null)
                            {
                                if (!RT1.Contains('['))
                                {

                                }
                                else
                                {
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                            }
                            else if (SB.Contains('['))
                            {
                                if (RT1.Contains('['))
                                {
                                    string[] RT2 = RT1.Split('>');
                                    RT1 = RT2[0];
                                }
                                else
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }
                            else if (RT1 != "null")
                            {

                            }
                        }
                        else
                        {
                            if (SB == null)
                            {
                                if (!RT1.Contains('['))
                                {

                                }
                                else
                                {
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                            }
                            else if (SB.Contains('['))
                            {
                                if (RT1.Contains('['))
                                {
                                    string[] RT2 = RT1.Split('>');
                                    RT1 = RT2[0];
                                }
                                else
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }
                            else if (RT1 != "null")
                            {

                            }
                        }
                    }
                    if (Inc_Dec(RT1))
                    {
                        if (AS_ST3(vp,N_,RT1,ref RT))
                        {
                            return true;
                        }
                    }
                }

            }
            else if (T[i].cls_part == "(")
            {
                i++;
                countPush = 0;
                if (Per_call(ref PL))
                { 
                    
                    generate(CreateVariable() + "= Call " + vp + "," + countPush);
                    countPush = 0;
                    if (T[i].cls_part == ")")
                    {
                        i++;
                        N_ += ">" + PL;
                        RT1 = m.Mlookup(N_, linq);
                        if (RT1 == "null")
                        {
                            Console.WriteLine(N_ + " Not Declared" + " at line number " + T[i].line_no);
                        }
                        if (terao(RT1,ref RT))
                        {
                            return true;
                        }
                    }
                }

            }

            return false;
        }
        public bool AS_ST3(string s1,string N_, string RT1, ref string RT)
        {
            
            if (MOP(N_, RT1, ref RT))
            {
                
                if (AS_ST4(s1,RT1,ref RT))
                {
                    return true;
                }
            }
            return false;
        }
        public bool AS_ST4(string s1, string RT1, ref string RT)
        {
            
            if (T[i].cls_part == "MDM" || T[i].cls_part == "PM" || T[i].cls_part == "RO" || T[i].cls_part == "&&" || T[i].cls_part == "||" || T[i].cls_part == ")" || T[i].cls_part == "." || T[i].cls_part == "," || T[i].cls_part == "}")
            {
                if (terao(RT1, ref RT))
                {
                    return true;
                }
            }
            else if (T[i].cls_part == "ASS_OP" || T[i].cls_part == "=")
            {
                if (init(s1,RT1))
                {
                    return true;
                }
            }
            return false;
        }
        public bool MOP(string N_, string RT1, ref string RT)
        {
            
            if (T[i].cls_part == "MOP")
            {
                if (m.Clookup(N_) == "Class")
                {

                }
                else
                {
                    Console.WriteLine("Member operator can not be apply on non object operand" + " at line number " + T[i].line_no);
                }
                i++;
                if (Mem(N_,RT1,ref RT))
                {
                    return true;
                }
            }
            else if (T[i].cls_part == "]" || T[i].cls_part == "=" || T[i].cls_part == "ASS_OP" || T[i].cls_part == "MDM" || T[i].cls_part == "PM" || T[i].cls_part == "RO" || T[i].cls_part == "&&" || T[i].cls_part == "||" || T[i].cls_part == ")" || T[i].cls_part == "." || T[i].cls_part == "," || T[i].cls_part == "}")
            {
                RT = RT1;
                return true;
            }
            return false;
        }
        public bool Mem(string N_, string RT1, ref string RT)
        {
            string N1 = null, RT2 = null;
            if (T[i].cls_part == "ID")
            {
                N1 = T[i].val_part;
                string s1 = T[i].val_part;
                i++;
                if (N(s1,N1,ref RT2))
                {
                    if (Inc_Dec(RT2))
                    {
                        if (MOP(N1,RT2,ref RT))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool N(string vp, string N_, ref string RT1)
        {
            string SB = null, PL = null, RT3 = null;

            if (T[i].cls_part == "[" || T[i].cls_part == "++" || T[i].cls_part == "}" || T[i].cls_part == "--" || T[i].cls_part == "MOP" || T[i].cls_part == "MDM" || T[i].cls_part == "PM" || T[i].cls_part == "RO" || T[i].cls_part == "&&" || T[i].cls_part == "||" || T[i].cls_part == ")" || T[i].cls_part == "." || T[i].cls_part == "," || T[i].cls_part == "ASS_OP" || T[i].cls_part == "=" || T[i].cls_part == "Create")
            {
                if (A(ref SB))
                {
                    if (link == null)
                    {
                        RT1 = m.Mlookup(N_, linq);
                        if (RT1 == "null")
                        {
                            Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                        }
                        else if (SB == null)
                        {
                            if (!RT1.Contains('['))
                            {

                            }
                            else
                            {
                                Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }
                        }
                        else if (SB.Contains('['))
                        {
                            if (RT1.Contains('['))
                            {
                                string[] RT2 = RT1.Split('>');
                                RT1 = RT2[0];
                            }
                            else
                                Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                        }
                        else if (RT1 != "null")
                        {

                        }
                    }
                    else
                    {
                        RT1 = m.flookup(N_,scope, link);
                        if (RT1 == "null")
                        {
                            RT1 = m.Mlookup(N_, linq);
                            if (RT1 == "null")
                            {
                                Console.WriteLine("Error undeclared");
                            }
                            else if (SB == null)
                            {
                                if (!RT1.Contains('['))
                                {

                                }
                                else
                                {
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                            }
                            else if (SB.Contains('['))
                            {
                                if (RT1.Contains('['))
                                {
                                    string[] RT2 = RT1.Split('>');
                                    RT1 = RT2[0];
                                }
                                else
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }
                            else if (RT1 != "null")
                            {

                            }
                        }
                        else
                        {
                            if (SB == null)
                            {
                                if (!RT1.Contains('['))
                                {

                                }
                                else
                                {
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                            }
                            else if (SB.Contains('['))
                            {
                                if (RT1.Contains('['))
                                {
                                    string[] RT2 = RT1.Split('>');
                                    RT1 = RT2[0];
                                }
                                else
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }
                            else if (RT1 != "null")
                            {

                            }
                        }
                    }
                    return true;
                }
            }
            else if (T[i].cls_part == "(")
            {
                countPush = 0;
                i++;
                if (Per_call(ref PL))
                {
                    generate(CreateVariable() + "= Call " + vp + "," + countPush);
                    countPush = 0;
                    if (T[i].cls_part == ")")
                    {
                        i++;
                        N_ += ">" + PL;
                        RT3 = m.Mlookup(N_, linq);
                        if (RT3 == "null")
                        {
                            Console.WriteLine("Function not declared " + N_);
                        }
                        else
                        {
                            RT1 = RT3;
                        }
                        return true;
                    }
                }
            }
            return false;
        }
        public bool Inc_Dec(string RT2)
        {

            if (T[i].cls_part == "++")
            {
                i++;
                if (T[i-2].cls_part == "ID")
                {
                    if (RT2 == "Num" || RT2 == "Dec")
                    {
                        
                    }
                    else
                    {
                        Console.WriteLine(RT2 + "Miss Match");
                    }
                }
                else
                {
                    Console.WriteLine("++ can not be apply to " + T[i-2].val_part);
                }
                return true;
            }
            else if (T[i].cls_part == "--")
            {
                i++;
                if (T[i - 2].cls_part == "ID")
                {
                    if (RT2 == "Num" || RT2 == "Dec")
                    {

                    }
                    else
                    {
                        Console.WriteLine(RT2 + "Miss Match");
                    }
                }
                else
                {
                    Console.WriteLine("-- can not be apply to " + T[i - 2].val_part);
                }
                return true;
            }
            else if (T[i].cls_part == "]" || T[i].cls_part == "=" || T[i].cls_part == "ASS_OP" || T[i].cls_part == "MOP" || T[i].cls_part == "MDM" || T[i].cls_part == "PM" || T[i].cls_part == "RO" || T[i].cls_part == "&&" || T[i].cls_part == "||" || T[i].cls_part == ")" || T[i].cls_part == "." || T[i].cls_part == "," || T[i].cls_part == "}")
            {

                return true;
            }
            return false;
        }
        //public bool Arr()
        //{
        //    if (T[i].cls_part == "DT")
        //    {
        //        i++;
        //        if (T[i].cls_part == "ID")
        //        {
        //            string s1 = T[i].val_part;
        //            i++;
        //            if (T[i].cls_part == "[")
        //            {
        //                i++;
        //                if (exp())
        //                {
        //                    if (T[i].cls_part == "]")
        //                    {
        //                        i++;
        //                        if (opt(s1))
        //                        {
        //                            return true;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}
        public bool opt1(string s1, string Tn, string A_M)
        {
            string N_ = null, Tn1 = null, temp = null;
            if (T[i].cls_part == ",")
            {
                i++;
                if (T[i].cls_part == "ID")
                {
                    N_ = T[i].val_part;
                    s1 = T[i].val_part;
                    i++;
                    if (T[i].cls_part == "[")
                    {
                        i++;
                        if (exp(ref Tn1))
                        {
                            if (Tn1 != "Num")
                            {
                                Console.WriteLine("Invalid Indexer");
                            }
                            if (T[i].cls_part == "]")
                            {
                                i++;
                                string r;
                                if (link == null)
                                {
                                    r = m.Mlookup(N_, linq);
                                    if (r == "null")
                                    {
                                        temp = Tn + ">[]";
                                        m.MethodInsert(N_, Tn, scope);

                                    }
                                    else
                                    {
                                        Console.WriteLine("Redeclaration");
                                    }
                                }
                                else
                                {
                                    r = m.Mlookup(N_, linq);

                                    if (r == "null")
                                    {
                                        r = m.flookup(N_, scope, link);
                                        if (r == "null")
                                        {
                                            temp = Tn + ">[]";
                                            m.MethodInsert(N_, temp, scope);

                                        }
                                        else
                                        {
                                            Console.WriteLine("Redeclaration");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(N_ + " Already exist in Parent scope");
                                    }
                                }

                                if (opt(s1,Tn,A_M))
                                {
                                    return true;
                                }
                            }
                        }
                    }

                }
            }
            else if (T[i].cls_part == ".")
            {
                return true;
            }
            return false;
        }
               
        
        public bool opt(string s1, string Tn, string A_M)
        {
            if (T[i].cls_part == "=")
            {
                i++;
                string v1 = CreateVariable();
                generate(s1 + "=" + v1);
                if (T[i].cls_part == "{")
                {
                    i++;
                    if (Val_List(v1,Tn))
                    {
                        if (T[i].cls_part == "}")
                        {
                            i++;
                            if (opt1(s1,Tn,A_M))
                            {
                                return true;
                            }
                        }
                    }
                }

            }
            else if (T[i].cls_part == ",")
            {
                if (opt1(s1,Tn,A_M))
                {
                    return true;
                }
            }
            else if (T[i].cls_part == ".")
            {
                return true;
            }
            return false;
        }
        public bool Val_List(string s1, string Tn)
        {
            string Tn1 = null;
            if (f_exp(ref Tn1))
            {
                if (Tn1 != Tn)
                {
                    Console.WriteLine(Tn + "and" + Tn1 + "Miss match");
                }
                if (Next(Tn))
                {
                    return true;
                }
            }
            return false;
        }
        public bool Next(string Tn)
        {
            string Tn1 = null;
            if (T[i].cls_part == ",")
            {
                i++;
                if (f_exp(ref Tn1))
                {
                    if (Tn1 != Tn)
                    {
                        Console.WriteLine(Tn + "and" + Tn1 + "Miss match");
                    }
                    if (Next(Tn))
                    {
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "}")
            {
                return true;
            }
            return false;
        }
        public bool Check(string RT)
        {

            string Tn = null;
            if (T[i].cls_part == "Check")
            {
                i++;
                if (T[i].cls_part == "(")
                {
                    i++;
                    if (f_exp(ref Tn))
                    {
                        if (Tn != "Bool")
                        {
                            Console.WriteLine("Error : Bool expected");
                        }
                        NL1 = CreateLable();
                        generate("if(N==false)");
                        generate("Jmp " + NL1);
                        if (T[i].cls_part == ")")
                        {
                            i++;
                            if (T[i].cls_part == "{")
                            {
                                scope = createScope();
                                i++;
                                if (Body(RT))
                                {
                                    NL2 = CreateLable();
                                    generate("Jmp " + NL2);

                                    if (T[i].cls_part == "}")
                                    {
                                        DestroyVariableScope();
                                        DestroyScope();
                                        i++;
                                        generate(NL1 + " :");
                                        if (then(NL2, RT))
                                        {
                                            if (def(RT))
                                            {
                                                generate(NL2 + " :");
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        public bool then(string NL2, string RT)
        {
            string Tn = null;
            if (T[i].cls_part == "Then")
            {
                i++;
                if (T[i].cls_part == "(")
                {
                    i++;
                    if (f_exp(ref Tn))
                    {
                        if (Tn != "Bool")
                        {
                            Console.WriteLine("Error : Bool expected");
                        }
                        NL1 = CreateLable();
                        generate("if(N==false)");
                        generate("Jmp " + NL1);
                        if (T[i].cls_part == ")")
                        {
                            i++;
                            if (T[i].cls_part == "{")
                            {
                                scope = createScope();
                                i++;
                                if (Body(RT))
                                {
                                    generate("Jmp " + NL2);
                                    if (T[i].cls_part == "}")
                                    {
                                        DestroyVariableScope();
                                        DestroyScope();
                                        i++;
                                        generate(NL1 + " :");
                                        if (then(NL2, RT))
                                        {
                                            return true;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            else if (T[i].cls_part == "Continue" || T[i].cls_part == "Break" || T[i].cls_part == "return" || T[i].cls_part == "Default" || T[i].cls_part == "++" || T[i].cls_part == "--" || T[i].cls_part == "ID" || T[i].cls_part == "DT" || T[i].cls_part == "Loop" || T[i].cls_part == "Check" || T[i].cls_part == "}")
            {
                return true;
            }
            return false;
        }
        public bool def(string RT)
        {
            if (T[i].cls_part == "Default")
            {
                i++;
                if (T[i].cls_part == "{")
                {
                    scope = createScope();
                    i++;
                    if (Body(RT))
                    {
                        if (T[i].cls_part == "}")
                        {
                            DestroyVariableScope();
                            DestroyScope();
                            i++;
                            return true;
                        }
                    }
                }
            }
            else if (T[i].cls_part == "Continue" || T[i].cls_part == "Break" || T[i].cls_part == "return" || T[i].cls_part == "++" || T[i].cls_part == "--" || T[i].cls_part == "ID" || T[i].cls_part == "DT" || T[i].cls_part == "Loop" || T[i].cls_part == "Check" || T[i].cls_part == "}")
            {
                return true;
            }
            return false;
        }
        //public bool Meth_cre()
        //{

        //    if (AM())
        //    {

        //        if (T[i].cls_part == "ID")
        //        {
        //            i++;
        //            if (T[i].cls_part == ":")
        //            {
        //                i++;
        //                if (T[i].cls_part == "DT" || T[i].cls_part == "None")
        //                {

        //                    i++;
        //                    if (T[i].cls_part == "(")
        //                    {
        //                        i++;

        //                        if (Per_cre())
        //                        {
        //                            if (T[i].cls_part == ")")
        //                            {
        //                                i++;
        //                                if (T[i].cls_part == "{")
        //                                {
        //                                    i++;
        //                                    if (Body())
        //                                    {
        //                                        if (T[i].cls_part == "}")
        //                                        {
        //                                            i++;
        //                                            return true;
        //                                        }
        //                                    }

        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    return false;
        //}
        public bool Per_cre(ref string PL_D, ref string PL_V)
        {

            if (T[i].cls_part == "DT")
            {
                PL_D = T[i].val_part;
                i++;
                if (T[i].cls_part == "ID")
                {
                    PL_V = T[i].val_part;
                    i++;

                    if (Per_L(ref PL_D, ref PL_V))
                    {
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == ")")
            {
                PL_D = "";
                PL_V = ""; 
                return true;
            }
            return false;
        }
        public bool Per_L(ref string PL_D, ref string PL_V)
        {
            if (T[i].cls_part == ",")
            {
                i++;
                if (T[i].cls_part == "DT")
                {
                    PL_D +=  "," + T[i].val_part;
                    i++;
                    if (T[i].cls_part == "ID")
                    {
                        PL_V += "," + T[i].val_part;
                        i++;
                        if (Per_L(ref PL_D, ref PL_V))
                        {
                            return true;
                        }
                    }
                }
            }
            else if (T[i].cls_part == ")")
            {
                return true;
            }
            return false;
        }
        public bool 
Loop( string RT)
        {

            if (T[i].cls_part == "Loop")
            {
                i++;
                if (T[i].cls_part == "(")
                {
                    i++;

                    if (Case(RT))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool Case(string RT)
        {
            string N_ = null, Tn = null, Tn1 = null, Tn2 = null;
            if (T[i].cls_part == "ID" || T[i].cls_part == "!" || T[i].cls_part == "(" || T[i].cls_part == "++" || T[i].cls_part == "--" || T[i].cls_part == "Num" || T[i].cls_part == "Dec" || T[i].cls_part == "string" || T[i].cls_part == "Bool")
            {
                if (f_exp(ref Tn1))
                {
                    if (Tn1 != "Bool")
                    {
                        Console.WriteLine("Error : Bool expected" + " at line number " + T[i].line_no );
                    }
                    if (T[i].cls_part == ")")
                    {
                        i++;
                        if (T[i].cls_part == "{")
                        {
                            scope = createScope();
                            i++;
                            if (Body(RT))
                            {
                                if (T[i].cls_part == "}")
                                {
                                    DestroyVariableScope();
                                    DestroyScope();
                                    i++;
                                    if (T[i].cls_part == ".")
                                    {
                                        i++;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            else if (T[i].cls_part == "DT")
            {
                Tn = T[i].val_part;
                i++;
                if (T[i].cls_part == "ID")
                {
                    N_ = T[i].val_part;
                    string s1 = T[i].val_part;
                    i++;
                    Tn2 = m.flookup(N_, scope, link);
                    if (Tn2 != "null")
                    {
                        Console.WriteLine("Redeclaration" + " at line number " + T[i].line_no);
                    }
                    else
                    {
                        m.MethodInsert(N_, Tn, scope);
                    }
                    
                    if (init(s1,Tn))
                    {

                     if (T[i].cls_part == ".")
                        {
                            string NL1;
                            string NL2;
                            i++;
                            NL1 = CreateLable();
                            generate(NL1 + " :");
                            if (f_exp(ref Tn1))
                            {
                                
                                if (Tn1 != "Bool")
                                {
                                    Console.WriteLine("Error : Bool expected" + " at line number " + T[i].line_no);
                                }
                                generate("if(N==false)");
                                if (T[i].cls_part == ".")
                                {
                                    i++;
                                    NL2 = CreateLable();
                                    generate("Jmp " + NL2);

                                    if (Cont(s1,ref Tn1))
                                    {
                                        if (Tn1 != Tn)
                                        {
                                            Console.WriteLine(Tn + "and" + Tn1 + "Miss match" + " at line number " + T[i].line_no);
                                        }
                                        if (T[i].cls_part == ")")
                                        {
                                            i++;
                                            if (T[i].cls_part == "{")
                                            {
                                                scope = createScope();
                                                i++;
                                                if (Body(RT))
                                                {
                                                    generate("Jmp " + NL1);
                                                    if (T[i].cls_part == "}")
                                                    {
                                                        DestroyVariableScope();
                                                        DestroyScope();
                                                        i++;
                                                        generate(NL2 + " :");
                                                        return true;
                                                        
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (T[i].cls_part == ")")
            {
                i++;
                if (T[i].cls_part == "{")
                {
                    scope = createScope();
                    i++;

                    if (Body(RT))
                    {
                        if (T[i].cls_part == "}")
                        {
                            DestroyVariableScope();
                            DestroyScope();
                            i++;
                            if (T[i].cls_part == "Loop")
                            {
                                i++;
                                if (T[i].cls_part == "(")
                                {
                                    i++;
                                    if (f_exp(ref Tn1))
                                    {
                                        if (Tn1 != "Bool")
                                        {
                                            Console.WriteLine("Error : Bool Expected" + " at line number " + T[i].line_no);
                                        }
                                        if (T[i].cls_part == ")")
                                        {
                                            i++;
                                            if (T[i].cls_part == ".")
                                            {
                                                i++;
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
      
        public bool Cont(string s1, ref string Tn2)
        {
            if (AS_ST(s1,ref Tn2))
            {
                return true;
            }
            return false;
        }
        public bool S_L(string RT)
        {
            string v1, Tn = "null" , N_ = null, RT1 = null , SB = null ;

            if (T[i].cls_part == "Continue")
            {
                i++;
                if (T[i].cls_part == ".")
                {
                    i++;
                    return true;
                }
            }
            else if (T[i].cls_part == "Break")
            {
                i++;
                if (T[i].cls_part == ".")
                {
                    i++;
                    return true;
                }
            }
            else if (T[i].cls_part == "return")
            {
                i++;
                if (f_exp(ref Tn))
                {
                    if (RT == "None")
                    {
                        Console.WriteLine("Can not return value with None return type" + " at line number " + T[i].line_no);
                    }
                    else if (RT != Tn)
                    {
                        Console.WriteLine(Tn + " and " + RT + " return type miss match" + " at line number " + T[i].line_no);
                    }
                    if (T[i].cls_part == ".")
                    {
                        i++;
                        Flag = true;
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "ID")
            {
                N_ = T[i].val_part;
                Tn = m.flookup(N_,scope, link);
                if (Tn == "null")
                {
                   Tn =  m.Mlookup(N_, linq);
                    if (Tn == "null")
                    {
                        Console.WriteLine("Undeclared Variable " + N_ + " at line number " + T[i].line_no);
                    }
                   
                }

                v1 = T[i].val_part;
                i++;

                if (S_L1(v1,N_, Tn))
                {
                    return true;
                }

            }
            else if (T[i].cls_part == "++")
            {
                
                i++;
                if (T[i].cls_part == "ID")
                {
                    N_ = T[i].val_part;
                    string temp = CreateVariable();
                    generate(temp + "=" + T[i].val_part + "+" + "1");
                    generate(T[i].val_part + "=" + temp);
                    i++;
                    if (A(ref SB))
                    {
                        if (link == null)
                        {
                            RT1 = m.Mlookup(N_, linq);
                            if (RT1 == "null")
                            {
                                Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                            }
                            else if (SB == null)
                            {
                                if (!RT1.Contains('['))
                                {

                                }
                                else
                                {
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                            }
                            else if (SB.Contains('['))
                            {
                                if (RT1.Contains('['))
                                {
                                    string[] RT2 = RT1.Split('>');
                                    RT1 = RT2[0];
                                }
                                else
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }
                            else if (RT1 != "null")
                            {

                            }
                        }
                        else
                        {
                            RT1 = m.flookup(N_,scope, link);
                            if (RT1 == "null")
                            {
                                RT1 = m.Mlookup(N_, linq);
                                if (RT1 == "null")
                                {
                                    Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                                }
                                else if (SB == null)
                                {
                                    if (!RT1.Contains('['))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                    }
                                }
                                else if (SB.Contains('['))
                                {
                                    if (RT1.Contains('['))
                                    {
                                        string[] RT2 = RT1.Split('>');
                                        RT1 = RT2[0];
                                    }
                                    else
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                                else if (RT1 != "null")
                                {

                                }
                            }
                            else
                            {
                                if (SB == null)
                                {
                                    if (!RT1.Contains('['))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                    }
                                }
                                else if (SB.Contains('['))
                                {
                                    if (RT1.Contains('['))
                                    {
                                        string[] RT2 = RT1.Split('>');
                                        RT1 = RT2[0];
                                    }
                                    else
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                                else if (RT1 != "null")
                                {

                                }
                            }
                        }
                        if (RT1 == "Num" || RT1 == "Dec")
                        {
                            RT = RT1;
                        }
                        else
                        {
                            Console.WriteLine(RT + " and " + RT1 + "Miss Match" + " at line number " + T[i].line_no);
                        }
                        if (T[i].cls_part == ".")
                        {
                            i++;
                            return true;
                        }
                    }
                }
            }
            else if (T[i].cls_part == "--")
            {
                i++;
                if (T[i].cls_part == "ID")
                {
                    N_ = T[i].val_part;
                    string temp = CreateVariable();
                    generate(temp + "=" + T[i].val_part + "-" + "1");
                    generate(T[i].val_part + "=" + temp);
                    i++;
                    if (A(ref SB))
                    {
                        if (link == null)
                        {
                            RT1 = m.Mlookup(N_, linq);
                            if (RT1 == "null")
                            {
                                Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                            }
                            else if (SB == null)
                            {
                                if (!RT1.Contains('['))
                                {

                                }
                                else
                                {
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                            }
                            else if (SB.Contains('['))
                            {
                                if (RT1.Contains('['))
                                {
                                    string[] RT2 = RT1.Split('>');
                                    RT1 = RT2[0];
                                }
                                else
                                    Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                            }
                            else if (RT1 != "null")
                            {

                            }
                        }
                        else
                        {
                            RT1 = m.flookup(N_,scope, link);
                            if (RT1 == "null")
                            {
                                RT1 = m.Mlookup(N_, linq);
                                if (RT1 == "null")
                                {
                                    Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                                }
                                else if (SB == null)
                                {
                                    if (!RT1.Contains('['))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                    }
                                }
                                else if (SB.Contains('['))
                                {
                                    if (RT1.Contains('['))
                                    {
                                        string[] RT2 = RT1.Split('>');
                                        RT1 = RT2[0];
                                    }
                                    else
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                                else if (RT1 != "null")
                                {

                                }
                            }
                            else
                            {
                                if (SB == null)
                                {
                                    if (!RT1.Contains('['))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                    }
                                }
                                else if (SB.Contains('['))
                                {
                                    if (RT1.Contains('['))
                                    {
                                        string[] RT2 = RT1.Split('>');
                                        RT1 = RT2[0];
                                    }
                                    else
                                        Console.WriteLine("Error : " + N_ + " invalid use with " + RT1);
                                }
                                else if (RT1 != "null")
                                {

                                }
                            }
                        }
                        if (RT1 == "Num" || RT1 == "Dec")
                        {
                            RT = RT1;
                        }
                        else
                        {
                            Console.WriteLine("Miss Match" + " at line number " + T[i].line_no);
                        }
                        if (T[i].cls_part == ".")
                        {
                            i++;
                            return true;
                        }
                    }
                }
            }
            else if (T[i].cls_part == "DT")
            {
                Tn = T[i].val_part;
                i++;
                if (T[i].cls_part == "ID")
                {
                    N_ = T[i].val_part;
                    v1 = T[i].val_part;
                    i++;

                    if (S_L2(v1,Tn,N_))
                    {
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "Loop")
            {
                if (Loop(RT))
                {
                    return true;
                }
            }
            else if (T[i].cls_part == "Check")
            {
                if (Check(RT))
                {
                    return true;
                }
            }
            return false;
        }
        public bool S_L1(string vp, string N_, string Tn)
        {
            string PL = null, SB = null;
            if (T[i].cls_part == "++")
            {
                if (Tn == "Num" || Tn == "Dec")
                {
                    
                }
                else
                {
                    Console.WriteLine("Miss Match" + " at line number " + T[i].line_no);
                }

                string temp = CreateVariable();
                generate(temp + "=" + vp + "+" + "1");
                generate(vp + "=" + temp);
                i++;
                if (T[i].cls_part == ".")
                {
                    i++;
                    return true;
                }
            }
            else if (T[i].cls_part == "--")
            {
                if (Tn == "Num" || Tn == "Dec")
                {

                }
                else
                {
                    Console.WriteLine("Miss Match" + " at line number " + T[i].line_no);
                }
                string temp = CreateVariable();
                generate(temp + "=" + vp + "-" + "1");
                generate(vp + "=" + temp);
                i++;
                if (T[i].cls_part == ".")
                {
                    i++;
                    return true;
                }
            }
            else if (T[i].cls_part == "(")
            {
                i++;
                countPush = 0;
                if (Per_call(ref PL))
                {
                    N_ += ">" + PL;
                     Tn = m.Mlookup(N_, linq);
                    if (Tn == "null")
                    {
                        Console.WriteLine(N_ + " Not Declared" + " at line number " + T[i].line_no);
                    }
                    
                    if (T[i].cls_part == ")")
                    {

                        generate(CreateVariable() + "= Call " + vp + "," + countPush);
                        countPush = 0;
                        i++;
                        if (T[i].cls_part == ".")
                        {
                            i++;
                            return true;
                        }
                    }
                }
            }
            else if (T[i].cls_part == "[" || T[i].cls_part == "++" || T[i].cls_part == "}" || T[i].cls_part == "--" || T[i].cls_part == "MOP" || T[i].cls_part == "MDM" || T[i].cls_part == "PM" || T[i].cls_part == "RO" || T[i].cls_part == "&&" || T[i].cls_part == "||" || T[i].cls_part == ")" || T[i].cls_part == "." || T[i].cls_part == "," || T[i].cls_part == "ASS_OP" || T[i].cls_part == "=" || T[i].cls_part == "Create")
            {

                if (A(ref SB))
                {
                    if (link == null)
                    {
                        Tn = m.Mlookup(N_, linq);
                        if (Tn == "null")
                        {
                            Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                        }
                        else if (Tn.Contains('[') && SB.Contains('['))
                        {
                            string[] RT2 = Tn.Split('>');
                            Tn = RT2[0];
                        }
                        else if (!(Tn.Contains('[') && SB.Contains('[')))
                        {
                            Console.WriteLine("Error : invalid use of " + N_ + " at line number " + T[i].line_no);
                        }
                        else if (Tn != "null")
                        {

                        }
                    }
                    else
                    {
                        Tn = m.flookup(N_, scope, link);
                        if (Tn == "null")
                        {
                            Tn = m.Mlookup(N_, linq);
                            if (Tn == "null")
                            {
                                Console.WriteLine("Error undeclared" + " at line number " + T[i].line_no);
                            }
                            else if (Tn.Contains('[') && SB.Contains('['))
                            {
                                string[] RT2 = Tn.Split('>');
                                Tn = RT2[0];
                            }
                            else if (!(Tn.Contains('[') && SB.Contains('[')))
                            {
                                Console.WriteLine("Error : invalid use of " + N_ + " at line number " + T[i].line_no);
                            }
                            else if (Tn != "null")
                            {

                            }
                        }
                        else
                        {
                            if (Tn.Contains('[') && SB.Contains('['))
                            {
                                string[] RT2 = Tn.Split('>');
                                Tn = RT2[0];
                            }
                            else if (!(Tn.Contains('[') && SB.Contains('[')))
                            {
                                Console.WriteLine("Error : invalid use of " + N_ + " at line number " + T[i].line_no);
                            }
                            else if (Tn != "null")
                            {

                            }
                        }
                    }
                  
                    if (z(vp,N_,Tn))
                    {
                        
                        if (T[i].cls_part == ".")
                        { 
                            i++;
                            return true;
                        }
                    }
                }

            }
            else if (T[i].cls_part == "ID")
            {
                
                if (obj_dec(N_,"null"))
                {
                    if (T[i].cls_part == ".")
                    {
                        i++;
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "Create")
            {
                if (obj_init(Tn,"null"))
                {
                    if (T[i].cls_part == ".")
                    {
                        i++;
                        return true;
                    }
                }
            }
            return false;
        }
        public bool S_L2(string vp, string Tn, string N_)
        {
            string Tn2 = null, temp = null;
            if (T[i].cls_part == "[")
            {
                i++;
                if (exp(ref Tn2))
                {
                    if (Tn2 != "null")
                    {
                        Console.WriteLine("Invalid Indexer type" + " at line number " + T[i].line_no);
                    }
                    if (T[i].cls_part == "]")
                    {
                        i++;
                        string r;
                        if (link == null)
                        {
                            r = m.Mlookup(N_, linq);
                            if (r == "null")
                            {
                                temp = Tn + ">[]";
                                m.MethodInsert(N_, Tn, scope);

                            }
                            else
                            {
                                Console.WriteLine("Redeclaration" + " at line number " + T[i].line_no);
                            }
                        }
                        else
                        {
                            r = m.Mlookup(N_, linq);

                            if (r == "null")
                            {
                                r = m.flookup(N_, scope, link);
                                if (r == "null")
                                {
                                    temp = Tn + ">[]";
                                    m.MethodInsert(N_, temp, scope);

                                }
                                else
                                {
                                    Console.WriteLine("Redeclaration" + " at line number " + T[i].line_no);
                                }
                            }
                            else
                            {
                                Console.WriteLine(N_ + " Already exist in Parent scope" + " at line number " + T[i].line_no);
                            }
                        }
                        if (opt(vp, Tn, "null"))
                        {
                            if (T[i].cls_part == ".")
                            {
                                i++;
                                return true;
                            }
                        }

                    }
                }
            }
            else if (T[i].cls_part == "," || T[i].cls_part == "=" || T[i].cls_part == ".")
            {
                string r;
                if (link == null)
                {
                    r = m.Mlookup(N_, linq);
                    if (r == "null")
                    {
                        m.MethodInsert(N_, Tn, scope);

                    }
                    else
                    {
                        Console.WriteLine("Redeclaration" + " at line number " + T[i].line_no);
                    }
                }
                else
                {
                    r = m.Mlookup(N_, linq);
                    //Console.WriteLine(N_ + " " + r);
                    if (r == "null")
                    { 
                        r = m.flookup(N_, scope, link);
                        if (r == "null")
                        {
                            //temp = Tn + ">[]";
                            m.MethodInsert(N_, Tn, scope);
                        }
                        else
                        {
                            Console.WriteLine("Redeclaration" + " at line number " + T[i].line_no);
                        }
                    }
                    else
                    {
                        
                        Console.WriteLine(N_ + " Already exist in Parent scope" + " at line number " + T[i].line_no);
                    }
                    if (go(vp, Tn,"null"))
                    {
                        if (T[i].cls_part == ".")
                        {
                            i++;
                            return true;
                        }
                    }
                }
               
            }
            return false;
        }

        public bool z(string s1, string N_, string RT)
        {
            string RT1 = null;
            if (T[i].cls_part == "MOP" || T[i].cls_part == "ASS_OP" || T[i].cls_part == "=")
            {
                if (Obj_ref(N_,RT,ref RT1))
                {
                    
                    if (init(s1,RT1))
                    {
                       
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "Create")
            {
                if (obj_init(RT, "null"))
                {
                    return true;
                }
            }
            return false;
        }
        public bool M_L(string TY)
        {
            if (T[i].cls_part == "DT" || T[i].cls_part == "ID" || T[i].cls_part == "--" || T[i].cls_part == "++" || T[i].cls_part == "Check" || T[i].cls_part == "Loop" || T[i].cls_part == "return" || T[i].cls_part == "Break" || T[i].cls_part == "Continue")
            {
                if (S_L(TY))
                {
                    if (M_L(TY))
                    {
                        return true;
                    }
                }
            }
            else if (T[i].cls_part == "}")
            {
                if (TY != "None"&& Flag==false && scope == 0)
                {
                    Console.WriteLine("All path not return a value" + " at line number " + T[i].line_no);
                }
                Flag = false;
                return true;
            }
           
            return false;
        }
        public bool Body(string TY)
        {

            if (T[i].cls_part == "DT" || T[i].cls_part == "ID" || T[i].cls_part == "--" || T[i].cls_part == "++" || T[i].cls_part == "Check" || T[i].cls_part == "Loop" || T[i].cls_part == "return" || T[i].cls_part == "Break" || T[i].cls_part == "Continue" || T[i].cls_part == "}")
            {
                if (M_L(TY))
                {
                    return true;
                }
            }
            return false;
        }
        public bool T1(ref string RT2)
        {
            string RT1 = null;
            if (F(ref RT1))
            {
               
               
                if (T_(RT1, ref RT2))
                {

                    return true;
                }
            }

            return false;
        }
        public bool RE(ref string RT2)
        {
            string RT1 = null;
            if (T[i].cls_part == "!" || T[i].cls_part == "ID" || T[i].cls_part == "(" || T[i].cls_part == "++" || T[i].cls_part == "--" || T[i].cls_part == "Num_Const" || T[i].cls_part == "Dec_Const" || T[i].cls_part == "string_Const" || T[i].cls_part == "Char_Const" || T[i].cls_part == "Bool_Const")
            {
                if (exp(ref RT1))
                {
                    if (RE_(RT1,ref RT2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool And(ref string RT2)
        {
            string RT1 = null;
            if (T[i].cls_part == "!" || T[i].cls_part == "ID" || T[i].cls_part == "(" || T[i].cls_part == "++" || T[i].cls_part == "--" || T[i].cls_part == "Num_Const" || T[i].cls_part == "Dec_Const" || T[i].cls_part == "string_Const" || T[i].cls_part == "Char_Const" || T[i].cls_part == "Bool_Const")
            {
                if (RE(ref RT1))
                {
                    if (And_(RT1,ref RT2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
