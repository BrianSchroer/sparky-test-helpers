using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SparkyTestHelpers.NonPublic.UnitTests
{
    public class TestClass
    {
        private string PrivateStringField;
        private static string PrivateStaticStringField;
        protected string ProtectedStringField;
        internal string InternalStringField;
        public string PublicStringField;

        private string _stringProperty;
        private static string _staticStringProperty;

        public List<string> CalledMembers { get; } = new List<string>(); 

        internal string InternalStringProperty
        {
            get
            {
                LogMemberCalledWithDescription("Get");
                return _stringProperty;
            }

            set
            {
                LogMemberCalledWithDescription("Set");
                _stringProperty = value;
            }
        }

        private string PrivateStringProperty
        {
            get
            {
                LogMemberCalledWithDescription("Get");
                return _stringProperty;
            }

            set
            {
                LogMemberCalledWithDescription("Set");
                _stringProperty = value;
            }
        }

        private static string PrivateStaticStringProperty
        {
            get
            {
                return _staticStringProperty;
            }

            set
            {
                _staticStringProperty = value;
            }
        }

        protected string ProtectedStringProperty
        {
            get
            {
                LogMemberCalledWithDescription("Get");
                return _stringProperty;
            }

            set
            {
                LogMemberCalledWithDescription("Set");
                _stringProperty = value;
            }
        }

        public string PublicStringProperty
        {
            get
            {
                LogMemberCalledWithDescription("Get");
                return _stringProperty;
            }

            set
            {
                LogMemberCalledWithDescription("Set");
                _stringProperty = value;
            }
        }

        internal void InternalVoidMethod()
        {
            LogMemberCalled();
        }

        private void PrivateVoidMethod()
        {
            LogMemberCalled();
        }

        protected void ProtectedVoidMethod()
        {
            LogMemberCalled();
        }

        public void PublicVoidMethod()
        {
            LogMemberCalled();
        }

        internal string InternalStringMethod()
        {
            LogMemberCalled();
            return "success";
        }

        private string PrivateStringMethod()
        {
            LogMemberCalled();
            return "success";
        }

        protected string ProtectedStringMethod()
        {
            LogMemberCalled();
            return "success";
        }

        public string PublicStringMethod()
        {
            LogMemberCalled();
            return "success";
        }

        private string PrivateMethodWithArgs(int intArg, string stringArg, DateTime dateTimeArg)
        {
            LogMemberCalled();
            return "success";
        }

        private string PrivateMethodWithOverrides(int intArg, string stringArg)
        {
            LogMemberCalledWithDescription("int, string");
            return "success";
        }

        private string PrivateMethodWithOverrides(string stringArg, int intArg)
        {
            LogMemberCalledWithDescription("string, int");
            return "success";
        }

        private static string PrivateStaticMethodWithOverrides(int intArg, string stringArg)
        {
            return "success";
        }

        private static string PrivateStaticMethodWithOverrides(string stringArg, int intArg)
        {
            return "success";
        }

        private void LogMemberCalledWithDescription(string description, [CallerMemberName] string callerMemberName = null)
        {
            string callerMemberDescription = $"{(string.IsNullOrWhiteSpace(description) ? "" : $"{description} ")}{callerMemberName}";

            CalledMembers.Add(callerMemberDescription);
            Console.WriteLine($"{callerMemberDescription} was called.");
        }

        private void LogMemberCalled([CallerMemberName] string callerMemberName = null) => LogMemberCalledWithDescription(null, callerMemberName);
    }
}