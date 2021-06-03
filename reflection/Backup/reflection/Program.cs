using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace reflection
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load an assembly from file
            Assembly myAssembly = Assembly.LoadFrom("C:\\cpp\\reflection\\testclass\\testclass\\bin\\Release\\testclass.dll");
            // Get the types contained in the assembly and print their names
            Type[] types = myAssembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.IsInterface)
                {
                    Console.WriteLine("\r\n" + type.FullName + " is an interface");
                }
                else if (type.IsClass)
                {
                    // Get and print the list of its parent class and interfaces
                    Console.WriteLine("\r\n" + type.FullName + " is a class whose base class is " + type.BaseType.FullName);
                    Console.WriteLine("\t It implements the following interfaces");
                    foreach (Type interfaceType in type.GetInterfaces())
                    {
                        Console.WriteLine("\t\t {0}", interfaceType.FullName);
                    }
                }

                Type myClassType = myAssembly.GetType(type.FullName);

                // Print each member information
                display_members(myClassType);


                //  Console.ReadLine();
                display_fields(myClassType);

                List<method> listmethods= new List<method>();
                listmethods = display_methods(myClassType);

               // MyClass myObj = (MyClass)myAssembly.CreateInstance(type.FullName, false, BindingFlags.Default, null, new object[] { 5 }, null, null);

                test_methods(myClassType,listmethods);

            }
            Console.ReadLine();


//TODO:
//            static void Main(string[] args)
//{
//    // Load an assembly from file
//    Assembly myAssembly = Assembly.LoadFrom("ReflectionTestAssembly.dll");
//    // Instantiate object
//    MyClass myObj = (MyClass) myAssembly.CreateInstance(
//"ReflectionTestAssembly.MyClass", 
//false, BindingFlags.Default, null, 
//new object[] {5}, null, null);
//    // Get the Type object for this class
//    Type myClassType = myAssembly.GetType("ReflectionTestAssembly.MyClass");
//    // Get the field value 
//    double piValue = (double) myClassType.InvokeMember("PI", 
//BindingFlags.GetField, null, myObj, null);
//    Console.WriteLine("PI = {0}", piValue);
//    // Set the property
//    myClassType.InvokeMember("Number", BindingFlags.SetProperty, null, 
//myObj, new object[] { 47 });
//    // Get the property value
//    int numValue = (int) myClassType.InvokeMember("Number", 
//                         BindingFlags.GetProperty, null, myObj, null);
//    Console.WriteLine("Number = {0}", numValue);
//    // Invoke a parameterless method that returns nothing
//    myClassType.InvokeMember("SayGreeting", BindingFlags.InvokeMethod, 
//null, myObj, null);
//    // Inovoke a method that takes an integer type parameter and returns nothing
//    myClassType.InvokeMember("Fun", BindingFlags.InvokeMethod, null, myObj, 
//new object[] {4});
//    // Invoke a parameterless method that returns an integer type value
//    int rndNumber = (int) myClassType.InvokeMember("GenerateRandom", 
//BindingFlags.InvokeMethod, null, myObj, null);
//    Console.WriteLine("Random Number = {0}", rndNumber);
            
