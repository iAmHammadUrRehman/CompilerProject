using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerProject
{
        class ClassTable
        {
            public string name;
            public string AccessModifier;
            public string Type;
            public List<MemberTable> MemberLink;

            public ClassTable(string name, string AccessModifier, string Type, List<MemberTable> MemberLink)
            {
                this.name = name;
                this.AccessModifier = AccessModifier;
                this.Type = Type;
                this.MemberLink = MemberLink;

            }
        }


        class MemberTable
        {

            public string name;
            public string AccessModifier;
            public string Type;
            public List<MethodTable> MethodLink;

            public MemberTable(string name, string AccessModifier, string Type, List<MethodTable> MethodLink)
            {
                this.name = name;
                this.AccessModifier = AccessModifier;
                this.Type = Type;
                this.MethodLink = MethodLink;
            }
            public MemberTable(string name, string AccessModifier, string Type )
            {
                this.name = name;
                this.AccessModifier = AccessModifier;
                this.Type = Type;
               // this.MethodLink = MethodLink;
            }

            
        }
        class MethodTable
        {
            public string name;
            public string Type;
            public int Scope;

            public MethodTable(string name, string Type, int Scope)
            {
                this.name = name;
                this.Type = Type;
                this.Scope = Scope;

            }
        }
        class EnumTable
        {

            public string name;
            public EnumTable(string name)
            {
                this.name = name;
            }
        }

        class MainClassTable
        {
            public List<ClassTable> Classlist;
            public List<MemberTable> Memberlist;
            public List<MethodTable> Methodlist;
            public MainClassTable()
            {
                Classlist = new List<ClassTable>();
            }

            public List<MemberTable> getmemberlink(string name)
            {
                foreach (var item in Classlist)
                {
                    if (item.name == name)
                    {
                        return item.MemberLink;
                    }

                }
                return null;
            }


            public List<MethodTable> getmethodlink(string name)
            {

                foreach (var item in Memberlist)
                {
                    if (item.name == name)
                    {
                        return item.MethodLink;
                    }

                }
                return null;
            }

            public string Clookup(string name)
            {
                foreach (var item in Classlist)
                {
                    if (item.name == name)
                    {
                        return item.Type;
                    }

                }

                return "null";
            }

            public string Mlookup(string name, List<MemberTable> mtable)
            {
                if (name.Contains('>'))
                {
                    string[] name1 = name.Split('>');

                    if (name1.Length < 2)
                    {

                        name = name1[0];
                    }
                    
                }
                if (mtable != null)
                {
                    foreach (var item in mtable)
                    {
                        if (item.name == name)
                        {
                            return item.Type;
                        }

                    }
                }

                return "null";
            }

            public string flookup(string name, int scope ,  List<MethodTable> fntable)
            {

                if (fntable != null)
                {
                    foreach (var item in fntable)
                    {
                        for (int i = scope; i >= 0; i--)
                        {

                            if (item.name == name && item.Scope == i)
                            {
                                return item.Type;
                            }
                        }

                    }
                }

                return "null";
            }
            public void ClassInsert(string name, string AccessModifier, string Type)
            {
                Memberlist = new List<MemberTable>();
                Classlist.Add(new ClassTable(name, AccessModifier, Type, Memberlist));
            }


            public void MemberInsert(string name, string AccessModifier, string Type)
            {
               // List<MethodTable> temp = new List<MethodTable>();
                try
                {
                    Memberlist.Add(new MemberTable(name, AccessModifier, Type, null));
                }
                catch { }
            }



            public void MemberInsert_Fn(string name, string AccessModifier, string Type)
            {
                Methodlist = new List<MethodTable>();
                Memberlist.Add(new MemberTable(name, AccessModifier, Type, Methodlist));
            }


            public void MethodInsert(string name, string Type, int Scope)
            {
                try
                {
                    Methodlist.Add(new MethodTable(name, Type, Scope));
                }
                catch { }
            }

        }

        //class Program
        //{
        //    static void Main(string[] args)
        //    {
        //        MainClassTable m = new MainClassTable();

        //        m.ClassInsert("abc", "public", "class");
        //        m.ClassInsert("hammad", "public", "class");


        //        string s1 = m.Clookup("abc");
        //        string s3 = m.Clookup("hammad");

        //        Console.WriteLine(s1);

        //        m.MemberInsert("fn", "public", "Num");
        //        m.MemberInsert_Fn("method>num,num", "public", "Dec");
        //        List<MethodTable> mlink = m.getmethodlink("method>num,num");
        //        m.MethodInsert("affan", "Dec", 1);
        //        Console.WriteLine(m.flookup("affan", mlink));


        //        List<MemberTable> link = m.getmemberlink("hammad");

        //        s1 = m.Mlookup("fn", link);

        //        if (s1 == null)
        //            //  Console.WriteLine("RED");
        //            //  link = m.getmemberlink("hammad");
        //            //s1 = m.Mlookup("fn", link);
        //            Console.WriteLine("no entry");
        //        else
        //            Console.WriteLine(s1);
        //    }
        //}
    }


