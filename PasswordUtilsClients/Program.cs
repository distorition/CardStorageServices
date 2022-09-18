using CardStorageServices.Utils;

var result=PasswordUtils.CreatePasswordHash("1123");
Console.WriteLine(result.passwordSlatm);
Console.WriteLine(result.passwordHash);