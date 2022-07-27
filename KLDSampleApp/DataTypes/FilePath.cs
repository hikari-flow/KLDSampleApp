using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace KLDSampleApp
{
    public class FilePath : IUserInput
    {
        private string _value = string.Empty;
        private Dictionary<string, string> _acceptedFlags = new();
        private List<string> _flags = new();

        public PathConstraint Constraint = PathConstraint.IsBoth;

        public string Value
        {
            get => _value;
            set
            {
                _value = Path.TrimEndingDirectorySeparator(value.Trim());

                if (string.IsNullOrWhiteSpace(_value) || !Path.IsPathFullyQualified(_value) || !Exists(_value))
                {
                    throw new Exception("Path doesn't exist or isn't fully qualified");
                }

                if (IsDirectory())
                {
                    _value += Path.DirectorySeparatorChar;
                }

                _value = Path.GetFullPath(_value);

                if (Constraint == PathConstraint.IsDirectory && IsFile())
                    throw new Exception("Path must point to a directory");

                if (Constraint == PathConstraint.IsFile && IsDirectory())
                    throw new Exception("Path must point to a file");
            }
        }

        // CONSTRUCTORS
        public FilePath(){}

        public FilePath(string path)
        {
            Value = path;
        }

        public FilePath(string path, PathConstraint constraint) : this(path)
        {
            Constraint = constraint;
        }

        public FilePath(string path, Dictionary<string, string> acceptedFlags) : this(path)
        {
            AddAcceptedFlag(acceptedFlags);
        }

        public FilePath(string path, PathConstraint constraint, Dictionary<string, string> acceptedFlags) : this(path, constraint)
        {
            AddAcceptedFlag(acceptedFlags);
        }

        public FilePath(PathConstraint constraint)
        {
            Constraint = constraint;
        }

        public FilePath(PathConstraint constraint, Dictionary<string, string> acceptedFlags) : this(constraint)
        {
            AddAcceptedFlag(acceptedFlags);
        }

        public FilePath(Dictionary<string, string> acceptedFlags)
        {
            AddAcceptedFlag(acceptedFlags);
        }

        // METHODS

        public bool IsDirectory()
        {
            FileAttributes attr = File.GetAttributes(_value);

            return (attr & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public bool IsFile()
        {
            return !IsDirectory();
        }

        public string[] GetAcceptedFlags() => _acceptedFlags.Keys.ToArray();
        public string GetAcceptedFlagDescription(string flag) => _acceptedFlags[flag];
        public bool AcceptsFlags() => _acceptedFlags.Count > 0 ? true : false;
        public void ClearAcceptedFlags() => _acceptedFlags.Clear();
        public void RemoveAcceptedFlag(string flag) => _acceptedFlags.Remove(flag);

        public void AddAcceptedFlag(string flag, string description)
        {
            if(IsValidFlagFormat(flag, GetAcceptedFlags()))
            {
                _acceptedFlags.Add(flag, description);
            }
        }

        public void AddAcceptedFlag(Dictionary<string, string> flags)
        {
            foreach (var item in flags)
            {
                AddAcceptedFlag(item.Key, item.Value);
            }
        }

        public string[] GetFlags() => _flags.ToArray();
        public void ClearFlags() => _flags.Clear();
        public void RemoveFlag(string flag) => _flags.Remove(flag);
        public bool ContainsFlag(string flag) => _flags.Contains(flag);

        public void AddFlag(string flag)
        {
            if(!_acceptedFlags.ContainsKey(flag))
            {
                throw new Exception($"{flag} is not an accepted flag");
            }

            if (IsValidFlagFormat(flag, GetFlags()))
            {
                _flags.Add(flag);
            }
        }

        public void AddFlag(string[] flags)
        {
            foreach (string flag in flags)
            {
                AddFlag(flag);
            }
        }

        public static bool Exists(string path)
        {
            return Directory.Exists(path) || File.Exists(path);
        }

        public override string ToString() => Value;

        private static bool IsValidFlagFormat(string newFlag, string[] currentFlags)
        {
            if (String.IsNullOrWhiteSpace(newFlag) || !newFlag[0].Equals('-'))
            {
                throw new Exception("Not in valid flag format");
            }

            if (currentFlags.Contains(newFlag))
            {
                throw new Exception($"{newFlag} flag already exists");
            }

            return true;
        }
    }
}
