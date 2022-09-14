if (args.Length != 1) throw new ArgumentException("Incorrect argument, please specify a path");
var filePath = args[0];
if (!Directory.Exists(filePath) && !File.Exists(filePath))
{
    throw new ArgumentException("No file or directory found");
}
MyNUnit.MyNUnit myNUnit = new();
myNUnit.RunAllByThePath(filePath);