//    Console.ReadLine();
//}
}
        //todo:generate values for each data type
        //     include more data types.
        private static void test_methods(Type myClassType, List<method> methodlist)
        {
            foreach (method method in methodlist)
            {
                object myObj = Activator.CreateInstance(myClassType);


                if (method.parameterlist.Count == 0)
                {
                    switch (method.type.ToUpper())
                    {
                        case "VOID":
                            // myClassType myObj = (myClassType)myAssembly.CreateInstance(myClassType.Name, false, BindingFlags.Default, null, new object[] { 5 }, null, null);
                            try
                            {
                                myClassType.InvokeMember(method.name, BindingFlags.InvokeMethod, null, myObj, null);
                            }
                            catch (Exception ex1)
                            {
                                Console.WriteLine(method.name + method.type + ex1.Message);
                            }
                            
                            break;
                        default:
                            object result = new object();
                            try
                            {
                                result = myClassType.InvokeMember(method.name, BindingFlags.InvokeMethod, null, myObj, null);
                            }
                            catch (Exception ex2)
                            {
                                Console.WriteLine(method.name + method.type + ex2.Message);
                            }
                            break;
                    }
                }
                else
                {
                    object[] objarray = new object[method.parameterlist.Count];
                                  
                    for (int i = 0; i < method.parameterlist.Count; i++)
                    {
                        switch (method.parameterlist[i].type.ToUpper())
                        {
                            case "SYSTEM.STRING":
                                objarray[i] = null;
                                break;
                            case "INT64":
                                Random rdnd = new Random();
                                // Convert.ToInt64(rdnd.Next(999999999))
                                objarray[i] = Convert.ToInt64(rdnd.Next(999999999));
                                break;
                            case "INT32":
                                Random rdn = new Random();
                                //rdn.Next(999999999)
                                objarray[i] = null;
                                break;
                            case "SYSTEM.BOOLEAN":
                                objarray[i] = null;
                                break;
                            default:
                                objarray[i] = "string";
                                break;

                        }

                    }
                    object resultt = new object();
                    try
                    {
                        resultt = myClassType.InvokeMember(method.name, BindingFlags.InvokeMethod, null, myObj, objarray);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(method.name +" " + method.type +" "+ e.Message);

                    }
                    
                 }
            }

        }

        private static void display_members(Type myClassType)
        {

            foreach (MemberInfo miObj in myClassType.GetMembers())
            {
                Console.WriteLine("The member {0} is of type {1}",
                      miObj.Name.PadRight(15), miObj.MemberType.ToString());

                if (miObj.MemberType.ToString().ToUpper().Equals("PROPERTY"))
                {
                    display_property(myClassType, miObj.Name.Trim());


                }


            }
        }

        private static void display_property(Type myClassType, string name)
        {
            PropertyInfo piObj = myClassType.GetProperty(name);
            Console.WriteLine("Name of Property:  {0}", piObj.Name.ToString());
            Console.WriteLine("Is Readable:       {0}", piObj.CanRead);
            Console.WriteLine("Is Modifiable:     {0}", piObj.CanWrite);
        }

        private static void display_fields(Type myClassType)
        {
            // Get Fields and print their info
            foreach (FieldInfo fiObj in myClassType.GetFields())
            {
                Console.WriteLine("Name of field:               {0}",
                                  fiObj.Name.ToString());
                Console.WriteLine("Attributes:                  {0}",
                                  fiObj.Attributes.ToString());
                Console.WriteLine("Containing (declaring type): {0}",
                                  fiObj.DeclaringType.ToString());
                Console.WriteLine("Field data type:             {0}",
                                  fiObj.FieldType.ToString());
            }

        }

        private static List<method> display_methods(Type myClassType)
        {
            List<method> list = new List<method>();

            // Get methods and print their info
            foreach (MethodInfo miObj in myClassType.GetMethods())
            {
                Console.WriteLine("\r\nName of Method: {0}", miObj.Name.ToString());
                Console.WriteLine("Return type:    {0}", miObj.ReturnType.ToString());
                Console.WriteLine("Signature:      {0}", miObj.ToString());


                method mymethod = new method();

                mymethod = get_method(miObj.ToString(), miObj.Name.ToString());
                list.Add(mymethod);
            }
            return list;
        }



        private static method get_method(string signature, string methodname)
        {
            method mymethod = new method();

            int pos = signature.IndexOf(" ");
            mymethod.type = signature.Substring(0, pos);

            mymethod.name =  methodname;


            int pos1 = signature.IndexOf("(");
            int pos2 = signature.IndexOf(")");
            if ((pos2 - pos1) > 1)
            {
                string parameters = signature.Substring(pos1+1, pos2-pos1-1);
                string[] parameterarray = parameters.Split(',');

                foreach (string parameter in parameterarray)
                {
                    parameter myparam = new parameter();
                    myparam.name = "param_name";
                    myparam.type = parameter.Trim();
                    mymethod.parameterlist.Add(myparam);
                }
            }

            return mymethod;
        }

        public class method
        {
            public string type;
            public string name;
            public List<parameter> parameterlist = new List<parameter>();

        }

        public class parameter
        {
            public string name;
            public string type;

        }
    }
}
