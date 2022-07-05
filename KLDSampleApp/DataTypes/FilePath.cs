using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace KLDSampleApp
{
    class FilePath : IUserInput
    {
        private string _value = string.Empty;
        private Dictionary<string, string> _acceptedFlags = new();
        private List<string> _flags = new();

        public PathConstraint Constraint = PathConstraint.IsBoth;

        public string Value
        {
            get => this._value;
            set
            {
                this._value = Path.TrimEndingDirectorySeparator(value.Trim());

                if (String.IsNullOrWhiteSpace(this._value) || !Path.IsPathFullyQualified(this._value) || !Exists(this._value))
                {
                    throw new Exception("Path doesn't exist or isn't fully qualified");
                }

                try
                {
                    if (IsDirectory())
                    {
                        this._value += Path.DirectorySeparatorChar;
                    }

                    this._value = Path.GetFullPath(this._value);

                    if (this.Constraint == PathConstraint.IsDirectory && IsFile())
                        throw new Exception("Path must point to a directory");

                    if (this.Constraint == PathConstraint.IsFile && IsDirectory())
                        throw new Exception("Path must point to a file");
                }
                catch (ArgumentException) { throw; }
                catch (SecurityException) { throw; }
                catch (NotSupportedException) { throw; }
                catch (PathTooLongException) { throw; }
            }
        }

        // CONSTRUCTORS
        public FilePath()
        {
            // Is there a cleaner way to do these constructors?
        }

        public FilePath(string path)
        {
            this.Value = path;
        }

        public FilePath(string path, PathConstraint constraint) : this(path)
        {
            this.Constraint = constraint;
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
            this.Constraint = constraint;
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
            FileAttributes attr = File.GetAttributes(this._value);

            return (attr & FileAttributes.Directory) == FileAttributes.Directory; // what does stuff in the parenthesis mean? why can't you just do (attr == FileAttributes.Directory)?
        }

        public bool IsFile()
        {
            FileAttributes attr = File.GetAttributes(this._value);

            return (attr & FileAttributes.Directory) != FileAttributes.Directory;
        }

        public string[] GetAcceptedFlags() => this._acceptedFlags.Keys.ToArray();
        public string GetAcceptedFlagDescription(string flag) => this._acceptedFlags[flag];
        public bool AcceptsFlags() => this._acceptedFlags.Count > 0 ? true : false;
        public void ClearAcceptedFlags() => this._acceptedFlags.Clear();
        public void RemoveAcceptedFlag(string flag) => this._acceptedFlags.Remove(flag);

        public void AddAcceptedFlag(string flag, string description)
        {
            if(IsValidFlagFormat(flag, GetAcceptedFlags()))
            {
                this._acceptedFlags.Add(flag, description);
            }
        }

        public void AddAcceptedFlag(Dictionary<string, string> flags)
        {
            foreach (var item in flags)
            {
                AddAcceptedFlag(item.Key, item.Value);
            }
        }

        public string[] GetFlags() => this._flags.ToArray();
        public void ClearFlags() => this._flags.Clear();
        public void RemoveFlag(string flag) => this._flags.Remove(flag);
        public bool ContainsFlag(string flag) => this._flags.Contains(flag);

        public void AddFlag(string flag)
        {
            if(!this._acceptedFlags.ContainsKey(flag))
            {
                throw new Exception($"{flag} is not an accepted flag");
            }

            if (IsValidFlagFormat(flag, GetFlags()))
            {
                this._flags.Add(flag);
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

        public override string ToString() => this.Value;

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
