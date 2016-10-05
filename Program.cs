using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    class Program
    {

        public static void DoMeeting(Person lady)
        {

            StringBuilder meeting = new StringBuilder(text.meeting);

            meeting.AppendFormat(text.mineInfo, lady.Status, lady.Name);

            if (lady.Status == text.mistress)
            {
                meeting.Append(text.haveSex);
            }

            Console.WriteLine(meeting);
        }
        
        static void Main(string[] args)
        {
            Room room = new Room();

            Console.WriteLine(text.welcome);

            // Adding girl
            Console.Write(text.addGirl);
            string wantAdd = Console.ReadLine();

            if (wantAdd == text.y)
                addingPerson(room, wantAdd);

            // Meeting 
            Console.Write(text.wantMeet);
            string wantMeet = Console.ReadLine();

            if (wantMeet == text.y)
            {
                room.Process(new Person.PersonDelegate(DoMeeting));
            }

            Lady.SheReady += DisplayMsg;

            // Uping age

            Console.Write("Up age? (y/n)");
            string wantUpAge = Console.ReadLine();

            if (wantUpAge == text.y)
                room.UpAgeForAllLadys();


            // Meeting agen

            Console.Write(text.wantMeet);
            wantMeet = Console.ReadLine();

            if (wantMeet == text.y)
            {
                room.Process(new Person.PersonDelegate(DoMeeting));
            }

            Console.WriteLine(text.endOfProgram);
            Console.ReadLine();
        }

        private static void addingPerson(Room room, string wantAdd)
        {
            while (wantAdd == text.y)
            {
                Lady person = createPersonPart();
                DisplayPersonInfo(person);
                room.AddGirlToRoom(person);

                Console.Write(text.addGirl);
                wantAdd = Console.ReadLine();
            }
        }


        private static void DisplayPersonInfo(Lady person)
        {
            Console.WriteLine(text.finaly);

            Console.WriteLine(text.mineInfo, person.Status, person.Name);
            if (person.getAge() > 0)
            {
                Console.WriteLine(text.yourAge, person.getAge());
            }
            if (person.Status == text.friend)
            {
                Console.WriteLine(text.yourHobbie, person.getHobbie());
            }
            else if (person.Status == text.mistress)
            {
                Console.WriteLine(text.yourServices, person.getServices());
            }
        }

        private static Lady createPersonPart()
        {

            string name;
            int age = 0;
            string status = "";

            Console.Write(text.q1);
            
            name = Console.ReadLine();

            Console.WriteLine(text.q2);
            Console.Write(text.q3);
            
            string choice = Console.ReadLine();
            
            if (choice == text.statusFriend)
            {
                status = text.friend;
            }
            else if (choice == text.statusMistress)
            {
                status = text.mistress;
            }

            Console.Write(text.q5);

            string ages = Console.ReadLine();
            age = int.Parse(ages);

            return new Lady(name, status, age);
        }


        public static void DisplayMsg(object sender, MassageEventArgs e) 
        {
            Console.WriteLine(e.Message);
        }
    }

    enum Status {
        friend = 1,
        mistress = 2
    }

    public class MassageEventArgs : EventArgs
    {
        public MassageEventArgs(string message)
        {
            Message = message;
        }

        public string Message {get; private set;}
    }

    class text 
    {
        public const string y = "y";
        public const string statusFriend = "1";
        public const string statusMistress = "2";
        public const string welcome = "===== Welcome to program =====";
        public const string addGirl = "Are you want add new girl to room? (y/n)";
        public const string endOfProgram = "This is the end of program. Thank you!";
        public const string q1 = "Enter name of girl: ";
        public const string q2 = "This girl will be friend or mistress? ";
        public const string q3 = "1 - Friend, 2 - Mistress. Your choice: ";
        public const string q5 = "How old is she? ";
        public const string friend = "friend";
        public const string mistress = "mistress";
        public const string sorry = "Sorry, she is to young for be mistress.";
        public const string hobbie = "run, swim";
        public const string services = "appointment, walk";
        public const string finaly = "Finaly information lady:";
        public const string mineInfo = "She is a {0}. Her name is {1}.";
        public const string yourHobbie = "Her hobbie is - {0}.";
        public const string yourServices = "And her services is - {0}.";
        public const string yourAge = "She is {0} ears old.";
        public const string wantMeet = "Are you want meet with ladys now? (y/n)";
        public const string meeting = "Now we go to park for walk.\n";
        public const string haveSex = "Then we will go to hostel and have fun.";
    }

    public class Person
    {
        protected string name;
        protected bool sexuallyMature;
        protected string status;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool SexuallyMature
        {
            get { return sexuallyMature; }
            set { sexuallyMature = value; }
        }
        
        public delegate void PersonDelegate(Person p);

    }


    public interface IMistress
    {
        void setServices(string text);
        string getServices();
    }

    public interface IFriend 
    {
        void setHobbie(string text);
        string getHobbie();
    }

    public abstract class Women : Person
    {
        protected int age;
        
        public int getAge()
        {
            return this.age;
        }

        public void setAge(int ages = 0)
        {
            this.age = ages;
        }

        public void upAge()
        {
            this.age++;
        }
    }

    public class Lady : Women, IMistress, IFriend
    {

        protected string services;
        protected string hobbie;
        
        public void setServices(string text = "")
        {
            this.services = text;
        }
        
        public string getServices()
        {
            return this.services;
        }

        public void setHobbie(string text = "")
        {
            this.hobbie = text;
        }

        public string getHobbie()
        {
            return this.services;
        }

        public void nowIcanBeMistress()
        {
            this.Status = text.mistress;
            this.SexuallyMature = true;
            this.setServices(text.services);

            SheReady(this, new MassageEventArgs(string.Format("My name is {0}. I heve {1} years \n I ready to be your mistress!", this.name, this.getAge())));
        }

        public static event EventHandler<MassageEventArgs> SheReady;

        // constructor 
        public Lady(string name, string status, int age) 
        {
            base.Name = name;
            this.setAge(age);

            if (age < 18)
                {
                    this.Status = text.friend;
                    this.SexuallyMature = false;
                    this.setServices(text.hobbie);
                }
                else {
                    this.Status = text.mistress;
                    this.SexuallyMature = true;
                    this.setServices(text.services);
                }
        }

    }

    public class Room
    {
        ArrayList aGirls = new ArrayList();

        public void AddGirlToRoom(Lady lady)
        {
            aGirls.Add(lady);
        }

        public void Process(Person.PersonDelegate someDelegateMethod) 
        {
            foreach (Lady lady in aGirls)
                someDelegateMethod(lady);
        }

        public void UpAgeForAllLadys() 
        {
            foreach (Lady lady in aGirls)
            {
                lady.upAge();

                if (lady.getAge() == 18)
                    lady.nowIcanBeMistress();
            }
        }
    }
}
